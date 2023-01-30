using AutoMapper;
using FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.api.Commands.CreateOpenReferralService;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Misc;

public class WhenUsingOpenReferralFunding : BaseCreateDbUnitTest
{
    [Fact]
    public void ThenCreateOpenReferralFunding()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var openReferralFunding = new OpenReferralFunding("96c0cfa6-0057-403c-9f4b-c9c111b742ec", "source");

        //Act
        mockApplicationDbContext.OpenReferralFundings.Add(openReferralFunding);
        mockApplicationDbContext.SaveChanges();
        var result = mockApplicationDbContext.OpenReferralFundings.FirstOrDefault(x => x.Id == "96c0cfa6-0057-403c-9f4b-c9c111b742ec");

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(openReferralFunding);
    }
}
