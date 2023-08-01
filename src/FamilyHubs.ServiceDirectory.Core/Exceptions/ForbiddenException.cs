namespace FamilyHubs.ServiceDirectory.Core.Exceptions
{
    public class ForbiddenException :ServiceDirectoryException
    {
        public ForbiddenException(string message) : base(message)
        {
            Title = "Forbidden";
            HttpStatusCode = 403;
            ErrorCode = ExceptionCodes.GenericAuthorizationException;
        }
    }
}
