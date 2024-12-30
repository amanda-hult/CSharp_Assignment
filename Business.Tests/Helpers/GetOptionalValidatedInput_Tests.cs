using Business.Helpers;
using Business.Models;

namespace Business.Tests.Helpers;

public class GetOptionalValidatedInput_Tests
{
    private readonly InputValidator _validator;

    public GetOptionalValidatedInput_Tests()
    {
        _validator = new InputValidator();
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

    [Fact]
    public void GetOptionalValidatedInput_ShouldReturnExistingValue_WhenInputIsEmpty()
    {
        //Arrange
        var existingObject = new StoredContact { FirstName = "Anna" };
        var inputProvider = new Queue<string>([""]).Dequeue;

        //Act
        var result = _validator.GetOptionalValidatedInput<StoredContact>(
        "Enter new first name: ",
        nameof(StoredContact.FirstName),
        existingObject,
        inputProvider);

        //Assert
        Assert.Equal("Anna", result);
    }
}
