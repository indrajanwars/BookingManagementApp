namespace API.DTOs.Accounts;

// DTO untuk mengelola OTP yang digunakan dalam proses lupa kata sandi.
public class ForgotPasswordDto
{
    public string Email { get; set; }

    // Properti untuk menyimpan OTP yang digenerate.
    public int Otp { get; set; }

    // Properti untuk menyimpan waktu kedaluwarsa OTP.
    public DateTime ExpiredDate { get; set; }
}