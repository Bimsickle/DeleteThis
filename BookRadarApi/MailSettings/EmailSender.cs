using System.Net.Mail;
using System.Net;

namespace BookRadarApi.MailSettings
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public Task SendEmailsAsync(string email, string subject, string message, string htmlMessage)
        {
            var fromDisplayName = _config.GetValue<string>("MailSettings:Display")!;
            var fomEmailAddress = _config.GetValue<string>("MailSettings:From")!;
            var pw = _config.GetValue<string>("MailSettings:Password")!;

            var fromAddress = new MailAddress(fomEmailAddress, fromDisplayName);

            var client = new SmtpClient(_config.GetValue<string>("MailSettings:Client")!, _config.GetValue<int>("MailSettings:Port")!)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(fomEmailAddress, pw)
            };

            var mailMessage = new MailMessage(fromAddress, new MailAddress(email))
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlMessage, null, "text/html"));

            return client.SendMailAsync(mailMessage);
        }
    }
}
