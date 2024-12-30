using Business.Helpers;
using Business.Models;

namespace Business.Tests.Helpers;

public class InputValidator_Tests
{
    private readonly InputValidator _validator;

    public InputValidator_Tests()
    {
        _validator = new InputValidator();
    }

    [Fact]
    public void GetValidatedInput_ShouldReturnInput_WhenInputIsValid()
    {
        //Arrange
        var inputProvider = () => "Anna";

        //Act
        var result = _validator.GetValidatedInput<ContactRegistration>(
            "Enter first name: ",
            nameof(ContactRegistration.FirstName),
            inputProvider);

        //Assert
        Assert.Equal("Anna", result);
    }

    [Fact]
    public void GetValidatedInput_ShouldWorkForDifferentProperties()
    {
        //Arrange
        var validFirstName = "Anna";
        var validEmail = "anna@domain.com";
        var inputQueue = new Queue<string>([validFirstName, validEmail]);
        var inputProvider = inputQueue.Dequeue;

        //Act
        var firstName = _validator.GetValidatedInput<ContactRegistration>(
            "Enter first name: ",
            nameof(ContactRegistration.FirstName),
            inputProvider);

        var email = _validator.GetValidatedInput<ContactRegistration>(
            "Enter email: ",
            nameof(ContactRegistration.Email),
            inputProvider);

        //Assert
        Assert.Equal("Anna", firstName);
        Assert.Equal("anna@domain.com", email);
    }

    [Fact]
    public void GetValidatedInput_ShoulRequestInputAgain_WhenInputIsInvalid()
    {
        //Arrange
        var invalidInput = "";
        var validInput = "Anna";
        var inputProvider = new Queue<string>([invalidInput, validInput]).Dequeue;

        //Act
        var result = _validator.GetValidatedInput<ContactRegistration>(
            "Enter first name: ",
            nameof(ContactRegistration.FirstName),
            inputProvider);

        //Assert
        Assert.Equal("Anna", result);
    }
}
