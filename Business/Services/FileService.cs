using System.Diagnostics;
using Business.Interfaces;

namespace Business.Services;

public class FileService : IFileService
{
    private readonly string _directoryPath;
    private readonly string _filePath;

    public FileService(string directoryPath = "Data", string fileName = "list.json")
    {
        _directoryPath = directoryPath;
        _filePath = Path.Combine(directoryPath, fileName);
    }

    public async Task<bool> SaveListToFileAsync<T>(string content)
    {
        try
        {
            if (!Directory.Exists(_directoryPath))
                Directory.CreateDirectory(_directoryPath);

            await File.WriteAllTextAsync(_filePath, content);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<string> LoadListFromFileAsync<T>()
    {
        try
        {
            if (!File.Exists(_filePath))
                return null!;

            return await File.ReadAllTextAsync(_filePath);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }
}
