using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.Contracts;
using API.DTOs.Universities;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

// Kontroler API yang berfungsi untuk mengelola data universitas.
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UniversityController : ControllerBase
{
    private readonly IUniversityRepository _universityRepository;

    public UniversityController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    // Mendapatkan semua data universitas.
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _universityRepository.GetAll();

        // Jika tidak ada data universitas yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk UniversityDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (UniversityDto)x);

        return Ok(new ResponseOKHandler<IEnumerable<UniversityDto>>(data));
    }

    // Mendapatkan data universitas berdasarkan GUID.
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _universityRepository.GetByGuid(guid);

        // Jika data universitas tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk UniversityDto dan mengembalikannya sebagai respons API.
        return Ok(new ResponseOKHandler<UniversityDto>((UniversityDto)result));
    }

    // Membuat data universitas baru.
    [HttpPost]
    public IActionResult Create(CreateUniversityDto universityDto)
    {
        try
        {
            // Membuat data universitas baru berdasarkan data yang diterima dalam permintaan API.
            var result = _universityRepository.Create(universityDto);

            // Jika gagal membuat data universitas, mengembalikan respons BadRequest.
            if (result is null)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to create data"
                });
            }

            // Mengkonversi hasil ke dalam bentuk UniversityDto dan mengembalikannya sebagai respons API.
            return Ok(new ResponseOKHandler<UniversityDto>((UniversityDto)result));
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

    // Memperbarui data universitas.
    [HttpPut]
    public IActionResult Update(UniversityDto universityDto)
    {
        try
        {
            // Mengambil data universitas berdasarkan GUID yang diberikan.
            var entity = _universityRepository.GetByGuid(universityDto.Guid);

            // Jika data universitas tidak ditemukan, mengembalikan respons NotFound.
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id Not Found"
                });
            }

            // Menyalin data dari UniversityDto ke objek University yang akan diperbarui dengan tetap mempertahankan CreatedDate dari entitas yang ada.
            University toUpdate = universityDto;
            toUpdate.CreatedDate = entity.CreatedDate;

            // Memperbarui data universitas.
            var result = _universityRepository.Update(toUpdate);

            // Jika gagal memperbarui data universitas, mengembalikan respons BadRequest.
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

    // Menghapus data universitas berdasarkan GUID.
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            // Mengambil data universitas berdasarkan GUID yang diberikan.
            var entity = _universityRepository.GetByGuid(guid);

            // Jika data universitas tidak ditemukan, mengembalikan respons NotFound.
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id Not Found"
                });
            }

            // Menghapus data universitas dari repositori.
            var result = _universityRepository.Delete(entity);

            // Jika gagal menghapus data universitas, mengembalikan respons BadRequest.
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