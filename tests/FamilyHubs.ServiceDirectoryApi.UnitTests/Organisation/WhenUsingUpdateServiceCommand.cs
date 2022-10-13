using AutoMapper;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.api.Commands.UpdateOpenReferralOrganisation;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilyHubs.ServiceDirectory.Shared.Builders;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralContacts;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLanguages;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralOrganisations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhones;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceDeliverysEx;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServiceTaxonomys;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Builders;
using fh_service_directory_api.api.Queries.GetOpenReferralOrganisationById;
using fh_service_directory_api.api.Queries.ListOrganisation;
using FluentAssertions;
using System.Collections.ObjectModel;
using fh_service_directory_api.api.Commands.UpdateOpenReferralService;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenUsingUpdateServiceCommand : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenUpdateOpenReferralServiceOnly()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testOrganisation = WhenUsingOrganisationCommands.GetTestCountyCouncilDto();
        var updatelogger = new Mock<ILogger<UpdateOpenReferralServiceCommandHandler>>();
        var mockMediator = new Mock<ISender>();
        CreateOpenReferralOrganisationCommand command = new(testOrganisation);
        CreateOpenReferralOrganisationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);
        var id = await handler.Handle(command, new System.Threading.CancellationToken());

        OpenReferralServiceDto openReferralService = WhenUsingOrganisationCommands.GetTestCountyCouncilServicesDto(testOrganisation.Id);


        openReferralService.Name = "Unit Test Update Service Name";
        openReferralService.Description = "Unit Test Update Service Name";
        UpdateOpenReferralServiceCommand updatecommand = new(openReferralService.Id, openReferralService);
        UpdateOpenReferralServiceCommandHandler updatehandler = new(mockApplicationDbContext, mapper, updatelogger.Object);

        //Act
        var result = await updatehandler.Handle(updatecommand, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(openReferralService.Id);
    }
}
