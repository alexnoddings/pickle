namespace Pickle;

public class Files
{
    public string ContentPath { get; }

    public Files(string contentPath)
    {
        ContentPath = contentPath ?? throw new ArgumentNullException(nameof(contentPath));
    }

    public string? GetFileExtension(int year, int month, int day)
    {
        var monthDirectory = Path.Combine(ContentPath, year.ToString("D4"), month.ToString("D2"));
        if (!Directory.Exists(monthDirectory))
            return null;

        var filesInDirectory = Directory.EnumerateFiles(monthDirectory, $"{day:D2}.*", SearchOption.TopDirectoryOnly);
        var fullFilePath = filesInDirectory.FirstOrDefault();
        return Path.GetExtension(fullFilePath);
    }
    public string? GetFileExtension(DateTime dateTime) =>
        GetFileExtension(dateTime.Year, dateTime.Month, dateTime.Day);
}
