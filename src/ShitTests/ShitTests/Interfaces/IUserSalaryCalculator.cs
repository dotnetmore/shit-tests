using ShitTests.Entites;

namespace ShitTests.Interfaces;

public interface IUserSalaryCalculator
{
    decimal? CalculateUserSalary(User? user);
}