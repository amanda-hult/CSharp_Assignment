using System.Diagnostics;
using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Models;

namespace Business.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository<StoredContact> _contactRepository;

    private List<StoredContact> _contacts = [];

    public ContactService(IContactRepository<StoredContact> contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task InitializeAsync()
    {
        _contacts = await _contactRepository.GetFromFileAsync() ?? new List<StoredContact>();
    }

    public async Task<ResponseResult<StoredContact>> CreateContact(ContactRegistration contactRegistration)
    {
        var createdContact = ContactFactory.Create(contactRegistration);

        if (!createdContact.Success)
        {
            return new ResponseResult<StoredContact>
            {
                Success = false,
                Message = createdContact.Message
            };
        }
        try
        {
            var storedContact = createdContact.Result!;
            storedContact.ContactId = UniqueIdGenerator.GenerateUniqueId();
            _contacts.Add(storedContact);
            await SaveContacts();

            return new ResponseResult<StoredContact>
            {
                Success = true,
                Message = createdContact.Message,
                Result = storedContact
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

            return new ResponseResult<StoredContact>
            {
                Success = false,
                Message = "An error occured creating the contact."
            };
        }
    }

    //lägg eventuellt till flera catch för specifika undantag

    public async Task<ResponseResult<List<DisplayedContact>>> GetAllContacts()
    {
        try
        {
            _contacts = await _contactRepository.GetFromFileAsync() ?? new List<StoredContact>();
            var list = new List<DisplayedContact>();

            foreach (var storedContact in _contacts)
            {
                var displayedContact = ContactFactory.Create(storedContact);

                if (displayedContact.Success)
                {
                    list.Add(displayedContact.Result!);
                }
                else
                {
                    Debug.WriteLine("");
                }   
            }
            return new ResponseResult<List<DisplayedContact>>
            {
                Success = true,
                Result = list
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);

            return new ResponseResult<List<DisplayedContact>>
            {
                Success = false,
                Message = "An error occured creating displayedContact.",
                Result = new List<DisplayedContact>()
            };
        }
    }


    //public List<DisplayedContact> GetAllContacts()
    //{
    //    try
    //    {
    //        _contacts = _contactRepository.GetFromFile() ?? new List<StoredContact>();
    //        var list = new List<DisplayedContact>();

    //        foreach (var storedContact in _contacts)
    //        {
    //            list.Add(ContactFactory.Create(storedContact));
    //        }
    //        return list;
    //    }

    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine(ex.Message);
    //        return new List<DisplayedContact>();
    //    }
    //}

    public StoredContact GetStoredContactById(string contactId)
    {
        var storedContact = _contacts.FirstOrDefault(contact => contact.ContactId == contactId);
        if (storedContact == null)
        {
            Debug.WriteLine($"Contact with id {contactId} was not found");
        }

        return storedContact!;

    }

    public async Task<bool> DeleteContact(int index)
    {
        if (index < 0 || index >= _contacts.Count)
        {
            Debug.WriteLine($"Invalid index: {index}");
            return false;
        }
        try
        {
            _contacts.RemoveAt(index);
            await SaveContacts();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task SaveContacts()
    {
        try
        {
            await _contactRepository.SaveToFileAsync(_contacts);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to save contact: {ex.Message}");
        }
    }

    public ResponseResult<ContactRegistration> IsEmailAvailable(string email)
    {
        bool emailAvailable = !_contacts.Any(contact => string.Equals(contact.Email, email, StringComparison.OrdinalIgnoreCase));

        return new ResponseResult<ContactRegistration>
        {
            Success = emailAvailable,
            Message = emailAvailable ? "" : "The email address is not available."
        };
    }
}
