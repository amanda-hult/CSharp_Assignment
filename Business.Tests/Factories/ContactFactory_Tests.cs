using Business.Factories;
using Business.Models;

namespace Business.Tests.Factories;

public class ContactFactory_Tests
{
    [Fact]
    public void Create_ShouldReturnSuccessTrue_WhenInputIsValid()
    {
        //Arrange
        var contactRegistration = new ContactRegistration
        {
            FirstName = "Anna",
            LastName = "Larsson ",
            Email = "Anna.Larsson@domail.COM",
            Phone = "1234567890",
            Address = "ExampleStreet ",
            Postcode = "12345",
            City = "ExampleCity"
        };

        //Act
        var result = ContactFactory.Create(contactRegistration);

        //Assert
        Assert.True(result.Success);
        Assert.Equal("Contact was created successfully", result.Message);
        Assert.NotNull(result.Result);
        Assert.Equal("Anna", result.Result.FirstName);
        Assert.Equal("Larsson", result.Result.LastName);
        Assert.Equal("anna.larsson@domail.com", result.Result.Email);
        Assert.Equal("1234567890", result.Result.Phone);
        Assert.Equal("ExampleStreet", result.Result.Address);
        Assert.Equal("12345", result.Result.Postcode);
        Assert.Equal("ExampleCity", result.Result.City);
    }

    [Fact]
    public void Create_ShouldReturnSuccessFalse_WhenInputIsNull()
    {
        //Arrange
        ContactRegistration? contactRegistration = null;

        //Act
        var result = ContactFactory.Create(contactRegistration!);

        //Assert
        Assert.False(result.Success);
        Assert.Contains("Failed to create StoredContact", result.Message);
        Assert.Null(result.Result);
    }
}
