using API.Models;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek Account berdasarkan data tersebut. */
namespace API.DTOs.Bookings;

public class CreateAccountDto
{
    public string Password { get; set; }
    public int OTP { get; set; }
    public bool IsUsed { get; set; }
    public DateTime ExpiredTime { get; set; }

    public static implicit operator Account(CreateAccountDto createAccountDto)
    {
        return new Account
        {
            Password = createAccountDto.Password,
            OTP = createAccountDto.OTP,
            IsUsed = createAccountDto.IsUsed,
            ExpiredTime = createAccountDto.ExpiredTime,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}