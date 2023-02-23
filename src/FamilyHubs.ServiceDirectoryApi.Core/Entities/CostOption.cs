using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class CostOption : EntityBase<string>, IAggregateRoot
{
    private CostOption() { }
    public CostOption(
        string id, 
        string amountDescription, 
        decimal amount, 
        string? linkId, 
        string? option, 
        DateTime? validFrom, 
        DateTime? validTo
        )
    {
        Id = id;
        AmountDescription = amountDescription;
        Amount = amount;
        LinkId = linkId;
        Option = option;
        ValidFrom = validFrom;
        ValidTo = validTo;
    }
    public string AmountDescription { get; set; } = default!;
    public decimal Amount { get; set; }
    public string? LinkId { get; set; }
    public string? Option { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public string ServiceId { get; set; } = default!;
}
