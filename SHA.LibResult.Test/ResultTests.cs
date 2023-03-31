using AutoFixture.Xunit2;

namespace SHA.LibResult.Test;

public class ResultTests
{
    [Theory, AutoData]
    public void CreateResult_WithValue_ReturnSuccessResult(Entity1 entity1)
    {
        //Act
        Result<Entity1> result = Result<Entity1>.Create(entity1);

        //Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFail);
        Assert.Equal(entity1, result.Value);
        Assert.Null(result.Exception);
    }

    [Fact]
    public void CreateResult_WithException_ReturnFailResult()
    {
        //Arrange
        Exception exception = new("Exception was thows");

        //Act
        Result<Entity1> result = Result<Entity1>.Create(exception);

        //Assert
        Assert.NotNull(result);
        Assert.True(result.IsFail);
        Assert.False(result.IsSuccess);
        Assert.Equal(exception, result.Exception);
        Assert.Null(result.Value);
    }
}

public class Entity1
{
    public int EntityId { get; init; }
}