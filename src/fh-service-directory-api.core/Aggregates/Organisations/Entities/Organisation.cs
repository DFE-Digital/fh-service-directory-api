using fh_service_directory_api.core.Aggregates.Services.Entities;
using fh_service_directory_api.core.Interfaces.Entities.Aggregates;
using LocalAuthorityInformationServices.SharedKernel;
using LocalAuthorityInformationServices.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Aggregates.Organisations.Entities;

/// <summary>
/// The organization record is used to provide basic description and details about each organization delivering or reviewing services.
/// Each service should be linked to the organization responsible for its delivery.
/// One organization may deliver many services.
///
/// Note that the LGA extension table 'link_taxonomy' enables many taxonomy terms to associated with an organization.
/// These can define the type of organization e.g.
///     "charity"
///     "voluntary group"
///     "local authority"
/// </summary>
public class Organisation :
    EntityBase<string>,
    IOrganisation,
    IAggregateRoot
{
    #region Public Properties
    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public string? Logo { get; private set; } = string.Empty;

    public string? Uri { get; private set; } = string.Empty;

    public string? Url { get; private set; } = string.Empty;

    public IEnumerable<IContact>? OrganisationContacts => _organisationContacts; // prevent consumers doing OrganisatinContacts.Add and OrganisatinContacts.Remove etc 

    public ICollection<IReview>? Reviews { get; private set; } = new HashSet<IReview>();

    public ICollection<IService>? Services { get; private set; } = new HashSet<IService>();
    #endregion Public Properties

    #region Constructors
    private Organisation() { }                                          // EF use only, EF needs a parameterless constructor

    public Organisation
    (
        string name,                                                    // The official or public name of the organization.
        string description,                                             // A brief summary about the organization. It can contain markup such as HTML or Markdown.
        string? logo = default,                                         // A URL to an image associated with the organization which can be presented alongside its name.
        string? uri = default,                                          // Added by Open Referral UK - TODO find out what it's use is.
        string? url = default,                                          // The URL (website address) of the organization.
        IEnumerable<IContact>? organisationContactsEx = default,        // FH extension to associate Contact information with an organization.
        ICollection<IReview>? reviews = default,                        // The review table contains service reviews made by organizations. This is an LGA Extension table. This table provides a structured version of the text information contained in the 'accreditations' field of the 'service' table.
        ICollection<IService>? services = default                       // Services are provided by organizations to a range of different groups. Details on where each service is delivered are contained in the services_at_location table.
    )
    {
        Name = name;
        Description = description;
        Logo = logo ?? string.Empty;
        Uri = uri ?? string.Empty;
        Url = url ?? string.Empty;
        _organisationContacts = (ICollection<IContact>)(organisationContactsEx ?? new List<IContact>().AsReadOnly());
        Reviews = reviews;
        Services = services;
    }
    #endregion Constructors

    #region Public Methods
    public void UpdateOrganisation
    (
        string name,
        string description,
        string logo,
        string uri,
        string url,
        IEnumerable<IContact>? organisationContacts
    )
    {
        Name = name;
        Description = description;
        Logo = logo ?? string.Empty;
        Uri = uri ?? string.Empty;
        Url = url ?? string.Empty;
        _organisationContacts = (ICollection<IContact>)(organisationContacts ?? new List<IContact>().AsReadOnly());
    }

    public void AddOrganisationContact(IContact contact)
    {
        ArgumentNullException.ThrowIfNull(contact, nameof(contact));
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        _organisationContacts.Add(contact);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
    #endregion Public Methods

    #region Private Methods
    #endregion Private Methods

    #region Private Properties
    // Private Properties
    private ICollection<IContact>? _organisationContacts = new List<IContact>().AsReadOnly();
    #endregion Private Properties
}

