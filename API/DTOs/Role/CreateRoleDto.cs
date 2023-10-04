using API.Models;

namespace API.DTOs.Roles;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek Role berdasarkan data tersebut. */
public class CreateRoleDto
{
    // Properti-properti berikut mewakili atribut-atribut yang akan diterima dari permintaan API.
    public string Name { get; set; }
    public string Description { get; set; }

    // Operator konversi implicit yang mengubah objek CreateRoleDto menjadi objek Role.
    public static implicit operator Role(CreateRoleDto createRoleDto)
    {
        // Membuat objek Role baru dengan nilai-nilai yang diambil dari CreateRoleDto.
        return new Role
        {
            Name = createRoleDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}