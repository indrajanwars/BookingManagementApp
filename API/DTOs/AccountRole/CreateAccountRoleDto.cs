using API.Models;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek AccountRole berdasarkan data tersebut. */
namespace API.DTOs.AccountRoles;

public class CreateAccountRoleDto
{
    public Guid AccountGuid { get; set; }
    public Guid RoleGuid { get; set; }

    public static implicit operator AccountRole(CreateAccountRoleDto createAccountRoleDto)
    {
        return new AccountRole
        {
            AccountGuid = createAccountRoleDto.AccountGuid,
            RoleGuid = createAccountRoleDto.RoleGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}