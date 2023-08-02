namespace FamilyHubs.ServiceDirectory.Core.Exceptions;

public class NotFoundException : ServiceDirectoryException
{
    public NotFoundException(string message) : base(message)
    {
        Title = "Not Found";
        HttpStatusCode = 404;
        ErrorCode = ExceptionCodes.NotFoundException;
    }
}
