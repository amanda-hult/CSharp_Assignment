using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json;
using Business.Factories;
using Business.Interfaces;
using Business.Models;

namespace Business.Services;

public class ContactService : IContactService
{
    private readonly IFileService _fileService;
    private List<StoredContact> _contacts;

    public ContactService(IFileService fileService)
    {
        _fileService = fileService;
        _contacts = _fileService.LoadListFromFile<StoredContact>();
    }

    public bool CreateContact(ContactRegistration contactRegistration)
    {
        try
        {
            StoredContact storedContact = ContactFactory.Create(contactRegistration);
            _contacts.Add(storedContact);
            SaveContacts(_contacts);
            return true;
        }
        //lägg eventuellt till flera catch för specifika undantag
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public List<DisplayedContact> GetAllContacts()
    {
        try
        {
            _contacts = _fileService.LoadListFromFile<StoredContact>();
            var list = new List<DisplayedContact>();

            foreach (var storedContact in _contacts)
            {
                list.Add(ContactFactory.Create(storedContact));
            }
            return list;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new List<DisplayedContact>();
        }
    }

    public StoredContact GetStoredContactById(string contactId)
    {
        var storedContact = _contacts.FirstOrDefault(contact => contact.ContactId == contactId);
        if (storedContact == null)
        {
            Debug.WriteLine($"Contact with id {contactId} was not found");
        }

        return storedContact!;

    }

    public bool DeleteContact(int index)
    {
        if (index < 0 || index >= _contacts.Count)
        {
            Debug.WriteLine($"Invalid index: {index}");
            return false;
        }
        try
        {
            _contacts.RemoveAt(index);
            SaveContacts(_contacts);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public void SaveContacts<T>(List<T> contacts) where T : class
    {
        try
        {
            _fileService.SaveListToFile(contacts);
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
