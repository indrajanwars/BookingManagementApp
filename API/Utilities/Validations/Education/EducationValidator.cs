using FluentValidation;
using API.DTOs.Educations;

namespace API.Utilities.Validations.Educations;

// Validator ini digunakan untuk memvalidasi data ketika mencoba mengubah data pendidikan yang ada.
public class EducationValidator : AbstractValidator<EducationDto>
{
    public EducationValidator()
    {
        // Pastikan 'Guid' tidak kosong (harus diisi).
        RuleFor(e => e.Guid)
            .NotEmpty();

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