namespace Business.Interfaces;

public interface IFileService
{
    void SaveListToFile<T>(List<T> list);
    List<T> LoadListFromFile<T>();
}
