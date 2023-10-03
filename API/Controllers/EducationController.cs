using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// Mendeklarasikan kelas EducationController adalah turunan dari ControllerBase.
[ApiController]
[Route("api/[controller]")]
public class EducationController : ControllerBase
{
    private readonly IEducationRepository _educationRepository;

    // Konstruktor kelas EducationController menerima instance IEducationRepository sebagai dependensi.
    public EducationController(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }

    // HTTP GET: Mengambil semua data Education dari repository.
    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _educationRepository.GetAll();
        if (!result.Any())
        {
            // Mengembalikan respons NotFound jika tidak ada data yang ditemukan.
            return NotFound("Data Not Found");
        }

        // Mengembalikan data dalam respons OK jika berhasil.
        return Ok(result);
    }

    // HTTP GET: Mengambil data Education berdasarkan GUID yang diberikan.
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _educationRepository.GetByGuid(guid);
        if (result is null)
        {
            // Mengembalikan respons NotFound jika GUID tidak ditemukan.
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    // HTTP POST: Membuat data Education baru dengan menggunakan objek Education yang diterima.
    [HttpPost]
    public IActionResult Create(Education education)
    {
        var result = _educationRepository.Create(education);
        if (result is null)
        {
            // Mengembalikan respons BadRequest jika gagal membuat data baru.
            return BadRequest("Failed to create data");
        }

        // Mengembalikan data yang berhasil dibuat dalam respons OK.
        return Ok(result);
    }

    // HTTP PUT: Memperbarui data Education berdasarkan objek Education yang diterima.
    [HttpPut]
    public IActionResult Update(Education education)
    {
        var result = _educationRepository.Update(education);
        if (!result)
        {
            // Mengembalikan respons BadRequest jika gagal memperbarui data.
            return BadRequest("Failed to update data");
        }

        // Mengembalikan respons OK dengan pesan "Data Updated" jika berhasil.
        return Ok("Data Updated");
    }

    // HTTP DELETE: Menghapus data Education berdasarkan GUID yang diberikan.
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var entity = _educationRepository.GetByGuid(guid);
        if (entity is null)
        {
            // Mengembalikan respons NotFound jika GUID tidak ditemukan.
            return NotFound("Id Not Found");
        }

        var result = _educationRepository.Delete(entity);
        if (!result)
        {
            // Mengembalikan respons BadRequest jika gagal menghapus data.
            return BadRequest("Failed to delete data");
        }

        // Mengembalikan respons OK dengan pesan "Data Deleted" jika berhasil.
        return Ok("Data Deleted");
    }
}