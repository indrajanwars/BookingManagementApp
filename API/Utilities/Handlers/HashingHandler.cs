namespace API.Utilities.Handlers;

public class HashingHandler
{
    private static string GetRandomSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(12); // Default 11
    }
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, GetRandomSalt());
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    internal static bool VerifyPassword(string password1, object password2)
    {
        throw new NotImplementedException();
    }
}
