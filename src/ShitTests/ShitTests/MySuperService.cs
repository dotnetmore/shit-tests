using ShitTests.Entites;
using ShitTests.Exceptions;
using ShitTests.Interfaces;
using ShitTests.ValueTypes;

namespace ShitTests;

public class MySuperService
{
    private readonly IDatabase _database;
    private readonly ICash _cash;
    private readonly ICurrencyRateProvider _currencyRateProvider;
    private readonly ISecurityChecker _securityChecker;
    private readonly IEmploymentService _employmentService;
    private readonly ICashier _cashier;
    private readonly IEmailSender _emailSender;
    private readonly IGovernmentService _governmentService;
    private readonly IWeatherService _weatherService;
    private readonly ITimeService _timeService;
    private readonly IEmailRegistry _emailRegistry;
    private readonly MySimpleService _mySimpleService;

    public MySuperService(IDatabase database,
        ICash cash,
        ICurrencyRateProvider currencyRateProvider,
        ISecurityChecker securityChecker,
        IEmploymentService employmentService,
        ICashier cashier,
        IEmailSender emailSender,
        IGovernmentService governmentService,
        IWeatherService weatherService,
        ITimeService timeService,
        IEmailRegistry emailRegistry,
        MySimpleService mySimpleService)
    {
        _database = database;
        _cash = cash;
        _currencyRateProvider = currencyRateProvider;
        _securityChecker = securityChecker;
        _employmentService = employmentService;
        _cashier = cashier;
        _emailSender = emailSender;
        _governmentService = governmentService;
        _weatherService = weatherService;
        _timeService = timeService;
        _emailRegistry = emailRegistry;
        _mySimpleService = mySimpleService;
    }

    public UserId RegisterUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
            throw new ArgumentException("Name is required");

        if (user.Age is < 0 or > 40_000)
            throw new ArgumentException("Age is invalid");

        var id = _database.AddUser(user);
        return new UserId(id);
    }

    public decimal? CalculateUserSalary(User? user)
    {
        if (user is null)
            return null;
        
        if (_cash.GetCachedSalary(user) is { } cached)
            return cached;
        
        if (user.Age < _governmentService.AgeToStartWork)
            throw new LaborLawViolationException("User is too young to work");

        if (!_governmentService.IsUserAlive(user))
            throw new DeadSpiritsViolationException("User is dead");

        if (!_securityChecker.Validate(user))
            throw new UnauthorizedAccessException("User is not authorized");

        if (_employmentService.GetEmployee(user) is not { } employee)
            return null;

        decimal salary = _mySimpleService.GetCommunismMoney(EmployeeClass.Proletariat, employee.Allowance);

        if (employee.Position == "CEO")
            salary *= 10;

        salary *= _currencyRateProvider.GetRate("USD");

        if (!_cashier.IsEnoughCash(salary))
            throw new InsufficientFundsException("Not enough cash to pay salary");

        if (_weatherService.GetWeather() == "snow")
            return null;

        var hour = _timeService.GetTime().Hour;
        if (hour is > 9 and < 18)
            _emailSender.SendEmail(_emailRegistry.GetEmail(user), $"New salary {salary}");

        _cash.SaveToCache(salary);

        return salary;
    }
}