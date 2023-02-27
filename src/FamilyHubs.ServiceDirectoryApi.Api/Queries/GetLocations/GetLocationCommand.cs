using AutoMapper;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel;
using MediatR;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetLocations
{
    public class GetLocationsCommand : IRequest<Result<PaginatedList<LocationDto>>>
    {
        public GetLocationsCommand(string? id, string? name, string? description, string? postCode)
        {
            AddQueryParameterIfNotNull("Id", id);
            AddQueryParameterIfNotNull("Name", name);
            AddQueryParameterIfNotNull("Description", description);
            AddQueryParameterIfNotNull("PosCode", postCode);
        }

        public Dictionary<string, object> QueryValues { get; } = new Dictionary<string, object>();

        private void AddQueryParameterIfNotNull(string key, string? value)
        {
            if (string.IsNullOrEmpty(value)) return;
            QueryValues.Add(key, value);
        }
    }

    public class GetLocationsCommandHandler : IRequestHandler<GetLocationsCommand, Result<PaginatedList<LocationDto>>>
    {
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;

        public GetLocationsCommandHandler(ILocationService locationService, IMapper mapper)
        {
            _locationService = locationService;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedList<LocationDto>>> Handle(GetLocationsCommand request, CancellationToken cancellationToken)
        {
            var locations = _locationService.GetLocations(request.QueryValues);
            var locationsDto = _mapper.Map<List<LocationDto>>(locations);
            var paginatedList = new PaginatedList<LocationDto>(locationsDto, locationsDto.Count, 1, 10);

            return Result<PaginatedList<LocationDto>>.Success(paginatedList);
        }
    }
}
