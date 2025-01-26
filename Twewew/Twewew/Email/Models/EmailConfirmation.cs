namespace Twewew.Email.Models;

public class EmailConfirmation
{
    public string UserName { get; }
    public string ActionUrl { get; }
    public string? OperatingSystem { get; }
    public string? Browser { get; }


    public EmailConfirmation(string userName, string actionUrl)
    {
        UserName = userName;
        ActionUrl = actionUrl;
    }
    public EmailConfirmation(string userName, string actionUrl, string? operatingSystem, string? browser)
    {
        UserName = userName;
        ActionUrl = actionUrl;
        OperatingSystem = operatingSystem;
        Browser = browser;
    }
}
