using System;
using NSubstitute;
using ShitTests.Entites;
using ShitTests.Exceptions;
using ShitTests.Interfaces;
using Xunit;

namespace ShitTests.Tests;

public class SafeUserSalaryCalculatorTests
{
    private readonly SafeUserSalaryCalculator _calculator;
    private readonly IUserSalaryCalculator _mockCalculator = Substitute.For<IUserSalaryCalculator>();
    private readonly ISecurityChecker _mockSecurityChecker = Substitute.For<ISecurityChecker>();
    private readonly ICashier _mockCashier = Substitute.For<ICashier>();
    private readonly IGovernmentService _mockGovernmentService = Substitute.For<IGovernmentService>();

    public SafeUserSalaryCalculatorTests() => 
        _calculator = new SafeUserSalaryCalculator(
            _mockCalculator,
            _mockSecurityChecker,
            _mockCashier,
            _mockGovernmentService);

    [Fact]
    public void CalculateUserSalary_UserIsTooYoungToWork_ThrowsLaborLawViolationException()
    {
        var user = new User { Name = "Test User", Age = 10 };
        _mockGovernmentService.AgeToStartWork.Returns(18);

        void Act() => _calculator.CalculateUserSalary(user);

        Assert.Throws<LaborLawViolationException>(Act);
    }

    [Fact]
    public void CalculateUserSalary_UserIsDead_ThrowsDeadSpiritsViolationException()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockGovernmentService.IsUserAlive(user).Returns(false);

        void Act() => _calculator.CalculateUserSalary(user);

        Assert.Throws<DeadSpiritsViolationException>(Act);
    }
    
    [Fact]
    public void CalculateUserSalary_UserIsNotAuthorized_ThrowsUnauthorizedAccessException()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(false);

        void Act() => _calculator.CalculateUserSalary(user);

        Assert.Throws<UnauthorizedAccessException>(Act);
    }
    
    [Fact]
    public void CalculateUserSalary_InsufficientCashToPay_ThrowsInsufficientFundsException()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockGovernmentService.IsUserAlive(user).Returns(true);
        _mockSecurityChecker.Validate(user).Returns(true);
        _mockCalculator.CalculateUserSalary(user).Returns(43);
        _mockCashier.IsEnoughCash(43).Returns(false);

        void Act() => _calculator.CalculateUserSalary(user);

        var exception = Assert.Throws<InsufficientFundsException>(Act);
        Assert.Equal("Not enough cash to pay salary", exception.Message);
    }
}