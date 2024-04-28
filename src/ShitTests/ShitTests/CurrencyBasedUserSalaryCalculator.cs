using Pure.DI;
using ShitTests.Entites;
using ShitTests.Interfaces;

namespace ShitTests;

public class CurrencyBasedUserSalaryCalculator(
    [Type(typeof(EmployeeBasedUserSalaryCalculator))] IUserSalaryCalculator baseCalculator,
    ICurrencyRateProvider currencyRateProvider)
    : IUserSalaryCalculator
{
    public decimal? CalculateUserSalary(User? user)
    {
        if (user is null)
            return null;
        
        var salary = baseCalculator.CalculateUserSalary(user);
        if (!salary.HasValue)
            return null;
        
        return salary * currencyRateProvider.GetRate("USD");
    }
}