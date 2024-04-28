using Pure.DI;
using ShitTests.Entites;
using ShitTests.Interfaces;

namespace ShitTests;

public class CachingUserSalaryCalculator(
    [Type(typeof(NotifyingUserSalaryCalculator))] IUserSalaryCalculator baseCalculator,
    ICache cache)
    : IUserSalaryCalculator
{
    public decimal? CalculateUserSalary(User? user)
    {
        if (user is null)
            return null;
        
        if (cache.GetCachedSalary(user) is { } cached)
            return cached;

        var salary = baseCalculator.CalculateUserSalary(user);
        if (!salary.HasValue)
        {
            return null;
        }
        
        cache.SaveToCache(salary.Value);
        return salary;
    }
}