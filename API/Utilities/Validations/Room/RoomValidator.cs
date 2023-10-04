using FluentValidation;
using API.DTOs.Rooms;

// Buat kelas RoomValidator yang merupakan turunan dari AbstractValidator
namespace API.Utilities.Validations.Accounts
{
    public class RoomValidator : AbstractValidator<RoomDto>
    {
        public RoomValidator()
        {
            // Aturan validasi untuk properti 'Guid'
            RuleFor(a => a.Guid)
                .NotEmpty();          // Guid tidak boleh kosong

            // Aturan validasi untuk properti 'Name'
            RuleFor(a => a.Name)
                .NotEmpty();          // Name tidak boleh kosong

            // Aturan validasi untuk properti 'Floor'
            RuleFor(a => a.Floor)
                .NotEmpty()           // Floor tidak boleh kosong
                .GreaterThan(0);      // Floor harus lebih besar dari 0

            // Aturan validasi untuk properti 'Capacity'
            RuleFor(a => a.Capacity)
                .NotEmpty()           // Capacity tidak boleh kosong
                .GreaterThan(0);      // Capacity harus lebih besar dari 0
        }
    }
}