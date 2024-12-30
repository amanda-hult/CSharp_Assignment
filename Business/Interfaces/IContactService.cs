using Business.Models;

namespace Business.Interfaces;

public interface IContactService
{
    Task<ResponseResult<StoredContact>> CreateContact(ContactRegistration contactRegistration);
    Task<ResponseResult<List<DisplayedContact>>> GetAllContacts();
    StoredContact GetStoredContactById(string contactId);
    Task<bool> DeleteContact(int index);
    Task SaveContacts();
    ResponseResult<ContactRegistration> IsEmailAvailable(string email);
}
