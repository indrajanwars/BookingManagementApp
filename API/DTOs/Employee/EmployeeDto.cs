using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Employees;

/* Kelas EmployeeDto adalah Data Transfer Object (DTO) yang digunakan untuk 
 * mengambil data dari objek Employee dan mengembalikannya sebagai respons dari API. */
public class EmployeeDto
{
    // Properti-properti berikut adalah atribut-atribut yang akan diambil dari objek Employee.
    public Guid Guid { get; set; }
    public string Nik { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public GenderLevel Gender { get; set; }
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    // Operator konversi implicit yang mengubah objek EmployeeDto menjadi objek Employee.
    public static implicit operator Employee(EmployeeDto employeeDto)
    {
        // Membuat objek Employee baru dengan nilai-nilai yang diambil dari EmployeeDto.
        return new Employee
        {
            Guid = employeeDto.Guid,
            Nik = employeeDto.Nik,
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            BirthDate = employeeDto.BirthDate,
            Gender = employeeDto.Gender,
            HiringDate = employeeDto.HiringDate,
            Email = employeeDto.Email,
            PhoneNumber = employeeDto.PhoneNumber,
            ModifiedDate = DateTime.Now  // Mengatur ModifiedDate sebagai waktu saat ini.
        };
    }

    // Operator konversi explicit yang mengubah objek Employee menjadi objek EmployeeDto.
    public static explicit operator EmployeeDto(Employee employee)
    {
        // Membuat objek EmployeeDto baru dengan nilai-nilai yang diambil dari Employee.
        return new EmployeeDto
        {
            Guid = employee.Guid,
            Nik = employee.Nik,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber
        };
    }
}