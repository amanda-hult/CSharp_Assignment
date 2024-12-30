namespace Business.Interfaces;

public interface IFileService
{
    Task<bool> SaveListToFileAsync<T>(string content);
    Task<string> LoadListFromFileAsync<T>();
}
