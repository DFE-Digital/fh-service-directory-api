using System.ComponentModel.DataAnnotations;

namespace FamilyHubs.ServiceDirectoryApi.Api.Endpoints.OldOpenReferralOrganisations;

public class CreateOrganisationRequest
{
    public const string Route = "/Organisations";

    [Required]
    public string? Name { get; set; }
}
