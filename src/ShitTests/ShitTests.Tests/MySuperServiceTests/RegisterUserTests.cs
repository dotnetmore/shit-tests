using System;
using NSubstitute;
using ShitTests.Entites;
using ShitTests.ValueTypes;
using Xunit;

namespace ShitTests.Tests.MySuperServiceTests;

public class RegisterUserTests : MySuperServiceTestsBase
{
    [Fact]
    public void RegisterUser_NameIsNullOrWhiteSpace_ThrowsArgumentException()
    {
        var user = new User { Name = "", Age = 20 };

        var exception = Assert.Throws<ArgumentException>(() => MySuperService.RegisterUser(user));

        Assert.Equal("Name is required", exception.Message);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(42_000)]
    public void RegisterUser_AgeIsInvalid_ThrowsArgumentException(int age)
    {
        var user = new User { Name = "Test User", Age = age };

        var exception = Assert.Throws<ArgumentException>(() => MySuperService.RegisterUser(user));

        Assert.Equal("Age is invalid", exception.Message);
    }

    [Fact]
    public void RegisterUser_UserIsValid_ReturnsUserId()
    {
        var user = new User { Name = "Test User", Age = 20 };
        MockDatabase.AddUser(user).Returns(3);
        var expectedUserId = new UserId(3);

        var result = MySuperService.RegisterUser(user);

        Assert.Equal(result, expectedUserId);
    }
}