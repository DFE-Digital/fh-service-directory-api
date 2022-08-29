namespace FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Security.Authorisation
{
    public interface IAuthoriseAttribute
    {
        string Policy { get; set; }
        string Roles { get; set; }
    }
}