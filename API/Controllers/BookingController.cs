using System;
using System.Net;
using API.Contracts;
using API.DTOs.Bookings;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// Kontroler API yang berfungsi untuk mengelola data pemesanan.
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public BookingController(IBookingRepository bookingRepository, IRoomRepository roomRepository, IEmployeeRepository employeeRepository)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _employeeRepository = employeeRepository;
    }

    // Endpoint untuk mendapatkan semua detail booking
    [HttpGet("AllBookingDetails")]
    [Authorize(Roles = "admin, superAdmin")]    // Akses hanya untuk user yang memiliki role admin atau superAdmin.
    public IActionResult GetAllBookingDetails()
    {
        // Mendapatkan semua data booking, employee, dan ruangan.
        var allBookings = _bookingRepository.GetAll();
        var allEmployees = _employeeRepository.GetAll();
        var allRooms = _roomRepository.GetAll();

        // Menggabungkan data booking dengan data employee dan ruangan untuk membuat detail booking.
        var bookingDetails = (from b in allBookings
                              join e in allEmployees on b.EmployeeGuid equals e.Guid
                              join r in allRooms on b.RoomGuid equals r.Guid
                              select new BookingDetailsDto
                              {
                                  Guid = b.Guid,
                                  BookedNIK = e.Nik,
                                  BookedBy = $"{e.FirstName} {e.LastName}",
                                  RoomName = r.Name,
                                  StartDate = b.StartDate,
                                  EndDate = b.EndDate,
                                  Status = b.Status,
                                  Remarks = b.Remarks
                              }).ToList();

        if (!bookingDetails.Any())  // Memeriksa apakah ada detail booking.
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Tidak ada detail booking yang ditemukan"
            });
        }

        // Mengembalikan daftar detail booking dalam respons OK.
        return Ok(new ResponseOKHandler<IEnumerable<BookingDetailsDto>>(bookingDetails));
    }

    // Endpoint untuk mengambil detail booking berdasarkan GUID yang diberikan
    [HttpGet("details/{guid}", Name = "GetBookingByGuid")]
    [Authorize(Roles = "admin, superAdmin")]
    public IActionResult GetBookingByGuid(Guid guid)
    {
        var booking = _bookingRepository.GetByGuid(guid);   // Mendapatkan data booking berdasarkan GUID yang diberikan.
        var allEmployees = _employeeRepository.GetAll();    // Mendapatkan semua data employee.
        var allRooms = _roomRepository.GetAll();            // Mendapatkan semua data ruangan

        if (booking == null)    // Memeriksa apakah booking dengan GUID yang diberikan ditemukan.
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Booking dengan GUID yang diberikan tidak ditemukan"
            });
        }
        // Menggabungkan data booking dengan data employee dan ruangan untuk membuat detail booking.
        var bookingDetail = (from b in new[] { booking }
                             join e in allEmployees on b.EmployeeGuid equals e.Guid
                             join r in allRooms on b.RoomGuid equals r.Guid
                             select new BookingDetailsDto
                             {
                                 Guid = b.Guid,
                                 BookedNIK = e.Nik,
                                 BookedBy = $"{e.FirstName} {e.LastName}",
                                 RoomName = r.Name,
                                 StartDate = b.StartDate,
                                 EndDate = b.EndDate,
                                 Status = b.Status,
                                 Remarks = b.Remarks
                             }).FirstOrDefault();

        if (bookingDetail == null)  // Memeriksa apakah detail booking ditemukan.
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Detail booking dengan GUID yang diberikan tidak ditemukan"
            });
        }

        // Mengembalikan detail booking dalam respons OK.
        return Ok(new ResponseOKHandler<BookingDetailsDto>(bookingDetail));
    }

    // Endpoint untuk menghitung durasi booking per ruangan, memperhitungkan hari libur.
    [HttpGet("BookingDuration")]
    public IActionResult GetBookingDuration()
    {
        try
        {
            var bookings = _bookingRepository.GetAll();     // Mengambil semua data pemesanan (booking) dari repositori.
            var rooms = _roomRepository.GetAll();           // Mengambil semua data room dari repositori.
            var nonWorkingDays = new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday };      // Daftar hari yang bukan hari kerja (akhir pekan).
            var roomBookingDuration = new List<RoomBookingDurationDto>();       // Daftar untuk menyimpan informasi durasi pemesanan per ruangan.

            foreach (var room in rooms)
            {
                var roomBookings = bookings.Where(b => b.RoomGuid == room.Guid);    // Mengambil semua pemesanan yang terkait dengan ruangan ini.

                if (roomBookings.Any())
                {
                    var totalBookingDurationInHours = 0;    // Variabel untuk menyimpan total durasi pemesanan dalam jam.

                    foreach (var booking in roomBookings)   // Hasil dari loop adalah total durasi pemesanan dalam jam, memperhitungkan hari libur.
                    {
                        // Menentukan tanggal awal dan akhir pemesanan.
                        var startDate = booking.StartDate;
                        var endDate = booking.EndDate;

                        while (startDate <= endDate)    // Menghitung durasi pemesanan dalam jam, memperhitungkan hari libur.
                        {
                            if (!nonWorkingDays.Contains(startDate.DayOfWeek))      // Memeriksa apakah hari saat ini bukan hari libur.
                            {
                                totalBookingDurationInHours += 1;   // Jika bukan hari libur, tambahkan 1 jam ke total durasi pemesanan.
                            }
                            startDate = startDate.AddHours(1);      // Pindah ke jam berikutnya.
                        }
                    }
                    // Menghitung durasi pemesanan dalam hari dan jam.
                    var totalBookingDurationInDays = totalBookingDurationInHours / 24;
                    var remainingHours = totalBookingDurationInHours % 24;

                    roomBookingDuration.Add(new RoomBookingDurationDto      // Menambahkan informasi durasi pemesanan ruangan ke daftar.
                    {
                        RoomGuid = room.Guid,
                        RoomName = room.Name,
                        BookingDuration = $"{totalBookingDurationInDays} Day {remainingHours} Hour"
                    });
                }
            }
            // Mengembalikan daftar durasi pemesanan ruangan dalam respons sukses.
            return Ok(new ResponseOKHandler<IEnumerable<RoomBookingDurationDto>>(roomBookingDuration));

        }
        catch (ExceptionHandler ex)     // Menangani kesalahan jika terjadi, dan mengembalikan respons 500 Internal Server Error.
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to calculate booking durations",
                Error = ex.Message
            });
        }
    }

    // Mendapatkan semua data booking.
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _bookingRepository.GetAll();

        // Jika tidak ada data booking yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk BookingDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (BookingDto)x);

        // Mengembalikan respons sukses dengan objek ResponseOKHandler yang berisi data.
        return Ok(new ResponseOKHandler<IEnumerable<BookingDto>>(data));
    }

    // Mendapatkan data booking berdasarkan GUID.
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _bookingRepository.GetByGuid(guid);

        // Jika data booking tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk BookingDto dan mengembalikannya sebagai respons API.
        return Ok(new ResponseOKHandler<BookingDto>((BookingDto)result));
    }

    // Membuat data booking baru.
    [HttpPost]
    public IActionResult Create(CreateBookingDto bookingDto)
    {
        try
        {
            // Membuat data booking baru dari CreateBookingDto.
            var result = _bookingRepository.Create(bookingDto);

            // Jika gagal membuat data booking, mengembalikan respons BadRequest.
            if (result is null)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to create data"
                });
            }

            // Mengkonversi hasil ke dalam bentuk BookingDto dan mengembalikannya sebagai respons API.
            return Ok(new ResponseOKHandler<BookingDto>((BookingDto)result));
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

    // Memperbarui data booking.
    [HttpPut]
    public IActionResult Update(BookingDto bookingDto)
    {
        try
        {
            // Mengambil data booking berdasarkan GUID yang diberikan.
            var entity = _bookingRepository.GetByGuid(bookingDto.Guid);

            // Jika data booking tidak ditemukan, mengembalikan respons NotFound.
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id Not Found"
                });
            }

            // Menyalin data dari BookingDto ke objek Booking yang akan diperbarui dengan tetap mempertahankan CreatedDate.
            Booking toUpdate = bookingDto;
            toUpdate.CreatedDate = entity.CreatedDate;

            // Memperbarui data booking di repositori.
            var result = _bookingRepository.Update(toUpdate);

            // Jika gagal memperbarui data booking, mengembalikan respons BadRequest.
            if (!result)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to update data"
                });
            }

            // Mengembalikan respons sukses dengan pesan.
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

    // Menghapus data booking berdasarkan GUID.
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            // Mengambil data booking berdasarkan GUID yang diberikan.
            var entity = _bookingRepository.GetByGuid(guid);

            // Jika data booking tidak ditemukan, mengembalikan respons NotFound.
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id Not Found"
                });
            }

            // Menghapus data booking dari repositori.
            var result = _bookingRepository.Delete(entity);

            // Jika gagal menghapus data booking, mengembalikan respons BadRequest.
            if (!result)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to delete data"
                });
            }

            // Mengembalikan respons sukses dengan pesan.
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