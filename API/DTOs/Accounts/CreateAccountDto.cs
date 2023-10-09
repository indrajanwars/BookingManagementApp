using API.Models;

namespace API.DTOs.Accounts;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek Account berdasarkan data tersebut. */
public class CreateAccountDto
{
    // Properti-properti berikut mewakili atribut-atribut yang akan diterima dari permintaan API.
    public Guid EmployeeGuid { get; set; }
    public string Password { get; set; }
    //public int OTP { get; set; }
    //public bool IsUsed { get; set; }
    //public DateTime ExpiredTime { get; set; }

    // Operator konversi implicit yang mengubah objek CreateAccountDto menjadi objek Account.
    public static implicit operator Account(CreateAccountDto createAccountDto)
    {
        // Membuat objek Account baru dengan nilai-nilai yang diambil dari CreateAccountDto.
        return new Account
        {
            Guid = createAccountDto.EmployeeGuid,
            Password = createAccountDto.Password,
            OTP = 123456,
            IsUsed = true,
            ExpiredTime = DateTime.Now.AddDays(-30),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}