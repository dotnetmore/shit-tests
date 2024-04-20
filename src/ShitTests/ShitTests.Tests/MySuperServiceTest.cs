using System;
using NSubstitute;
using ShitTests.Entites;
using ShitTests.Exceptions;
using ShitTests.Interfaces;
using ShitTests.ValueTypes;
using Xunit;

namespace ShitTests.Tests;

/// <summary>
/// All tests generated using AI Assistant with the following prompt.
/// <remarks>
/// As a Senior Software Engineer and QA Expert, you will write tests for `MySuperService` class in C# using the latest language syntax and the xUnit library.
/// Use the Arrange/Act/Assert approach.
/// It is important to cover 100% of possible cases.
/// Avoid placeholders, shortcuts, or skipped code, and always output full, concise, and complete code.
/// If you really need mocks, use the `NSubstitute` library.
/// If you need clarification on the task, feel free to ask questions. For tasks with more than 5 tests, please describe the test cases first and wait for user approval before implementing them.
/// When naming tests, follow provided in tripe quotes rules.
/// Generate tests for all methods in class ShitTests.MySuperService
/// </remarks>
/// </summary>
public class MySuperServiceTest
{
    private readonly MySuperService _mySuperService;
    private readonly IDatabase _mockDatabase;
    private readonly ICash _mockCash;
    private readonly ICurrencyRateProvider _mockCurrencyRateProvider;
    private readonly ISecurityChecker _mockSecurityChecker;
    private readonly IEmploymentService _mockEmploymentService;
    private readonly ICashier _mockCashier;
    private readonly IEmailSender _mockEmailSender;
    private readonly IGovernmentService _mockGovernmentService;
    private readonly IWeatherService _mockWeatherService;
    private readonly ITimeService _mockTimeService;
    private readonly IEmailRegistry _mockEmailRegistry;

    public MySuperServiceTest()
    {
        _mockDatabase = Substitute.For<IDatabase>();
        _mockCash = Substitute.For<ICash>();
        _mockCurrencyRateProvider = Substitute.For<ICurrencyRateProvider>();
        _mockSecurityChecker = Substitute.For<ISecurityChecker>();
        _mockEmploymentService = Substitute.For<IEmploymentService>();
        _mockCashier = Substitute.For<ICashier>();
        _mockEmailSender = Substitute.For<IEmailSender>();
        _mockGovernmentService = Substitute.For<IGovernmentService>();
        _mockWeatherService = Substitute.For<IWeatherService>();
        _mockTimeService = Substitute.For<ITimeService>();
        _mockEmailRegistry = Substitute.For<IEmailRegistry>();

        _mySuperService = new MySuperService(_mockDatabase, _mockCash, _mockCurrencyRateProvider,
            _mockSecurityChecker, _mockEmploymentService, _mockCashier,
            _mockEmailSender, _mockGovernmentService, _mockWeatherService,
            _mockTimeService, _mockEmailRegistry, new MySimpleService());
    }

    #region RegisterUser

    [Fact]
    public void RegisterUser_NameIsNullOrWhiteSpace_ThrowsArgumentException()
    {
        var user = new User { Name = "", Age = 20 };

        var exception = Assert.Throws<ArgumentException>(() => _mySuperService.RegisterUser(user));

        Assert.Equal("Name is required", exception.Message);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(42_000)]
    public void RegisterUser_AgeIsInvalid_ThrowsArgumentException(int age)
    {
        var user = new User { Name = "Test User", Age = age };

        var exception = Assert.Throws<ArgumentException>(() => _mySuperService.RegisterUser(user));

        Assert.Equal("Age is invalid", exception.Message);
    }

    [Fact]
    public void RegisterUser_UserIsValid_ReturnsUserId()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockDatabase.AddUser(user).Returns(3);
        var expectedUserId = new UserId(3);

        var result = _mySuperService.RegisterUser(user);

