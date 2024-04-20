namespace ShitTests.Interfaces;

public interface IEmailSender
{
    void SendEmail(string email, string message);
}