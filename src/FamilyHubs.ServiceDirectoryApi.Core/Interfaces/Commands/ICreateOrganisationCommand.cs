using FamilyHubs.ServiceDirectory.Shared.Dto;

namespace FamilyHubs.ServiceDirectory.Core.Interfaces.Commands
{
    public interface ICreateOrganisationCommand
    {
        OrganisationWithServicesDto Organisation { get; init; }
    }
}