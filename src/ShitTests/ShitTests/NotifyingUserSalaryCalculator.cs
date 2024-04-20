using ShitTests.Entites;
using ShitTests.Interfaces;

namespace ShitTests;

public class NotifyingUserSalaryCalculator(
    IUserSalaryCalculator baseCalculator,
    IEmailSender emailSender,
    IEmailRegistry emailRegistry,
    ITimeService timeService)
    : IUserSalaryCalculator
{
    public decimal? CalculateUserSalary(User? user)
    {
        if (user is null)
            return null;
        
        var salary = baseCalculator.CalculateUserSalary(user);
        if (!salary.HasValue)
            return null;
        
        var hour = timeService.GetTime().Hour;
        if (hour is > 9 and < 18)
            emailSender.SendEmail(emailRegistry.GetEmail(user), $"New salary {salary}");

        return salary;
    }
}