using API.Models;

namespace API.DTOs.Rooms;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek Room berdasarkan data tersebut. */
public class CreateRoomDto
{
    // Properti-properti berikut mewakili atribut-atribut yang akan diterima dari permintaan API.
    public string Name { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; }

    // Operator konversi implicit yang mengubah objek CreateRoomDto menjadi objek Room.
    public static implicit operator Room(CreateRoomDto createRoomDto)
    {
        // Membuat objek Room baru dengan nilai-nilai yang diambil dari CreateRoomDto.
        return new Room
        {
            Name = createRoomDto.Name,
            Floor = createRoomDto.Floor,
            Capacity = createRoomDto.Capacity,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}