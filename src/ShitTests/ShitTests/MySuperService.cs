using LetPlayTestGenerators;

public class MySuperService
{
    private readonly IDatabase _database;
    private readonly ICash _cash;
    private readonly ICurrencyRateProvider _currencyrateprovider;
    private readonly ISecurityChecker _securitychecker;
    private readonly IEmploymentService _employmentservice;
    private readonly ICashier _cashier;
    private readonly IEmailSender _emailsender;
    private readonly IGovernmentService _governmentservice;
    private readonly IWeatherService _weatherservice;
    private readonly ITimeService _timeservice;
    private readonly IEmailRegistry _emailregistry;
    private readonly MySimpleService _mySimpleService;

    public MySuperService(IDatabase database,
        ICash cash,
        ICurrencyRateProvider currencyrateprovider,
        ISecurityChecker securitychecker,
        IEmploymentService employmentservice,
        ICashier cashier,
        IEmailSender emailsender,
        IGovernmentService governmentservice,
        IWeatherService weatherservice,
        ITimeService timeservice,
        IEmailRegistry emailregistry,
        MySimpleService mySimpleService)
    {
        _database = database;
        _cash = cash;
        _currencyrateprovider = currencyrateprovider;
        _securitychecker = securitychecker;
        _employmentservice = employmentservice;
        _cashier = cashier;
        _emailsender = emailsender;
        _governmentservice = governmentservice;
        _weatherservice = weatherservice;
        _timeservice = timeservice;
        _emailregistry = emailregistry;
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
        
        if (user.Age < _governmentservice.AgeToStartWork)
            throw new LaborLawViolationException("User is too young to work");

        if (!_governmentservice.IsUserAlive(user))
            throw new DeadSpiritsViolationException("User is dead");

        if (!_securitychecker.Validate(user))
            throw new UnauthorizedAccessException("User is not authorized");

        if (_employmentservice.GetEmployee(user) is not { } employee)
            return null;

        decimal salary = _mySimpleService.GetCommunismMoney(EmployeeClass.Proletariat, employee.Allowance);

        if (employee.Position == "CEO")
            salary *= 10;

        salary *= _currencyrateprovider.GetRate("USD");

        if (!_cashier.IsEnoughCash(salary))
            throw new InsufficientFundsException("Not enough cash to pay salary");

        if (_weatherservice.GetWeather() == "snow")
            return null;

        var hour = _timeservice.GetTime().Hour;
        if (hour is > 9 and < 18)
            _emailsender.SendEmail(_emailregistry.GetEmail(user), $"New salary {salary}");

        _cash.SaveToCache(salary);

        return salary;
    }
}

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string msg) : base(msg)
    {
    }
}

public class DeadSpiritsViolationException : Exception
{
    public DeadSpiritsViolationException(string msg) : base(msg)
    {
    }
}

public class LaborLawViolationException : Exception
{
    public LaborLawViolationException(string msg) : base(msg)
    {
    }
}

public class User
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public record struct UserId(int Value);

public interface ICash
{
    decimal? GetCachedSalary(User user);
    void SaveToCache(decimal salary);
}

public interface ICurrencyRateProvider
{
    decimal GetRate(string currency);
}

public interface ISecurityChecker
{
    bool Validate(User user);
}

public interface IEmploymentService
{
    Employee? GetEmployee(User user);
}

public interface ICashier
{
    bool IsEnoughCash(decimal amount);
}

public interface IEmailSender
{
    void SendEmail(string email, string message);
}

public interface IGovernmentService
{
    bool IsUserAlive(User user);
    int AgeToStartWork { get; }
}

public interface IWeatherService
{
    string GetWeather();
}

public interface ITimeService
{
    DateTime GetTime();
}

public interface IEmailRegistry
{
    string GetEmail(User user);
}

public class Employee
{
    public string Name { get; set; }
    public string Position { get; set; }

    public decimal Allowance { get; set; }
}