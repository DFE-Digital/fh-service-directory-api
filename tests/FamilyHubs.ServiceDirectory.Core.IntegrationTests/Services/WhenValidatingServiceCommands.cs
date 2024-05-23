using AutoMapper;
using AutoMapper.EquivalencyExpression;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.CreateService;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.DeleteService;
using FamilyHubs.ServiceDirectory.Core.Commands.Services.UpdateService;
using FamilyHubs.ServiceDirectory.Data.Interceptors;
using FamilyHubs.ServiceDirectory.Data.Repository;
using FamilyHubs.SharedKernel.Security;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamilyHubs.ServiceDirectory.Core.IntegrationTests.Services;

public class WhenValidatingServiceCommands
{
    protected IHttpContextAccessor _httpContextAccessor;
    public IMapper Mapper { get; }

    public WhenValidatingServiceCommands()
    {
        var serviceProvider = CreateNewServiceProvider();

        Mapper = serviceProvider.GetRequiredService<IMapper>();
    }

    private ServiceProvider CreateNewServiceProvider()
    {
        var serviceDirectoryConnection = $"Data Source=sd-{Random.Shared.Next().ToString()}.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;Recursive Triggers=True;Default Timeout=30;Pooling=True";

        //todo: do we need a (mock) _httpContextAccessor?
        var auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor(_httpContextAccessor);

        var inMemorySettings = new Dictionary<string, string?> {
            {"UseSqlite", "true"},
            {"Crypto:UseKeyVault", "False"},
        };

        var key = AesProvider.GenerateKey(AesKeySize.AES128Bits);

        inMemorySettings.Add("Crypto:DbEncryptionKey", string.Join(",", key.Key));
        inMemorySettings.Add("Crypto:DbEncryptionIVKey", string.Join(",", key.IV));

        var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

        IKeyProvider keyProvider = new KeyProvider(configuration);

        return new ServiceCollection().AddEntityFrameworkSqlite()
            .AddDbContext<ApplicationDbContext>(dbContextOptionsBuilder =>
            {
                //dbContextOptionsBuilder.UseLoggerFactory(TestLoggerFactory);
                dbContextOptionsBuilder.UseSqlite(serviceDirectoryConnection, opt =>
                {
                    opt.UseNetTopologySuite().MigrationsAssembly(typeof(ApplicationDbContext).Assembly.ToString());
                });
            })
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton(keyProvider)
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

    [Fact]
    public void ThenShouldCreateServiceCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper, Random.Shared.Next());
        var validator = new CreateServiceCommandValidator();
        var testModel = new CreateServiceCommand(testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldUpdateServiceCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper, Random.Shared.Next());
        testService.Id = 1;
        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldCreateServiceCommandNotErrorWhenNameIsLessThen255Char()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper, Random.Shared.Next());
        testService.Name = string.Join(string.Empty, Enumerable.Range(0, 254).Select(_ => "a"));
        var validator = new CreateServiceCommandValidator();
        var testModel = new CreateServiceCommand(testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldUpdateServiceCommandNotErrorWhenNameIsLessThen255Char()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper,Random.Shared.Next());
        testService.Id = Random.Shared.Next();
        testService.Name = string.Join(string.Empty, Enumerable.Range(0, 254).Select(_ => "a"));
        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldCreateServiceCommandHasErrorsWhenNameIsGreaterThen255Char()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper, Random.Shared.Next());
        testService.Name = string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "a"));
        var validator = new CreateServiceCommandValidator();
        var testModel = new CreateServiceCommand(testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }

    [Fact]
    public void ThenShouldUpdateServiceCommandHasErrorsWhenNameIsGreaterThen255Char()
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper, Random.Shared.Next());
        testService.Name = string.Join(string.Empty, Enumerable.Range(0, 256).Select(_ => "a"));
        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }

    [Fact]
    public void ThenShouldDeleteServiceByIdCommandNotErrorWhenModelIsValid()
    {
        //Arrange
        var validator = new DeleteServiceByIdCommandValidator();
        var testModel = new DeleteServiceByIdCommand(1);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Theory]
    [InlineData(default)]
    [InlineData("http://wwww.google.co.uk")]
    [InlineData("http://wwww.google.com")]
    [InlineData("https://wwww.google.com")]
    public void ThenShouldValidateContactUrlWhenCreatingService_ShouldReturnNoErrors(string? url)
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper, Random.Shared.Next());
        testService.Id = 0;
        foreach (var item in testService.Contacts)
        {
            item.Url = url;
        }

        var validator = new CreateServiceCommandValidator();
        var testModel = new CreateServiceCommand(testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("someurl")]
    [InlineData("http://someurl")]
    [InlineData("https://someurl")]
    public void ThenShouldValidateContactUrlWhenCreatingService_ShouldReturnErrors(string url)
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper, Random.Shared.Next());
        testService.Id = 1;
        foreach (var item in testService.Contacts)
        {
            item.Url = url;
        }

        var validator = new CreateServiceCommandValidator();
        var testModel = new CreateServiceCommand(testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }

    [Theory]
    [InlineData(default)]
    [InlineData("http://wwww.google.co.uk")]
    [InlineData("http://wwww.google.com")]
    [InlineData("https://wwww.google.com")]
    public void ThenShouldValidateContactUrlWhenUpdatingService_ShouldReturnNoErrors(string? url)
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper, Random.Shared.Next());
        testService.Id = 1;
        foreach (var item in testService.Contacts)
        {
            item.Url = url;
        }

        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("someurl")]
    [InlineData("http:/someurl.")]
    [InlineData("https//someurl.")]
    public void ThenShouldValidateContactUrlWhenUpdatingService_ShouldReturnErrors(string url)
    {
        //Arrange
        var testService = TestDataProvider.GetTestCountyCouncilServicesChangeDto2(Mapper, Random.Shared.Next());
        testService.Id = 1;
        foreach (var item in testService.Contacts)
        {
            item.Url = url;
        }

        var validator = new UpdateServiceCommandValidator();
        var testModel = new UpdateServiceCommand(testService.Id, testService);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }
}
