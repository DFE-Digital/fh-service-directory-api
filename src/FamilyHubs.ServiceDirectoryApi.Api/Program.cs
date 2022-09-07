using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.api;
using fh_service_directory_api.api.Endpoints;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Interfaces;
using fh_service_directory_api.core.Interfaces.Infrastructure;
using fh_service_directory_api.infrastructure;
using fh_service_directory_api.infrastructure.Persistence.Interceptors;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using fh_service_directory_api.infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

ConfigurWebApplicationBuilderHost(builder);
ConfigurWebApplicationBuilderServices(builder);

var autofacContainerbuilder = builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new DefaultCoreModule());
    containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));

    containerBuilder.RegisterType<HttpContextAccessor>()
            .As<IHttpContextAccessor>().SingleInstance();

    containerBuilder.RegisterType<DateTimeService>()
            .As<IDateTime>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<CurrentUserService>()
            .As<ICurrentUserService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<AuditableEntitySaveChangesInterceptor>();

    // Register Entity Framework
    DbContextOptions<ApplicationDbContext> options;

    if (builder.Configuration.GetValue<bool>("UseInMemoryDatabase"))
    {
        options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase("FH-LAHubDb").Options;
    }
    else if (builder.Configuration.GetValue<bool>("UseSqlServerDatabase"))
    {
        options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                         .Options;
    }
    else
    {
        options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                         .Options;
    }

    containerBuilder.RegisterType<ApplicationDbContext>()
       .AsSelf()
       .WithParameter("options", options);

    containerBuilder.RegisterType<MinimalOrganisationEndPoints>();
    containerBuilder.RegisterType<MinimalGeneralEndPoints>();
    containerBuilder.RegisterType<MinimalServiceEndPoints>();
    containerBuilder.RegisterType<MinimalTaxonomyEndPoints>();
    containerBuilder.RegisterType<MinimalUICacheEndPoints>();
    containerBuilder.RegisterType<ApplicationDbContextInitialiser>();

    containerBuilder
    .RegisterAssemblyTypes(typeof(IRequest<>).Assembly)
    .Where(t => t.IsClosedTypeOf(typeof(IRequest<>)))
    .AsImplementedInterfaces();

    containerBuilder
        .RegisterAssemblyTypes(typeof(IRequestHandler<>).Assembly)
        .Where(t => t.IsClosedTypeOf(typeof(IRequestHandler<>)))
        .AsImplementedInterfaces();

    containerBuilder.Register(c => new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new AutoMappingProfiles());

    })).AsSelf().SingleInstance();

    containerBuilder.Register(c =>
    {
        //This resolves a new context that can be used later.
        var context = c.Resolve<IComponentContext>();
        var config = context.Resolve<MapperConfiguration>();
        return config.CreateMapper(context.Resolve);
    })
        .As<IMapper>()
        .InstancePerLifetimeScope();

});

var webApplication = builder.Build();
webApplication.UseSerilogRequestLogging();
ConfigureWebApplication(webApplication);

using (var scope = webApplication.Services.CreateScope())
{
    var orgservice = scope.ServiceProvider.GetService<MinimalOrganisationEndPoints>();
    if (orgservice != null)
        orgservice.RegisterOrganisationEndPoints(webApplication);

    var serservice = scope.ServiceProvider.GetService<MinimalServiceEndPoints>();
    if (serservice != null)
        serservice.RegisterServiceEndPoints(webApplication);

    var taxonyservice = scope.ServiceProvider.GetService<MinimalTaxonomyEndPoints>();
    if (taxonyservice != null)
        taxonyservice.RegisterTaxonomyEndPoints(webApplication);

    var uiCacheservice = scope.ServiceProvider.GetService<MinimalUICacheEndPoints>();
    if (uiCacheservice != null)
        uiCacheservice.RegisterUICacheEndPoints(webApplication);

    var genservice = scope.ServiceProvider.GetService<MinimalGeneralEndPoints>();
    if (genservice != null)
        genservice.RegisterMinimalGeneralEndPoints(webApplication);

    try
    {
        // Seed Database
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync(builder.Configuration);
        await initialiser.SeedAsync();

    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
        if (logger != null)
            logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
    }
}


webApplication.Run();


static void ConfigurWebApplicationBuilderHost(WebApplicationBuilder builder)
{
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
}

static void ConfigurWebApplicationBuilderServices(WebApplicationBuilder builder)
{
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "fh-service-directory-api.api", Version = "v1" });
        c.EnableAnnotations();
    });

    var assemblies = new Assembly[]
          {
        typeof(Program).Assembly,
        typeof(ApplicationDbContext).Assembly,
        typeof(OpenReferralOrganisation).Assembly
          };
    builder.Services.AddMediatR(assemblies);
}

static void ConfigureWebApplication(WebApplication webApplication)
{
    // Configure the HTTP request pipeline.
    if (webApplication.Environment.IsDevelopment())
    {
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI();
    }

    webApplication.UseHttpsRedirection();
    webApplication.UseAuthorization();
    webApplication.MapControllers();
}


public partial class Program { }