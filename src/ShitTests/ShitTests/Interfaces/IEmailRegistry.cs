using ShitTests.Entites;

namespace ShitTests.Interfaces;

public interface IEmailRegistry
{
    string GetEmail(User user);
}