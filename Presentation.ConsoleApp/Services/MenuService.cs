using Business.Factories;
using Business.Models;
using Business.Interfaces;
using Presentation.ConsoleApp.Interfaces;
using Business.Helpers;
using Presentation.ConsoleApp.Helpers;

namespace Presentation.ConsoleApp.Services;

public class MenuService : IMenuService
{
    private readonly IContactService _contactService;
    private readonly InputValidator _inputValidator;

    public MenuService(IContactService contactService, InputValidator inputValidator)
    {
        _contactService = contactService;
        _inputValidator = inputValidator;
    }

    public async Task MainMenu()
    {
        string userInput;
        do
        {
            Console.Clear();
            DisplayHelper.ShowHeader("MAIN MENU");
            Console.WriteLine("\n1. Add contact");
            Console.WriteLine("\n2. Show all contacts");
            Console.WriteLine("\n3. Edit contact");
            Console.WriteLine("\n4. Delete contact");
            Console.WriteLine("\n5. Exit");
            Console.WriteLine("\n##################################################");
            Console.Write("\nChoose your menu option: ");
            userInput = Console.ReadLine()!;

            await MenuOptionSelector(userInput);
        }
        while (userInput != "5");
    }

    public async Task MenuOptionSelector(string userInput)
    {
        switch (userInput)
        {
            case "1":
                await AddContact();
                break;
            case "2":
                await ShowAllContacts();
                break;
            case "3":
                await EditContact();
                break;
            case "4":
                await DeleteContact();
                break;
            case "5":
                await ExitApp();
                break;
            default:
                InvalidInput();
                break;
        }
    }

    public async Task AddContact()
    {
        Console.Clear();
        DisplayHelper.ShowHeader("ADD CONTACT");

        ContactRegistration contactRegistration = ContactFactory.Create();

        contactRegistration.FirstName = _inputValidator.GetValidatedInput<ContactRegistration>("Enter first name: ", nameof(ContactRegistration.FirstName), () => Console.ReadLine());
        contactRegistration.LastName = _inputValidator.GetValidatedInput<ContactRegistration>("Enter last name: ", nameof(ContactRegistration.LastName), () => Console.ReadLine());
        contactRegistration.Email = _inputValidator.GetValidatedEmail<ContactRegistration>("Enter email: ", nameof(ContactRegistration.Email), () => Console.ReadLine());
        contactRegistration.Phone = _inputValidator.GetValidatedInput<ContactRegistration>("Enter phone number: ", nameof(ContactRegistration.Phone), () => Console.ReadLine());
        contactRegistration.Address = _inputValidator.GetValidatedInput<ContactRegistration>("Enter address: ", nameof(ContactRegistration.Address), () => Console.ReadLine());
        contactRegistration.Postcode = _inputValidator.GetValidatedInput<ContactRegistration>("Enter postcode: ", nameof(ContactRegistration.Postcode), () => Console.ReadLine());
        contactRegistration.City = _inputValidator.GetValidatedInput<ContactRegistration>("Enter city: ", nameof(ContactRegistration.City), () => Console.ReadLine());

        var result = await _contactService.CreateContact(contactRegistration);
        DisplayHelper.ShowMessage($"{result.Message}");

        DisplayHelper.ShowMessage("Press enter to return to main menu.", pause: true);
    }

    //try/catch?
    public async Task ShowAllContacts()
    {
        var response = await _contactService.GetAllContacts();
        Console.Clear();
        DisplayHelper.ShowHeader("ALL CONTACTS");

        if (!response.Success)
        {
            DisplayHelper.ShowMessage("Failed to load contacts. Press enter to return to main menu.", pause: true);
            return;
        }

        var contacts = response.Result;

        if (contacts.Count == 0)
        {
            DisplayHelper.ShowMessage("No contacts to show. Press enter to return to main menu.", pause: true);
            return;
        }

        for (int i = 0; i < contacts.Count; i++)
        {
            DisplayHelper.ShowMessage($"{i + 1}. {DisplayHelper.GetBriefContactDetails(contacts[i])}");
        }

        DisplayHelper.ShowMessage($"\nEnter the number of a contact to see all details, or press 'q' to return to the main menu.");
        var input = Console.ReadKey(true);
        if (char.IsDigit(input.KeyChar))
        {
            int selectedIndex = int.Parse(input.KeyChar.ToString());
            if (selectedIndex > 0 && selectedIndex <= contacts.Count)
            {
                var selectedContact = contacts[selectedIndex - 1];
                await ShowContactDetails(selectedContact);
            }
            else
            {
                DisplayHelper.ShowMessage("Invalid number.Press any key to try again: ", pause: true);
                await ShowAllContacts();
            }
        }
        else if (input.KeyChar.ToString().Equals("q", StringComparison.OrdinalIgnoreCase))
        {
            await MainMenu();
        }
        else
        {
            DisplayHelper.ShowMessage("Invalid input.Press any key to try again: ", pause: true);
            await ShowAllContacts();
        }
    }

    public async Task ShowContactDetails(DisplayedContact contact)
    {
        Console.Clear();
        DisplayHelper.ShowHeader("CONTACT DETAILS");
        DisplayHelper.ShowMessage($"{DisplayHelper.GetContactDetails(contact)}");

        DisplayHelper.ShowMessage("Press \"C\" to return to contact list, press any other key to return to main menu.", pause: false);
        var userInput = Console.ReadKey(true);
        if (userInput.KeyChar.ToString().Equals("c", StringComparison.OrdinalIgnoreCase))
            await ShowAllContacts();
        else
            await MainMenu();
    }

