using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.Contracts;
using API.DTOs.Roles;
using API.Models;
using API.Utilities.Handlers;

namespace API.Controllers;

// RoleController adalah sebuah kontroler API yang berfungsi untuk mengelola data peran (roles).
[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;

    public RoleController(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    // Mendapatkan semua data peran.
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _roleRepository.GetAll();

        // Jika tidak ada data peran yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk RoleDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (RoleDto)x);

        return Ok(new ResponseOKHandler<IEnumerable<RoleDto>>(data));
    }

    // Mendapatkan data peran berdasarkan GUID.
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _roleRepository.GetByGuid(guid);

        // Jika data peran tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk RoleDto dan mengembalikannya sebagai respons API.
        return Ok(new ResponseOKHandler<RoleDto>((RoleDto)result));
    }

    // Membuat data peran baru.
    [HttpPost]
    public IActionResult Create(CreateRoleDto roleDto)
    {
        try
        {
            // Membuat data peran baru berdasarkan data yang diterima dalam permintaan API.
            var result = _roleRepository.Create(roleDto);

            // Jika gagal membuat data peran, mengembalikan respons BadRequest.
            if (result is null)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to create data"
                });
            }

            // Mengkonversi hasil ke dalam bentuk RoleDto dan mengembalikannya sebagai respons API.
            return Ok(new ResponseOKHandler<RoleDto>((RoleDto)result));
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

    // Memperbarui data peran.
    [HttpPut]
    public IActionResult Update(RoleDto roleDto)
    {
        try
        {
            // Mengambil data peran berdasarkan GUID yang diberikan.
            var entity = _roleRepository.GetByGuid(roleDto.Guid);

            // Jika data peran tidak ditemukan, mengembalikan respons NotFound.
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id Not Found"
                });
            }

            // Menyalin data dari RoleDto ke objek Role yang akan diperbarui dengan tetap mempertahankan CreatedDate dari entitas yang ada.
            Role toUpdate = roleDto;
            toUpdate.CreatedDate = entity.CreatedDate;

            // Memperbarui data peran.
            var result = _roleRepository.Update(toUpdate);

            // Jika gagal memperbarui data peran, mengembalikan respons BadRequest.
            if (!result)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to update data"
                });
            }

            // Mengembalikan respons sukses dengan pesan "Data Updated".
            return Ok(new ResponseOKHandler<string>("Data Updated"));
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

    // Menghapus data peran berdasarkan GUID.
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            // Mengambil data peran berdasarkan GUID yang diberikan.
            var entity = _roleRepository.GetByGuid(guid);

            // Jika data peran tidak ditemukan, mengembalikan respons NotFound.
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id Not Found"
                });
            }

            // Menghapus data peran dari repositori.
            var result = _roleRepository.Delete(entity);

            // Jika gagal menghapus data peran, mengembalikan respons BadRequest.
            if (!result)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to delete data"
                });
            }

            // Mengembalikan respons sukses dengan pesan "Data Deleted".
            return Ok(new ResponseOKHandler<string>("Data Deleted"));
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
}