using System.Net;
using API.Contracts;
using API.DTOs.Educations;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// EducationController adalah sebuah kontroler API yang berfungsi untuk mengelola data pendidikan.
[ApiController]
[Route("api/[controller]")]
[Authorize]
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

        // Mengkonversi hasil ke dalam bentuk EducationDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (EducationDto)x);

        // Mengembalikan data dalam respons OK dengan objek ResponseOKHandler yang berisi data.
        return Ok(new ResponseOKHandler<IEnumerable<EducationDto>>(data));
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
        return Ok(new ResponseOKHandler<EducationDto>((EducationDto)result));
    }

    // Membuat data pendidikan baru.
    [HttpPost]
    public IActionResult Create(CreateEducationDto educationDto)
    {
        try
        {
            // Membuat data pendidikan baru dengan menggunakan objek Education yang diterima.
            var result = _educationRepository.Create(educationDto);

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
            return Ok(new ResponseOKHandler<EducationDto>((EducationDto)result));
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
    public IActionResult Update(EducationDto educationDto)
    {
        try
        {
            // Memeriksa apakah entitas Education yang akan diperbarui ada dalam database.
            var existingEducation = _educationRepository.GetByGuid(educationDto.Guid);

            if (existingEducation == null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Education not found"
                });
            }

            // Menyimpan tanggal pembuatan entitas Education sebelum pembaruan.
            Education toUpdate = educationDto;
            toUpdate.CreatedDate = existingEducation.CreatedDate;

            // Memanggil metode Update dari _educationRepository.
            var result = _educationRepository.Update(toUpdate);

            // Memeriksa apakah pembaruan data berhasil atau gagal.
            if (!result)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to update data"
                });
            }

            // Mengembalikan pesan sukses dalam respons OK.
            return Ok(new ResponseOKHandler<string>("Data Updated"));
        }
        catch (ExceptionHandler ex)
        {
            // Jika terjadi pengecualian saat mengupdate data, akan mengembalikan respons kesalahan dengan pesan pengecualian.
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to update data",
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
            // Memanggil metode GetByGuid dari _educationRepository untuk mendapatkan entitas yang akan dihapus.
            var existingEducation = _educationRepository.GetByGuid(guid);

            // Memeriksa apakah entitas yang akan dihapus ada dalam database.
            if (existingEducation is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Education not found"
                });
            }

            // Memanggil metode Delete dari _educationRepository.
            var deleted = _educationRepository.Delete(existingEducation);

            // Memeriksa apakah penghapusan data berhasil atau gagal.
            if (!deleted)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to delete education"
                });
            }

            // Mengembalikan kode status 204 (No Content) untuk sukses penghapusan tanpa respons.
            return NoContent();
        }
        catch (ExceptionHandler ex)
        {
            // Jika terjadi pengecualian saat menghapus data, akan mengembalikan respons kesalahan dengan pesan pengecualian.
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to delete education",
                Error = ex.Message
            });
        }
    }
}