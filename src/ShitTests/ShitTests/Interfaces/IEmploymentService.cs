using ShitTests.Entites;

namespace ShitTests.Interfaces;

public interface IEmploymentService
{
    Employee? GetEmployee(User user);
}