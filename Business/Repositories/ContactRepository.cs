using System.Diagnostics;
using System.Text.Json;
using Business.Interfaces;

namespace Business.Repositories;

public class ContactRepository<T> : IContactRepository<T>
{

    private readonly IFileService _fileService;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ContactRepository(IFileService fileService)
    {
        _fileService = fileService;
        _jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
    }

    public string Serialize(List<T> list)
    {
        return JsonSerializer.Serialize(list, _jsonSerializerOptions);
    }

    public List<T>? Deserialize(string json)
    {
        return JsonSerializer.Deserialize<List<T>>(json, _jsonSerializerOptions);
    }

    public async Task<bool> SaveToFileAsync(List<T> list)
    {
        try
        {
            var json = Serialize(list);
            return await _fileService.SaveListToFileAsync<T>(json);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<List<T>?> GetFromFileAsync()
    {
        try
        {
            var json = await _fileService.LoadListFromFileAsync<T>();
            var list = Deserialize(json);
            return list;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }


    }
}
