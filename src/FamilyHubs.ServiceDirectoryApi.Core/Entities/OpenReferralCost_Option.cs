using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralCost_Option : EntityBase<string>, IAggregateRoot
{
    private OpenReferralCost_Option() { }
    public OpenReferralCost_Option(string id, string amount_description, decimal amount, string? linkId, string? option, DateTime? valid_from
        , DateTime? valid_to
        )
    {
        Id = id;
        Amount_description = amount_description;
        Amount = amount;
        LinkId = linkId;
        Option = option;
        Valid_from = valid_from;
        Valid_to = valid_to;
    }
    public string Amount_description { get; set; } = default!;
    public decimal Amount { get; set; }
    public string? LinkId { get; set; }
    public string? Option { get; set; }
    public DateTime? Valid_from { get; set; }
    public DateTime? Valid_to { get; set; }
    public string OpenReferralServiceId { get; set; } = default!;
}
