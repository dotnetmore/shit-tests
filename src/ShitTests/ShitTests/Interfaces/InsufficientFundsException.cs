namespace ShitTests;

public class InsufficientFundsException : Exception
{
    public InsufficientFundsException(string msg) : base(msg)
    {
    }
}