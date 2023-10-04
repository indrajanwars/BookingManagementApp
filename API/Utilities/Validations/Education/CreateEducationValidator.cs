using FluentValidation;
using API.DTOs.Educations;

namespace API.Utilities.Validations.Educations;

// Validator ini digunakan untuk memvalidasi data ketika mencoba membuat data pendidikan baru.
public class CreateEducationValidator : AbstractValidator<CreateEducationDto>
{
    public CreateEducationValidator()
    {
        // Pastikan 'Major' tidak kosong (harus diisi).
        RuleFor(e => e.Major)
            .NotEmpty();

        // Pastikan 'Degree' tidak kosong (harus diisi).
        RuleFor(e => e.Degree)
            .NotEmpty();

        // Pastikan 'Gpa' berada dalam rentang 0 hingga 4.
        RuleFor(e => e.Gpa)
            .InclusiveBetween(0, 4)
            .WithMessage("GPA harus dalam rentang antara 0 hingga 4");

        // Pastikan 'UniversityGuid' tidak kosong (harus diisi).
        RuleFor(e => e.UniversityGuid)
            .NotEmpty();
    }
}