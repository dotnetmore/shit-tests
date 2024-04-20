namespace ShitTests;

public interface ISecurityChecker
{
    bool Validate(User user);
}