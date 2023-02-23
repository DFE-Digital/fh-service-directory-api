namespace FamilyHubs.ServiceDirectory.Core.Security
{
    public interface IAuthoriseAttribute
    {
        string Policy { get; set; }
        string Roles { get; set; }
    }
}