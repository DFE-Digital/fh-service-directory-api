using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace FamilyHubs.ServiceDirectory.Core.Entities;

public class PhysicalAddress : EntityBase<string>, IAggregateRoot
{
    private PhysicalAddress() { }
    public PhysicalAddress(
        string id, 
        string address1, 
        string? city, 
        string postCode, 
        string? country, 
        string? stateProvince)
    {
        Id = id;
        Address1 = address1;
        City = city;
        PostCode = postCode;
        Country = country;
        StateProvince = stateProvince;
    }

    public string LocationId { get; set; } = default!;
    public string Address1 { get; set; } = default!;
    public string? City { get; set; }
    public string PostCode { get; set; } = default!;
    public string? Country { get; set; }
    public string? StateProvince { get; set; }
}
