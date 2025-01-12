using Business.Helpers;
using Business.Interfaces;
using Business.Models;
using Moq;

namespace Business.Tests.Helpers;

public class InputValidator_Tests
{
    private readonly Mock<IContactService> _contactServiceMock;
    private readonly InputValidator _validator;

    public InputValidator_Tests()
    {
        _contactServiceMock = new Mock<IContactService>();
        _validator = new InputValidator(_contactServiceMock.Object);
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

    [Fact]
    public void GetValidatedEmail_ShouldReturnExistingEmail_WhenInputIsEmpty()
    {
        //Arrange
        var existingEmail = "anna@domain.com";
        var inputQueue = new Queue<string>(["", existingEmail]);
        var inputProvider = inputQueue.Dequeue;

        _contactServiceMock
            .Setup(cs => cs.IsEmailAvailable(It.IsAny<string>()))
            .Returns(new ResponseResult<ContactRegistration>
            {
                Success = true,
                Message = ""
            
            });

        //Act
        var result = _validator.GetValidatedEmail<ContactRegistration>(
            "Enter email: ",
            nameof(ContactRegistration.Email),
            inputProvider,
            existingEmail);

        //Assert
        Assert.Equal(existingEmail , result);
    }

    [Fact]
    public void GetOptionalValidatedInput_ShouldReturnExistingValue_WhenInputIsEmpty()
    {
        //Arrange
        var existingContact = new StoredContact { FirstName = "Anna" };
        var inputQueue = new Queue<string>([""]);
        var inputProvider = inputQueue.Dequeue;

        //Act
        var result = _validator.GetOptionalValidatedInput<StoredContact>(
            "Enter new first name: ",
            nameof(StoredContact.FirstName),
            existingContact,
            inputProvider);

        //Assert
        Assert.Equal(existingContact.FirstName, result);
    }

    [Fact]
    public void GetOptionalValidatedInput_ShouldReturnInput_WhenInputIsValid()
    {
        //Arrange
        var existingObject = new StoredContact { FirstName = "Anna" };
        var inputProvider = new Queue<string>(["John"]).Dequeue;

        //Act
        var result = _validator.GetOptionalValidatedInput<StoredContact>(
        "Enter new first name: ",
        nameof(StoredContact.FirstName),
        existingObject,
        inputProvider);

        //Assert
        Assert.Equal("John", result);
    }
}
