using API.Models;

namespace API.DTOs.Rooms;

/* Kelas RoomDto adalah Data Transfer Object (DTO) yang digunakan untuk 
 * mengambil data dari objek Room dan mengembalikannya sebagai respons dari API. */
public class RoomDto
{
    // Properti-properti berikut adalah atribut-atribut yang akan diambil dari objek Room.
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; }

    // Operator konversi explicit yang mengubah objek Room menjadi objek RoomDto.
    public static explicit operator RoomDto(Room room)
    {
        // Membuat objek RoomDto baru dengan nilai-nilai yang diambil dari Room.
        return new RoomDto
        {
            Guid = room.Guid,
            Name = room.Name,
            Floor = room.Floor,
            Capacity = room.Capacity
        };
    }

    // Operator konversi implicit yang mengubah objek RoomDto menjadi objek Room.
    public static implicit operator Room(RoomDto roomDto)
    {
        // Membuat objek Room baru dengan nilai-nilai yang diambil dari RoomDto.
        return new Room
        {
            Guid = roomDto.Guid,
            Name = roomDto.Name,
            Floor = roomDto.Floor,
            Capacity = roomDto.Capacity,
            ModifiedDate = DateTime.Now  // Mengatur ModifiedDate sebagai waktu saat ini.
        };
    }
}