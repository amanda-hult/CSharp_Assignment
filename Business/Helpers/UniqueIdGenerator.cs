namespace Business.Helpers;

public static class UniqueIdGenerator
{
    //Generates a unique id with the default format "D"
    public static string GenerateUniqueId(string format = "D")
    {
        if (string.IsNullOrWhiteSpace(format))
        {
            throw new ArgumentException("Format cannot be null or empty.", nameof(format));
        }
        return Guid.NewGuid().ToString(format);
    }
}
