using Business.Models;

namespace Business.Interfaces;

public interface IContactService
{
    bool CreateContact(ContactRegistration contactRegistration);
    List<DisplayedContact> GetAllContacts();
    bool DeleteContact(int index);
    bool ValidateContact(ContactRegistration contactRegistration, out List<string> errors);
    void SaveContacts();
}
