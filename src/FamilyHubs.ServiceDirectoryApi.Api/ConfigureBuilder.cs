using Autofac.Extensions.DependencyInjection;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace fh_service_directory_api.api;

public static class ConfigureBuilder
{
    public static void ConfigurWebApplicationBuilderHost(WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }

    public static void ConfigurWebApplicationBuilderServices(WebApplicationBuilder builder)
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
}
