using Business.Helpers;

namespace Business.Tests.Helpers;

public class UniqueIdGenerator_Tests
{
    [Fact]
    public void GenerateUniqueId_ShouldReturnValidGuid()
    {
        //act
        var result = UniqueIdGenerator.GenerateUniqueId();

        //assert
        Assert.True(Guid.TryParse(result, out Guid guid));
        Assert.False(string.IsNullOrWhiteSpace(result));
    }

    [Fact]
    public void GenerateUniqueId_ShouldThrowException_WhenFormatIsNull()
    {
        //assert
        Assert.Throws<ArgumentException>(() => UniqueIdGenerator.GenerateUniqueId(string.Empty));   
    }
}
