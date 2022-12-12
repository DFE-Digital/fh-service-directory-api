using Autofac;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Shared.Extensions;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.api;
using fh_service_directory_api.api.Endpoints;
using fh_service_directory_api.core;
using fh_service_directory_api.core.Interfaces;
using fh_service_directory_api.infrastructure;
using fh_service_directory_api.infrastructure.Persistence.Interceptors;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using fh_service_directory_api.infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
    logging.AddConsole();
    logging.AddDebug();
    logging.AddAzureWebAppDiagnostics();
})
.ConfigureServices(serviceCollection => serviceCollection
    .Configure<AzureFileLoggerOptions>(options =>
    {
        options.FileName = "azure-diagnostics-";
        options.FileSizeLimit = 50 * 1024;
        options.RetainedFileCountLimit = 5;
    })
//.Configure<AzureBlobLoggerOptions>(options =>
//{
//    options.BlobName = "log.txt";
//})
);



ConfigureBuilder.ConfigurWebApplicationBuilderHost(builder);
ConfigureBuilder.ConfigurWebApplicationBuilderServices(builder);

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? "JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"))
    };
});

//https://www.youtube.com/watch?v=cbtK3U2aOlg

builder.Services.AddAuthorization(options =>
{
    if (builder.Environment.IsProduction())
    {
        options.AddPolicy("AllAdminAccess", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole("DfEAdmin") ||
                context.User.IsInRole("LAAdmin") ||
                context.User.IsInRole("VCSAdmin")));

        options.AddPolicy("OrgAccess", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole("DfEAdmin") ||
                context.User.IsInRole("LAAdmin")));

        options.AddPolicy("ServiceAccess", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole("LAAdmin") ||
                context.User.IsInRole("VCSAdmin")));
    }
    else //LocalHost, Dev, Test, PP, disable Authorization
    {
        options.AddPolicy("AllAdminAccess", policy =>
            policy.RequireAssertion(context => true));

        options.AddPolicy("OrgAccess", policy =>
            policy.RequireAssertion(context => true));

        options.AddPolicy("ServiceAccess", policy =>
            policy.RequireAssertion(context => true));
    }
});

// ApplicationInsights


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


    string useDbType = builder.Configuration.GetValue<string>("UseDbType");

#if USE_AZURE_VAULT
    if (builder.Configuration.GetValue<bool>("UseVault"))
    {
        (bool isOk, string errorMessage) = builder.AddAzureKeyVault();
        if (!isOk)
        {
            useDbType = "UseInMemoryDatabase";
            if (!string.IsNullOrEmpty(errorMessage))
            {
                Log.Error($"An error occurred Initializing Azure Vault");
            }
        }
    }
#endif

    

    // Register Entity Framework
    DbContextOptions<ApplicationDbContext> options;

    switch (useDbType)
    {
        case "UseInMemoryDatabase":
            {
                options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase("FH-LAHubDb").Options;
            }
            break;

        case "UseSqlServerDatabase":
            {
                options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseSqlServer(builder.Configuration.GetConnectionString("ServiceDirectoryConnection") ?? string.Empty)
                         .Options;
            }
            break;

        case "UsePostgresDatabase":
            {
                options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseNpgsql(builder.Configuration.GetConnectionString("ServiceDirectoryConnection") ?? string.Empty)
                         .Options;
            }
            break;

        default:
            {
                options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase("FH-LAHubDb").Options;
            }
            break;
    }

    containerBuilder.RegisterType<ApplicationDbContext>()
       .AsSelf()
       .WithParameter("options", options);

    containerBuilder.RegisterType<MinimalOrganisationEndPoints>();
    containerBuilder.RegisterType<MinimalGeneralEndPoints>();
    containerBuilder.RegisterType<MinimalServiceEndPoints>();
    containerBuilder.RegisterType<MinimalTaxonomyEndPoints>();
    containerBuilder.RegisterType<MinimalLocationEndPoints>();
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

var applicationBuilder = WebApplication.CreateBuilder(args);
RegisterComponents(builder.Services, applicationBuilder.Configuration);
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

    var locationservice = scope.ServiceProvider.GetService<MinimalLocationEndPoints>();
    if (locationservice != null)
        locationservice.RegisterLocationEndPoints(webApplication);

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
        await initialiser.InitialiseAsync(builder.Configuration, webApplication.Environment.IsProduction());
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




static void ConfigureWebApplication(WebApplication webApplication)
{
    // Configure the HTTP request pipeline.
    webApplication.UseSwagger();
    webApplication.UseSwaggerUI();
    

    webApplication.UseHttpsRedirection();
    webApplication.UseAuthentication();
    webApplication.UseAuthorization();
    webApplication.MapControllers();
}

static void RegisterComponents(IServiceCollection builder, IConfiguration configuration)
{
    builder.AddApplicationInsights(configuration, "fh_service_directory_api.api");
}



public partial class Program { }