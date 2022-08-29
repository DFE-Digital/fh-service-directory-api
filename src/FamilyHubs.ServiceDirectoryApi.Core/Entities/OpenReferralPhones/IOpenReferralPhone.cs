namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralPhones;

public interface IOpenReferralPhone : IEntityBase<string>
{
    string Number { get; init; }
}