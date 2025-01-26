namespace Twewew.Email.Models;

public class ResetPassword
{
    public string UserName { get; }
    public string ActionUrl { get; }
    public string? OS { get; }
    public string? Browser { get; }

    public ResetPassword(string userName, string actionUrl)
    {
        UserName = userName;
        ActionUrl = actionUrl;

    }
    public ResetPassword(string userName, string actionUrl, string? oS, string? browser)
    {
        UserName = userName;
        ActionUrl = actionUrl;
        OS = oS;
        Browser = browser;

    }
}
