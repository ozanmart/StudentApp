using SendGrid;
using SendGrid.Helpers.Mail;

namespace StudentApp.Models;

public class EmailSender : IEmailSender
{
    private readonly string _apiKey;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
    {
        _apiKey = configuration["SendGrid:ApiKey"]; // will come from appsettings.json
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SendGridClient(_apiKey);
        var from = new EmailAddress("ozosdumagu@outlook.com", "Student App"); // your verified sender
        var to = new EmailAddress(email);
        var htmlMessage = message.Replace("\n", "<br>"); // Convert newlines to <br> for HTML emails
        var msg = MailHelper.CreateSingleEmail(from, to, subject, message, htmlMessage);
        var response = await client.SendEmailAsync(msg);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Email successfully sent to {Recipient} with subject '{Subject}'.", email, subject);
        }
        else
        {
            var body = await response.Body.ReadAsStringAsync();
            _logger.LogError("Failed to send email: {StatusCode} - {Body}", response.StatusCode, body);
            throw new Exception($"Failed to send email: {response.StatusCode} - {body}");
        }

    }

}