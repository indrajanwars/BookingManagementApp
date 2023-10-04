using API.DTOs.Employees;
using FluentValidation;

namespace API.Utilities.Validations.Employees
{
    // Validator untuk DTO CreateEmployeeDto
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            // Aturan validasi untuk FirstName
            RuleFor(e => e.FirstName)
               .NotEmpty(); // Tidak boleh kosong

            // Aturan validasi untuk BirthDate
            RuleFor(e => e.BirthDate)
               .NotEmpty() // Tidak boleh kosong
               .LessThan(DateTime.Now.AddYears(-18)) // Usia minimal 18 tahun
               .WithMessage("Usia minimal 18 tahun"); // Pesan error kustom jika aturan tidak terpenuhi

            // Aturan validasi untuk Gender
            RuleFor(e => e.Gender)
               .NotEmpty() // Tidak boleh kosong
               .IsInEnum() // Harus merupakan salah satu dari nilai-nilai enum GenderLevel
               .WithMessage("1 = Male, 2 = Female"); // Pesan error kustom jika aturan tidak terpenuhi

            // Aturan validasi untuk HiringDate
            RuleFor(e => e.HiringDate)
               .NotEmpty(); // Tidak boleh kosong

            // Aturan validasi untuk Email
            RuleFor(e => e.Email)
               .NotEmpty().WithMessage("Tidak Boleh Kosong") // Tidak boleh kosong, dengan pesan error kustom
               .EmailAddress().WithMessage("Format Email Salah"); // Harus berformat email, dengan pesan error kustom

            // Aturan validasi untuk PhoneNumber
            RuleFor(e => e.PhoneNumber)
               .NotEmpty() // Tidak boleh kosong
               .MinimumLength(10) // Panjang minimal 10 karakter
               .MaximumLength(15); // Panjang maksimal 15 karakter
        }
    }
}