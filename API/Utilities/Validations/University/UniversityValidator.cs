using FluentValidation;
using API.DTOs.Universities;

// Buat kelas UniversityValidator yang merupakan turunan dari AbstractValidator
namespace API.Utilities.Validations.Universities
{
    public class UniversityValidator : AbstractValidator<UniversityDto>
    {
        public UniversityValidator()
        {
            // Aturan validasi untuk properti 'Guid'
            RuleFor(u => u.Guid)
                .NotEmpty();          // Guid tidak boleh kosong

            // Aturan validasi untuk properti 'Code'
            RuleFor(u => u.Code)
                .NotEmpty()           // Code tidak boleh kosong
                .MinimumLength(2)     // Code harus memiliki panjang minimal 2 dan maksimal 10 karakter
                .MaximumLength(10);

            // Aturan validasi untuk properti 'Name'
            RuleFor(u => u.Name)
                .NotEmpty();          // Name tidak boleh kosong
        }
    }
}