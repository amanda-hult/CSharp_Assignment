namespace Business.Models;

public class DisplayedContact : BaseContact
{
    //empty constructor as an alternative to setting the parameters directly in DisplayedContact Create() in ContactFactory
    public DisplayedContact() { }
    public DisplayedContact(string firstName, string lastName, string email, string phone, string address, string postcode, string city)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Address = address;
        Postcode = postcode;
        City = city;
    }

    public string GetBriefContactDetails()
    {
        return $"{FirstName} {LastName} - {City}";
    }

    public string GetContactDetails()
    {
        return $"Name: {FirstName} {LastName}\n" +
               $"Email: {Email}\n" +
               $"Phone: {Phone}\n" +
               $"Address: {Address} {Postcode} {City}\n";
    }
}
