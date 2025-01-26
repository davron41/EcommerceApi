namespace Twewew.Exceptions;

public class UsernameAlreadyExistsException : ApplicationException
{
    public UsernameAlreadyExistsException() : base() { }
    public UsernameAlreadyExistsException(string message) : base(message) { }
}
