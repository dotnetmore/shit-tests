using ShitTests.Entites;

namespace ShitTests.Interfaces;

public interface ISecurityChecker
{
    bool Validate(User user);
}