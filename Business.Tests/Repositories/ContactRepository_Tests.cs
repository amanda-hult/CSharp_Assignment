using System.Text.Json;
using Business.Interfaces;
using Business.Repositories;
using Moq;

namespace Business.Tests.Repositories;

public class ContactRepository_Tests
{
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly ContactRepository<TestModel> _contactRepository;

    public ContactRepository_Tests()
    {
        _fileServiceMock = new Mock<IFileService>();
        _contactRepository = new ContactRepository<TestModel>(_fileServiceMock.Object);
    }

    [Fact]
    public void Serialize_ShouldReturnJsonString()
    {
        //Arrange
        var list = new List<TestModel>
        {
            new TestModel { Id = "1", Name = "Test Name", Email = "Test@email" }
        };

        //Act
        var result = _contactRepository.Serialize(list);

        //Assert
        var expectedJson = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        Assert.Equal(expectedJson, result);
    }

    [Fact]
    public void Deserialize_ShouldReturnList()
    {
        //Arrange
        var json = @"[
            {
            ""Id"": ""12345"",
            ""Name"": ""John Doe"",
            ""Email"": ""john.doe@example.com""
            }
        ]";

        //Act
        var result = _contactRepository.Deserialize(json);

        //Assert
        Assert.Equal("12345", result[0].Id);
        Assert.NotNull(result);
        Assert.Equal("John Doe", result[0].Name);
        Assert.Equal("john.doe@example.com", result[0].Email);
    }

    [Fact]
    public void SaveToFile_ShouldCallFileServiceSaveListToFile()
    {
        //Arrange
        var list = new List<TestModel>
        {
            new TestModel { Id = "1", Name = "Test Name", Email = "Test@email.com" }
        };
        _fileServiceMock.Setup(fs => fs.SaveListToFile<TestModel>(It.IsAny<string>())).Returns(true);

        //Act
        var result = _contactRepository.SaveToFile(list);

        //Assert
        Assert.True(result);
        _fileServiceMock.Verify(fs => fs.SaveListToFile<TestModel>(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public void GetFromFile_ShouldReturnList()
    {
        //Arrange
        var json = @"[
            {
            ""Id"": ""12345"",
            ""Name"": ""John Doe"",
            ""Email"": ""john.doe@example.com""
            }
        ]";
        _fileServiceMock.Setup(fs => fs.LoadListFromFile<TestModel>()).Returns(json);

        //Act
        var result = _contactRepository.GetFromFile();

        //Assert
        Assert.Equal("12345", result[0].Id);
        Assert.NotNull(result);
        Assert.Equal("John Doe", result[0].Name);
    }

    public class TestModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
