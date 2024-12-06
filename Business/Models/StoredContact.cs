using System.ComponentModel.DataAnnotations;
namespace Business.Models;

[Serializable]
public class StoredContact : BaseContact
{
    [Key]
    public string ContactId { get; set; } = null!;

    //empty constructor to make serialization possible
    public StoredContact() { }
}
