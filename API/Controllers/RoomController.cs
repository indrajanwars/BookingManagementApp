using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.Contracts;
using API.DTOs.Rooms;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

// RoomController adalah sebuah kontroler API yang berfungsi untuk mengelola data ruangan.
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public RoomController(IRoomRepository roomRepository, IBookingRepository bookingRepository, IEmployeeRepository employeeRepository)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
        _employeeRepository = employeeRepository;
    }

    // Endpoint untuk mendapatkan ruangan yang digunakan pada hari ini
    [HttpGet("RoomOrderedToday")]
    public IActionResult GetRoomUsedToday()
    {
        // Mengambil semua data booking
        var allBooking = _bookingRepository.GetAll();
        // Mengambil semua data ruangan
        var allRoom = _roomRepository.GetAll();
        // Mengambil semua employee data
        var allEmployees = _employeeRepository.GetAll();

        // Mendapatkan tanggal hari ini
        DateTime today = DateTime.Now.Date;

        // Cek jika tidak ada data booking atau ruangan
        if (!(allBooking.Any() && allRoom.Any()))
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Booking atau Room Tidak Ditemukan"
            });
        }

        var roomsUsedToday = (from b in allBooking
                              join r in allRoom on b.RoomGuid equals r.Guid
                              join e in allEmployees on b.EmployeeGuid equals e.Guid
                              where b.StartDate.Date <= today && today <= b.EndDate.Date
                              select new RoomUsedDto
                              {
                                  BookingGuid = b.Guid,
                                  Status = b.Status,
                                  RoomName = r.Name,
                                  Floor = r.Floor,
                                  BookedBy = $"{e.FirstName} {e.LastName}"
                              }).ToList();

        if (!roomsUsedToday.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Tidak ada ruangan yang digunakan hari ini"
            });
        }

        // Mengembalikan daftar ruangan yang digunakan hari ini dalam bentuk respons OK
        return Ok(new ResponseOKHandler<IEnumerable<RoomUsedDto>>(roomsUsedToday));
    }

    // Endpoint untuk mendapatkan daftar ruangan yang tersedia
    [HttpGet("AvailableRooms")]
    public IActionResult GetAvailableRooms()
    {
        try
        {
            // Mengambil semua data ruangan
            var allRooms = _roomRepository.GetAll();

            // Mengambil semua data booking
            var allBookings = _bookingRepository.GetAll();

            // Mendapatkan tanggal hari ini
            DateTime today = DateTime.Now.Date;

            // Menentukan ruangan mana saja yang sudah dipesan hari ini
            var usedRoomGuids = allBookings
                .Where(b => b.StartDate.Date <= today && today <= b.EndDate.Date)
                .Select(b => b.RoomGuid)
                .Distinct()
                .ToList();

            // Mengambil daftar ruangan yang belum dipesan
            var availableRooms = allRooms
                .Where(r => !usedRoomGuids.Contains(r.Guid))
                .Select(r => new RoomDto
                {
                    Guid = r.Guid,
                    Name = r.Name,
                    Floor = r.Floor,
                    Capacity = r.Capacity
                })
                .ToList();

            // Jika tidak ada ruangan yang tersedia
            if (!availableRooms.Any())
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Tidak ada ruangan yang tersedia hari ini"
                });
            }

            // Mengembalikan daftar ruangan yang tersedia
            return Ok(new ResponseOKHandler<IEnumerable<RoomDto>>(availableRooms));
        }
        // Penanganan jika terjadi kesalahan
        catch (ExceptionHandler ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to retrieve available rooms",
                Error = ex.Message
            });
        }
    }

    // Mendapatkan semua data ruangan.
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _roomRepository.GetAll();

        // Jika tidak ada data ruangan yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk RoomDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (RoomDto)x);

        return Ok(new ResponseOKHandler<IEnumerable<RoomDto>>(data));
    }

    // Mendapatkan data ruangan berdasarkan GUID.
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _roomRepository.GetByGuid(guid);

        // Jika data ruangan tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk RoomDto dan mengembalikannya sebagai respons API.
        return Ok(new ResponseOKHandler<RoomDto>((RoomDto)result));
    }

    // Membuat data ruangan baru.
    [HttpPost]
    public IActionResult Create(CreateRoomDto roomDto)
    {
        try
        {
            // Membuat data ruangan baru berdasarkan data yang diterima dalam permintaan API.
            var result = _roomRepository.Create(roomDto);

            // Jika gagal membuat data ruangan, mengembalikan respons BadRequest.
            if (result is null)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to create data"
                });
            }

            // Mengkonversi hasil ke dalam bentuk RoomDto dan mengembalikannya sebagai respons API.
            return Ok(new ResponseOKHandler<RoomDto>((RoomDto)result));
        }
        catch (ExceptionHandler ex)
        {
            // Mengembalikan respons dengan kode status 500 dan pesan error jika terjadi kesalahan.
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to create data",
                Error = ex.Message
            });
        }
    }

    // Memperbarui data ruangan.
    [HttpPut]
    public IActionResult Update(RoomDto roomDto)
    {
        try
        {
            // Mengambil data ruangan berdasarkan GUID yang diberikan.
            var entity = _roomRepository.GetByGuid(roomDto.Guid);

            // Jika data ruangan tidak ditemukan, mengembalikan respons NotFound.
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id Not Found"
                });
            }

            // Menyalin data dari RoomDto ke objek Room yang akan diperbarui dengan tetap mempertahankan CreatedDate dari entitas yang ada.
            Room toUpdate = roomDto;
            toUpdate.CreatedDate = entity.CreatedDate;

            // Memperbarui data ruangan.
            var result = _roomRepository.Update(toUpdate);

            // Jika gagal memperbarui data ruangan, mengembalikan respons BadRequest.
            if (!result)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to update data"
                });
            }

            // Mengembalikan respons sukses dengan pesan "Data Updated".
            return Ok(new ResponseOKHandler<string>("Data Updated"));
        }
        catch (ExceptionHandler ex)
        {
            // Mengembalikan respons dengan kode status 500 dan pesan error jika terjadi kesalahan.
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to create data",
                Error = ex.Message
            });
        }
    }

    // Menghapus data ruangan berdasarkan GUID.
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            // Mengambil data ruangan berdasarkan GUID yang diberikan.
            var entity = _roomRepository.GetByGuid(guid);

            // Jika data ruangan tidak ditemukan, mengembalikan respons NotFound.
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id Not Found"
                });
            }

            // Menghapus data ruangan dari repositori.
            var result = _roomRepository.Delete(entity);

            // Jika gagal menghapus data ruangan, mengembalikan respons BadRequest.
            if (!result)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to delete data"
                });
            }

            // Mengembalikan respons sukses dengan pesan "Data Deleted".
            return Ok(new ResponseOKHandler<string>("Data Deleted"));
        }
        catch (ExceptionHandler ex)
        {
            // Mengembalikan respons dengan kode status 500 dan pesan error jika terjadi kesalahan.
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to create data",
                Error = ex.Message
            });
        }
    }
}