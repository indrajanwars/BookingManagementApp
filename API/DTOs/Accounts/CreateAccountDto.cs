using API.Models;

/* Kelas ini digunakan untuk Data Transfer Object (DTO) yang mengambil data
 * dari request API, dan membuat objek Account berdasarkan data tersebut. */
namespace API.DTOs.Accounts;

public class CreateAccountDto
{
    public Guid Guid { get; set; }
    public string Password { get; set; }
    public int OTP { get; set; }
    public bool IsUsed { get; set; }
    public DateTime ExpiredTime { get; set; }

    public static implicit operator Account(CreateAccountDto createAccountDto)
    {
        return new Account
        {
            Guid = createAccountDto.Guid,
            Password = createAccountDto.Password,
            OTP = createAccountDto.OTP,
            IsUsed = createAccountDto.IsUsed,
            ExpiredTime = createAccountDto.ExpiredTime
        };
    }
}