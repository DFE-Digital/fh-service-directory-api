using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Events;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Api.Commands.UpdateLocation;

public class UpdateLocationCommand : IRequest<string>
{
    public UpdateLocationCommand(LocationDto locationDto)
    {
        LocationDto = locationDto;
    }

    public LocationDto LocationDto { get; init; }
}

public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateLocationCommandHandler> _logger;
    public UpdateLocationCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateLocationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            
            var existingLocation = await _context.Locations
                .Include(l => l.PhysicalAddresses)
                .SingleOrDefaultAsync(p => p.Name == request.LocationDto.Name, cancellationToken);

            if (existingLocation == null)
            {
                throw new NotFoundException(nameof(Taxonomy), request.LocationDto.Id);
            }

            existingLocation.Description = request.LocationDto.Description;
            existingLocation.Latitude = request.LocationDto.Latitude;
            existingLocation.Longitude = request.LocationDto.Longitude;

            if (request.LocationDto.PhysicalAddresses != null)
            {
                foreach (var addressDto in request.LocationDto.PhysicalAddresses)
                {
                    var existingAddress = await _context.PhysicalAddresses.SingleOrDefaultAsync(p => p.Id == addressDto.Id, cancellationToken);
                    if (existingAddress != null)
                    {
                        existingAddress.LocationId = request.LocationDto.Id;
                        existingAddress.Address1 = addressDto.Address1;
                        existingAddress.City = addressDto.City;
                        existingAddress.PostCode = addressDto.PostCode;
                        existingAddress.Country = addressDto.Country;
                        existingAddress.StateProvince = addressDto.StateProvince;
                    }
                    else
                    {
                        var newAddress = _mapper.Map<PhysicalAddress>(addressDto);
                        newAddress.LocationId = request.LocationDto.Id;
                        ArgumentNullException.ThrowIfNull(newAddress);
                        existingLocation.RegisterDomainEvent(new PhysicalAddressCreatedEvent(newAddress));

                        _context.PhysicalAddresses.Add(newAddress);
                    }
                }
            }

            if (request.LocationDto.LinkTaxonomies != null && request.LocationDto.LinkTaxonomies.Any())
            {
                foreach (var linkTaxonomyDto in request.LocationDto.LinkTaxonomies)
                {
                    var linkTaxonomy = await _context.LinkTaxonomies.SingleOrDefaultAsync(p => p.Id == linkTaxonomyDto.Id, cancellationToken);
                    if (linkTaxonomy != null)
                    {
                        linkTaxonomy.LinkType = linkTaxonomyDto.LinkType;
                        linkTaxonomy.LinkId = linkTaxonomyDto.LinkId;

                        if (linkTaxonomyDto.Taxonomy != null)
                        {
                            var taxonomy = _context.Taxonomies.FirstOrDefault(x => linkTaxonomy.Taxonomy != null && x.Id == linkTaxonomy.Taxonomy.Id);
                            if (taxonomy != null)
                            {
                                linkTaxonomy.Taxonomy = taxonomy;
                            }
                        }
                    }
                    else
                    {
                        var linkTaxonomyEntity = _mapper.Map<LinkTaxonomy>(linkTaxonomyDto);

                        if (linkTaxonomyEntity.Taxonomy != null)
                        {
                            var taxonomy = _context.Taxonomies.FirstOrDefault(x => x.Id == linkTaxonomyEntity.Taxonomy.Id);
                            if (taxonomy != null)
                            {
                                linkTaxonomyEntity.Taxonomy = taxonomy;
                            }
                        }

                        ArgumentNullException.ThrowIfNull(linkTaxonomyEntity);

                        existingLocation.RegisterDomainEvent(new LinkTaxonomyCreatedEvent(linkTaxonomyEntity));

                        await _context.LinkTaxonomies.AddAsync(linkTaxonomyEntity, cancellationToken);
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

        return request.LocationDto.Id;
    }
}

