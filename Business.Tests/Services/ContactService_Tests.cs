using System.Text.Json;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Moq;

namespace Business.Tests.Services;

public class ContactService_Tests
{
    private readonly Mock<IContactRepository<StoredContact>> _contactRepositoryMock;
    private readonly ContactService _contactService;

    public ContactService_Tests()
    {
        _contactRepositoryMock = new Mock<IContactRepository<StoredContact>>();
        _contactService = new ContactService(_contactRepositoryMock.Object);
    }

    [Fact]
    public void CreateContact_ShouldReturnSuccessTrue_WhenContactIsCreatedSuccessfully()
    {
        //Arrange
        var contactRegistration = new ContactRegistration
        {
            FirstName = "Anna",
            LastName = "Larsson",
            Email = "anna@domain.com",
            Phone = "1234567890",
            Address = "TestAddress",
            Postcode = "12345",
            City = "TestCity"
        };

        var expectedStoredContact = new StoredContact
        {
            ContactId = "1",
            FirstName = "Anna",
            LastName = "Larsson",
            Email = "anna@domain.com",
            Phone = "1234567890",
            Address = "TestAddress",
            Postcode = "12345",
            City = "TestCity"
        };
        _contactRepositoryMock
            .Setup(repo => repo.SaveToFile(It.IsAny<List<StoredContact>>()))
            .Returns(true);

        //Act
        var result = _contactService.CreateContact(contactRegistration);

        //Assert
        Assert.True(result.Success);
        Assert.Equal(expectedStoredContact.FirstName, result.Result.FirstName);
        Assert.Equal(expectedStoredContact.Postcode, result.Result.Postcode);
        Assert.Equal("Contact was created successfully", result.Message);
        Assert.NotNull(result.Result);
    }

    [Fact]
    public void CreateContact_ShouldReturnSuccessFalse_WhenContactIsNotCreatedSuccessfully()
    {
        //Arrange
        var contactRegistration = new ContactRegistration
        {
            FirstName = "Anna",
            LastName = "Larsson",
            Email = "anna@domain.com",
            Phone = "1234567890",
            Address = "TestAddress",
            Postcode = "12345",
            City = "TestCity"
        };

        _contactRepositoryMock
            .Setup(repo => repo.SaveToFile(It.IsAny<List<StoredContact>>()))
            .Throws(new Exception("Failed to save contact"));

        //Act
        var result = _contactService.CreateContact(contactRegistration);

        //Assert
        Assert.False(result.Success);
        Assert.Equal("Failed to create StoredContact", result.Message);
    }

    [Fact]
    public void GetAllContacts_ShouldReturnResponseResultTrue_WhenListIsLoadedSuccessfully()
    {
        //Arrange
        var storedContacts = new List<StoredContact>
        {
            new() {
                ContactId = "1",
                FirstName = "Anna",
                LastName = "Larsson",
                Email = "anna@domain.com",
                Phone = "1234567890",
                Address = "TestAddress 1",
                Postcode = "12345",
                City = "TestCity 1"
            },
            new() {
                ContactId = "2",
                FirstName = "Johan",
                LastName = "Andersson",
                Email = "johan@domain.com",
                Phone = "0987654321",
                Address = "TestAddress 2",
                Postcode = "54321",
                City = "TestCity 2"
            }
        };
        var expectedDisplayedContacts = new List<DisplayedContact>
        {
            new DisplayedContact
            {
                ContactId = "1",
                FirstName = "Anna",
                LastName = "Larsson",
                Email = "anna@domain.com",
                Phone = "1234567890",
                Address = "TestAddress 1",
                Postcode = "12345",
                City = "TestCity 1"
            },
            new DisplayedContact
            {
                ContactId = "2",
                FirstName = "Johan",
                LastName = "Andersson",
                Email = "johan@domain.com",
                Phone = "0987654321",
                Address = "TestAddress 2",
                Postcode = "54321",
                City = "TestCity 2"
            }
        };

        _contactRepositoryMock
            .Setup(repo => repo.GetFromFile())
            .Returns(storedContacts);

        //Act
        var result = _contactService.GetAllContacts();

        //Assert
        Assert.True(result.Success);
        Assert.Equal(expectedDisplayedContacts.Count, result.Result.Count);
    }

    //[Fact]
    //public void GetAllContacts_ShouldReturnResponseResultFalse_WhenListIsNotLoadedSuccessfully()
    //{
    //    //Arrange

    //    //Act

    //    //Assert

    //}

    [Fact]
    public void GetStoredContactById_ShouldReturnCorrectStoredContact()
    {
        //Arrange
        var storedContact1 = new StoredContact
        {
            ContactId = "1",
            FirstName = "Anna",
            LastName = "Larsson",
            Email = "anna@domain.com",
            Phone = "1234567890",
            Address = "TestAddress 1",
            Postcode = "12345",
            City = "TestCity 1"
        };
        var storedContact2 = new StoredContact
        {
            ContactId = "2",
            FirstName = "Johan",
            LastName = "Andersson",
            Email = "johan@domain.com",
            Phone = "0987654321",
            Address = "TestAddress 2",
            Postcode = "54321",
            City = "TestCity 2"
        };

        var contacts = new List<StoredContact> { storedContact1, storedContact2 };

 
        //Mocka ifileservice
        _contactRepositoryMock
            .Setup(repo => repo.GetFromFile())
                .Returns(contacts);

        var contactService = new ContactService(_contactRepositoryMock.Object);


        //Act
        var result = _contactService.GetStoredContactById("2");

        //Assert
        Assert.Equal(storedContact2, result);
        Assert.NotNull(result);
    }




    ////Arrange
    //var storedContact = new StoredContact
    //{
    //    ContactId = "1",
    //    FirstName = "Anna",
    //    LastName = "Larsson",
    //    Email = "anna@domain.com",
    //    Phone = "1234567890",
    //    Address = "TestAddress 1",
    //    Postcode = "12345",
    //    City = "TestCity 1"
    //};

    //var contacts = new List<StoredContact> { storedContact };
    //contacts.Add(storedContact);


    //    //Act
    //    var result = _contactService.GetStoredContactById("1");

    ////Assert
    //Assert.Equal(storedContact, result);
    //    Assert.NotNull(result);
}
