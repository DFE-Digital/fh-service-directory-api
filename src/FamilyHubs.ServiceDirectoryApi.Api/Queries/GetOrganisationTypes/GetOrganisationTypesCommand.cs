using FamilyHubs.ServiceDirectory.Shared.Enums;
using MediatR;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationTypes;

public class GetOrganisationTypesCommand : IRequest<List<string>>
{
}

public class GetOrganisationTypesCommandHandler : IRequestHandler<GetOrganisationTypesCommand, List<string>>
{
    public Task<List<string>> Handle(GetOrganisationTypesCommand request, CancellationToken cancellationToken)
    {
        var organisationTypes = Enum.GetNames(typeof(OrganisationType)).ToList();

        return Task.FromResult(organisationTypes);
    }
}

