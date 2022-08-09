namespace fh_service_directory_api.core.Interfaces.Entities.Aggregates
{
    public interface IOrganisation
    {
        string? Description { get; }
        string? Logo { get; }
        string Name { get; }
        IEnumerable<IContact>? OrganisationContacts { get; }
        string? Uri { get; }
        string? Url { get; }

        void AddOrganisationContact(IContact contact);
        void UpdateOrganisation(string name, string? description = null, string? logo = null, string? uri = null, string? url = null, IEnumerable<IContact>? organisationContacts = null);
    }
}