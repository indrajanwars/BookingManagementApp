using API.Models;

namespace API.DTOs.Universities;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek University berdasarkan data tersebut. */
public class CreateUniversityDto
{
    // Properti Code digunakan untuk menampung kode universitas.
    public string Code { get; set; }

    // Properti Name digunakan untuk menampung nama universitas.
    public string Name { get; set; }

    /* Operator implicit secara otomatis mengkonversi instance CreateUniversityDto menjadi objek University.
     * Memungkinkan untuk membuat objek University dari data yang diterima dalam request API. */
    public static implicit operator University(CreateUniversityDto createUniversityDto)
    {

        // Dalam metode ini membuat objek University baru dan menginisialisasinya dengan properti-properti yang diterima.
        return new University
        {
            Code = createUniversityDto.Code,
            Name = createUniversityDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}