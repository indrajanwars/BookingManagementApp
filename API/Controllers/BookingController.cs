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
    [Authorize(Roles = "admin, superAdmin")]
    public IActionResult GetAllBookingDetails()
    {
        // Mengambil semua data booking dari repository
        var allBookings = _bookingRepository.GetAll();

        // Mengambil semua data employee (Employee) yang berelasi dengan booking dari repository
        var allEmployees = _employeeRepository.GetAll();

        // Mengambil semua data room dari repository
        var allRooms = _roomRepository.GetAll();

        // Menggabungkan (join) tabel booking, employee, dan room untuk mendapatkan detail booking
        var bookingDetails = (from b in allBookings
                              join e in allEmployees on b.EmployeeGuid equals e.Guid
                              join r in allRooms on b.RoomGuid equals r.Guid
                              select new BookingDetailsDto
                              {
                                  Guid = b.Guid,
                                  BookedNIK = e.Nik, // NIK employee yang melakukan booking
                                  BookedBy = $"{e.FirstName} {e.LastName}", // Nama lengkap employee yang melakukan booking
                                  RoomName = r.Name, // Nama room yang dipesan
                                  StartDate = b.StartDate, // Tanggal mulai booking
                                  EndDate = b.EndDate, // Tanggal akhir booking
                                  Status = b.Status, // Status dari booking
                                  Remarks = b.Remarks // Catatan terkait booking
                              }).ToList();

        // Jika tidak ada detail booking yang ditemukan, kembalikan respons NotFound
        if (!bookingDetails.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Tidak ada detail booking yang ditemukan"
            });
        }

        // Mengembalikan daftar detail booking dalam bentuk respons OK
        return Ok(new ResponseOKHandler<IEnumerable<BookingDetailsDto>>(bookingDetails));
    }

    // Endpoint untuk mengambil detail booking berdasarkan GUID yang diberikan
    [HttpGet("details/{guid}", Name = "GetBookingByGuid")]
    [Authorize(Roles = "admin, superAdmin")]
    public IActionResult GetBookingByGuid(Guid guid)
    {
        // Mengambil data booking berdasarkan GUID
        var booking = _bookingRepository.GetByGuid(guid);
        // Mengambil semua data employee
        var allEmployees = _employeeRepository.GetAll();
        // Mengambil semua data room
        var allRooms = _roomRepository.GetAll();

        // Jika booking tidak ditemukan
        if (booking == null)
        {
            // Mengembalikan respons NotFound dengan pesan kesalahan
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Booking dengan GUID yang diberikan tidak ditemukan"
            });
        }

        // Membuat objek detail booking dengan join antara booking, employee, dan room
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

        // Jika detail booking tidak ditemukan
        if (bookingDetail == null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Detail booking dengan GUID yang diberikan tidak ditemukan"
            });
        }

        // Mengembalikan detail booking dalam bentuk respons OK
        return Ok(new ResponseOKHandler<BookingDetailsDto>(bookingDetail));
    }

    // Endpoint untuk mengetahui durasi pemesanan room
    [HttpGet("BookingDuration")]
    public IActionResult GetBookingDuration()
    {
        try
        {
            // Mengambil semua data booking dan room
            var bookings = _bookingRepository.GetAll();
            var rooms = _roomRepository.GetAll();

            // Mendefinisikan hari yang tidak dihitung (Sabtu dan Minggu)
            var nonWorkingDays = new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday };

            // List untuk menyimpan hasil perhitungan durasi pemesanan
            var roomBookingDuration = new List<RoomBookingDurationDto>();

            // Mengiterasi setiap room
            foreach (var room in rooms)
            {
                // Mengambil semua booking untuk room tertentu
                var roomBookings = bookings.Where(b => b.RoomGuid == room.Guid);

                // Jika ada booking untuk room tersebut
                if (roomBookings.Any())
                {
                    // Menghitung durasi booking total
                    var totalBookingDurationInHours = 0;

                    // Mengiterasi setiap booking
                    foreach (var booking in roomBookings)
                    {
                        var startDate = booking.StartDate;
                        var endDate = booking.EndDate;

                        while (startDate <= endDate)
                        {
                            // Menambahkan durasi jika bukan hari Sabtu atau Minggu
                            if (!nonWorkingDays.Contains(startDate.DayOfWeek))
                            {
                                totalBookingDurationInHours += 1;
                            }
                            startDate = startDate.AddHours(1);
                        }
                    }

                    // Mengkonversi durasi dari jam ke hari
                    var totalBookingDurationInDays = totalBookingDurationInHours / 24;
                    var remainingHours = totalBookingDurationInHours % 24;

                    // Menambahkan hasil perhitungan ke list
                    roomBookingDuration.Add(new RoomBookingDurationDto
                    {
                        RoomGuid = room.Guid,
                        RoomName = room.Name,
                        BookingDuration = $"{totalBookingDurationInDays} Day {remainingHours} Hour"
                    });
                }
            }

            // Mengembalikan daftar hasil perhitungan
            return Ok(new ResponseOKHandler<IEnumerable<RoomBookingDurationDto>>(roomBookingDuration));

        }
        // Menangkap pengecualian jika ada kesalahan saat eksekusi kode
        catch (ExceptionHandler ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to calculate booking lengths",
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