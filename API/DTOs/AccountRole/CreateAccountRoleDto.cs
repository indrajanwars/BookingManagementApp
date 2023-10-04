using API.Models;

namespace API.DTOs.AccountRoles;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek AccountRole berdasarkan data tersebut. */
public class CreateAccountRoleDto
{
    // Properti-properti berikut mewakili atribut-atribut yang akan diterima dari permintaan API.
    public Guid AccountGuid { get; set; }
    public Guid RoleGuid { get; set; }

    // Operator konversi implicit yang mengubah objek CreateAccountRoleDto menjadi objek AccountRole.
    public static implicit operator AccountRole(CreateAccountRoleDto createAccountRoleDto)
    {
        // Membuat objek AccountRole baru dengan nilai-nilai yang diambil dari CreateAccountRoleDto.
        return new AccountRole
        {
            AccountGuid = createAccountRoleDto.AccountGuid,
            RoleGuid = createAccountRoleDto.RoleGuid,
            CreatedDate = DateTime.Now,        // Mengatur CreatedDate sebagai waktu saat ini.
            ModifiedDate = DateTime.Now        // Mengatur ModifiedDate sebagai waktu saat ini.
        };
    }
}