using Business.Helpers;
using Business.Models;

namespace Business.Factories;

public class ContactFactory
{
    public static ContactRegistration Create()
    {
        return new ContactRegistration();
    }

    public static StoredContact Create(ContactRegistration contactRegistration)
    {
        //try/catch
        return new StoredContact()
        {
            ContactId = UniqueIdGenerator.GenerateUniqueId(),
            FirstName = contactRegistration.FirstName.Trim(),
            LastName = contactRegistration.LastName.Trim(),
            Email = contactRegistration.Email.Trim().ToLower(),
            Phone = contactRegistration.Phone,
            Address = contactRegistration.Address.Trim(),
            Postcode = contactRegistration.Postcode,
            City = contactRegistration.City.Trim()
        };
    }

    public static DisplayedContact Create(StoredContact storedContact)
    {
        return new DisplayedContact()
        {
            ContactId = storedContact.ContactId,
            FirstName = storedContact.FirstName,
            LastName = storedContact.LastName,
            Email = storedContact.Email.ToLower(),
            Phone = storedContact.Phone,
            Address = storedContact.Address,
            Postcode = storedContact.Postcode,
            City = storedContact.City
        };
    }
}
