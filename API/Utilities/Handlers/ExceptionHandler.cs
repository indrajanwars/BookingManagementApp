namespace API.Utilities.Handlers;

// Class yang merupakan turunan (inherit) dari class Exception.
public class ExceptionHandler : Exception
{
    // Konstruktor dari class ExceptionHandler yang menerima satu parameter 'message'.
    public ExceptionHandler(string message) : base(message)
    {
        /* Konstruktor ini digunakan untuk membuat instance (objek) dari class ExceptionHandler
         * dengan pesan (message) yang akan dijelaskan mengenai pengecualian (exception) yang terjadi.*/
    }
}