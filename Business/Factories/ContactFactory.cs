using System.Diagnostics;
using Business.Models;

namespace Business.Factories;

public static class ContactFactory
{
    public static ContactRegistration Create()
    {
        return new ContactRegistration();
    }

    public static ResponseResult<StoredContact> Create(ContactRegistration contactRegistration)
    {
        try
        {
            var storedContact = new StoredContact()
            {
                FirstName = contactRegistration.FirstName.Trim(),
                LastName = contactRegistration.LastName.Trim(),
                Email = contactRegistration.Email.Trim().ToLower(),
                Phone = contactRegistration.Phone,
                Address = contactRegistration.Address.Trim(),
                Postcode = contactRegistration.Postcode,
                City = contactRegistration.City.Trim()
            };

            return new ResponseResult<StoredContact>
            {
                Success = true,
                Message = "Contact was created successfully",
                Result = storedContact
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating StoredContact: {ex.Message}");

            return new ResponseResult<StoredContact>
            {
                Success = false,
                Message = $"Failed to create StoredContact {ex.Message}"
            };
        }
    }

    public static ResponseResult<DisplayedContact> Create(StoredContact storedContact)
    {
        try
        {
            var displayedContact = new DisplayedContact()
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
            return new ResponseResult<DisplayedContact>
            {
                Success = true,
                Message = "Contact wa created successfully",
                Result = displayedContact
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating DisplayedContact: {ex.Message}");

            return new ResponseResult<DisplayedContact>
            {
                Success = false,
                Message = $"Failed to create DisplayedContact {ex.Message}"
            };
        }
    }
}
