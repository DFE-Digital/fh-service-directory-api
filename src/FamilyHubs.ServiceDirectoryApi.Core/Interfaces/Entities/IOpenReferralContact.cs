using fh_service_directory_api.core.Entities;

namespace fh_service_directory_api.core.Interfaces.Entities;

public interface IOpenReferralContact : IEntityBase<string>
{
    string Name { get; set; }
    ICollection<OpenReferralPhone>? Phones { get; set; }
    ICollection<OpenReferralLinkContact>? ContactLinks { get; set; }
    string? Title { get; set; }
}