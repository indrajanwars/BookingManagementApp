using API.Models;

namespace API.DTOs.Universities;

/* Kelas UniversityDto adalah Data Transfer Object (DTO) yang digunakan untuk 
 * mengambil data dari objek University dan mengembalikannya sebagai respons dari API. */
public class UniversityDto
{
    // Properti Guid digunakan untuk menampung GUID universitas.
    public Guid Guid { get; set; }

    // Properti Code digunakan untuk menampung kode universitas.
    public string Code { get; set; }

    // Properti Name digunakan untuk menampung nama universitas.
    public string Name { get; set; }

    /* Operator explicit di sini adalah metode khusus yang secara eksplisit mengkonversi objek University
     * menjadi UniversityDto dengan menjadi format yang sesuai untuk respons API. */
    public static explicit operator UniversityDto(University university)
    {
        // Dalam metode ini membuat objek UniversityDto baru dan menginisialisasinya dengan properti yang diterima dari objek University.
        return new UniversityDto
        {
            Guid = university.Guid,
            Code = university.Code,
            Name = university.Name
        };
    }

    // Operator implicit di sini adalah metode khusus yang secara otomatis mengkonversi objek UniversityDto menjadi objek University.
    public static implicit operator University(UniversityDto universityDto)
    {
        /* Dalam metode ini membuat objek University baru dan menginisialisasinya dengan properti yang diterima dari
         * objek UniversityDto, dan juga mengatur properti ModifiedDate ke nilai saat ini. */
        return new University
        {
            Guid = universityDto.Guid,
            Code = universityDto.Code,
            Name = universityDto.Name,
            ModifiedDate = DateTime.Now
        };
    }
}