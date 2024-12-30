using Business.Models;

namespace Presentation.ConsoleApp.Helpers;

public static class DisplayHelper
{
    public static void ShowHeader(string title)
    {
        Console.WriteLine($"\n###############-----{title}-----###############");
    }

    public static void ShowMessage(string message, bool pause = false)
    {
        Console.WriteLine($"\n{message}");
        if (pause)
            Console.ReadKey();
    }

    public static void DisplayContacts(List<DisplayedContact> contacts)
    {
        Console.Clear();
        Console.WriteLine("\nAll contacts:");
        for (int i = 0; i < contacts.Count; i++)
        {
            Console.WriteLine($"\n{i + 1}. {GetBriefContactDetails(contacts[i])}");
        }
    }
    public static string GetBriefContactDetails(DisplayedContact contact)
    {
        return $"{contact.FirstName} {contact.LastName} - {contact.City}";
    }

    public static string GetContactDetails(DisplayedContact contact)
    {
        return $"Name: {contact.FirstName} {contact.LastName}\n" +
               $"Email: {contact.Email}\n" +
               $"Phone: {contact.Phone}\n" +
               $"Address: {contact.Address} {contact.Postcode} {contact.City}\n";
    }
}
