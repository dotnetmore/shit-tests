using ShitTests.Entites;
using ShitTests.Interfaces;
using ShitTests.ValueTypes;

namespace ShitTests;

public class UserRegistry(IDatabase database)
{
    public UserId RegisterUser(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Name))
            throw new ArgumentException("Name is required");

        if (user.Age is < 0 or > 40_000)
            throw new ArgumentException("Age is invalid");

        var id = database.AddUser(user);
        return new UserId(id);
    }
}