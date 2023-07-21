namespace FamilyHubs.ServiceDirectory.Core.Exceptions;

public class AlreadyExistsException : ServiceDirectoryException
{
    public AlreadyExistsException(string message) : base(message)
    {
        Title = "Already Exists";
        HttpStatusCode = 400;
        ErrorCode = ExceptionCodes.AlreadyExistsException;
    }
}
