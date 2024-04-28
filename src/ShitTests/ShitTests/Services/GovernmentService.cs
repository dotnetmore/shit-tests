using ShitTests.Entites;
using ShitTests.Interfaces;

namespace ShitTests.Services;

internal class GovernmentService : IGovernmentService
{
    public bool IsUserAlive(User user)
    {
        throw new NotImplementedException();
    }

    public int AgeToStartWork { get; }
}