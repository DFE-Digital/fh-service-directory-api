namespace fh_service_directory_api.core.Interfaces.Security
{
    public interface IAuthoriseAttribute
    {
        string Policy { get; set; }
        string Roles { get; set; }
    }
}