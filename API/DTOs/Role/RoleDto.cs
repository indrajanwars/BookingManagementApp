using API.Models;

namespace API.DTOs.Roles;

/* Kelas RoleDto adalah Data Transfer Object (DTO) yang digunakan untuk 
 * mengambil data dari objek Role dan mengembalikannya sebagai respons dari API. */
public class RoleDto
{
    // Properti-properti berikut adalah atribut-atribut yang akan diambil dari objek Role.
    public Guid Guid { get; set; }
    public string Name { get; set; }

    // Operator konversi explicit yang mengubah objek Role menjadi objek RoleDto.
    public static explicit operator RoleDto(Role role)
    {
        // Membuat objek RoleDto baru dengan nilai-nilai yang diambil dari Role.
        return new RoleDto
        {
            Guid = role.Guid,
            Name = role.Name
        };
    }

    // Operator konversi implicit yang mengubah objek RoleDto menjadi objek Role.
    public static implicit operator Role(RoleDto roleDto)
    {
        // Membuat objek Role baru dengan nilai-nilai yang diambil dari RoleDto.
        return new Role
        {
            Guid = roleDto.Guid,
            Name = roleDto.Name,
            ModifiedDate = DateTime.Now  // Mengatur ModifiedDate sebagai waktu saat ini.
        };
    }
}