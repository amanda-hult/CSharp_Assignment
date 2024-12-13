using Business.Factories;
using Business.Models;
using Business.Interfaces;
using Presentation.ConsoleApp.Interfaces;
using System.ComponentModel.DataAnnotations;
using Business.Helpers;

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

    public void MainMenu()
    {
        string userInput;
        do
        {
            Console.Clear();
            Console.WriteLine("\n###############-----MAIN MENU-----###############");
            Console.WriteLine("\n1. Add contact");
            Console.WriteLine("\n2. Show all contacts");
            Console.WriteLine("\n3. Edit contact");
            Console.WriteLine("\n4. Delete contact");
            Console.WriteLine("\n5. Exit");
            Console.WriteLine("\n##################################################");
            Console.Write("\nChoose your menu option: ");
            userInput = Console.ReadLine()!;

            MenuOptionSelector(userInput);
        }
        while (userInput != "5");
    }

    public void MenuOptionSelector(string userInput)
    {
        switch (userInput)
        {
            case "1":
                AddContact();
                break;
            case "2":
                ShowAllContacts();
                break;
            case "3":
                EditContact();
                break;
            case "4":
                DeleteContact();
                break;
            case "5":
                ExitApp();
                break;
            default:
                InvalidInput();
                break;
        }
    }

    public void AddContact()
    {
        Console.Clear();
        Console.WriteLine("\n###############-----ADD CONTACT-----###############");

        ContactRegistration contactRegistration = ContactFactory.Create();

        contactRegistration.FirstName = _inputValidator.GetValidatedInput<ContactRegistration>("Enter first name: ", nameof(ContactRegistration.FirstName));
        contactRegistration.LastName = _inputValidator.GetValidatedInput<ContactRegistration>("Enter last name: ", nameof(ContactRegistration.LastName));
        while (true)
        {
            string email = _inputValidator.GetValidatedInput<ContactRegistration>("Enter email: ", nameof(ContactRegistration.Email));

            var emailCheck = _contactService.IsEmailAvailable(email);
            if (emailCheck.Success)
            {
                contactRegistration.Email = email;
                break;
            }
            else
            {
                Console.WriteLine($"{emailCheck.Message}");
            }
        }
        contactRegistration.Phone = _inputValidator.GetValidatedInput<ContactRegistration>("Enter phone number: ", nameof(ContactRegistration.Phone));
        contactRegistration.Address = _inputValidator.GetValidatedInput<ContactRegistration>("Enter address: ", nameof(ContactRegistration.Address));
        contactRegistration.Postcode = _inputValidator.GetValidatedInput<ContactRegistration>("Enter postcode: ", nameof(ContactRegistration.Postcode));
        contactRegistration.City = _inputValidator.GetValidatedInput<ContactRegistration>("Enter city: ", nameof(ContactRegistration.City));

        bool result = _contactService.CreateContact(contactRegistration);
        if (result)
        {
            Console.WriteLine("\nContact was added successfully.");
        }
        else
        {
            Console.WriteLine("\nContact was not added.");
        }

        Console.WriteLine("\nPress any key to return to main menu");
        Console.ReadKey();
    }

    public void ShowAllContacts()
    {
        var contacts = _contactService.GetAllContacts();
        Console.Clear();
        Console.WriteLine("\n###############-----ALL CONTACTS-----###############");

        if (contacts.Count == 0)
        {
            Console.WriteLine("\nNo contacts to show.");
            Console.WriteLine("\nPress any key to return to main menu");
            Console.ReadKey();
        }
        else
        {
            int index = 1;
            foreach (var contact in contacts)
            {
                Console.WriteLine($"{index}. {contact.GetBriefContactDetails()}");
                index++;
            }

            Console.Write($"\nEnter the number of a contact to see all details, or press any other key to return to the main menu.");
            if (int.TryParse(Console.ReadLine(), out int selectedIndex) && selectedIndex > 0 && selectedIndex <= contacts.Count)
            {
                var selectedContact = contacts[selectedIndex - 1];
                ShowContactDetails(selectedContact);
            }
        }
    }

    public void EditContact()
    {
        var contacts = _contactService.GetAllContacts();

        Console.Clear();
        Console.WriteLine("\n###############-----EDIT CONTACT-----###############");

        if (contacts.Count == 0)
        {
            Console.WriteLine("\nNo contacts to show.");
            Console.WriteLine("\nPress any key to return to main menu");
            Console.ReadKey();
            return;
        }
        while (true)
        {
            Console.Clear();

            Console.WriteLine("\t\tAll contacts:\n");
            var index = 1;
            foreach (var contact in contacts)
            {
                Console.WriteLine($"{index}. {contact.FirstName} {contact.LastName}");
                index++;
            }

            Console.Write("\nEnter the number of the contact you want to edit. Press 'q' to return to main menu: ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= contacts.Count)
            {
                var selectedContact = contacts[selectedIndex - 1];
                Console.WriteLine($"{selectedContact.GetContactDetails()}");

                var storedContact = _contactService.GetStoredContactById(selectedContact.ContactId);

                Console.WriteLine("Enter new contact information. Press enter to keep the current contact information");

                storedContact.FirstName = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new first name: ", nameof(StoredContact.FirstName), storedContact);
                storedContact.LastName = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new last name: ", nameof(StoredContact.LastName), storedContact);
                while (true)
                {
                    string email = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new email: ", nameof(StoredContact.Email), storedContact);

                    var emailCheck = _contactService.IsEmailAvailable(email);
                    if (emailCheck.Success || string.Equals(email, selectedContact.Email, StringComparison.OrdinalIgnoreCase))
                    {
                        selectedContact.Email = email;
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"{emailCheck.Message}");
                    }
                }
                storedContact.Phone = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new phone number: ", nameof(StoredContact.Phone), storedContact);
                storedContact.Address = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new address: ", nameof(StoredContact.Address), storedContact);
                storedContact.Postcode = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new postcode: ", nameof(StoredContact.Postcode), storedContact);
                storedContact.City = _inputValidator.GetOptionalValidatedInput<StoredContact>("Enter new city: ", nameof(StoredContact.City), storedContact);


                _contactService.SaveContacts(contacts);

                    Console.WriteLine("\nContact was updated successfully.");
                break;
            }
            else if (input.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                MainMenu();
                return;
            }
            else
            {
                Console.WriteLine("Invalid number. Press any key to try again.");
                Console.ReadKey();
            }
        }

    }

    public void DeleteContact()
    {
        var contacts = _contactService.GetAllContacts();
        Console.Clear();
        Console.WriteLine("\n###############-----DELETE CONTACT-----###############");

        if (contacts.Count == 0)
        {
            Console.WriteLine("\nNo contacts to show.");
            Console.WriteLine("\nPress any key to return to main menu");
            Console.ReadKey();
            return;
        }
        while (true)
        {
            Console.Clear();

            Console.WriteLine("\t\tAll contacts:\n");
            var index = 1;
            foreach (var contact in contacts)
            {
                Console.WriteLine($"{index}. {contact.FirstName} {contact.LastName}");
                index++;
            }

            Console.Write("\nEnter the number of the contact you want to delete. Press 'q' to return to main menu: ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= contacts.Count)
            {
                bool result = _contactService.DeleteContact(selectedIndex - 1);
                if (result)
                {
                    Console.WriteLine($"Contact was successfully deleted. Press any key to return to main menu.");
                    Console.ReadKey();
                    contacts = _contactService.GetAllContacts();
                    return;
                }
                else
                {
                    Console.WriteLine("Something went wrong. The user could not be deleted.");
                    Console.ReadKey();
                    return;
                }
            }
            else if (input.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                MainMenu();
                return;
            }
            else
            {
                Console.WriteLine("Invalid number. Press any key to try again.");
                Console.ReadKey();
            }
        }
    }

    public void ExitApp()
    {
        Console.Clear();
        Console.Write("You are exiting the app. Do you want to continue (y/n)?: ");
        var userInput = Console.ReadLine();
        if (userInput!.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            Environment.Exit(0);
        }
        else
        {
            MainMenu();
        }
    }

    public void InvalidInput()
    {
        Console.WriteLine("Invalid input. Try Again.");
        Console.ReadLine();
    }

    public void ShowContactDetails(DisplayedContact contact)
    {
        Console.Clear();
        Console.WriteLine("\n###############-----CONTACT DETAILS-----###############");
        Console.WriteLine($"{contact.GetContactDetails()}");
        //Console.WriteLine($"Name: {contact.FirstName} {contact.LastName}");
        //Console.WriteLine($"Email: {contact.Email}");
        //Console.WriteLine($"Phone: {contact.Phone}");
        //Console.WriteLine($"Address: {contact.Address} {contact.Postcode} {contact.City}");

        Console.WriteLine("\nPress \"C\" to return to contact list, press any other key to return to main menu.");
        string userInput = Console.ReadLine();
        if (userInput!.Equals("c", StringComparison.OrdinalIgnoreCase))
            ShowAllContacts();
        else
            MainMenu();
    }
}
