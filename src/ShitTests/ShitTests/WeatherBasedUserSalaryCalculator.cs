using ShitTests.Entites;
using ShitTests.Interfaces;

namespace ShitTests;

public class WeatherBasedUserSalaryCalculator(
    IUserSalaryCalculator baseCalculator,
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
        
        if (weatherService.GetWeather() == "snow")
            return null;

        return salary;
    }
}