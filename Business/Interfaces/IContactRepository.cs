namespace Business.Interfaces;

public interface IContactRepository<T>
{
    string Serialize(List<T> list);
    List<T>? Deserialize(string json);
    Task<bool> SaveToFileAsync(List<T> list);
    Task<List<T>?> GetFromFileAsync();
}
