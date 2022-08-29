namespace FamilyHubs.ServiceDirectoryApi.Api.Endpoints.OldOpenReferralOrganisations;

public class CreateOrganisationResponse
{
    public CreateOrganisationResponse(int id, string name)
    {
        Id = id;
        Name = name;
    }
    public int Id { get; set; }
    public string Name { get; set; }
}
