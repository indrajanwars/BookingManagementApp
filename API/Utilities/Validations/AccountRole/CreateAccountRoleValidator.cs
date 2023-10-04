using FluentValidation;
using API.DTOs.AccountRoles;

namespace API.Utilities.Validations.AccountRoles;

// Validator untuk membuat AccountRole baru.
public class CreateAccountRoleValidator : AbstractValidator<CreateAccountRoleDto>
{
    public CreateAccountRoleValidator()
    {
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