namespace ShitTests;

public interface IGovernmentService
{
    bool IsUserAlive(User user);
    int AgeToStartWork { get; }
}