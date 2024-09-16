namespace Qorpe.Application.Common.Services;

public class FileService(string filePath)
{
    public IEnumerable<string> GetFiles()
    {
        if (!Directory.Exists(filePath))
            throw new DirectoryNotFoundException($"The directory {filePath} does not exist.");

        return Directory.GetFiles(filePath, "*.db")
                        .Where(file => !file.Contains("-log"));
    }
}
