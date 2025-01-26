using Twewew.Email.Models;
using Twewew.Models;

namespace Twewew.Services.Interfaces;

public interface IEmailService
{
    void SendEmailConfirmation(EmailMessage message, UserInfo info);
    void SendWelcome(EmailMessage message);
    void SendResetPassword(EmailMessage message, UserInfo info);
}
