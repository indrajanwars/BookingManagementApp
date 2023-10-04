namespace API.Utilities.Handlers;

public static class GenerateHandler
{
    public static string Nik(string lastNik)
    {
        if (string.IsNullOrEmpty(lastNik))
        {
            return "111111";
        }

        long nextNik = long.Parse(lastNik) + 1;
        return nextNik.ToString();
    }
}