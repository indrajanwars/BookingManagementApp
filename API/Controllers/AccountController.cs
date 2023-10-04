using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.Contracts;
using API.DTOs.Accounts;
using API.Models;
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
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk AccountDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (AccountDto)x);

        // Mengembalikan respons sukses dengan objek ResponseOKHandler yang berisi data.
        return Ok(new ResponseOKHandler<IEnumerable<AccountDto>>(data));
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        // Mengambil data akun berdasarkan GUID yang diberikan.
        var result = _accountRepository.GetByGuid(guid);

        // Jika data akun tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk AccountDto dan mengembalikannya sebagai respons API.
        return Ok(new ResponseOKHandler<AccountDto>((AccountDto)result));
    }

    [HttpPost]
    public IActionResult Create(CreateAccountDto accountDto)
    {
        try
        {
            // Membuat data akun baru berdasarkan data yang diterima dalam permintaan API.
            var hashedPassword = HashingHandler.HashPassword(accountDto.Password);
            accountDto.Password = hashedPassword;

            var result = _accountRepository.Create(accountDto);

            // Jika gagal membuat data akun, mengembalikan respons BadRequest.
            if (result is null)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to create data"
                });
            }

            // Mengkonversi hasil ke dalam bentuk AccountDto dan mengembalikannya sebagai respons API.
            return Ok(new ResponseOKHandler<AccountDto>((AccountDto)result));
        }
        catch (ExceptionHandler ex)
        {
            // Mengembalikan respons dengan kode status 500 dan pesan error jika terjadi kesalahan.
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to create data",
                Error = ex.Message
            });
        }
    }

    [HttpPut]
    public IActionResult Update(AccountDto accountDto)
    {
        try
        {
            // Mengambil data akun berdasarkan GUID yang diberikan.
            var entity = _accountRepository.GetByGuid(accountDto.Guid);

            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
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
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to update data"
                });
            }

            // Mengembalikan respons sukses dengan pesan.
            return Ok(new ResponseOKHandler<string>("Data Updated"));
        }
        catch (ExceptionHandler ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to create data",
                Error = ex.Message
            });
        }
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            var entity = _accountRepository.GetByGuid(guid);

            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            // Menghapus data akun dari repositori.
            _accountRepository.Delete(entity);

            return Ok(new ResponseOKHandler<string>("Data Deleted"));
        }
        catch (ExceptionHandler ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to create data",
                Error = ex.Message
            });
        }
    }
}