using API.Contracts;
using API.DTOs.Rooms;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// RoomController adalah sebuah kontroler API yang berfungsi untuk mengelola data ruangan.

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;

    // Konstruktor untuk RoomController, menerima instance dari IRoomRepository yang akan digunakan untuk mengakses data ruangan.
    public RoomController(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        // Mengambil semua data ruangan dari repositori.
        var result = _roomRepository.GetAll();

        // Jika tidak ada data ruangan yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk RoomDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (RoomDto)x);

        return Ok(data);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        // Mengambil data ruangan berdasarkan GUID yang diberikan.
        var result = _roomRepository.GetByGuid(guid);

        // Jika data ruangan tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound("Id Not Found");
        }

        // Mengkonversi hasil ke dalam bentuk RoomDto dan mengembalikannya sebagai respons API.
        return Ok((RoomDto)result);
    }

    [HttpPost]
    public IActionResult Create(CreateRoomDto roomDto)
    {
        // Membuat data ruangan baru berdasarkan data yang diterima dalam permintaan API.
        var result = _roomRepository.Create(roomDto);

        // Jika gagal membuat data ruangan, mengembalikan respons BadRequest.
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        // Mengkonversi hasil ke dalam bentuk RoomDto dan mengembalikannya sebagai respons API.
        return Ok((RoomDto)result);
    }

    [HttpPut]
    public IActionResult Update(RoomDto roomDto)
    {
        // Mengambil data ruangan berdasarkan GUID yang diberikan.
        var entity = _roomRepository.GetByGuid(roomDto.Guid);

        // Jika data ruangan tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Menyalin data dari RoomDto ke objek Room yang akan diperbarui dengan tetap mempertahankan CreatedDate dari entitas yang ada.
        Room toUpdate = roomDto;
        toUpdate.CreatedDate = entity.CreatedDate;

        // Memperbarui data ruangan.
        var result = _roomRepository.Update(toUpdate);

        // Jika gagal memperbarui data ruangan, mengembalikan respons BadRequest.
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
        // Mengambil data ruangan berdasarkan GUID yang diberikan.
        var entity = _roomRepository.GetByGuid(guid);

        // Jika data ruangan tidak ditemukan, mengembalikan respons NotFound.
        if (entity is null)
        {
            return NotFound("Id Not Found");
        }

        // Menghapus data ruangan.
        var result = _roomRepository.Delete(entity);

        // Jika gagal menghapus data ruangan, mengembalikan respons BadRequest.
        if (!result)
        {
            return BadRequest("Failed to delete data");
        }

        // Mengembalikan respons sukses.
        return Ok("Data Deleted");
    }
}