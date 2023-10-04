using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Employees;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek Employee berdasarkan data tersebut. */
public class CreateEmployeeDto
{
    // Properti-properti berikut mewakili atribut-atribut yang akan diterima dari permintaan API.
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public GenderLevel Gender { get; set; }
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    // Operator konversi implicit yang mengubah objek CreateEmployeeDto menjadi objek Employee.
    public static implicit operator Employee(CreateEmployeeDto createEmployeeDto)
    {
        // Membuat objek Employee baru dengan nilai-nilai yang diambil dari CreateEmployeeDto.
        return new Employee
        {
            Guid = new Guid(),                 // Membuat GUID baru.
            FirstName = createEmployeeDto.FirstName,
            LastName = createEmployeeDto.LastName,
            BirthDate = createEmployeeDto.BirthDate,
            Gender = createEmployeeDto.Gender,
            HiringDate = createEmployeeDto.HiringDate,
            Email = createEmployeeDto.Email,
            PhoneNumber = createEmployeeDto.PhoneNumber,
            CreatedDate = DateTime.Now,        // Mengatur CreatedDate sebagai waktu saat ini.
            ModifiedDate = DateTime.Now        // Mengatur ModifiedDate sebagai waktu saat ini.
        };
    }
}