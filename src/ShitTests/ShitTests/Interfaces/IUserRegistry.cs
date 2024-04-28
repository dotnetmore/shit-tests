using ShitTests.Entites;
using ShitTests.ValueTypes;

namespace ShitTests.Interfaces;

public interface IUserRegistry
{
    UserId RegisterUser(User user);
}