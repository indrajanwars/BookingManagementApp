using API.Models;

namespace API.DTOs.Educations;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek Education berdasarkan data tersebut. */
public class CreateEducationDto
{
    // Properti-properti berikut mewakili atribut-atribut yang akan diterima dari permintaan API.
    public Guid Guid { get; set; }
    public string Major { get; set; }
    public string Degree { get; set; }
    public float Gpa { get; set; }
    public Guid UniversityGuid { get; set; }

    // Operator konversi implicit yang mengubah objek CreateEducationDto menjadi objek Education.
    public static implicit operator Education(CreateEducationDto createEducationDto)
    {
        // Membuat objek Education baru dengan nilai-nilai yang diambil dari CreateEducationDto.
        return new Education
        {
            Guid = createEducationDto.Guid,
            Major = createEducationDto.Major,
            Degree = createEducationDto.Degree,
            Gpa = createEducationDto.Gpa,
            UniversityGuid = createEducationDto.UniversityGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}