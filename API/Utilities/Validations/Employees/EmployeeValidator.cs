using API.DTOs.Employees;
using FluentValidation;
namespace API.Utilities.Validations.Employees;

// Validator untuk DTO EmployeeDto
public class EmployeeValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeValidator()
    {
        // Aturan validasi untuk Guid
        RuleFor(e => e.Guid)
           .NotEmpty(); // Tidak boleh kosong

        // Aturan validasi untuk Nik
        RuleFor(e => e.Nik)
           .NotEmpty() // Tidak boleh kosong
           .Length(6); // Harus memiliki panjang 6 karakter

        // Aturan validasi untuk FirstName
        RuleFor(e => e.FirstName)
           .NotEmpty();

        // Aturan validasi untuk BirthDate
        RuleFor(e => e.BirthDate)
           .NotEmpty()
           .LessThan(DateTime.Now.AddYears(-18)) // Usia minimal 18 tahun
           .WithMessage("Usia minimal 18 tahun"); // Pesan error kustom jika aturan tidak terpenuhi

        // Aturan validasi untuk Gender
        RuleFor(e => e.Gender)
           .NotEmpty()
           .IsInEnum() // Harus merupakan salah satu dari nilai-nilai enum GenderLevel
           .WithMessage("1 = Male, 2 = Female"); // Pesan error kustom jika aturan tidak terpenuhi

        // Aturan validasi untuk HiringDate
        RuleFor(e => e.HiringDate).NotEmpty();

        // Aturan validasi untuk Email
        RuleFor(e => e.Email)
           .NotEmpty().WithMessage("Tidak Boleh Kosong")
           .EmailAddress().WithMessage("Format Email Salah"); // Harus berformat email, dengan pesan error kustom

        // Aturan validasi untuk PhoneNumber
        RuleFor(e => e.PhoneNumber)
           .NotEmpty() // Tidak boleh kosong
           .MinimumLength(10).MaximumLength(15); // Panjang minimal 10 dan maksimal 15  karakter
    }
}
