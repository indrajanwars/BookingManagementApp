using System.Net;

namespace API.Utilities.Handlers;

// Class yang digunakan untuk mengelola respons HTTP status 200 OK.
public class ResponseOKHandler<TEntity>
{
    // Properti Code digunakan untuk menyimpan kode status HTTP.
    public int Code { get; set; }

    // Properti Status digunakan untuk menyimpan pesan status HTTP.
    public string Status { get; set; }

    // Properti Message digunakan untuk menyimpan pesan atau keterangan tambahan terkait respons.
    public string Message { get; set; }

    // Properti Data digunakan untuk menyimpan data yang akan dikirimkan bersama dengan respons.
    // TEntity? adalah tipe generik yang akan digunakan untuk menyimpan data.
    public TEntity? Data { get; set; }

    // Konstruktor ini digunakan ketika ingin membuat respons HTTP 200 OK dengan data.
    public ResponseOKHandler(TEntity? data)
    {
        // Mengatur kode status HTTP ke 200 OK.
        Code = StatusCodes.Status200OK;

        // Mengatur pesan status HTTP ke "OK".
        Status = HttpStatusCode.OK.ToString();

        // Mengatur pesan tambahan yang mengindikasikan berhasil mengambil data.
        Message = "Success to Retrieve Data";

        // Mengatur data yang akan dikirimkan bersama dengan respons.
        Data = data;
    }

    // Konstruktor ini digunakan ketika ingin membuat respons HTTP 200 OK dengan pesan kustom.
    public ResponseOKHandler(string message, TEntity data)
    {
        // Mengatur kode status HTTP ke 200 OK.
        Code = StatusCodes.Status200OK;

        // Mengatur status teks ke "OK".
        Status = HttpStatusCode.OK.ToString();

        // Mengatur pesan respons sesuai dengan yang diberikan sebagai parameter.
        Message = message;
        Data = data;
    }
}