using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.ModelLink;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using fh_service_directory_api.api.Commands.CreateModelLink;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace fh_service_directory_api.api.Commands.UpdateLocation;

public class UpdateOpenReferralLocationCommand : IRequest<string>
{
    public UpdateOpenReferralLocationCommand(OpenReferralLocationDto openReferralLocationDto, string openReferralTaxonomyId, string openReferralOrganisationId)
    {
        OpenReferralLocationDto = openReferralLocationDto;
        OpenReferralTaxonomyId = openReferralTaxonomyId;
        OpenReferralOrganisationId = openReferralOrganisationId;
    }

    public OpenReferralLocationDto OpenReferralLocationDto { get; init; }
    public string OpenReferralTaxonomyId { get; init; } = default!;
    public string OpenReferralOrganisationId { get; init; } = default!;
}

public class UpdateOpenReferralLocationCommandHandler : IRequestHandler<UpdateOpenReferralLocationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ISender _mediator;
    private readonly ILogger<UpdateOpenReferralLocationCommandHandler> _logger;
    public UpdateOpenReferralLocationCommandHandler(ApplicationDbContext context, IMapper mapper, ISender mediator, ILogger<UpdateOpenReferralLocationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateOpenReferralLocationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _context.OpenReferralLocations
                        .SingleOrDefaultAsync(p => p.Id == request.OpenReferralLocationDto.Id, cancellationToken: cancellationToken);
            
            if (entity == null)
            {
                throw new NotFoundException(nameof(OpenReferralTaxonomy), request.OpenReferralLocationDto.Id);
            }

            entity.Name = request.OpenReferralLocationDto.Name;
            entity.Description = request.OpenReferralLocationDto.Description;
            entity.Latitude= request.OpenReferralLocationDto.Latitude;
            entity.Longitude = request.OpenReferralLocationDto.Longitude;

            if (request.OpenReferralLocationDto.Physical_addresses != null)
            {
                foreach (var addressDto in request.OpenReferralLocationDto.Physical_addresses)
                {
                    var address = await _context.OpenReferralPhysical_Addresses.SingleOrDefaultAsync(p => p.Id == addressDto.Id, cancellationToken: cancellationToken);
                    if (address != null) 
                    {
                        address.Address_1 = addressDto.Address_1;
                        address.City = addressDto.City;
                        address.Postal_code = addressDto.Postal_code;
                        address.Country = addressDto.Country;
                        address.State_province = addressDto.State_province;
                    }
                    else
                    {
                        var addressEntity = _mapper.Map<OpenReferralPhysical_Address>(addressDto);
                        ArgumentNullException.ThrowIfNull(addressEntity, nameof(addressEntity));
                        entity.RegisterDomainEvent(new OpenReferralPhysicalAddressCreatedEvent(addressEntity));

                        _context.OpenReferralPhysical_Addresses.Add(addressEntity);
                    }
                }
            }

            //Create Links if they do not already exist
            if (!_context.ModelLinks.Where(x => x.ModelOneId == entity.Id && x.LinkType == fh_service_directory_api.core.LinkType.Location).Any())
            {
                ModelLinkDto modelLinkDto = new ModelLinkDto(Guid.NewGuid().ToString(), fh_service_directory_api.core.LinkType.Location, entity.Id, request.OpenReferralTaxonomyId);
                CreateModelLinkCommand command = new CreateModelLinkCommand(modelLinkDto);
                var taxonomyLinkresult = await _mediator.Send(command, cancellationToken);
                ArgumentNullException.ThrowIfNull(taxonomyLinkresult, nameof(taxonomyLinkresult));
            }

            if (!_context.ModelLinks.Where(x => x.ModelOneId == entity.Id && x.LinkType == fh_service_directory_api.core.LinkType.Location_Organisation).Any())
            {
                ModelLinkDto modelLinkDto = new ModelLinkDto(Guid.NewGuid().ToString(), fh_service_directory_api.core.LinkType.Location_Organisation, entity.Id, request.OpenReferralOrganisationId);
                CreateModelLinkCommand command = new CreateModelLinkCommand(modelLinkDto);
                var organisationLinkresult = await _mediator.Send(command, cancellationToken);
                ArgumentNullException.ThrowIfNull(organisationLinkresult, nameof(organisationLinkresult));
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
                            var taxonomy = await _context.OpenReferralTaxonomies.SingleOrDefaultAsync(p => p.Id == request.OpenReferralTaxonomyId, cancellationToken);

                            if (taxonomy != null)
                            {
                                taxonomy.Name = linkTaxonomyDto.Taxonomy.Name;
                                taxonomy.Parent = linkTaxonomyDto.Taxonomy.Parent;
                                taxonomy.Name = linkTaxonomyDto.Taxonomy.Name;
                            }
                            else
                            {
                                var taxonomyEntity = _mapper.Map<OpenReferralTaxonomy>(linkTaxonomyDto);

                                ArgumentNullException.ThrowIfNull(taxonomyEntity, nameof(taxonomyEntity));

                                entity.RegisterDomainEvent(new OpenReferralTaxonomyCreatedEvent(taxonomyEntity));

                                await _context.OpenReferralTaxonomies.AddAsync(taxonomyEntity, cancellationToken);
                            }
                        }
                    }
                    else
                    {
                        var linkTaxonomyEntity = _mapper.Map<OpenReferralLinkTaxonomy>(linkTaxonomyDto);

                        ArgumentNullException.ThrowIfNull(linkTaxonomyEntity, nameof(linkTaxonomyEntity));

                        entity.RegisterDomainEvent(new OpenReferralLinkTaxonomyCreatedEvent(linkTaxonomyEntity));

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

        if (request is not null && request.OpenReferralLocationDto is not null)
            return request.OpenReferralLocationDto.Id;
        else
            return string.Empty;
    }
}

