using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;

namespace fh_service_directory_api.core.Entities;

public class OpenReferralContact : EntityBase<string>, IAggregateRoot
{
    private OpenReferralContact() { }
    public OpenReferralContact(string id, string? title, string name, string telephone, string textPhone)
    {
        Id = id;
        Title = title;
        Name = name;
        Telephone = telephone;
        TextPhone = textPhone;
    }
    public string? Title { get; set; }
    public string Name { get; set; } = default!;
    public string OpenReferralServiceId { get; set; } = default!;
    public string Telephone { get; set; } = default!;
    public string TextPhone { get; set; } = default!;
}
