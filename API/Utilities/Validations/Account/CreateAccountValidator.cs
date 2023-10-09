using FluentValidation;
using API.DTOs.Accounts;

namespace API.Utilities.Validations.Accounts
{
    // Validator untuk CreateAccountDto
    public class CreateAccountValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountValidator()
        {
            // Validasi untuk GUID (tidak boleh kosong)
            RuleFor(a => a.EmployeeGuid)
                .NotEmpty().WithMessage("Guid Tidak Boleh Kosong");

            // Validasi untuk Password (tidak boleh kosong dan minimal 8 karakter)
            RuleFor(a => a.Password)
                .NotEmpty().WithMessage("Password tidak boleh kosong")
                .MinimumLength(8).WithMessage("Password minimal terdiri dari 8 karakter")
                .MaximumLength(20).WithMessage("Password maksimal terdiri dari 20 karakter")
                .Matches("[A-Z]").WithMessage("Password harus memiliki minimal 1 huruf KAPITAL")
                .Matches("[a-z]").WithMessage("Password harus memiliki minimal 1 huruf kecil")
                .Matches("[0-9]").WithMessage("Password harus memiliki minimal 1 angka");

            // Validasi untuk OTP (tidak boleh kosong dan harus angka positif)
            //RuleFor(a => a.OTP)
            //    .NotEmpty()
            //    .GreaterThan(0)
            //    .WithMessage("OTP harus angka positif.");

            // Validasi untuk ExpiredTime (tidak boleh kosong dan harus lebih besar dari waktu sekarang)
            //RuleFor(a => a.ExpiredTime)
            //    .NotEmpty()
            //    .GreaterThan(DateTime.Now)
            //    .WithMessage("Waktu kedaluwarsa harus lebih besar dari waktu sekarang.");
        }
    }
}