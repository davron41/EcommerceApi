using FluentEmail.Core;
using Twewew.Email.Models;
using Twewew.Models;
using Twewew.Services.Interfaces;

namespace Twewew.Email;

public sealed class EmailService : IEmailService
{
    private readonly IFluentEmailFactory _emailFactory;

    public EmailService(IFluentEmailFactory emailFactory)
    {
        _emailFactory = emailFactory;
    }

    public void SendEmailConfirmation(EmailMessage message, UserInfo info)
    {
        var emailConfirmation = new EmailConfirmation(
            message.Username,
            message.FallbackUrl,
            info.OS,
            info.Browser);

        var templatePath = Path.Combine(AppContext.BaseDirectory, "Email\\Templates", "EmailConfirmation.cshtml");

        _emailFactory
            .Create()
            .To(message.To)
            .Subject(message.Subject)
            .UsingTemplateFromFile(templatePath, emailConfirmation)
            .Send();

    }

    public void SendResetPassword(EmailMessage message, UserInfo info)
    {
        var resetPassword = new ResetPassword(
            message.Username,
            message.FallbackUrl,
            info.OS,
            info.Browser);

        var templatePath = Path.Combine(AppContext.BaseDirectory, "Email\\Templates", "ResetPassword.cshtml");

        _emailFactory
            .Create()
            .To(message.To)
            .Subject(message.Subject)
            .UsingTemplateFromFile(templatePath, resetPassword)
            .Send();
    }

    public void SendWelcome(EmailMessage message)
    {
        var welcome = new Welcome(
            message.Username,
            message.To,
            message.FallbackUrl);

        var templatePath = Path.Combine(AppContext.BaseDirectory, "Email\\Templates", "Welcome.cshtml");
    }
}
