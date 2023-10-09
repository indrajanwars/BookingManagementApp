using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.Contracts;
using API.DTOs.AccountRoles;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

// AccountRoleController adalah sebuah kontroler API yang berfungsi untuk mengelola data peran akun.
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "superAdmin")]
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
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk AccountRoleDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (AccountRoleDto)x);

        return Ok(new ResponseOKHandler<IEnumerable<AccountRoleDto>>(data));
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        // Mengambil data peran akun berdasarkan GUID yang diberikan.
        var result = _accountRoleRepository.GetByGuid(guid);

        // Jika data peran akun tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk AccountRoleDto dan mengembalikannya sebagai respons API.
        return Ok(new ResponseOKHandler<AccountRoleDto>((AccountRoleDto)result));
    }

    [HttpPost]
    public IActionResult Create(CreateAccountRoleDto accountRoleDto)
    {
        try
        {
            // Membuat data peran akun baru berdasarkan data yang diterima dalam permintaan API.
            var result = _accountRoleRepository.Create(accountRoleDto);

            // Jika gagal membuat data peran akun, mengembalikan respons BadRequest.
            if (result is null)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to create data"
                });
            }

            // Mengkonversi hasil ke dalam bentuk AccountRoleDto dan mengembalikannya sebagai respons API.
            return Ok(new ResponseOKHandler<AccountRoleDto>((AccountRoleDto)result));
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
    public IActionResult Update(AccountRoleDto accountRoleDto)
    {
        try
        {
            // Mengambil data peran akun berdasarkan GUID yang diberikan.
            var entity = _accountRoleRepository.GetByGuid(accountRoleDto.Guid);

            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            // Menyalin data dari AccountRoleDto ke objek AccountRole yang akan diperbarui dengan tetap mempertahankan CreatedDate dari entitas yang ada.
            AccountRole toUpdate = accountRoleDto;
            toUpdate.CreatedDate = entity.CreatedDate;

            // Memperbarui data peran akun.
            var result = _accountRoleRepository.Update(toUpdate);

            // Jika gagal memperbarui data peran akun, mengembalikan respons BadRequest.
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
            var entity = _accountRoleRepository.GetByGuid(guid);

            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            // Menghapus data peran akun dari repositori.
            _accountRoleRepository.Delete(entity);

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