using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLinkTaxonomies;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralLocations;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralTaxonomys;
using fh_service_directory_api.api.Commands.CreateLocation;
using fh_service_directory_api.api.Commands.UpdateLocation;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
    public async Task ThenUpdateLocation()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<UpdateOpenReferralLocationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.OpenReferralTaxonomies.Add(GetTestTaxonomy());
        mockApplicationDbContext.SaveChanges();
        var testLocation = GetTestOpenReferralLocationDto();
        CreateOpenReferralLocationCommand createcommand = new(testLocation);
        CreateOpenReferralLocationCommandHandler createHandler = new(mockApplicationDbContext, mapper, new Mock<ILogger<CreateOpenReferralLocationCommandHandler>>().Object);
        await createHandler.Handle(createcommand, new System.Threading.CancellationToken());
        UpdateOpenReferralLocationCommand command = new(testLocation);
        UpdateOpenReferralLocationCommandHandler handler = new UpdateOpenReferralLocationCommandHandler(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testLocation.Id);
    }

    [Fact]
    public async Task ThenUpdateLocationWithNewAddressAndTaxonomy()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<UpdateOpenReferralLocationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.OpenReferralTaxonomies.Add(GetTestTaxonomy());
        mockApplicationDbContext.SaveChanges();
        var testLocation = GetTestOpenReferralLocationDto();
        CreateOpenReferralLocationCommand createcommand = new(testLocation);
        CreateOpenReferralLocationCommandHandler createHandler = new(mockApplicationDbContext, mapper, new Mock<ILogger<CreateOpenReferralLocationCommandHandler>>().Object);
        await createHandler.Handle(createcommand, new System.Threading.CancellationToken());

        var physicalAddresses = new List<OpenReferralPhysicalAddressDto>
        {
            new OpenReferralPhysicalAddressDto(
                id: "fa4f96b1-a9f8-4cd9-8c2b-de40794e0fb0",
                address_1: "New Address Line 1",
                city: "New City1",
                postal_code: "E14 3BG",
                country: "United Kingdom",
                state_province: "New County"
            )
        };

        var newTaxonomyList = new List<OpenReferralLinkTaxonomyDto>
        {
            new OpenReferralLinkTaxonomyDto(
                id: "491a553a-8f97-4ac7-941d-8eafa981042b",
                linkType: "Location",
                linkId: "661cab6d-81f6-46cd-a05b-4ec2e19b03fa",
                taxonomy: new OpenReferralTaxonomyDto("eeca6a0b-36d9-4a7f-9f74-d6bc3913cddd", "Test Taxonomy Update", "Test Vocabulary Update", null)
                )

        };

        var updateLocation = new OpenReferralLocationDto(testLocation.Id, testLocation.Name, testLocation.Description, testLocation.Latitude, testLocation.Longitude, physicalAddresses, newTaxonomyList);

        UpdateOpenReferralLocationCommand command = new(updateLocation);
        UpdateOpenReferralLocationCommandHandler handler = new UpdateOpenReferralLocationCommandHandler(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testLocation.Id);
    }

    [Fact]
    public async Task ThenUpdateLocationThatDoesNotExist()
    {
        //Arange
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<UpdateOpenReferralLocationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.OpenReferralTaxonomies.Add(GetTestTaxonomy());
        mockApplicationDbContext.SaveChanges();
        var testLocation = GetTestOpenReferralLocationDto();
        var updateLocation = new OpenReferralLocationDto("d3948216-3b71-49a4-86b0-0d6d63758a3c", testLocation.Name, testLocation.Description, testLocation.Latitude, testLocation.Longitude, testLocation.Physical_addresses, testLocation.LinkTaxonomies);

        UpdateOpenReferralLocationCommand command = new(updateLocation);
        UpdateOpenReferralLocationCommandHandler handler = new UpdateOpenReferralLocationCommandHandler(mockApplicationDbContext, mapper, logger.Object);

        //Act
        Func<Task> act = async () => { await handler.Handle(command, new System.Threading.CancellationToken()); };


        //Assert
        await act.Should().ThrowAsync<System.Exception>();
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
        id: "661cab6d-81f6-46cd-a05b-4ec2e19b03fa",
        name: "Test Location",
        description: "Unit Test Location",
        latitude: 53.474103227856105D,
        longitude: -2.2721559641660787D,
        new List<OpenReferralPhysicalAddressDto>
        {
            new OpenReferralPhysicalAddressDto(
                id: "e2c83465-70f4-4252-92d8-487ea7da97e0",
                address_1: "Address Line 1",
                city: "City1",
                postal_code: "E14 2BG",
                country: "United Kingdom",
                state_province: "County"
                )
        },
        new List<OpenReferralLinkTaxonomyDto>
        {
            new OpenReferralLinkTaxonomyDto(
                id: "a8587b60-e9cd-4527-9bdd-d55955faa8c1",
                linkType: "Location",
                linkId: "661cab6d-81f6-46cd-a05b-4ec2e19b03fa",
                taxonomy: new OpenReferralTaxonomyDto("a3226044-5c89-4257-8b07-f29745a22e2c", "Test Taxonomy", "Test Vocabulary", null)
                )

        });
        
    }

    public static OpenReferralTaxonomy GetTestTaxonomy()
    {
        return new OpenReferralTaxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test Taxonomy", "Test Vocabulary", null);
    }
}
