namespace ShitTests.Exceptions;

public class DeadSpiritsViolationException : Exception
{
    public DeadSpiritsViolationException(string msg) : base(msg)
    {
    }
}