namespace fh_service_directory_api.core.Interfaces.Entities;

public interface IOpenReferralContact
{
    string Name { get; init; }
    ICollection<IOpenReferralPhone>? Phones { get; init; }
    string Title { get; init; }
}