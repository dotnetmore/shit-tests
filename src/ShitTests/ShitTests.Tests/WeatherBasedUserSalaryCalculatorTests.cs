using NSubstitute;
using ShitTests.Entites;
using ShitTests.Interfaces;
using Xunit;

namespace ShitTests.Tests;

public class WeatherBasedUserSalaryCalculatorTests
{
    private readonly WeatherBasedUserSalaryCalculator _calculator;
    private readonly IWeatherService _mockWeatherService = Substitute.For<IWeatherService>();
    private readonly IUserSalaryCalculator _mockCalculator = Substitute.For<IUserSalaryCalculator>();

    public WeatherBasedUserSalaryCalculatorTests() => 
        _calculator = new WeatherBasedUserSalaryCalculator(_mockCalculator, _mockWeatherService);

    [Fact]
    public void CalculateUserSalary_IsSnow_ReturnNull()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockCalculator.CalculateUserSalary(user).Returns(43);
        _mockWeatherService.GetWeather().Returns("snow");
        
        var result = _calculator.CalculateUserSalary(user);

        Assert.Equal(null, result);
    }
    
    [Fact]
    public void CalculateUserSalary_HasNoSalary_ReturnSalary()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockCalculator.CalculateUserSalary(user).Returns(43);
        _mockWeatherService.GetWeather().Returns("sunny");
        
        var result = _calculator.CalculateUserSalary(user);

        Assert.Equal(43, result);
    }
}