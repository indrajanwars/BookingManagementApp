using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using API.DTOs.Accounts;
using API.Utilities.Handlers;

namespace API.Controllers;

// AccountController adalah sebuah kontroler API yang berfungsi untuk mengelola data akun (account).
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;

    // Konstruktor untuk AccountController, menerima instance dari IAccountRepository yang akan digunakan untuk mengakses data akun.
    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        // Mengambil semua data akun dari repositori.
        var result = _accountRepository.GetAll();

        // Jika tidak ada data akun yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk AccountDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (AccountDto)x);

        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        // Mengambil data akun berdasarkan GUID yang diberikan.
        var result = _accountRepository.GetByGuid(guid);

        // Jika data akun tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound("Id Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk AccountDto dan mengembalikannya sebagai respons API.
        return Ok((AccountDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreateAccountDto accountDto)
    {
        // Membuat data akun baru berdasarkan data yang diterima dalam permintaan API.
        var hashedPassword = HashingHandler.HashPassword(accountDto.Password);
        accountDto.Password = hashedPassword;

        var result = _accountRepository.Create(accountDto);

        // Jika gagal membuat data akun, mengembalikan respons BadRequest.
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        // Mengkonversi hasil ke dalam bentuk AccountDto dan mengembalikannya sebagai respons API.
        return Ok((AccountDto)result);
    }

    [HttpPut]
    public IActionResult Update(AccountDto accountDto)
    {
        // Mengambil data akun berdasarkan GUID yang diberikan.
        var entity = _accountRepository.GetByGuid(accountDto.Guid);

        // Jika data akun tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Memeriksa apakah kata sandi berubah.
        if (!string.IsNullOrEmpty(accountDto.Password))
        {
            // Meng-hash kata sandi baru sebelum menyimpannya ke database.
            string hashedPassword = HashingHandler.HashPassword(accountDto.Password);


            // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
            Account toUpdate = accountDto;
            toUpdate.CreatedDate = entity.CreatedDate;
            // Mengganti kata sandi asli dengan yang di-hash pada objek entity.
            entity.Password = hashedPassword;
        }

        // Memperbarui data akun.
        var result = _accountRepository.Update(entity);

        // Jika gagal memperbarui data akun, mengembalikan respons BadRequest.
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
        // Mengambil data akun berdasarkan GUID yang diberikan.
        var entity = _accountRepository.GetByGuid(guid);

        // Jika data akun tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Menghapus data akun.
        var result = _accountRepository.Delete(entity);

        // Jika gagal menghapus data akun, mengembalikan respons BadRequest.
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }

        // Mengembalikan respons sukses.
        return Ok("Data Deleted");
    }
}