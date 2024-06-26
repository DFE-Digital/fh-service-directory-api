﻿using AutoFixture;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using FamilyHubs.ServiceDirectory.Core.Helper;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Interceptors;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.ServiceDirectory.Shared.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests;

public class DataIntegrationTestBase : IDisposable, IAsyncDisposable
{
    public OrganisationDetailsDto TestOrganisation { get; set; }
    public OrganisationDetailsDto TestOrganisationFreeService { get; set; }
    public OrganisationDto TestOrganisationWithoutAnyServices { get; set; }
    public IMapper Mapper { get; }
    public IConfiguration Configuration { get; }
    public ApplicationDbContext TestDbContext { get; }
    public static NullLogger<T> GetLogger<T>() => new();
    protected readonly IHttpContextAccessor HttpContextAccessor;
    public readonly Fixture FixtureObjectGenerator;
    public static readonly ILoggerFactory TestLoggerFactory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });

    public DataIntegrationTestBase()
    {
        FixtureObjectGenerator = new Fixture();
        TestOrganisation = TestDataProvider.GetTestCountyCouncilDto();
        TestOrganisationFreeService = TestDataProvider.GetTestCountyCouncilWithFreeServiceDto();
        TestOrganisationWithoutAnyServices = TestDataProvider.GetTestCountyCouncilWithoutAnyServices();

        var serviceProvider = CreateNewServiceProvider();

        TestDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        Mapper = serviceProvider.GetRequiredService<IMapper>();
        Configuration = serviceProvider.GetRequiredService<IConfiguration>();

        HttpContextAccessor = Mock.Of<IHttpContextAccessor>();

        InitialiseDatabase();
    }

    public LocationDto GetTestLocation()
    {
        var locationDto = TestOrganisation.Services.ElementAt(0).Locations.ElementAt(0);
        locationDto.OrganisationId = TestDbContext.Organisations.First().Id;
        return locationDto;
    }

    public async Task<OrganisationDetailsDto> CreateOrganisationDetails(OrganisationDetailsDto? organisationDto = null)
    {
        var organisationWithServices = Mapper.Map<Organisation>(organisationDto ?? TestOrganisation);

        organisationWithServices.Locations.Add(organisationWithServices.Services.First().Locations.First());

        TestDbContext.Organisations.Add(organisationWithServices);

        await TestDbContext.SaveChangesAsync();

        return Mapper.Map(organisationWithServices, organisationDto ?? TestOrganisation);
    }

    public async Task<OrganisationDto> CreateOrganisation(OrganisationDto? organisationDto = null)
    {
        var organisationWithoutAnyServices = Mapper.Map<Organisation>(organisationDto ?? TestOrganisationWithoutAnyServices);

        var organisationWithServices = Mapper.Map<Organisation>(organisationDto ?? TestOrganisation);

        organisationWithoutAnyServices.Locations.Add(organisationWithServices.Services.First().Locations.First());

        TestDbContext.Organisations.Add(organisationWithoutAnyServices);

        await TestDbContext.SaveChangesAsync();

        return Mapper.Map(organisationWithoutAnyServices, organisationDto ?? TestOrganisationWithoutAnyServices);
    }

    public async Task<long> CreateLocation(LocationDto locationDto)
    {
        var existingLocation = Mapper.Map<Location>(locationDto);

        TestDbContext.Locations.Add(existingLocation);

        await TestDbContext.SaveChangesAsync();

        return existingLocation.Id;
    }

    public async Task<long> AddServiceAtLocationSchedule(long serviceAtLocationId, ScheduleDto scheduleDto)
    {
        var schedule = Mapper.Map<Schedule>(scheduleDto);

        var serviceAtLocation = TestDbContext.ServiceAtLocations.First(sal => sal.Id == serviceAtLocationId);

        serviceAtLocation.Schedules.Add(schedule);

        await TestDbContext.SaveChangesAsync();

        return schedule.Id;
    }

    //public async Task<long> CreateContact(ContactDto contactDto)
    //{
    //    var existingContact = Mapper.Map<Contact>(contactDto);

    //    TestDbContext.Contacts.Add(existingContact);

    //    await TestDbContext.SaveChangesAsync();

    //    return existingContact.Id;
    //}

    //public async Task<long> CreateTaxonomy(TaxonomyDto taxonomyDto)
    //{
    //    var existingLocation = Mapper.Map<Taxonomy>(taxonomyDto);

    //    TestDbContext.Taxonomies.Add(existingLocation);
    //    await TestDbContext.SaveChangesAsync();

    //    return existingLocation.Id;
    //}

    public async Task<List<Service>> CreateManyTestServicesQueryTesting()
    {
        var testOrganisations = TestDbContext.Organisations.Select(o => new { o.Id, o.Name })
            .Where(o => o.Name == "Bristol County Council" || o.Name == "Salford City Council")
            .ToDictionary(arg => arg.Name, arg => arg.Id);

        var services = new List<Service>();

        services.AddRange(TestDataProvider.SeedBristolServices(testOrganisations["Bristol County Council"]));
        services.AddRange(TestDataProvider.SeedSalfordService(testOrganisations["Salford City Council"]));


        TestDbContext.Services.AddRange(services);

        await TestDbContext.SaveChangesAsync();

        return services;
    }

    private void InitialiseDatabase()
    {
        TestDbContext.Database.EnsureDeleted();
        TestDbContext.Database.EnsureCreated();
        TestDbContext.Database.ExecuteSqlRaw($"UPDATE geometry_columns SET srid = {GeoPoint.WGS84} WHERE f_table_name = 'locations';");

        var organisationSeedData = new OrganisationSeedData(TestDbContext);

        if (!TestDbContext.Taxonomies.Any())
            organisationSeedData.SeedTaxonomies().GetAwaiter().GetResult();

        if (!TestDbContext.Organisations.Any())
            organisationSeedData.SeedOrganisations().GetAwaiter().GetResult();
    }

    protected ServiceProvider CreateNewServiceProvider()
    {
        var serviceDirectoryConnection = $"Data Source=sd-{Random.Shared.Next().ToString()}.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;Recursive Triggers=True;Default Timeout=30;Pooling=True";
        
        var auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor(HttpContextAccessor);

        var inMemorySettings = new Dictionary<string, string?> {
            {"UseSqlite", "true"},
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

        return new ServiceCollection().AddEntityFrameworkSqlite()
            .AddDbContext<ApplicationDbContext>(dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder.UseLoggerFactory(TestLoggerFactory);
                dbContextOptionsBuilder.UseSqlite(serviceDirectoryConnection, opt =>
                {
                    opt.UseNetTopologySuite().MigrationsAssembly(typeof(ApplicationDbContext).Assembly.ToString());
                });
            })
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton(auditableEntitySaveChangesInterceptor)
            .AddAutoMapper((serviceProvider, cfg) =>
            {
                var auditProperties = new[] { "CreatedBy", "Created", "LastModified", "LastModified" };
                cfg.AddProfile<AutoMappingProfiles>();
                cfg.AddCollectionMappers();
                cfg.UseEntityFrameworkCoreModel<ApplicationDbContext>(serviceProvider);
                cfg.ShouldMapProperty = pi => !auditProperties.Contains(pi.Name);
            }, typeof(AutoMappingProfiles))
            .BuildServiceProvider();
    }

    protected Organisation CreateChildOrganisation(Organisation parent)
    {
        var child = new Organisation 
        { 
            AdminAreaCode = parent.AdminAreaCode,
            AssociatedOrganisationId = parent.Id,
            Description = FixtureObjectGenerator.Create<string>(),
            Name = FixtureObjectGenerator.Create<string>(),
            OrganisationType = Shared.Enums.OrganisationType.VCFS,
            Id = FixtureObjectGenerator.Create<long>()
        };

        return child;
    }

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await TestDbContext.Database.EnsureDeletedAsync();
    }
}
