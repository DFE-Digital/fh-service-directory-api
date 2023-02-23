using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.CreateService;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateOrganisation;
using FamilyHubs.ServiceDirectory.Api.Commands.UpdateService;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationAdminByOrganisationId;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationById;
using FamilyHubs.ServiceDirectory.Api.Queries.GetOrganisationTypes;
using FamilyHubs.ServiceDirectory.Api.Queries.ListOrganisation;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FamilyHubs.ServiceDirectoryApi.UnitTests.Organisation;

public class WhenUsingGetOrganisationCommands : BaseCreateDbUnitTest
{
    public WhenUsingGetOrganisationCommands()
    {
        TestOrganisation = TestDataProvider.GetTestCountyCouncilDto();

        var myProfile = new AutoMappingProfiles();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        Mapper = new Mapper(configuration);
        MockApplicationDbContext = GetApplicationDbContext();
        
        MockMediatR = new Mock<ISender>();
        var createServiceCommandHandler = new CreateServiceCommandHandler(MockApplicationDbContext, Mapper, NullLogger<CreateServiceCommandHandler>.Instance);
        var updateServiceCommandHandler = new UpdateServiceCommandHandler(MockApplicationDbContext, Mapper, NullLogger<UpdateServiceCommandHandler>.Instance);
        MockMediatR.Setup(m => m.Send(It.IsAny<CreateServiceCommand>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((notification, cToken) => 
                createServiceCommandHandler.Handle((CreateServiceCommand)notification, cToken).GetAwaiter().GetResult());

        MockMediatR.Setup(m => m.Send(It.IsAny<UpdateServiceCommand>(), It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((notification, cToken) => 
                updateServiceCommandHandler.Handle((UpdateServiceCommand)notification, cToken).GetAwaiter().GetResult());
    }

    private void CreateOrganisation()
    {
        var createOrganisationCommand = new CreateOrganisationCommand(TestOrganisation);
        var handler = new CreateOrganisationCommandHandler(MockApplicationDbContext, Mapper, MockMediatR.Object,
            GetLogger<CreateOrganisationCommandHandler>());
        handler.Handle(createOrganisationCommand, new CancellationToken()).GetAwaiter().GetResult();
    }

    private OrganisationWithServicesDto TestOrganisation { get; }
    private IMapper Mapper { get; }
    private Mock<ISender> MockMediatR { get; }
    private ApplicationDbContext MockApplicationDbContext { get; }
    private static NullLogger<T> GetLogger<T>() => new NullLogger<T>();
    
    [Fact]
    public async Task ThenGetOrganisationById()
    {
        //Arrange
        CreateOrganisation();

        var getCommand = new GetOrganisationByIdCommand { Id = TestOrganisation.Id };
        var getHandler = new GetOrganisationByIdHandler(MockApplicationDbContext);
        TestOrganisation.Logo = "";

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(TestOrganisation, opts => opts.Excluding(si => si.AdminAreaCode));
    }

    [Fact]
    public async Task ThenGetOrganisationById_ShouldThrowExceptionWhenIdDoesNotExist()
    {
        //Arrange
        var getCommand = new GetOrganisationByIdCommand { Id = Guid.NewGuid().ToString() };
        var getHandler = new GetOrganisationByIdHandler(MockApplicationDbContext);

        // Act 
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => getHandler.Handle(getCommand, new CancellationToken()));
    }

    [Fact]
    public async Task ThenListOrganisations()
    {
        //Arrange
        CreateOrganisation();
        
        var getCommand = new ListOrganisationCommand();
        var getHandler = new ListOrganisationCommandHandler(MockApplicationDbContext);
        TestOrganisation.Logo = "";

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result[0].Should().BeEquivalentTo(TestOrganisation, opts => opts
            .Excluding(si => si.Services)
            .Excluding(si => si.AdminAreaCode)
            .Excluding(si => si.LinkContacts)
        );
    }

    [Fact]
    public async Task ThenListOrganisationTypes()
    {
        //Arrange
        var seedData = new OrganisationSeedData(false);
        if (!MockApplicationDbContext.AdminAreas.Any())
        {
            MockApplicationDbContext.OrganisationTypes.AddRange(seedData.SeedOrganisationTypes());
            await MockApplicationDbContext.SaveChangesAsync();
        }

        var getCommand = new GetOrganisationTypesCommand();
        var getHandler = new GetOrganisationTypesCommandHandler(MockApplicationDbContext);

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(3);
    }

    [Fact]
    public async Task ThenGetAdminByOrganisationId()
    {
        //Arrange
        CreateOrganisation();
        
        var getCommand = new GetOrganisationAdminByOrganisationIdCommand(TestOrganisation.Id);
        var getHandler = new GetOrganisationAdminByOrganisationIdCommandHandler(MockApplicationDbContext);
        TestOrganisation.Logo = "";

        //Act
        var result = await getHandler.Handle(getCommand, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be("XTEST");
    }
}