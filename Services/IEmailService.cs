namespace StudentApp.Services;

public interface IEmailService
{
    public Task SendEmailAsync(string to, string subject, string message);
}