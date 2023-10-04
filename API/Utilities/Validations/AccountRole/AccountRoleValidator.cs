using FluentValidation;
using API.DTOs.AccountRoles;

namespace API.Utilities.Validations.AccountRoles;

// Validator untuk mengupdate atau menghapus AccountRole.
public class AccountRoleValidator : AbstractValidator<AccountRoleDto>
{
    public AccountRoleValidator()
    {
        // Aturan validasi untuk Guid: Tidak boleh kosong.
        RuleFor(a => a.Guid)
            .NotEmpty()
            .WithMessage("Guid tidak boleh kosong.");

        // Aturan validasi untuk AccountGuid: Tidak boleh kosong dan harus memiliki nilai yang valid (tidak sama dengan Guid.Empty).
        RuleFor(a => a.AccountGuid)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("AccountGuid tidak boleh kosong.");

        // Aturan validasi untuk RoleGuid: Tidak boleh kosong dan harus memiliki nilai yang valid (tidak sama dengan Guid.Empty).
        RuleFor(a => a.RoleGuid)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("RoleGuid tidak boleh kosong.");
    }
}