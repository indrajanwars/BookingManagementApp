using FluentValidation;
using API.DTOs.Accounts;

namespace API.Utilities.Validations.Accounts
{
    // Validator untuk AccountDto
    public class AccountValidator : AbstractValidator<AccountDto>
    {
        public AccountValidator()
        {
            // Validasi untuk GUID (tidak boleh kosong)
            RuleFor(a => a.EmployeeGuid)
                .NotEmpty();

            // Validasi untuk Password (tidak boleh kosong dan minimal 6 karakter)
            RuleFor(a => a.Password)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("Password minimal 8 karakter.");

            // Validasi untuk OTP (tidak boleh kosong dan harus angka positif)
            RuleFor(a => a.OTP)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("OTP harus angka positif.");

            // Validasi untuk ExpiredTime (tidak boleh kosong dan harus lebih besar dari waktu sekarang)
            RuleFor(a => a.ExpiredTime)
                .NotEmpty()
                .GreaterThan(DateTime.Now)
                .WithMessage("Waktu kedaluwarsa harus lebih besar dari waktu sekarang.");
        }
    }
}