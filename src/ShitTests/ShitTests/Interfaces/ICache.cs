using ShitTests.Entites;

namespace ShitTests.Interfaces;

public interface ICache
{
    decimal? GetCachedSalary(User user);
    void SaveToCache(decimal salary);
}