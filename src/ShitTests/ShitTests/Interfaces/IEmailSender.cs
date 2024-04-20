namespace ShitTests;

public interface IEmailSender
{
    void SendEmail(string email, string message);
}