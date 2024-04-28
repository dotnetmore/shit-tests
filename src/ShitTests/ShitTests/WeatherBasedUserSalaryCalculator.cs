using Pure.DI;
using ShitTests.Entites;
using ShitTests.Interfaces;

namespace ShitTests;

public class WeatherBasedUserSalaryCalculator(
    [Type(typeof(SafeUserSalaryCalculator))] IUserSalaryCalculator baseCalculator,
    IWeatherService weatherService)
    : IUserSalaryCalculator
{
    public decimal? CalculateUserSalary(User? user)
    {
        if (user is null)
            return null;
        
        var salary = baseCalculator.CalculateUserSalary(user);
        
        if (!salary.HasValue)
            return null;
        
        return weatherService.GetWeather() == "snow" ? null : salary;
    }
}