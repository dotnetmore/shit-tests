namespace ShitTests;

public interface ICash
{
    decimal? GetCachedSalary(User user);
    void SaveToCache(decimal salary);
}