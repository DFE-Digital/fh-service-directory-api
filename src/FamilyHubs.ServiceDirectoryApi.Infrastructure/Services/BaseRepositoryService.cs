namespace FamilyHubs.ServiceDirectory.Infrastructure.Services
{
    public abstract class BaseRepositoryService
    {
        protected string ToLowerWithoutWhitespace(string inValue)
        {
            return inValue.ToLower().Replace(" ","");
        }
    }
}
