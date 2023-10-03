using API.Contracts;
using API.DTOs.AccountRoles;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// AccountRoleController adalah sebuah kontroler API yang berfungsi untuk mengelola data peran akun.
[ApiController]
[Route("api/[controller]")]
public class AccountRoleController : ControllerBase
{
    private readonly IAccountRoleRepository _accountRoleRepository;

    // Konstruktor untuk AccountRoleController, menerima instance dari IAccountRoleRepository yang akan digunakan untuk mengakses data peran akun.
    public AccountRoleController(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        // Mengambil semua data peran akun dari repositori.
        var result = _accountRoleRepository.GetAll();

        // Jika tidak ada data peran akun yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk AccountRoleDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (AccountRoleDto)x);

        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        // Mengambil data peran akun berdasarkan GUID yang diberikan.
        var result = _accountRoleRepository.GetByGuid(guid);

        // Jika data peran akun tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound("Id Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk AccountRoleDto dan mengembalikannya sebagai respons API.
        return Ok((AccountRoleDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreateAccountRoleDto accountRoleDto)
    {
        // Membuat data peran akun baru berdasarkan data yang diterima dalam permintaan API.
        var result = _accountRoleRepository.Create(accountRoleDto);

        // Jika gagal membuat data peran akun, mengembalikan respons BadRequest.
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        // Mengkonversi hasil ke dalam bentuk AccountRoleDto dan mengembalikannya sebagai respons API.
        return Ok((AccountRoleDto)result);
    }

    [HttpPut]
    public IActionResult Update(AccountRoleDto accountRoleDto)
    {
        // Mengambil data peran akun berdasarkan GUID yang diberikan.
        var entity = _accountRoleRepository.GetByGuid(accountRoleDto.Guid);

        // Jika data peran akun tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Menyalin data dari AccountRoleDto ke objek AccountRole yang akan diperbarui dengan tetap mempertahankan CreatedDate dari entitas yang ada.
        AccountRole toUpdate = accountRoleDto;
        toUpdate.CreatedDate = entity.CreatedDate;

        // Memperbarui data peran akun.
        var result = _accountRoleRepository.Update(toUpdate);

        // Jika gagal memperbarui data peran akun, mengembalikan respons BadRequest.
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
        // Mengambil data peran akun berdasarkan GUID yang diberikan.
        var entity = _accountRoleRepository.GetByGuid(guid);

        // Jika data peran akun tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Menghapus data peran akun.
        var result = _accountRoleRepository.Delete(entity);

        // Jika gagal menghapus data peran akun, mengembalikan respons BadRequest.
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }

        // Mengembalikan respons sukses.
        return Ok("Data Deleted");
    }
}