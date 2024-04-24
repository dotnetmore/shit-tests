using ShitTests.Entites;
using ShitTests.Interfaces;

namespace ShitTests;

public class MySuperService: IUserSalaryCalculator
{
    private readonly IUserSalaryCalculator _calculator;
    
    public MySuperService(
        ICache cache,
        ICurrencyRateProvider currencyRateProvider,
        ISecurityChecker securityChecker,
        IEmploymentService employmentService,
        ICashier cashier,
        IEmailSender emailSender,
        IGovernmentService governmentService,
        IWeatherService weatherService,
        ITimeService timeService,
        IEmailRegistry emailRegistry,
        ISalaryCalculator salaryCalculator)
    {
        var employeeBased = new EmployeeBasedUserSalaryCalculator(
            employmentService,
            salaryCalculator);

        var currencyBased = new CurrencyBasedUserSalaryCalculator(
            employeeBased,
            currencyRateProvider);
        
        var safe = new SafeUserSalaryCalculator(
            currencyBased,
            securityChecker,
            cashier,
            governmentService);

        var weather = new WeatherBasedUserSalaryCalculator(
            safe,
            weatherService);
        
        var notifying = new NotifyingUserSalaryCalculator(
            weather,
            emailSender,
            emailRegistry,
            timeService);

        var caching = new CachingUserSalaryCalculator(
            notifying,
            cache);

        _calculator = caching;
    }

    public MySuperService(IUserSalaryCalculator calculator)
    {
        _calculator = calculator;
    }

    public decimal? CalculateUserSalary(User? user) => 
        _calculator.CalculateUserSalary(user);
}