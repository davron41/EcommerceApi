namespace Twewew.Exceptions;

public class ProductIsNotExistException : ApplicationException
{
    public ProductIsNotExistException() : base() { }
    public ProductIsNotExistException(string message) : base(message) { }
}
