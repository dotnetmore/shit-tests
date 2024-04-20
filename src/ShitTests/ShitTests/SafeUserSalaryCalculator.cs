using ShitTests.Entites;
using ShitTests.Exceptions;
using ShitTests.Interfaces;

namespace ShitTests;

public class SafeUserSalaryCalculator(
    IUserSalaryCalculator baseCalculator,
    ISecurityChecker securityChecker,
    ICashier cashier,
    IGovernmentService governmentService)
    : IUserSalaryCalculator
{
    public decimal? CalculateUserSalary(User? user)
    {
        if (user is null)
            return null;
        
        if (user.Age < governmentService.AgeToStartWork)
            throw new LaborLawViolationException("User is too young to work");

        if (!governmentService.IsUserAlive(user))
            throw new DeadSpiritsViolationException("User is dead");

        if (!securityChecker.Validate(user))
            throw new UnauthorizedAccessException("User is not authorized");

        var salary = baseCalculator.CalculateUserSalary(user);
        
        if (!salary.HasValue)
            return null;
        
        if (!cashier.IsEnoughCash(salary.Value))
            throw new InsufficientFundsException("Not enough cash to pay salary");

        return salary;
    }
}