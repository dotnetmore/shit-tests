using System;
using NSubstitute;
using ShitTests.Entites;
using ShitTests.Interfaces;
using Xunit;

namespace ShitTests.Tests;

public class NotifyingUserSalaryCalculatorTests
{
    private readonly NotifyingUserSalaryCalculator _calculator;
    private readonly IEmailSender _mockEmailSender = Substitute.For<IEmailSender>();
    private readonly ITimeService _mockTimeService = Substitute.For<ITimeService>();
    private readonly IEmailRegistry _mockEmailRegistry = Substitute.For<IEmailRegistry>();
    private readonly IUserSalaryCalculator _mockCalculator = Substitute.For<IUserSalaryCalculator>();

    public NotifyingUserSalaryCalculatorTests() => 
        _calculator = new NotifyingUserSalaryCalculator(
            _mockCalculator,
            _mockEmailSender,
            _mockEmailRegistry,
            _mockTimeService);

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionNormalWorkingTime_SendEmail()
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockTimeService.GetTime().Returns(new DateTime(2024, 4, 20, 12, 0, 0));
        const string employeeEmail = "Good.Employee@company.com";
        _mockEmailRegistry.GetEmail(user).Returns(employeeEmail);
        _mockCalculator.CalculateUserSalary(user).Returns(42);

        _calculator.CalculateUserSalary(user);

        _mockEmailSender.Received().SendEmail(employeeEmail, "New salary 42");
    }

    [Theory]
    [InlineData(4)]
    [InlineData(21)]
    public void CalculateUserSalary_UserIsValidAndConditionOutOfWorkingTime_DoNotSendEmail(int hour)
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockTimeService.GetTime().Returns(new DateTime(2024, 4, 20, hour, 0, 0));
        const string employeeEmail = "Good.Employee@company.com";
        _mockEmailRegistry.GetEmail(user).Returns(employeeEmail);

        _calculator.CalculateUserSalary(user);

        _mockEmailSender.DidNotReceive().SendEmail(employeeEmail, Arg.Any<string>());
    }
}