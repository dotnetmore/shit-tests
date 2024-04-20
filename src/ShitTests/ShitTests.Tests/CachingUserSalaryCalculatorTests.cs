using NSubstitute;
using ShitTests.Entites;
using ShitTests.Interfaces;
using Xunit;

namespace ShitTests.Tests;

public class CachingUserSalaryCalculatorTests
{
    private readonly CachingUserSalaryCalculator _calculator;
    private readonly ICache _mockCache = Substitute.For<ICache>();
    private readonly IUserSalaryCalculator _mockCalculator = Substitute.For<IUserSalaryCalculator>();

    public CachingUserSalaryCalculatorTests() => 
        _calculator = new CachingUserSalaryCalculator(_mockCalculator, _mockCache);

    [Fact]
    public void CalculateUserSalary_UserInCache_ReturnsCached()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockCache.GetCachedSalary(user).Returns(42);

        var result = _calculator.CalculateUserSalary(user);

        Assert.Equal(42, result);
    }
    
    [Fact]
    public void CalculateUserSalary_UserNotInCache_CalculateAndSave()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockCalculator.CalculateUserSalary(user).Returns(43);

        var result = _calculator.CalculateUserSalary(user);

        Assert.Equal(43, result);
        _mockCache.Received().SaveToCache(43);
    }
    
    [Fact]
    public void CalculateUserSalary_UserNotInCache_CalculateAndNotSaveWhenNull()
    {
        var user = new User { Name = "Test User", Age = 20 };

        _calculator.CalculateUserSalary(user);

        _mockCache.Received(0).SaveToCache(Arg.Any<decimal>());
    }
}