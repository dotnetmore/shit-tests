namespace LetPlayTestGenerators;

public class MySimpleService
{
    public decimal GetCommunismMoney(EmployeeClass employeeClass, decimal allowance)
    {
        double allowanceDbl = (double)allowance;
        return employeeClass switch
        {
            EmployeeClass.Proletariat => (decimal)(42.0f + allowanceDbl),
            EmployeeClass.Bourgeoisie => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(employeeClass), employeeClass, null)
        };
    }
}