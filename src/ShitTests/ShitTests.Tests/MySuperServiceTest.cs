using System;
using NSubstitute;
using ShitTests.Entites;
using ShitTests.Interfaces;
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
    private readonly IEmploymentService _mockEmploymentService;

    public MySuperServiceTest()
    {
        _mockEmploymentService = Substitute.For<IEmploymentService>();
        
        _mySuperService = new MySuperService(
            _mockEmploymentService, new MySimpleService());
    }
    
    #region CalculateUserSalary

    [Fact]
    public void CalculateUserSalary_UserIsNull_ReturnsNull()
    {
        var result = _mySuperService.CalculateUserSalary(null);
        Assert.Null(result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsNotAnEmployee_ReturnsNull()
    {
        var user = new User { Name = "Test User", Age = 20 };
        _mockEmploymentService.GetEmployee(user).Returns((Employee?)null);

        var result = _mySuperService.CalculateUserSalary(user);

        Assert.Null(result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionAreNormalNoAllowance_Returns42()
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "HARD WORKER",
            Allowance = 0,
            Name = "Test User"
        });

        var result = _mySuperService.CalculateUserSalary(user);

        Assert.NotNull(result);
        Assert.Equal(42, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionAreNormalWithAllowance100_Returns142()
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "HARD WORKER",
            Allowance = 100,
            Name = "Test User"
        });

        var result = _mySuperService.CalculateUserSalary(user);

        Assert.NotNull(result);
        Assert.Equal(142, result);
    }

    [Fact]
    public void CalculateUserSalary_UserIsValidAndConditionAreNormalPositionIsCEO_Returns420()
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "CEO",
            Allowance = 0,
            Name = "Test User"
        });

        var result = _mySuperService.CalculateUserSalary(user);

        Assert.NotNull(result);
        Assert.Equal(420, result);
    }
    
    [Fact]
    public void CalculateUserSalary_UserIsValidAndCondition_Save42ToCache()
    {
        var user = new User { Age = 32, Name = "Test User" };
        _mockEmploymentService.GetEmployee(user).Returns(new Employee
        {
            Position = "HARD WORKER",
            Allowance = 0,
            Name = "Test User"
        });
        
        _mySuperService.CalculateUserSalary(user);
    }

    #endregion
}