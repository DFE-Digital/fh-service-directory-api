using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Helper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FamilyHubs.SharedKernel;
using MediatR;

namespace FamilyHubs.ServiceDirectory.Api.Queries.GetLocations
{
    public class GetLocationsCommand : IRequest<Result<PaginatedList<LocationDto>>>
    {
        public GetLocationsCommand(string? id, string? name, string? description, string? postCode, int? pageSize, int? pageNumber)
        {
            QueryValues = new LocationQuery { 
                Id = id, 
                Name = name,
                Description = description, 
                PostCode = postCode,
                PageNumber = pageNumber, 
                PageSize =pageSize
            };
        }

        public LocationQuery QueryValues { get; }
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
            var locations = await _locationService.GetLocations(request.QueryValues);
            var locationsDto = _mapper.Map<List<LocationDto>>(locations);
            var paginatedList = PaginationHelper.PaginatedResults(locationsDto, request.QueryValues);

            return Result<PaginatedList<LocationDto>>.Success(paginatedList);
        }
    }
}
