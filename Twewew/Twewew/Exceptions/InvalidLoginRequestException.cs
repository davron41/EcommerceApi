namespace Twewew.Exceptions;

public class InvalidLoginRequestException : ApplicationException
{
    public InvalidLoginRequestException() : base() { }
    public InvalidLoginRequestException(string message) : base(message) { }

}
