using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Bookings;

/* Kelas BookingDto adalah Data Transfer Object (DTO) yang digunakan untuk 
 * mengambil data dari objek Booking dan mengembalikannya sebagai respons dari API. */
public class BookingDto
{
    // Properti-properti berikut adalah atribut-atribut yang akan diambil dari objek Booking.
    public Guid Guid { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public StatusLevel Status { get; set; }
    public string Remarks { get; set; }
    public Guid RoomGuid { get; set; }
    public Guid EmployeeGuid { get; set; }

    // Operator konversi explicit yang mengubah objek Booking menjadi objek BookingDto.
    public static explicit operator BookingDto(Booking booking)
    {
        // Membuat objek BookingDto baru dengan nilai-nilai yang diambil dari Booking.
        return new BookingDto
        {
            Guid = booking.Guid,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Status = booking.Status,
            Remarks = booking.Remarks,
            RoomGuid = booking.RoomGuid,
            EmployeeGuid = booking.EmployeeGuid
        };
    }

    // Operator konversi implicit yang mengubah objek BookingDto menjadi objek Booking.
    public static implicit operator Booking(BookingDto bookingDto)
    {
        // Membuat objek Booking baru dengan nilai-nilai yang diambil dari BookingDto.
        return new Booking
        {
            Guid = bookingDto.Guid,
            StartDate = bookingDto.StartDate,
            EndDate = bookingDto.EndDate,
            Status = bookingDto.Status,
            Remarks = bookingDto.Remarks,
            RoomGuid = bookingDto.RoomGuid,
            EmployeeGuid = bookingDto.EmployeeGuid,
            ModifiedDate = DateTime.Now  // Mengatur ModifiedDate sebagai waktu saat ini.
        };
    }
}