    public async Task EditContact()
    {
        Console.Clear();
        DisplayHelper.ShowHeader("EDIT CONTACT");

        var response = await _contactService.GetAllContacts();
        var contacts = response.Result;

        if (contacts.Count == 0)
        {
            DisplayHelper.ShowMessage("No contacts to show. Press any key to return to main menu", pause: true);
            return;
        }

        while (true)
        {
            DisplayHelper.DisplayContacts(contacts);

            DisplayHelper.ShowMessage("Enter the number of the contact you want to edit. Press 'q' to return to main menu.", pause: false);

            var input = Console.ReadKey(true);
            if (char.IsDigit(input.KeyChar))
            {
                int selectedIndex = int.Parse(input.KeyChar.ToString());
                if (selectedIndex > 0 && selectedIndex <= contacts.Count)
                {
                    await EditSelectedContact(contacts[selectedIndex - 1]);
                    break;
                }
                else
                {
                    DisplayHelper.ShowMessage("Invalid number. Press any key to try again.", pause: true);
                    await ShowAllContacts();
                }
            }
            else if (input.KeyChar.ToString().Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                await MainMenu();
            }
            else
            {
                DisplayHelper.ShowMessage("Invalid input. Press any key to try again.", pause: true);
                await ShowAllContacts();
            }
        }
    }

    public async Task DeleteContact()
    {
        Console.Clear();
        DisplayHelper.ShowHeader("DELETE CONTACT");

        var response = await _contactService.GetAllContacts();
        var contacts = response.Result;

        if (contacts.Count == 0)
        {
            DisplayHelper.ShowMessage("No contacts to show. Press any key to return to main menu.", pause: true);
            return;
        }

        while (true)
        {
            DisplayHelper.DisplayContacts(contacts);

            DisplayHelper.ShowMessage("Enter the number of the contact you want to delete. Press 'q' to return to main menu:", pause: false);
            var input = Console.ReadKey(true);

            if (char.IsDigit(input.KeyChar))
            {
                int selectedIndex = int.Parse(input.KeyChar.ToString());
                if (selectedIndex > 0 && selectedIndex <= contacts.Count)
                {
                    await ConfirmAndDeleteContact(contacts, selectedIndex - 1);
                    return;
                }
                else
                {
                    DisplayHelper.ShowMessage("Invalid number. Press any key to try again.", pause: true);
                }
            }
            else if (input.KeyChar.ToString().Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                await MainMenu();
                return;
            }
            else
            {
                DisplayHelper.ShowMessage("Invalid input. Press any key to try again.", pause: true);
            }
        }
    }

    public async Task ExitApp()
    {
        Console.Clear();
        DisplayHelper.ShowMessage("You are exiting the app. Do you want to continue (y/n)?", pause: false);
        var userInput = Console.ReadKey(true);
        if (userInput.KeyChar.ToString().Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            Environment.Exit(0);
        }
        else
        {
            await MainMenu();
        }
    }

    public void InvalidInput()
    {
        DisplayHelper.ShowMessage("Invalid input. Try Again.", pause: true);
    }

    public async Task EditSelectedContact(DisplayedContact selectedContact)
    {
        Console.Clear();
        DisplayHelper.ShowMessage($"{DisplayHelper.GetContactDetails(selectedContact)}");

        var storedContact = _contactService.GetStoredContactById(selectedContact.ContactId);

        DisplayHelper.ShowMessage("Enter new contact information. Press enter to keep the current contact information.", pause: false);

        storedContact.FirstName = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new first name: ", nameof(StoredContact.FirstName), storedContact, () => Console.ReadLine());
        storedContact.LastName = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new last name: ", nameof(StoredContact.LastName), storedContact, () => Console.ReadLine());
        storedContact.Email = _inputValidator.GetValidatedEmail<StoredContact>("Enter new email: ", nameof(StoredContact.Email), () => Console.ReadLine(), storedContact.Email);
        storedContact.Phone = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new phone number: ", nameof(StoredContact.Phone), storedContact, () => Console.ReadLine());
        storedContact.Address = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new address: ", nameof(StoredContact.Address), storedContact, () => Console.ReadLine());
        storedContact.Postcode = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new postcode: ", nameof(StoredContact.Postcode), storedContact, () => Console.ReadLine());
        storedContact.City = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new city: ", nameof(StoredContact.City), storedContact, () => Console.ReadLine());

        await _contactService.SaveContacts();

        DisplayHelper.ShowMessage("Contact was updated successfully.", pause: true);
    }

    public async Task ConfirmAndDeleteContact(List<DisplayedContact> contacts, int index)
    {
        var contactToDelete = contacts[index];
        Console.Clear();
        DisplayHelper.ShowMessage($"Please confirm that you want to delete {contactToDelete.FirstName} {contactToDelete.LastName} from the contactlist (Y).", pause: false);
        var userInput = Console.ReadKey(true);

        if (userInput.KeyChar.ToString().Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            bool result = await _contactService.DeleteContact(index);
            if (result)
            {
                DisplayHelper.ShowMessage($"Contact {contactToDelete.FirstName} {contactToDelete.LastName} was successfully deleted.", pause: false);
                //_contactService.GetAllContacts();
            }
            else
            {
                DisplayHelper.ShowMessage("Something went wrong. The user could not be deleted.", pause: false);
            }
        }
        else
        {
            DisplayHelper.ShowMessage("The contact was not deleted.", pause: false);
        }
        DisplayHelper.ShowMessage("Press any key to return to main menu.", pause: true);
    }
}
