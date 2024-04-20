using ShitTests.ValueTypes;

namespace ShitTests.Interfaces;

public interface ISalaryCalculator
{
    decimal GetCommunismMoney(EmployeeClass employeeClass, decimal allowance);
}