
namespace BookRadarApi.MailSettings
{
    public interface IEmailSender
    {
        Task SendEmailsAsync(string email, string subject, string message, string htmlMessage);
    }
}