        Assert.Equal(result, expectedUserId);
    }

    #endregion

    #region CalculateUserSalary

    [Fact]
    public void CalculateUserSalary_UserIsNull_ReturnsNull()
    {
        var result = _mySuperService.CalculateUserSalary(null);
        Assert.Null(result);
    }

    [Fact]
    public void CalculateUserSalary_UserInCache_ReturnsCached()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockCash.GetCachedSalary(user).Returns(42);

        var result = _mySuperService.CalculateUserSalary(user);

        Assert.Equal(42, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsTooYoungToWork_ThrowsLaborLawViolationException()
    {
        var user = new User { Name = "Test User", Age = 10 };
        _mockGovernmentService.AgeToStartWork.Returns(18);

        void Act() => _mySuperService.CalculateUserSalary(user);

        Assert.Throws<LaborLawViolationException>(Act);
    }

    [Fact]
    public void CalculateUserSalary_UserIsDead_ThrowsDeadSpiritsViolationException()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockGovernmentService.IsUserAlive(user).Returns(false);

        void Act() => _mySuperService.CalculateUserSalary(user);

        Assert.Throws<DeadSpiritsViolationException>(Act);
    }

    [Fact]
    public void CalculateUserSalary_UserIsNotAuthorized_ThrowsUnauthorizedAccessException()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(false);

        void Act() => _mySuperService.CalculateUserSalary(user);

        Assert.Throws<UnauthorizedAccessException>(Act);
    }

    [Fact]
    public void CalculateUserSalary_UserIsNotAnEmployee_ReturnsNull()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(true);
        _mockEmploymentService.GetEmployee(user).Returns((Employee?)null);

        var result = _mySuperService.CalculateUserSalary(user);

        Assert.Null(result);
    }

    [Fact]
    public void CalculateUserSalary_InsufficientCashToPay_ThrowsInsufficientFundsException()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(true);
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "HARD WORKER",
            Allowance = 0,
            Name = "Test User"
        });
        _mockCashier.IsEnoughCash(Arg.Any<decimal>()).Returns(false);

        void Act() => _mySuperService.CalculateUserSalary(user);

        var exception = Assert.Throws<InsufficientFundsException>(Act);
        Assert.Equal("Not enough cash to pay salary", exception.Message);
    }

    [Fact]
    public void CalculateUserSalary_WeatherConditionIsSnow_ReturnsNull()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(true);
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "HARD WORKER",
            Allowance = 0,
            Name = "Test User"
        });
        _mockCashier.IsEnoughCash(Arg.Any<decimal>()).Returns(true);
        _mockWeatherService.GetWeather().Returns("snow");

        var result = _mySuperService.CalculateUserSalary(user);

        Assert.Null(result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionAreNormalNoAllowance_Returns42()
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(true);
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "HARD WORKER",
            Allowance = 0,
            Name = "Test User"
        });
        _mockCashier.IsEnoughCash(Arg.Any<decimal>()).Returns(true);
        _mockWeatherService.GetWeather().Returns("sunny");
        _mockCurrencyRateProvider.GetRate("USD").Returns(1);

        var result = _mySuperService.CalculateUserSalary(user);

        Assert.NotNull(result);
        Assert.Equal(42, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionAreNormalWithAllowance100_Returns142()
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(true);
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "HARD WORKER",
            Allowance = 100,
            Name = "Test User"
        });
        _mockCashier.IsEnoughCash(Arg.Any<decimal>()).Returns(true);
        _mockWeatherService.GetWeather().Returns("sunny");
        _mockCurrencyRateProvider.GetRate("USD").Returns(1);

        var result = _mySuperService.CalculateUserSalary(user);

        Assert.NotNull(result);
        Assert.Equal(142, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionAreNormalNoAllowanceRateX2_Returns84()
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(true);
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "HARD WORKER",
            Allowance = 0,
            Name = "Test User"
        });
        _mockCashier.IsEnoughCash(Arg.Any<decimal>()).Returns(true);
        _mockWeatherService.GetWeather().Returns("sunny");
        _mockCurrencyRateProvider.GetRate("USD").Returns(2);

        var result = _mySuperService.CalculateUserSalary(user);

        Assert.NotNull(result);
        Assert.Equal(84, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionAreNormalPositionIsCEO_Returns420()
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(true);
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "CEO",
            Allowance = 0,
            Name = "Test User"
        });
        _mockCashier.IsEnoughCash(Arg.Any<decimal>()).Returns(true);
        _mockWeatherService.GetWeather().Returns("sunny");
        _mockCurrencyRateProvider.GetRate("USD").Returns(1);

        var result = _mySuperService.CalculateUserSalary(user);

        Assert.NotNull(result);
        Assert.Equal(420, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionNormalWorkingTime_SendEmail()
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(true);
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "HARD WORKER",
            Allowance = 0,
            Name = "Test User"
        });
        _mockCashier.IsEnoughCash(Arg.Any<decimal>()).Returns(true);
        _mockWeatherService.GetWeather().Returns("sunny");
        _mockCurrencyRateProvider.GetRate("USD").Returns(1);
        _mockTimeService.GetTime().Returns(new DateTime(2024, 4, 20, 12, 0, 0));
        const string employeeEmail = "Good.Employee@company.com";
        _mockEmailRegistry.GetEmail(user).Returns(employeeEmail);

        _mySuperService.CalculateUserSalary(user);

        _mockEmailSender.Received().SendEmail(employeeEmail, "New salary 42");
    }

    [Theory]
    [InlineData(4)]
    [InlineData(21)]
    public void CalculateUserSalary_UserIsValidAndConditionOutOfWorkingTime_DoNotSendEmail(int hour)
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(true);
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "HARD WORKER",
            Allowance = 0,
            Name = "Test User"
        });
        _mockCashier.IsEnoughCash(Arg.Any<decimal>()).Returns(true);
        _mockWeatherService.GetWeather().Returns("sunny");
        _mockCurrencyRateProvider.GetRate("USD").Returns(1);
        _mockTimeService.GetTime().Returns(new DateTime(2024, 4, 20, hour, 0, 0));
        const string employeeEmail = "Good.Employee@company.com";
        _mockEmailRegistry.GetEmail(user).Returns(employeeEmail);

        _mySuperService.CalculateUserSalary(user);

        _mockEmailSender.DidNotReceive().SendEmail(employeeEmail, Arg.Any<string>());
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndCondition_Save42ToCache()
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(true);
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "HARD WORKER",
            Allowance = 0,
            Name = "Test User"
        });
        _mockCashier.IsEnoughCash(Arg.Any<decimal>()).Returns(true);
        _mockWeatherService.GetWeather().Returns("sunny");
        _mockCurrencyRateProvider.GetRate("USD").Returns(1);

        _mySuperService.CalculateUserSalary(user);

        _mockCash.Received().SaveToCache(42);
    }

    #endregion
}