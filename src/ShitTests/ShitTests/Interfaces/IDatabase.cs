using ShitTests.Entites;

namespace ShitTests.Interfaces;

public interface IDatabase
{
    int AddUser(User user);
}