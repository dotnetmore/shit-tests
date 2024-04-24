using System;
using NSubstitute;
using ShitTests.Entites;
using ShitTests.Exceptions;
using ShitTests.Interfaces;
using Xunit;

namespace ShitTests.Tests.MySuperServiceTests;

public class CalculateUserSalaryTests : MySuperServiceTestsBase
{
    private User _user = new() { Age = 32, Name = "Test User" };

    private Employee _employee = new()
    {
        Position = "HARD WORKER",
        Allowance = 0,
        Name = "Test User"
    };

    const string Currency = "USD";
    const string EmployeeEmail = "Good.Employee@company.com";

    public CalculateUserSalaryTests()
    {
        // Arrange Defaults
        Cache.GetCachedSalary(Arg.Is<User>(u => u == _user)).Returns((decimal?)null);
        GovernmentService.AgeToStartWork.Returns(18);
        GovernmentService.IsUserAlive(Arg.Is<User>(u => u == _user)).Returns(true);
        SecurityChecker.Validate(Arg.Is<User>(u => u == _user)).Returns(true);
        EmploymentService.GetEmployee(Arg.Is<User>(u => u == _user)).Returns(_ => _employee);
        Cashier.IsEnoughCash(Arg.Any<decimal>()).Returns(true);
        WeatherService.GetWeather().Returns("sunny");
        CurrencyRateProvider.GetRate(Currency).Returns(1);
    }

    private decimal? Act() => MySuperService.CalculateUserSalary(_user);

    [Fact]
    public void CalculateUserSalary_UserIsNull_ReturnsNull()
    {
        _user = null!;
        var result = Act();
        Assert.Null(result);
    }

    [Fact]
    public void CalculateUserSalary_UserInCache_ReturnsCached()
    {
        Cache.GetCachedSalary(_user).Returns(42);
        var result = Act();
        Assert.Equal(42, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsTooYoungToWork_ThrowsLaborLawViolationException()
    {
        _user = _user with { Age = 10 };
        Assert.Throws<LaborLawViolationException>(() => Act());
    }

    [Fact]
    public void CalculateUserSalary_UserIsDead_ThrowsDeadSpiritsViolationException()
    {
        GovernmentService.IsUserAlive(_user).Returns(false);
        Assert.Throws<DeadSpiritsViolationException>(() => Act());
    }

    [Fact]
    public void CalculateUserSalary_UserIsNotAuthorized_ThrowsUnauthorizedAccessException()
    {
        SecurityChecker.Validate(_user).Returns(false);
        Assert.Throws<UnauthorizedAccessException>(() => Act());
    }

    [Fact]
    public void CalculateUserSalary_UserIsNotAnEmployee_ReturnsNull()
    {
        EmploymentService.GetEmployee(_user).Returns((Employee?)null);
        var result = Act();
        Assert.Null(result);
    }

    [Fact]
    public void CalculateUserSalary_InsufficientCashToPay_ThrowsInsufficientFundsException()
    {
        Cashier.IsEnoughCash(Arg.Any<decimal>()).Returns(false);
        Assert.Throws<InsufficientFundsException>(() => Act());
    }

    [Fact]
    public void CalculateUserSalary_WeatherConditionIsSnow_ReturnsNull()
    {
        WeatherService.GetWeather().Returns("snow");
        var result = Act();
        Assert.Null(result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionAreNormalNoAllowance_Returns42()
    {
        var result = Act();
        Assert.Equal(42, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionAreNormalWithAllowance100_Returns142()
    {
        _employee = _employee with { Allowance = 100 };
        var result = Act();
        Assert.Equal(142, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionAreNormalNoAllowanceRateX2_Returns84()
    {
        CurrencyRateProvider.GetRate(Currency).Returns(2);
        var result = Act();
        Assert.Equal(84, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionAreNormalPositionIsCEO_Returns420()
    {
        _employee = _employee with { Position = "CEO" };
        var result = Act();
        Assert.Equal(420, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionNormalWorkingTime_SendEmail()
    {
        TimeService.GetTime().Returns(new DateTime(2024, 4, 20, 12, 0, 0));
        EmailRegistry.GetEmail(_user).Returns(EmployeeEmail);

        Act();

        EmailSender.Received().SendEmail(EmployeeEmail, "New salary 42");
    }

    [Theory]
    [InlineData(4)]
    [InlineData(21)]
    public void CalculateUserSalary_UserIsValidAndConditionOutOfWorkingTime_DoNotSendEmail(int hour)
    {
        TimeService.GetTime().Returns(new DateTime(2024, 4, 20, hour, 0, 0));
        EmailRegistry.GetEmail(_user).Returns(EmployeeEmail);

        Act();

        EmailSender.DidNotReceive().SendEmail(EmployeeEmail, Arg.Any<string>());
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndCondition_Save42ToCache()
    {
        Act();
        Cache.Received().SaveToCache(42);
    }
}