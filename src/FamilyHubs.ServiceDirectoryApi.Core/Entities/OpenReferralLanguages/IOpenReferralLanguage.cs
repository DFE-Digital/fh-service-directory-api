namespace FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLanguages;

public interface IOpenReferralLanguage : IEntityBase<string>
{
    string Language { get; init; }
}