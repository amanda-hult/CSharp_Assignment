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
            new() { Id = "1", Name = "Test Name", Email = "Test@email" }
        };
        string expectedJson = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });

        //Act
        var result = _contactRepository.Serialize(list);

        //Assert
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
        Assert.Equal("12345", result![0].Id);
        Assert.NotNull(result);
        Assert.Equal("John Doe", result[0].Name);
        Assert.Equal("john.doe@example.com", result[0].Email);
    }

    [Fact]
    public async Task SaveToFileAsync_ShouldCallFileServiceSaveListToFileAsync()
    {
        //Arrange
        var list = new List<TestModel>
        {
            new TestModel { Id = "1", Name = "Test Name", Email = "Test@email.com" }
        };
        var serializedList = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });

        _fileServiceMock.Setup(fs => fs.SaveListToFileAsync<TestModel>(It.Is<string>(json => json == serializedList)))
            .ReturnsAsync(true);

        //Act
        var result = await _contactRepository.SaveToFileAsync(list);

        //Assert
        Assert.True(result);
        _fileServiceMock.Verify(fs => fs.SaveListToFileAsync<TestModel>(It.Is<string>(json => json == serializedList)), Times.Once());
    }

    [Fact]
    public async Task GetFromFileAsync_ShouldReturnList()
    {
        //Arrange
        var json = @"[
            {
            ""Id"": ""12345"",
            ""Name"": ""John Doe"",
            ""Email"": ""john.doe@example.com""
            }
        ]";
        var deserializedList = JsonSerializer.Deserialize<List<TestModel>>(json, new JsonSerializerOptions { WriteIndented = true });

        _fileServiceMock.Setup(fs => fs.LoadListFromFileAsync<TestModel>()).ReturnsAsync(json);

        //Act
        var result = await _contactRepository.GetFromFileAsync();

        //Assert
        Assert.Equal("12345", result![0].Id);
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
