using Pure.DI;
using ShitTests.Entites;
using ShitTests.Interfaces;
using ShitTests.Services;
using ShitTests.ValueTypes;

namespace ShitTests;

public class MySuperService(
    [Type(typeof(CachingUserSalaryCalculator))] IUserSalaryCalculator baseCalculator,
    IUserRegistry baseRegistry)
{
    public decimal? CalculateUserSalary(User? user) => 
        baseCalculator.CalculateUserSalary(user);

    public UserId RegisterUser(User user) => 
        baseRegistry.RegisterUser(user);
}