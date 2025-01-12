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
    public async Task CreateContact_ShouldReturnSuccessTrue_WhenContactIsCreatedSuccessfully()
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
            .Setup(repo => repo.SaveToFileAsync(It.IsAny<List<StoredContact>>()))
            .ReturnsAsync(true);

        //Act
        var result = await _contactService.CreateContact(contactRegistration);

        //Assert
        Assert.True(result.Success);
        Assert.Equal(expectedStoredContact.FirstName, result.Result.FirstName);
        Assert.Equal(expectedStoredContact.Postcode, result.Result.Postcode);
        Assert.Equal("Contact was created successfully", result.Message);
        Assert.NotNull(result.Result);
    }

    [Fact]
    public async Task GetAllContacts_ShouldReturnResponseResultTrue_WhenListIsLoadedSuccessfully()
    {
        //Arrange
        var storedContacts = new List<StoredContact>
        {
            new() {
                ContactId = "1",
                FirstName = "Anna",
                LastName = "Larsson",
                Email = "anna@domain.com",
            },
            new() {
                ContactId = "2",
                FirstName = "Johan",
                LastName = "Andersson",
                Email = "johan@domain.com",
            }
        };
        var expectedDisplayedContacts = new List<DisplayedContact>
        {
            new() {
                ContactId = "1",
                FirstName = "Anna",
                LastName = "Larsson",
                Email = "anna@domain.com",
            },
            new() {
                ContactId = "2",
                FirstName = "Johan",
                LastName = "Andersson",
                Email = "johan@domain.com",
            }
        };

        _contactRepositoryMock
            .Setup(repo => repo.GetFromFileAsync())
            .ReturnsAsync(storedContacts);

        //Act
        var result = await _contactService.GetAllContacts();

        //Assert
        Assert.True(result.Success);
        Assert.Equal(expectedDisplayedContacts.Count, result.Result.Count);
    }


    [Fact]
    public async Task GetStoredContactById_ShouldReturnCorrectStoredContact()
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

        _contactRepositoryMock
            .Setup(repo => repo.GetFromFileAsync())
            .ReturnsAsync(contacts);

        var contactService = new ContactService(_contactRepositoryMock.Object);
        await contactService.InitializeAsync();

        //Act
        var result = contactService.GetStoredContactById("1");

        //Assert
        Assert.Equal(storedContact1, result);
        Assert.NotNull(result);

        _contactRepositoryMock.Verify(repo => repo.GetFromFileAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteContact_ShouldReturnTrue_WhenContactIsSuccessfullyDeleted()
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

        _contactRepositoryMock
            .Setup(repo => repo.GetFromFileAsync())
            .ReturnsAsync(contacts);

        var contactService = new ContactService(_contactRepositoryMock.Object);
        await contactService.InitializeAsync();

        //Act
        var result = await contactService.DeleteContact(1);

        //Assert
        Assert.Single(contacts);
        Assert.Equal(storedContact1, contacts[0]);
    }

    [Fact]
    public async Task SaveContacts_ShouldCallContactRepositorySaveToFileAsync()
    {
        //Arrange
        var storedContact1 = new StoredContact
        {
            FirstName = "Anna",
            LastName = "Larsson",
            Email = "anna@domain.com",
            Phone = "1234567890",
            Address = "TestAddress 1",
            Postcode = "12345",
            City = "TestCity 1"
        };

        var contacts = new List<StoredContact> { storedContact1 };

        _contactRepositoryMock
            .Setup(repo => repo.SaveToFileAsync(It.IsAny<List<StoredContact>>()))
            .ReturnsAsync(true);

        var contactService = new ContactService(_contactRepositoryMock.Object);

        //Act
        await contactService.SaveContacts();

        //Assert
        Assert.Single(contacts);
        _contactRepositoryMock.Verify(repo => repo.SaveToFileAsync(It.IsAny<List<StoredContact>>()), Times.Once());
    }

    [Fact]
    public async Task IsEmailAvailable_ShouldReturnResponseResultTrue_WhenEmailDoesNotExistInList()
    {
        //Arrange
        var contacts = new List<StoredContact>
        {
            new() { Email = "existing@domain.com "},
        };

        _contactRepositoryMock
            .Setup(repo => repo.GetFromFileAsync())
            .ReturnsAsync(contacts);
        
        var contactService = new ContactService(_contactRepositoryMock.Object);
        await contactService.InitializeAsync();

        //Act
        var result = contactService.IsEmailAvailable("newemail@domain.com");
        
        //Assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task IsEmailAvailable_ShouldReturnResponseResultFalse_WhenEmailIsNotAvailable()
    {
        //Arrange
        var contacts = new List<StoredContact>
        {
            new() { Email = "test@domain.com"},
        };

        _contactRepositoryMock
            .Setup(repo => repo.GetFromFileAsync())
            .ReturnsAsync(contacts);

        var contactService = new ContactService(_contactRepositoryMock.Object);
        await contactService.InitializeAsync();

        //Act
        var result = contactService.IsEmailAvailable("test@domain.com");

        //Assert
        Assert.False(result.Success);
        Assert.Equal("The email address is not available.", result.Message);
    }
}
