using Ardalis.GuardClauses;
using Ardalis.Specification;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.api.Commands.UpdateOpenReferralService;
using fh_service_directory_api.api.Helper;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Events;
using fh_service_directory_api.core.Interfaces.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace fh_service_directory_api.api.Commands.UpdateOpenReferralOrganisation;

public class UpdateOpenReferralOrganisationCommand : IRequest<string>
{
    public UpdateOpenReferralOrganisationCommand(string id, OpenReferralOrganisationWithServicesDto openReferralOrganisation)
    {
        Id = id;
        OpenReferralOrganisation = openReferralOrganisation;
    }

    public OpenReferralOrganisationWithServicesDto OpenReferralOrganisation { get; init; }

    public string Id { get; set; }
}

public class UpdateOpenReferralOrganisationCommandHandler : IRequestHandler<UpdateOpenReferralOrganisationCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UpdateOpenReferralOrganisationCommandHandler> _logger;
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public UpdateOpenReferralOrganisationCommandHandler(ApplicationDbContext context, ILogger<UpdateOpenReferralOrganisationCommandHandler> logger, ISender mediator, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateOpenReferralOrganisationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var entity = await _context.OpenReferralOrganisations
          .Include(x => x.OrganisationType)
          .Include(x => x.Services!)
          .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(OpenReferralOrganisation), request.Id);
        }

        try
        {
            
            OpenReferralOrganisation org = _mapper.Map<OpenReferralOrganisation>(request.OpenReferralOrganisation);
            var organisationType = _context.OrganisationTypes.FirstOrDefault(x => x.Id == request.OpenReferralOrganisation.OrganisationType.Id);
            if (organisationType != null)
            {
                org.OrganisationType = organisationType;
            }
            
            entity.Update(org);
            AddOrUpdateAdministractiveDistrict(request, entity);

            if (entity.Services != null && request.OpenReferralOrganisation.Services != null)
            {
                // Delete children (does this need to be a soft delete)
                foreach (var existingChild in entity.Services)
                {
                    if (!request.OpenReferralOrganisation.Services.Any(c => c.Id == existingChild.Id))
                    {
                        // Replace with soft delete
                        //_context.OpenReferralServices.Remove(existingChild);
                    }
                }

                // Update and Insert children
                foreach (var childModel in request.OpenReferralOrganisation.Services)
                {
                    var existingChild = entity.Services
                        .Where(c => c.Id == childModel.Id && c.Id != default)
                        .SingleOrDefault();

                    if (existingChild != null)
                    {
                        UpdateOpenReferralServiceCommand updateOpenReferralServiceCommand = new(existingChild.Id, childModel);
                        var serviceUpdateResult = await _mediator.Send(updateOpenReferralServiceCommand, cancellationToken);  
                    }
                    else
                    {
                        if (childModel != null)
                        {
                            OpenReferralService service = _mapper.Map<OpenReferralService>(childModel);

                            var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == childModel.ServiceType.Id);
                            if (serviceType != null)
                                service.ServiceType = serviceType;

                            if (childModel.Service_taxonomys != null)
                            {
                                for (int i = 0; i < childModel?.Service_taxonomys.Count; i++)
                                {
                                    if (childModel.Service_taxonomys.ElementAt(i) != null && childModel.Service_taxonomys.ElementAt(i).Taxonomy != null)
                                    {
                                        string id = childModel?.Service_taxonomys?.ElementAt(i)?.Taxonomy?.Id ?? string.Empty;
                                        var tx = _context.OpenReferralTaxonomies.FirstOrDefault(x => x.Id == id);
                                        if (childModel != null)
                                            service.Service_taxonomys.ElementAt(i).Taxonomy = tx;
                                    }
                                }
                            }

                            entity.RegisterDomainEvent(new OpenReferralServiceCreatedEvent(service));
                            _context.OpenReferralServices.Add(service);
                        }
                    }
                }
            }

            //Review not yet included
            //if (entity.Reviews != null && request.OpenReferralOrganisation.Reviews != null) //TODO - also check for count=0 s if count==0, dont enter if block
            //{
            //    // Delete children (does this need to be a soft delete)
            //    foreach (var existingChild in entity.Reviews)
            //    {
            //        if (!request.OpenReferralOrganisation.Reviews.Any(c => c.Id == existingChild.Id))
            //            _context.OpenReferralReviews.Remove(existingChild as OpenReferralReview);
            //    }

            //    foreach (var childModel in request.OpenReferralOrganisation.Reviews)
            //    {
            //        var existingChild = entity.Reviews
            //            .Where(c => c.Id == childModel.Id && c.Id != default)
            //            .SingleOrDefault();

            //        if (existingChild != null)
            //            existingChild.Update(childModel);
            //        else
            //        {
            //            entity.RegisterDomainEvent(new OpenReferralReviewCreatedEvent(childModel));

            //            _context.OpenReferralReviews.Add(childModel as OpenReferralReview);

            //        }
            //    }
            //}

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating organisation. {exceptionMessage}", ex.Message);
            throw new Exception(ex.Message, ex);
        }

        return entity.Id;
    }

    private void AddOrUpdateAdministractiveDistrict(UpdateOpenReferralOrganisationCommand request, OpenReferralOrganisation openReferralOrganisation)
    {
        if (!string.IsNullOrEmpty(request.OpenReferralOrganisation.AdministractiveDistrictCode))
        {
            var organisationAdminDistrict = _context.OrganisationAdminDistricts.FirstOrDefault(x => x.OpenReferralOrganisationId == openReferralOrganisation.Id);
            if (organisationAdminDistrict == null)
            {
                var entity = new OrganisationAdminDistrict(
                    Guid.NewGuid().ToString(),
                    request.OpenReferralOrganisation.AdministractiveDistrictCode,
                    openReferralOrganisation.Id);

                entity.RegisterDomainEvent(new OrganisationAdminDistrictCreatedEvent(entity));
                _context.OrganisationAdminDistricts.Add(entity);
            }
            else
            {
                organisationAdminDistrict.Code = request.OpenReferralOrganisation.AdministractiveDistrictCode;
            }
            
        }
    }

}


