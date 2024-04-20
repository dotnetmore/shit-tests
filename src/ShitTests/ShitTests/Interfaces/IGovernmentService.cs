using ShitTests.Entites;

namespace ShitTests.Interfaces;

public interface IGovernmentService
{
    bool IsUserAlive(User user);
    int AgeToStartWork { get; }
}