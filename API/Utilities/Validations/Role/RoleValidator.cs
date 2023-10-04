using FluentValidation;
using API.DTOs.Roles;

namespace API.Utilities.Validations.Accounts;

// Validator untuk validasi data saat mengubah peran (RoleDto).
public class RoleValidator : AbstractValidator<RoleDto>
{
    public RoleValidator()
    {
        // Aturan validasi: GUID peran tidak boleh kosong.
        RuleFor(a => a.Guid)
            .NotEmpty()
            .WithMessage("GUID peran tidak boleh kosong");

        // Aturan validasi: Nama peran tidak boleh kosong.
        RuleFor(a => a.Name)
            .NotEmpty()
            .WithMessage("Nama peran harus diisi");
    }
}