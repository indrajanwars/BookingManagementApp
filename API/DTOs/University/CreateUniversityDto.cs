using API.Models;

namespace API.DTOs.Universities;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek University berdasarkan data tersebut. */
public class CreateUniversityDto
{
    // Properti-properti berikut mewakili atribut-atribut yang akan diterima dari permintaan API.
    public string Code { get; set; }
    public string Name { get; set; }

    // Operator konversi implicit yang mengubah objek CreateUniversityDto menjadi objek University.
    public static implicit operator University(CreateUniversityDto createUniversityDto)
    {
        // Membuat objek University baru dengan nilai-nilai yang diambil dari CreateUniversityDto.
        return new University
        {
            Code = createUniversityDto.Code,
            Name = createUniversityDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}