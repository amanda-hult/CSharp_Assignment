using Business.Interfaces;
using Moq;

namespace Business.Tests.Services;

public class FileService_Tests
{
    private readonly Mock<IFileService> _fileServiceMock;

    public FileService_Tests()
    {
        _fileServiceMock = new Mock<IFileService> ();
    }

    [Fact]
    public void SaveListToFile_ShouldReturnTrue_WhenFileIsSavesSuccessfully()
    {
        //Arrange
        var content = @"[
            {
            ""Id"": ""12345"",
            ""Name"": ""John Doe"",
            ""Email"": ""john.doe@example.com""
            }
        ]";

        _fileServiceMock
            .Setup(fs => fs.SaveListToFile<string>(content))
            .Returns(true);

        //Act
        var result = _fileServiceMock.Object.SaveListToFile<string>(content);

        //Assert
        Assert.True(result);
        _fileServiceMock.Verify(fs => fs.SaveListToFile<string>(content), Times.Once());
    }


    [Fact]
    public void LoadListFromFile_ShouldReturnCorrectContent()
    {
        //Arrange
        var content = "Test Content";

        _fileServiceMock
        .Setup(fs => fs.LoadListFromFile<string>())
        .Returns(content);

        //Act
        var result = _fileServiceMock.Object.LoadListFromFile<string>();

        //Assert
        Assert.Equal(content, result);
        _fileServiceMock.Verify(fs => fs.LoadListFromFile<string>(), Times.Once());
    }

    [Fact]
    public void LoadListFromFile_ShouldReturnNull_WhenFileDoesNotExists()
    {
        //Arrange
        _fileServiceMock
        .Setup(fs => fs.LoadListFromFile<string>())
        .Returns((string)null!);

        //Act
        var result = _fileServiceMock.Object.LoadListFromFile<string>();

        //Assert
        Assert.Null(result);
        _fileServiceMock.Verify(fs => fs.LoadListFromFile<string>(), Times.Once());
    }
}
