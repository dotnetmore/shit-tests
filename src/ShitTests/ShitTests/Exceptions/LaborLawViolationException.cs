namespace ShitTests.Exceptions;

public class LaborLawViolationException : Exception
{
    public LaborLawViolationException(string msg) : base(msg)
    {
    }
}