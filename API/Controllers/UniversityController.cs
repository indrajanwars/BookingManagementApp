using API.Contracts;
using API.DTOs.Universities;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// UniversityController adalah sebuah kontroler API yang berfungsi untuk mengelola data universitas.
[ApiController]
[Route("api/[controller]")]
public class UniversityController : ControllerBase
{
    private readonly IUniversityRepository _universityRepository;

    // Konstruktor untuk UniversityController, menerima instance dari IUniversityRepository yang akan digunakan untuk mengakses data universitas.
    public UniversityController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        // Mengambil semua data universitas dari repositori.
        var result = _universityRepository.GetAll();

        // Jika tidak ada data universitas yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk UniversityDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (UniversityDto)x);

        /* Alternatif cara konversi menggunakan loop:
        var universityDto = new List<UniversityDto>();
        foreach (var university in result)
        {
            universityDto.Add((UniversityDto) university);
        }*/

        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        // Mengambil data universitas berdasarkan GUID yang diberikan.
        var result = _universityRepository.GetByGuid(guid);

        // Jika data universitas tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound("Id Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk UniversityDto dan mengembalikannya sebagai respons API.
        return Ok((UniversityDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreateUniversityDto universityDto)
    {
        // Membuat data universitas baru berdasarkan data yang diterima dalam permintaan API.
        var result = _universityRepository.Create(universityDto);

        // Jika gagal membuat data universitas, mengembalikan respons BadRequest.
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        // Mengkonversi hasil ke dalam bentuk UniversityDto dan mengembalikannya sebagai respons API.
        return Ok((UniversityDto)result);
    }

    [HttpPut]
    public IActionResult Update(UniversityDto universityDto)
    {
        // Mengambil data universitas berdasarkan GUID yang diberikan.
        var entity = _universityRepository.GetByGuid(universityDto.Guid);

        // Jika data universitas tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Menyalin data dari UniversityDto ke objek University yang akan diperbarui dengan tetap mempertahankan CreatedDate dari entitas yang ada.
        University toUpdate = universityDto;
        toUpdate.CreatedDate = entity.CreatedDate;

        // Memperbarui data universitas.
        var result = _universityRepository.Update(toUpdate);

        // Jika gagal memperbarui data universitas, mengembalikan respons BadRequest.
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
        // Mengambil data universitas berdasarkan GUID yang diberikan.
        var entity = _universityRepository.GetByGuid(guid);

        // Jika data universitas tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Menghapus data universitas.
        var result = _universityRepository.Delete(entity);

        // Jika gagal menghapus data universitas, mengembalikan respons BadRequest.
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }

        // Mengembalikan respons sukses.
        return Ok("Data Deleted");
    }
}