using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLinkTaxonomies;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.api.Commands.CreateLocation;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Locations;

public class WhenUsingLocationCommand : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenCreateLocation()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralLocationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.OpenReferralTaxonomies.Add(GetTestTaxonomy());
        mockApplicationDbContext.SaveChanges();
        var testLocation = GetTestOpenReferralLocationDto();
        CreateOpenReferralLocationCommand command = new(testLocation);
        CreateOpenReferralLocationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testLocation.Id);
    }

    [Fact]
    public async Task ThenAttemptToCreateLocationThatAlreadyExists()
    {
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateOpenReferralLocationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testLocation = GetTestOpenReferralLocationDto();
        var entity = mapper.Map<OpenReferralLocation>(testLocation);
        mockApplicationDbContext.OpenReferralLocations.Add(entity);
        mockApplicationDbContext.SaveChanges();

        CreateOpenReferralLocationCommand command = new(testLocation);
        CreateOpenReferralLocationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);

        //Act
        Func<Task> act = async () => { await handler.Handle(command, new System.Threading.CancellationToken()); };
        

        //Assert
        await act.Should().ThrowAsync<System.Exception>().WithMessage("Location Already Exists, Please use Update Location");
        
    }
    public static OpenReferralLocationDto GetTestOpenReferralLocationDto()
    {
        return new OpenReferralLocationDto(
        "661cab6d-81f6-46cd-a05b-4ec2e19b03fa",
        "Test Location",
        "Unit Test Location",
        53.474103227856105D,
        -2.2721559641660787D,
        new List<OpenReferralPhysicalAddressDto>
        {
            new OpenReferralPhysicalAddressDto(
                "e2c83465-70f4-4252-92d8-487ea7da97e0",
                "Address Line 1",
                "City1",
                "E14 2BG",
                "United Kingdom",
                "County"
                )
        },
        new List<OpenReferralLinkTaxonomyDto>
        {
            new OpenReferralLinkTaxonomyDto(
                "a8587b60-e9cd-4527-9bdd-d55955faa8c1",
                "Location",
                "661cab6d-81f6-46cd-a05b-4ec2e19b03fa",
                new OpenReferralTaxonomyDto("a3226044-5c89-4257-8b07-f29745a22e2c", "Test Taxonomy", "Test Vocabulary", null)
                )

        });
        
    }

    public static OpenReferralTaxonomy GetTestTaxonomy()
    {
        return new OpenReferralTaxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test Taxonomy", "Test Vocabulary", null);
    }
}
