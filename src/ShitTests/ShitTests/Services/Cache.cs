using ShitTests.Entites;
using ShitTests.Interfaces;

namespace ShitTests.Services;

internal class Cache : ICache
{
    public decimal? GetCachedSalary(User user)
    {
        throw new NotImplementedException();
    }

    public void SaveToCache(decimal salary)
    {
        throw new NotImplementedException();
    }
}