using AutoMapper;
using fh_service_directory_api.api.Commands.CreateOpenReferralOrganisation;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Misc;

public class WhenUsingOpenReferralParent : BaseCreateDbUnitTest
{
    [Fact]
    public void ThenCreateOpenReferralParent()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralOrganisationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var openReferralParent = new OpenReferralParent(
            id: "c5d202c6-2390-494c-bd96-6ef423425b13", 
            name: "Name",
            vocabulary: "vocabulary",
            serviceTaxonomyCollection: new List<OpenReferralService_Taxonomy>(),
            linkTaxonomyCollection: new List<OpenReferralLinkTaxonomy>()
            
            );

        //Act
        mockApplicationDbContext.OpenReferralParents.Add(openReferralParent);
        mockApplicationDbContext.SaveChanges();
        var result = mockApplicationDbContext.OpenReferralParents.FirstOrDefault(x => x.Id == "c5d202c6-2390-494c-bd96-6ef423425b13");

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(openReferralParent);
    }
}
