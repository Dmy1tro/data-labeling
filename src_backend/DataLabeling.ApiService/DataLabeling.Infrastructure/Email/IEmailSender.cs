using System.Threading.Tasks;

namespace DataLabeling.Infrastructure.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmailConfirmationMessageAsync(string toEmail, string link);
    }
}
