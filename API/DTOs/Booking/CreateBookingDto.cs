using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Bookings;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek Booking berdasarkan data tersebut. */
public class CreateBookingDto
{
    // Properti-properti berikut mewakili atribut-atribut yang akan diterima dari permintaan API.
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public StatusLevel Status { get; set; }
    public string Remarks { get; set; }
    public Guid RoomGuid { get; set; }
    public Guid EmployeeGuid { get; set; }

    // Operator konversi implicit yang mengubah objek CreateBookingDto menjadi objek Booking.
    public static implicit operator Booking(CreateBookingDto createBookingDto)
    {
        // Membuat objek Booking baru dengan nilai-nilai yang diambil dari CreateBookingDto.
        return new Booking
        {
            StartDate = createBookingDto.StartDate,
            EndDate = createBookingDto.EndDate,
            Status = createBookingDto.Status,
            Remarks = createBookingDto.Remarks,
            RoomGuid = createBookingDto.RoomGuid,
            EmployeeGuid = createBookingDto.EmployeeGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}