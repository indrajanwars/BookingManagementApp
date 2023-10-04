using Microsoft.AspNetCore.Mvc;
using System.Net;
using API.Contracts;
using API.Models;
using API.Utilities.Handlers;

namespace API.Controllers;

// EducationController adalah sebuah kontroler API yang berfungsi untuk mengelola data pendidikan.
[ApiController]
[Route("api/[controller]")]
public class EducationController : ControllerBase
{
    private readonly IEducationRepository _educationRepository;

    public EducationController(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }

    // Mendapatkan semua data pendidikan.
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _educationRepository.GetAll();

        // Jika tidak ada data pendidikan yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengembalikan data dalam respons OK dengan objek ResponseOKHandler yang berisi data.
        return Ok(new ResponseOKHandler<IEnumerable<Education>>(result));
    }

    // Mendapatkan data pendidikan berdasarkan GUID.
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _educationRepository.GetByGuid(guid);

        // Jika data pendidikan tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id Not Found"
            });
        }

        // Mengembalikan data dalam respons OK dengan objek ResponseOKHandler yang berisi data.
        return Ok(new ResponseOKHandler<Education>(result));
    }

    // Membuat data pendidikan baru.
    [HttpPost]
    public IActionResult Create(Education education)
    {
        try
        {
            // Membuat data pendidikan baru dengan menggunakan objek Education yang diterima.
            var result = _educationRepository.Create(education);

            // Jika gagal membuat data pendidikan, mengembalikan respons BadRequest.
            if (result is null)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to create data"
                });
            }

            // Mengembalikan data yang berhasil dibuat dalam respons OK dengan objek ResponseOKHandler.
            return Ok(new ResponseOKHandler<Education>(result));
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

    // Memperbarui data pendidikan.
    [HttpPut]
    public IActionResult Update(Education education)
    {
        try
        {
            // Memperbarui data pendidikan berdasarkan objek Education yang diterima.
            var result = _educationRepository.Update(education);

            // Jika gagal memperbarui data pendidikan, mengembalikan respons BadRequest.
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

    // Menghapus data pendidikan berdasarkan GUID.
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            // Mengambil data pendidikan berdasarkan GUID yang diberikan.
            var entity = _educationRepository.GetByGuid(guid);

            // Jika data pendidikan tidak ditemukan, mengembalikan respons NotFound.
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Id Not Found"
                });
            }

            // Menghapus data pendidikan dari repositori.
            var result = _educationRepository.Delete(entity);

            // Jika gagal menghapus data pendidikan, mengembalikan respons BadRequest.
            if (!result)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to delete data"
                });
            }

            // Mengembalikan respons sukses dengan pesan.
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