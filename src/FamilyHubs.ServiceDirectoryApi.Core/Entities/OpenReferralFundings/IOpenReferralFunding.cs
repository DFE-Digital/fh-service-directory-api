namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralFundings;

public interface IOpenReferralFunding : IEntityBase<string>
{
    string Source { get; init; }
}