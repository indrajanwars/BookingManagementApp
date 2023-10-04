using FluentValidation;
using API.DTOs.Roles;

namespace API.Utilities.Validations.Accounts;

// Validator untuk validasi data saat membuat peran (CreateRoleDto).
public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleValidator()
    {
        // Aturan validasi: Nama peran tidak boleh kosong.
        RuleFor(a => a.Name)
            .NotEmpty()
            .WithMessage("Nama peran harus diisi");

        // Aturan validasi: Deskripsi peran tidak boleh kosong.
        RuleFor(a => a.Description)
            .NotEmpty()
            .WithMessage("Deskripsi peran harus diisi");
    }
}