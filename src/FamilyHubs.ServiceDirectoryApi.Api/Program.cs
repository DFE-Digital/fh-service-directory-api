using Autofac.Extensions.DependencyInjection;
using FamilyHubs.ServiceDirectory.Shared.Interfaces.Entities;
using fh_service_directory_api.infrastructure.Persistence.Repository;
using MediatR;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ConfigurWebApplicationBuilderHost(builder);
ConfigurWebApplicationBuilderServices(builder);

var webApplication = builder.Build();
ConfigureWebApplication(webApplication);
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