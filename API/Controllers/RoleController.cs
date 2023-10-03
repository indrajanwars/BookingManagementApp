using API.Contracts;
using API.DTOs.Roles;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// RoleController adalah sebuah kontroler API yang berfungsi untuk mengelola data peran (roles).

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;

    // Konstruktor untuk RoleController, menerima instance dari IRoleRepository yang akan digunakan untuk mengakses data peran.
    public RoleController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        // Mengambil semua data peran dari repositori.
        var result = _roleRepository.GetAll();

        // Jika tidak ada data peran yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk RoleDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (RoleDto)x);

        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        // Mengambil data peran berdasarkan GUID yang diberikan.
        var result = _roleRepository.GetByGuid(guid);

        // Jika data peran tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound("Id Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk RoleDto dan mengembalikannya sebagai respons API.
        return Ok((RoleDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreateRoleDto roleDto)
    {
        // Membuat data peran baru berdasarkan data yang diterima dalam permintaan API.
        var result = _roleRepository.Create(roleDto);

        // Jika gagal membuat data peran, mengembalikan respons BadRequest.
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        // Mengkonversi hasil ke dalam bentuk RoleDto dan mengembalikannya sebagai respons API.
        return Ok((RoleDto)result);
    }

    [HttpPut]
    public IActionResult Update(RoleDto roleDto)
    {
        // Mengambil data peran berdasarkan GUID yang diberikan.
        var entity = _roleRepository.GetByGuid(roleDto.Guid);

        // Jika data peran tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Menyalin data dari RoleDto ke objek Role yang akan diperbarui dengan tetap mempertahankan CreatedDate dari entitas yang ada.
        Role toUpdate = roleDto;
        toUpdate.CreatedDate = entity.CreatedDate;

        // Memperbarui data peran.
        var result = _roleRepository.Update(toUpdate);

        // Jika gagal memperbarui data peran, mengembalikan respons BadRequest.
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
        // Mengambil data peran berdasarkan GUID yang diberikan.
        var entity = _roleRepository.GetByGuid(guid);

        // Jika data peran tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Menghapus data peran.
        var result = _roleRepository.Delete(entity);

        // Jika gagal menghapus data peran, mengembalikan respons BadRequest.
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }

        // Mengembalikan respons sukses.
        return Ok("Data Deleted");
    }
}