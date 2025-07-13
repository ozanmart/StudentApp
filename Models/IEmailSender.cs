namespace StudentApp.Models;

public interface IEmailSender
{
    public Task SendEmailAsync(string to, string subject, string message);
}