using Business.Interfaces;
using Business.Services;

namespace Business.Tests.Services;

public class FileService_Tests
{
    [Fact]
    public async Task SaveListToFileAsync_ShouldReturnTrue_WhenFileIsSavedSuccessfully()
    {
        //Arrange
        var content = @"[
            {
            ""Id"": ""12345"",
            ""Name"": ""John Doe"",
            ""Email"": ""john.doe@example.com""
            }
        ]";
        var directoryPath = Path.Combine(Path.GetTempPath(), "directory");
        var fileName = "testfile.json";
        var filePath = Path.Combine(directoryPath, fileName);

        IFileService fileService = new FileService(directoryPath, fileName);

        try
        {
            //Act
            var result = await fileService.SaveListToFileAsync<string>(content);

            //Assert
            Assert.True(result);
        }
        finally
        {
            if (File.Exists(fileName)) File.Delete(fileName);
        }
    }


    [Fact]
    public async Task LoadListFromFileAsync_ShouldReturnCorrectContent()
    {
        //Arrange
        var content = @"[
            {
            ""Id"": ""12345"",
            ""Name"": ""John Doe"",
            ""Email"": ""john.doe@example.com""
            }
        ]";
        var directoryPath = Path.Combine(Path.GetTempPath(), "directory");
        var fileName = "testfile.json";
        var filePath = Path.Combine(directoryPath, fileName);
        await File.WriteAllTextAsync(filePath, content);

        IFileService fileService = new FileService(directoryPath, fileName);

        try
        {
            //Act
            var result = await fileService.LoadListFromFileAsync<string>();

            //Assert
            Assert.Equal(content, result);
        }
        finally
        {
            if (File.Exists(fileName)) File.Delete(fileName);
        }

    }


    [Fact]
    public async Task LoadListFromFileAsync_ShouldReturnNull_WhenFileDoesNotExists()
    {
        //Arrange
        var directoryPath = Path.Combine(Path.GetTempPath(), "testdirectory");
        var fileName = "testfile.json";
        var filePath = Path.Combine(directoryPath, fileName);

        if (Directory.Exists(directoryPath))
            Directory.Delete(directoryPath, true);

        IFileService fileService = new FileService(directoryPath, fileName);

        //Act
        var result = await fileService.LoadListFromFileAsync<string>();

        //Assert
        Assert.Null(result);
    }
}
