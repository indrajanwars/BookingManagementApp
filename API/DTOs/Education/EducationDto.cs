using API.Models;

namespace API.DTOs.Educations;

/* Kelas EducationDto adalah Data Transfer Object (DTO) yang digunakan untuk 
 * mengambil data dari objek Education dan mengembalikannya sebagai respons dari API. */
public class EducationDto
{
    // Properti-properti berikut adalah atribut-atribut yang akan diambil dari objek Education.
    public Guid Guid { get; set; }
    public string Major { get; set; }
    public string Degree { get; set; }
    public float Gpa { get; set; }
    public Guid UniversityGuid { get; set; }

    // Operator konversi explicit yang mengubah objek Education menjadi objek EducationDto.
    public static explicit operator EducationDto(Education education)
    {
        // Membuat objek EducationDto baru dengan nilai-nilai yang diambil dari Education.
        return new EducationDto
        {
            Guid = education.Guid,
            Major = education.Major,
            Degree = education.Degree,
            Gpa = education.Gpa,
            UniversityGuid = education.UniversityGuid
        };
    }

    // Operator konversi implicit yang mengubah objek EducationDto menjadi objek Education.
    public static implicit operator Education(EducationDto educationDto)
    {
        // Membuat objek Education baru dengan nilai-nilai yang diambil dari EducationDto.
        return new Education
        {
            Guid = educationDto.Guid,
            Major = educationDto.Major,
            Degree = educationDto.Degree,
            Gpa = educationDto.Gpa,
            UniversityGuid = educationDto.UniversityGuid,
            ModifiedDate = DateTime.Now  // Mengatur ModifiedDate sebagai waktu saat ini.
        };
    }
}