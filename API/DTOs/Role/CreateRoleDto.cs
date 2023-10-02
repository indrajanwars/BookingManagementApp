using API.Models;

namespace API.DTOs.Roles;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek Role berdasarkan data tersebut. */
public class CreateRoleDto
{
    public string Name { get; set; }
    public string Description { get; set; }

    public static implicit operator Role(CreateRoleDto createRoleDto)
    {
        return new Role
        {
            Name = createRoleDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}
