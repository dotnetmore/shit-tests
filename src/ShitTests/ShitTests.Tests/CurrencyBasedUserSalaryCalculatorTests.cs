using NSubstitute;
using ShitTests.Entites;
using ShitTests.Interfaces;
using Xunit;

namespace ShitTests.Tests;

public class CurrencyBasedUserSalaryCalculatorTests
{
    private readonly CurrencyBasedUserSalaryCalculator _calculator;
    private readonly ICurrencyRateProvider _mockCurrencyRateProvider = Substitute.For<ICurrencyRateProvider>();
    private readonly IUserSalaryCalculator _mockCalculator = Substitute.For<IUserSalaryCalculator>();

    public CurrencyBasedUserSalaryCalculatorTests() => 
        _calculator = new CurrencyBasedUserSalaryCalculator(_mockCalculator, _mockCurrencyRateProvider);

    [Fact]
    public void CalculateUserSalary_HasSalary_Recalculate()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockCalculator.CalculateUserSalary(user).Returns(43);
        _mockCurrencyRateProvider.GetRate("USD").Returns(2);
        
        var result = _calculator.CalculateUserSalary(user);

        Assert.Equal(43 * 2, result);
    }
    
    [Fact]
    public void CalculateUserSalary_HasNoSalary_ReturnNull()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockCurrencyRateProvider.GetRate("USD").Returns(2);
        
        var result = _calculator.CalculateUserSalary(user);

        Assert.Equal(null, result);
    }
}