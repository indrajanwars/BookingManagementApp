using API.Contracts;
using API.DTOs.Employees;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// EmployeeController adalah sebuah kontroler API yang berfungsi untuk mengelola data karyawan.

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    // Konstruktor untuk EmployeeController, menerima instance dari IEmployeeRepository yang akan digunakan untuk mengakses data karyawan.

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        // Mengambil semua data karyawan dari repositori.
        var result = _employeeRepository.GetAll();

        // Jika tidak ada data karyawan yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk EmployeeDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (EmployeeDto)x);

        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        // Mengambil data karyawan berdasarkan GUID yang diberikan.
        var result = _employeeRepository.GetByGuid(guid);

        // Jika data karyawan tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound("Id Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk EmployeeDto dan mengembalikannya sebagai respons API.
        return Ok((EmployeeDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreateEmployeeDto employeeDto)
    {
        // Membuat data karyawan baru berdasarkan data yang diterima dalam permintaan API.
        var result = _employeeRepository.Create(employeeDto);

        // Jika gagal membuat data karyawan, mengembalikan respons BadRequest.
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        // Mengkonversi hasil ke dalam bentuk EmployeeDto dan mengembalikannya sebagai respons API.
        return Ok((EmployeeDto)result);
    }

    [HttpPut]
    public IActionResult Update(EmployeeDto employeeDto)
    {
        // Mengambil data karyawan berdasarkan GUID yang diberikan.
        var entity = _employeeRepository.GetByGuid(employeeDto.Guid);

        // Jika data karyawan tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Menyalin data dari EmployeeDto ke objek Employee yang akan diperbarui dengan tetap mempertahankan CreatedDate dari entitas yang ada.
        Employee toUpdate = employeeDto;
        toUpdate.CreatedDate = entity.CreatedDate;

        // Memperbarui data karyawan.
        var result = _employeeRepository.Update(toUpdate);

        // Jika gagal memperbarui data karyawan, mengembalikan respons BadRequest.
        if (!result)
        {
            return BadRequest("Failed to update data");
        }

        // Mengembalikan respons sukses.
        return Ok("Data Updated");
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        // Mengambil data karyawan berdasarkan GUID yang diberikan.
        var entity = _employeeRepository.GetByGuid(guid);

        // Jika data karyawan tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Menghapus data karyawan.
        var result = _employeeRepository.Delete(entity);

        // Jika gagal menghapus data karyawan, mengembalikan respons BadRequest.
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }

        // Mengembalikan respons sukses.
        return Ok("Data Deleted");
    }
}