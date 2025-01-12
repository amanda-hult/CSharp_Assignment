using System.ComponentModel.DataAnnotations;
using Business.Interfaces;

namespace Business.Helpers;

public class InputValidator
{
    private readonly IContactService _contactService;
    public InputValidator(IContactService contactService)
    {
        _contactService = contactService;
    }

    public string GetValidatedInput<T>(string prompt, string propertyName, Func<string> inputProvider) where T : new()
    {
        while (true)
        {
            Console.Write(prompt);
            var input = inputProvider()?.Trim();

            var tempContact = new T();
            typeof(T).GetProperty(propertyName)?.SetValue(tempContact, input);

            var results = new List<ValidationResult>();
            var context = new ValidationContext(tempContact) { MemberName = propertyName };

            if (Validator.TryValidateProperty(input, context, results))
            {
                return input;
            }

            Console.WriteLine($"{results[0].ErrorMessage}. Please try again.");
        }
    }

    public string GetValidatedEmail<T>(string prompt, string propertyName, Func<string> inputProvider, string? existingEmail = null) where T : new()
    {
        while (true)
        {
            string email = GetValidatedInput<T>(prompt, propertyName, inputProvider);

            //keep existing email if user leaves input field empty when updating contact
            if (string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(existingEmail))
            {
                return existingEmail;
            }
            
            var emailCheck = _contactService.IsEmailAvailable(email);
            if (emailCheck.Success)
            {
                return email;
            }
            else
            {
                Console.WriteLine($"{emailCheck.Message}");
            }
        }          
    }
        
    public string GetOptionalValidatedInput<T>(string prompt, string propertyName, T existingObject, Func<string> inputProvider) where T : new()
    {
        while (true)
        {
            Console.Write(prompt);
            var input = inputProvider()?.Trim();

            //allow user to leave field empty and keep current information
            if (string.IsNullOrEmpty(input))
            {
                var currentValue = typeof(T)
                    .GetProperty(propertyName)?
                    .GetValue(existingObject)?
                    .ToString();
                return currentValue ?? string.Empty;
            }

            //validate input if user chooses to register new information
            var tempContact = new T();
            typeof(T).GetProperty(propertyName)?.SetValue(tempContact, input);

            var results = new List<ValidationResult>();
            var context = new ValidationContext(tempContact) { MemberName = propertyName };

            if (Validator.TryValidateProperty(input, context, results))
            {
                return input;
            }

            Console.WriteLine($"{results[0].ErrorMessage}. Please try again.");
        }
    }
}
