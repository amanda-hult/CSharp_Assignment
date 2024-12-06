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
            SaveContacts();
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
            SaveContacts();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public bool ValidateContact(ContactRegistration contact, out List<string> errors)
    {
        errors = new List<string>();

        var validationResults  = new List<ValidationResult>();
        var validationContext = new ValidationContext(contact);

        bool isValid = Validator.TryValidateObject(contact, validationContext, validationResults, true);

        if (!isValid)
        {
            foreach (var validationResult in validationResults)
            {
                errors.Add(validationResult.ErrorMessage ?? "Invalid input");
            }
        }
        return isValid;
    }

    public void SaveContacts()
    {
        try
        {
            _fileService.SaveListToFile(_contacts);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to save contact: {ex.Message}");
        }
    }
}
