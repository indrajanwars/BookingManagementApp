using API.Models;

namespace API.DTOs.AccountRoles;

/* Kelas AccountRoleDto adalah Data Transfer Object (DTO) yang digunakan untuk 
 * mengambil data dari objek AccountRole dan mengembalikannya sebagai respons dari API. */
public class AccountRoleDto
{
    // Properti-properti berikut adalah atribut-atribut yang akan diambil dari objek AccountRole.
    public Guid Guid { get; set; }
    public Guid AccountGuid { get; set; }
    public Guid RoleGuid { get; set; }

    // Operator konversi explicit yang mengubah objek AccountRole menjadi objek AccountRoleDto.
    public static explicit operator AccountRoleDto(AccountRole accountRole)
    {
        // Membuat objek AccountRoleDto baru dengan nilai-nilai yang diambil dari AccountRole.
        return new AccountRoleDto
        {
            Guid = accountRole.Guid,
            AccountGuid = accountRole.AccountGuid,
            RoleGuid = accountRole.RoleGuid
        };
    }

    // Operator konversi implicit yang mengubah objek AccountRoleDto menjadi objek AccountRole.
    public static implicit operator AccountRole(AccountRoleDto accountRoleDto)
    {
        // Membuat objek AccountRole baru dengan nilai-nilai yang diambil dari AccountRoleDto.
        return new AccountRole
        {
            Guid = accountRoleDto.Guid,
            AccountGuid = accountRoleDto.AccountGuid,
            RoleGuid = accountRoleDto.RoleGuid,
            ModifiedDate = DateTime.Now  // Mengatur ModifiedDate sebagai waktu saat ini.
        };
    }
}