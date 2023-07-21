namespace FamilyHubs.ServiceDirectory.Core.Exceptions;

public class ServiceDirectoryException : Exception
{
    public string Title { get; set; } = "Server Error";
    public string ErrorCode { get; set; } = ExceptionCodes.UnhandledException;
    public int HttpStatusCode { get; set; } = 500;

    public ServiceDirectoryException(string message) : base(message)
    {

    }
}

