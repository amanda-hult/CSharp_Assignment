using Business.Factories;
using Business.Models;
using Business.Interfaces;
using Presentation.ConsoleApp.Interfaces;

namespace Presentation.ConsoleApp.Services;

public class MenuService : IMenuService
{
    private readonly IContactService _contactService;
    public MenuService(IContactService contactService)
    {
        _contactService = contactService;
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
            Console.WriteLine("\n3. Delete contact");
            Console.WriteLine("\n4. Exit");
            Console.WriteLine("\n##################################################");
            Console.Write("\nChoose your menu option: ");
            userInput = Console.ReadLine()!;

            MenuOptionSelector(userInput);
        }
        while (userInput != "4");
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
                DeleteContact();
                break;
            case "4":
                ExitApp();
                break;
            default:
                InvalidInput();
                break;
        }
    }

    public void AddContact()
    {
        ContactRegistration contactRegistration = ContactFactory.Create();

        Console.Clear();
        Console.WriteLine("\n###############-----ADD CONTACT-----###############");

        Console.Write("\nEnter first name: ");
        contactRegistration.FirstName = Console.ReadLine();

        Console.Write("\nEnter last name: ");
        contactRegistration.LastName = Console.ReadLine();

        Console.Write("\nEnter email address: ");
        contactRegistration.Email = Console.ReadLine();

        Console.Write("\nEnter phone number: ");
        contactRegistration.Phone = Console.ReadLine();

        Console.Write("\nEnter address: ");
        contactRegistration.Address = Console.ReadLine();

        Console.Write("\nEnter postcode: ");
        contactRegistration.Postcode = Console.ReadLine();

        Console.Write("\nEnter city: ");
        contactRegistration.City = Console.ReadLine();

        //Ändring - Validera i varje iteration
        if (_contactService.ValidateContact(contactRegistration, out var errors))
        {
            bool result = _contactService.CreateContact(contactRegistration);
            if (result)
            {
                Console.WriteLine("\nContact was added successfully.");
            }
            else
            {
                Console.WriteLine("\nContact was not added.");
            }
        }
        else
        {
            Console.WriteLine("\nContact was not added. The following errors occured: ");
            foreach (var error in errors)
            {
                Console.WriteLine($"\n{error}");
            }
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
            var index = 1;
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
