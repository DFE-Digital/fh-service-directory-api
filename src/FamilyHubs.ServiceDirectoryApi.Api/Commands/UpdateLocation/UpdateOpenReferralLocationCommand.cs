using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Commands.UpdateLocation;

public class UpdateOpenReferralLocationCommand : IRequest<string>
{
    public UpdateOpenReferralLocationCommand(OpenReferralLocationDto openReferralLocationDto)
    {
        OpenReferralLocationDto = openReferralLocationDto;
    }

    public OpenReferralLocationDto OpenReferralLocationDto { get; init; }
}

public class UpdateOpenReferralLocationCommandHandler : IRequestHandler<UpdateOpenReferralLocationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateOpenReferralLocationCommandHandler> _logger;
    public UpdateOpenReferralLocationCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateOpenReferralLocationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateOpenReferralLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            
            var existingLocation = await _context.OpenReferralLocations
                .Include(l => l.Physical_addresses)
                .SingleOrDefaultAsync(p => p.Name == request.OpenReferralLocationDto.Name, cancellationToken);

            if (existingLocation == null)
            {
                throw new NotFoundException(nameof(OpenReferralTaxonomy), request.OpenReferralLocationDto.Id);
            }

            existingLocation.Description = request.OpenReferralLocationDto.Description;
            existingLocation.Latitude = request.OpenReferralLocationDto.Latitude;
            existingLocation.Longitude = request.OpenReferralLocationDto.Longitude;

            if (request.OpenReferralLocationDto.Physical_addresses != null)
            {
                foreach (var addressDto in request.OpenReferralLocationDto.Physical_addresses)
                {
                    var existingAddress = await _context.OpenReferralPhysical_Addresses.SingleOrDefaultAsync(p => p.Id == addressDto.Id, cancellationToken);
                    if (existingAddress != null)
                    {
                        existingAddress.OpenReferralLocationId = request.OpenReferralLocationDto.Id;
                        existingAddress.Address_1 = addressDto.Address_1;
                        existingAddress.City = addressDto.City;
                        existingAddress.Postal_code = addressDto.Postal_code;
                        existingAddress.Country = addressDto.Country;
                        existingAddress.State_province = addressDto.State_province;
                    }
                    else
                    {
                        var newAddress = _mapper.Map<OpenReferralPhysical_Address>(addressDto);
                        newAddress.OpenReferralLocationId = request.OpenReferralLocationDto.Id;
                        ArgumentNullException.ThrowIfNull(newAddress, nameof(newAddress));
                        existingLocation.RegisterDomainEvent(new OpenReferralPhysicalAddressCreatedEvent(newAddress));

                        _context.OpenReferralPhysical_Addresses.Add(newAddress);
                    }
                }
            }

            if (request.OpenReferralLocationDto.LinkTaxonomies != null && request.OpenReferralLocationDto.LinkTaxonomies.Any())
            {
                foreach (var linkTaxonomyDto in request.OpenReferralLocationDto.LinkTaxonomies)
                {
                    var linkTaxonomy = await _context.OpenReferralLinkTaxonomies.SingleOrDefaultAsync(p => p.Id == linkTaxonomyDto.Id, cancellationToken);
                    if (linkTaxonomy != null)
                    {
                        linkTaxonomy.LinkType = linkTaxonomyDto.LinkType;
                        linkTaxonomy.LinkId = linkTaxonomyDto.LinkId;

                        if (linkTaxonomyDto.Taxonomy != null)
                        {
                            var taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => linkTaxonomy.Taxonomy != null && x.Id == linkTaxonomy.Taxonomy.Id);
                            if (taxonomy != null)
                            {
                                linkTaxonomy.Taxonomy = taxonomy;
                            }
                        }
                    }
                    else
                    {
                        var linkTaxonomyEntity = _mapper.Map<OpenReferralLinkTaxonomy>(linkTaxonomyDto);

                        if (linkTaxonomyEntity.Taxonomy != null)
                        {
                            var taxonomy = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == linkTaxonomyEntity.Taxonomy.Id);
                            if (taxonomy != null)
                            {
                                linkTaxonomyEntity.Taxonomy = taxonomy;
                            }
                        }

                        ArgumentNullException.ThrowIfNull(linkTaxonomyEntity, nameof(linkTaxonomyEntity));

                        existingLocation.RegisterDomainEvent(new OpenReferralLinkTaxonomyCreatedEvent(linkTaxonomyEntity));

                        await _context.OpenReferralLinkTaxonomies.AddAsync(linkTaxonomyEntity, cancellationToken);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating Location. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return request.OpenReferralLocationDto.Id;
    }
}

