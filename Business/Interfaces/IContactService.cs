using Business.Models;

namespace Business.Interfaces;

public interface IContactService
{
    bool CreateContact(ContactRegistration contactRegistration);
    List<DisplayedContact> GetAllContacts();
    StoredContact GetStoredContactById(string contactId);
    bool DeleteContact(int index);
    void SaveContacts<T>(List<T> contacts) where T : class;
    ResponseResult<ContactRegistration> IsEmailAvailable(string email);
}
