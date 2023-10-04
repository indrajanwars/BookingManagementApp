using FluentValidation;
using API.DTOs.Bookings;

namespace API.Utilities.Validations.Accounts
{
    // Validator untuk objek Booking (BookingDto).
    public class BookingValidator : AbstractValidator<BookingDto>
    {
        public BookingValidator()
        {
            // Aturan validasi untuk properti Guid.
            RuleFor(a => a.Guid)
                .NotEmpty(); // Memastikan Guid tidak kosong.

            // Aturan validasi untuk properti StartDate.
            RuleFor(a => a.StartDate)
                .NotEmpty(); // Memastikan StartDate tidak kosong.

            // Aturan validasi untuk properti EndDate.
            RuleFor(a => a.EndDate)
                .NotEmpty() // Memastikan EndDate tidak kosong.
                .GreaterThan(a => a.StartDate) // Memastikan EndDate lebih besar dari StartDate.
                .WithMessage("EndDate harus lebih besar dari StartDate"); // Pesan kesalahan yang akan ditampilkan jika aturan tidak terpenuhi.

            // Aturan validasi untuk properti Status.
            RuleFor(a => a.Status)
                .NotEmpty(); // Memastikan Status tidak kosong.

            // Aturan validasi untuk properti Remarks.
            RuleFor(a => a.Remarks)
                .NotEmpty(); // Memastikan Remarks tidak kosong.

            // Aturan validasi untuk properti RoomGuid.
            RuleFor(a => a.RoomGuid)
                .NotEmpty() // Memastikan RoomGuid tidak kosong.
                .NotEqual(Guid.Empty); // Memastikan RoomGuid tidak sama dengan Guid.Empty.

            // Aturan validasi untuk properti EmployeeGuid.
            RuleFor(a => a.EmployeeGuid)
                .NotEmpty() // Memastikan EmployeeGuid tidak kosong.
                .NotEqual(Guid.Empty); // Memastikan EmployeeGuid tidak sama dengan Guid.Empty.
        }
    }
}
