using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateLocation;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateLocation;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Shared.Dto;
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
        var logger = new Mock<ILogger<CreateLocationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.Taxonomies.Add(GetTestTaxonomy());
        await mockApplicationDbContext.SaveChangesAsync();
        var testLocation = GetTestLocationDto();
        CreateLocationCommand command = new(testLocation);
        CreateLocationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

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
        var logger = new Mock<ILogger<UpdateLocationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.Taxonomies.Add(GetTestTaxonomy());
        await mockApplicationDbContext.SaveChangesAsync();
        var testLocation = GetTestLocationDto();
        CreateLocationCommand createcommand = new(testLocation);
        CreateLocationCommandHandler createHandler = new(mockApplicationDbContext, mapper, new Mock<ILogger<CreateLocationCommandHandler>>().Object);
        await createHandler.Handle(createcommand, new CancellationToken());
        UpdateLocationCommand command = new(testLocation);
        var handler = new UpdateLocationCommandHandler(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

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
        var logger = new Mock<ILogger<UpdateLocationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.Taxonomies.Add(GetTestTaxonomy());
        await mockApplicationDbContext.SaveChangesAsync();
        var testLocation = GetTestLocationDto();
        CreateLocationCommand createcommand = new(testLocation);
        CreateLocationCommandHandler createHandler = new(mockApplicationDbContext, mapper, new Mock<ILogger<CreateLocationCommandHandler>>().Object);
        await createHandler.Handle(createcommand, new CancellationToken());

        var physicalAddresses = new List<PhysicalAddressDto>
        {
            new PhysicalAddressDto(
                "fa4f96b1-a9f8-4cd9-8c2b-de40794e0fb0",
                "New Address Line 1",
                "New City1",
                "E14 3BG",
                "United Kingdom",
                "New County"
            )
        };

        var newTaxonomyList = new List<LinkTaxonomyDto>
        {
            new LinkTaxonomyDto(
                "491a553a-8f97-4ac7-941d-8eafa981042b",
                "Location",
                "661cab6d-81f6-46cd-a05b-4ec2e19b03fa",
                new TaxonomyDto("eeca6a0b-36d9-4a7f-9f74-d6bc3913cddd", "Test Taxonomy Update", "Test Vocabulary Update", null)
                )

        };

        var updateLocation = new LocationDto(testLocation.Id, testLocation.Name, testLocation.Description, testLocation.Latitude, testLocation.Longitude, physicalAddresses, newTaxonomyList);

        UpdateLocationCommand command = new(updateLocation);
        var handler = new UpdateLocationCommandHandler(mockApplicationDbContext, mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

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
        var logger = new Mock<ILogger<UpdateLocationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.Taxonomies.Add(GetTestTaxonomy());
        await mockApplicationDbContext.SaveChangesAsync();
        var testLocation = GetTestLocationDto();
        var updateLocation = new LocationDto("d3948216-3b71-49a4-86b0-0d6d63758a3c", testLocation.Name, testLocation.Description, testLocation.Latitude, testLocation.Longitude, testLocation.PhysicalAddresses, testLocation.LinkTaxonomies);

        UpdateLocationCommand command = new(updateLocation);
        var handler = new UpdateLocationCommandHandler(mockApplicationDbContext, mapper, logger.Object);

        //Act
        Func<Task> act = async () => { await handler.Handle(command, new CancellationToken()); };


        //Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task ThenAttemptToCreateLocationThatAlreadyExists()
    {
        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        IMapper mapper = new Mapper(configuration);
        var logger = new Mock<ILogger<CreateLocationCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var testLocation = GetTestLocationDto();
        var entity = mapper.Map<Location>(testLocation);
        mockApplicationDbContext.Locations.Add(entity);
        await mockApplicationDbContext.SaveChangesAsync();

        CreateLocationCommand command = new(testLocation);
        CreateLocationCommandHandler handler = new(mockApplicationDbContext, mapper, logger.Object);

        //Act
        Func<Task> act = async () => { await handler.Handle(command, new CancellationToken()); };
        

        //Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Location Already Exists, Please use Update Location");
        
    }
    public static LocationDto GetTestLocationDto()
    {
        return new LocationDto(
        "661cab6d-81f6-46cd-a05b-4ec2e19b03fa",
        "Test Location",
        "Unit Test Location",
        53.474103227856105D,
        -2.2721559641660787D,
        new List<PhysicalAddressDto>
        {
            new PhysicalAddressDto(
                "e2c83465-70f4-4252-92d8-487ea7da97e0",
                "Address Line 1",
                "City1",
                "E14 2BG",
                "United Kingdom",
                "County"
                )
        },
        new List<LinkTaxonomyDto>
        {
            new LinkTaxonomyDto(
                "a8587b60-e9cd-4527-9bdd-d55955faa8c1",
                "Location",
                "661cab6d-81f6-46cd-a05b-4ec2e19b03fa",
                new TaxonomyDto("a3226044-5c89-4257-8b07-f29745a22e2c", "Test Taxonomy", "Test Vocabulary", null)
                )

        });
        
    }

    public static Taxonomy GetTestTaxonomy()
    {
        return new Taxonomy("a3226044-5c89-4257-8b07-f29745a22e2c", "Test Taxonomy", "Test Vocabulary", null);
    }
}
