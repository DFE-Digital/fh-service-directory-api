using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FamilyHubs.ServiceDirectory.Api.Endpoints;
using FamilyHubs.ServiceDirectory.Api.Middleware;
using FamilyHubs.ServiceDirectory.Core;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Interfaces;
using FamilyHubs.ServiceDirectory.Infrastructure;
using FamilyHubs.ServiceDirectory.Infrastructure.Domain;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Interceptors;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectory.Infrastructure.Services;
using FamilyHubs.SharedKernel.Interfaces;
using IdGen.DependencyInjection;
using MediatR;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

namespace FamilyHubs.ServiceDirectory.Api;

public static class StartupExtensions
{
    public static void ConfigureHost(this WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        // ApplicationInsights
        builder.Host.UseSerilog((_, services, loggerConfiguration) =>
        {
            var logLevelString = builder.Configuration["LogLevel"];

            var parsed = Enum.TryParse<LogEventLevel>(logLevelString, out var logLevel);

            loggerConfiguration.WriteTo.ApplicationInsights(
                services.GetRequiredService<TelemetryConfiguration>(), 
                TelemetryConverter.Traces, 
                parsed ? logLevel : LogEventLevel.Warning);

            loggerConfiguration.WriteTo.Console(
                parsed ? logLevel : LogEventLevel.Warning);
        });
    }

    public static void RegisterApplicationComponents(this WebApplicationBuilder builder)
    {

        var idGenerationInstanceId = builder.Configuration.GetValue<int?>("IdGenerationInstanceId");
        if(idGenerationInstanceId.HasValue)
        {
            builder.Services.AddIdGen(idGenerationInstanceId.Value);
        }

        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.IsDevelopment()));

            containerBuilder.RegisterType<LocationService>().As<ILocationService>().SingleInstance();
            containerBuilder.RegisterType<ContactService>().As<IContactService>().SingleInstance();
            containerBuilder.RegisterType<LocationRootAggregate>().As<ILocationRootAggregate>().SingleInstance();

            containerBuilder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            containerBuilder.RegisterType<DateTimeService>().As<IDateTime>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<CurrentUserService>().As<ICurrentUserService>().InstancePerLifetimeScope();

            containerBuilder.RegisterType<AuditableEntitySaveChangesInterceptor>();

            var useDbType = builder.Configuration.GetValue<string>("UseDbType");

#if USE_AZURE_VAULT
    if (builder.Configuration.GetValue<bool>("UseVault"))
    {
        (bool isOk, string errorMessage)
 = builder.AddAzureKeyVault();
        if (!isOk)
        {
            useDbType
 = "UseInMemoryDatabase";
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
                            .UseSqlServer(builder.Configuration.GetConnectionString("ServiceDirectoryConnection") ??
                                          string.Empty)
                            .Options;
                    }
                    break;

                case "UsePostgresDatabase":
                    {
                        options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseNpgsql(builder.Configuration.GetConnectionString("ServiceDirectoryConnection") ?? string.Empty)
                            .UseLowerCaseNamingConvention()
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

            containerBuilder.RegisterType<ApplicationDbContext>().AsSelf().WithParameter("options", options);

            containerBuilder.RegisterType<MinimalOrganisationEndPoints>();

            containerBuilder.RegisterType<MinimalGeneralEndPoints>();

            containerBuilder.RegisterType<MinimalServiceEndPoints>();

            containerBuilder.RegisterType<MinimalContactsEndPoints>();

            containerBuilder.RegisterType<MinimalTaxonomyEndPoints>();

            containerBuilder.RegisterType<MinimalLocationEndPoints>();

            containerBuilder.RegisterType<MinimalUiCacheEndPoints>();

            containerBuilder.RegisterType<ApplicationDbContextInitialiser>();

            containerBuilder
                .RegisterAssemblyTypes(typeof(IRequest<>).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IRequest<>)))
                .AsImplementedInterfaces();

            containerBuilder
                .RegisterAssemblyTypes(typeof(IRequestHandler<>).Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces();

            containerBuilder.Register(_ => new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMappingProfiles()); }))
                .AsSelf().SingleInstance();

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
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration, bool isProduction)
    {
        services.AddApplicationInsightsTelemetry();

        // Add services to the container.
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "FamilyHubs.ServiceDirectory.Api", Version = "v1" });
            c.EnableAnnotations();
        });

        var assemblies = new[]
        {
            typeof(Program).Assembly,
            typeof(ApplicationDbContext).Assembly,
            typeof(Organisation).Assembly
        };

        services.AddMediatR(assemblies);

        // Adding Authentication
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Adding Jwt Bearer
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? "JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"))
                };
            });

        //https://www.youtube.com/watch?v=cbtK3U2aOlg
        services.AddAuthorization(options =>
        {
            if (isProduction)
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
            else //LocalHost, Dev, Test, PP, disable Authorisation
            {
                options.AddPolicy("AllAdminAccess", policy =>
                    policy.RequireAssertion(_ => true));

                options.AddPolicy("OrgAccess", policy =>
                    policy.RequireAssertion(_ => true));

                options.AddPolicy("ServiceAccess", policy =>
                    policy.RequireAssertion(_ => true));
            }
        });
    }

    public static async Task ConfigureWebApplication(this WebApplication webApplication)
    {
        webApplication.UseSerilogRequestLogging();
        webApplication.UseMiddleware<CorrelationMiddleware>();

        // Configure the HTTP request pipeline.
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI();

        webApplication.UseHttpsRedirection();
        webApplication.UseAuthentication();
        webApplication.UseAuthorization();
        webApplication.MapControllers();

        await RegisterEndPoints(webApplication);
    }

    private static async Task RegisterEndPoints(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var orgservice = scope.ServiceProvider.GetService<MinimalOrganisationEndPoints>();
        orgservice?.RegisterOrganisationEndPoints(webApplication);

        var contactservice = scope.ServiceProvider.GetService<MinimalContactsEndPoints>();
        contactservice?.RegisterContactsEndPoints(webApplication);

        var serservice = scope.ServiceProvider.GetService<MinimalServiceEndPoints>();
        serservice?.RegisterServiceEndPoints(webApplication);

        var taxonyservice = scope.ServiceProvider.GetService<MinimalTaxonomyEndPoints>();
        taxonyservice?.RegisterTaxonomyEndPoints(webApplication);

        var locationservice = scope.ServiceProvider.GetService<MinimalLocationEndPoints>();
        locationservice?.RegisterLocationEndPoints(webApplication);

        var uiCacheservice = scope.ServiceProvider.GetService<MinimalUiCacheEndPoints>();
        uiCacheservice?.RegisterUiCacheEndPoints(webApplication);

        var genservice = scope.ServiceProvider.GetService<MinimalGeneralEndPoints>();
        genservice?.RegisterMinimalGeneralEndPoints(webApplication);

        try
        {
            // Seed Database
            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
            await initialiser.InitialiseAsync(webApplication.Environment.IsProduction());
            await initialiser.SeedAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
        }
    }
}
