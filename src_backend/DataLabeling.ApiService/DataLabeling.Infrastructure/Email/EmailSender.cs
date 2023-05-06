using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DataLabeling.Infrastructure.Email
{
    public record EmailConfig(string DisplayName, string UserName, string Password);

    public class EmailSender : IEmailSender
    {
        private EmailConfig _config;

        public EmailSender(EmailConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public Task<bool> SendEmailConfirmationMessageAsync(string toEmail, string link)
        {
            try
            {
                using var smpt = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_config.UserName, _config.Password)
                };

                var from = new MailAddress(_config.UserName, _config.DisplayName);
                var to = new MailAddress(toEmail);
                var mailMessage = new MailMessage(from, to)
                {
                    Subject = "Email confirmation",
                    Body = string.Format("Welcome to Data-Labeling project!" + Environment.NewLine +
                        "To complete registration, follow the link: " +
                        "<a href=\"{0}\" title=\"Confirm registration\">click me</a>", link),
                    IsBodyHtml = true
                };

                smpt.Send(mailMessage);

                return Task.FromResult(true);
            }
            catch(Exception ex)
            {
                return Task.FromResult(false);
            }
        }
    }
}
