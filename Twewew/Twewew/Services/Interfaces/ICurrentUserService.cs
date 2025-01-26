namespace Twewew.Services.Interfaces;

public interface ICurrentUserService
{
    Guid GetUserId();
    string GetUserName();

}
