using System.ComponentModel.DataAnnotations;
using Business.Models;

namespace Business.Helpers;

public class InputValidator
{
    public string GetValidatedInput<T>(string prompt, string propertyName) where T : new()
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();

            var tempContact = new T();
            typeof(T).GetProperty(propertyName)?.SetValue(tempContact, input);

            var results = new List<ValidationResult>();
            var context = new ValidationContext(tempContact) { MemberName = propertyName };

            if (Validator.TryValidateProperty(input, context, results))
            {
                return input;
            }

            Console.WriteLine($"{results[0].ErrorMessage}. Please try again");
        }
    }

    public string GetOptionalValidatedInput<T>(string prompt, string propertyName, T existingObject) where T : new()
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim();

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

            Console.WriteLine($"{results[0].ErrorMessage}. Please try again");
        }
    }
}
