using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using fh_service_directory_api.api.Endpoints;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Interfaces.Entities;
using fh_service_directory_api.infrastructure;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ConfigurWebApplicationBuilderHost(builder);
ConfigurWebApplicationBuilderServices(builder);

var autofacContainerbuilder = builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new DefaultCoreModule());
    containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));


    // Register Entity Framework
    var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer("DefaultConnection");

    containerBuilder.RegisterType<ApplicationDbContext>()
        .WithParameter("options", dbContextOptionsBuilder.Options)
        .InstancePerLifetimeScope();

    


    containerBuilder.RegisterType<MinimalOrganisationEndPoints>();
    containerBuilder.RegisterType<MinimalGeneralEndPoints>();
    containerBuilder.RegisterType<MinimalServiceEndPoints>();
    containerBuilder.RegisterType<MinimalTaxonomyEndPoints>();

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

    var genservice = scope.ServiceProvider.GetService<MinimalGeneralEndPoints>();
    if (genservice != null)
        genservice.RegisterMinimalGeneralEndPoints(webApplication);

    try
    {
        // Seed Database

        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        ////                    context.Database.Migrate();
        context.Database.EnsureCreated();
        ////new OpenReferralOrganisationSeedData().SeedOpenReferralOrganistions();

    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
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
        typeof(IOpenReferralOrganisation).Assembly
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
