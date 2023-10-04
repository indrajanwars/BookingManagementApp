namespace API.Utilities.Handlers;

// Class yang digunakan untuk mengelola respons error HTTP.
public class ResponseErrorHandler
{
    // Properti Code digunakan untuk menyimpan kode status HTTP yang berisi jenis error.
    public int Code { get; set; }

    // Properti Status digunakan untuk menyimpan pesan status HTTP yang terkait dengan error.
    public string Status { get; set; }

    // Properti Message digunakan untuk menyimpan pesan tambahan yang menjelaskan error yang terjadi.
    public string Message { get; set; }

    // Properti Error digunakan untuk menyimpan informasi tambahan tentang error jika diperlukan.
    // Tipe data string? digunakan agar properti ini bisa memiliki nilai null.
    public string? Error { get; set; }
}