using ShitTests.Entites;

namespace ShitTests.Interfaces;

public interface ICash
{
    decimal? GetCachedSalary(User user);
    void SaveToCache(decimal salary);
}