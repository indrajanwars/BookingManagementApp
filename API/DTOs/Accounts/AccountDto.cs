using API.Models;

namespace API.DTOs.Accounts;

/* Kelas AccountDto adalah Data Transfer Object (DTO) yang digunakan untuk 
 * mengambil data dari objek Account dan mengembalikannya sebagai respons dari API. */
public class AccountDto
{
    // Properti-properti berikut adalah atribut-atribut yang akan diambil dari objek Account.
    public Guid EmployeeGuid { get; set; }
    public string Password { get; set; }
    public int OTP { get; set; }
    public bool IsUsed { get; set; }
    public DateTime ExpiredTime { get; set; }

    // Operator konversi explicit yang mengubah objek Account menjadi objek AccountDto.
    public static explicit operator Account(AccountDto account)
    {
        // Membuat objek AccountDto baru dengan nilai-nilai yang diambil dari Account.
        return new Account
        {
            Guid = account.EmployeeGuid,
            Password = account.Password,
            OTP = account.OTP,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        };
    }

    // Operator konversi implicit yang mengubah objek AccountDto menjadi objek Account.
    public static implicit operator AccountDto(Account accountDto)
    {
        // Membuat objek Account baru dengan nilai-nilai yang diambil dari AccountDto.
        return new AccountDto
        {
            EmployeeGuid = accountDto.Guid,
            Password = accountDto.Password,
            OTP = accountDto.OTP,
            IsUsed = accountDto.IsUsed,
            ExpiredTime = accountDto.ExpiredTime,
        };
    }
}