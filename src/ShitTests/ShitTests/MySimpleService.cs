using ShitTests.Interfaces;
using ShitTests.ValueTypes;

namespace ShitTests;

public class MySimpleService : ISalaryCalculator
{
    public decimal GetCommunismMoney(EmployeeClass employeeClass, decimal allowance)
    {
        return employeeClass switch
        {
            EmployeeClass.Proletariat => 42 + allowance,
            EmployeeClass.Bourgeoisie => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(employeeClass), employeeClass, null)
        };
    }
}