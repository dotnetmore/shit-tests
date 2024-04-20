using ShitTests.Entites;
using ShitTests.Interfaces;
using ShitTests.ValueTypes;

namespace ShitTests;

public class MySuperService(
    IEmploymentService employmentService,
    ISalaryCalculator mySimpleService) : IUserSalaryCalculator
{
    public decimal? CalculateUserSalary(User? user)
    {
        if (user is null)
            return null;
        
        if (employmentService.GetEmployee(user) is not { } employee)
            return null;
        
        var salary = mySimpleService.GetCommunismMoney(EmployeeClass.Proletariat, employee.Allowance);

        if (employee.Position == "CEO")
            salary *= 10;

        return salary;
    }
}