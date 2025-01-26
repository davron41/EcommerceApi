namespace Twewew.Models;

public class Welcome
{
    public string UserName { get; }
    public string Email { get; }
    public string ActionUrl { get; }

    public Welcome(string userName, string email, string actionUrl)
    {
        UserName = userName;
        Email = email;
        ActionUrl = actionUrl;
    }
}
