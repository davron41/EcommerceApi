namespace Twewew.Exceptions;

public class ProductAlreadyExistsException : ApplicationException
{
    public ProductAlreadyExistsException() : base() { }
    public ProductAlreadyExistsException(string message) : base(message) { }
}
