using FamilyHubs.ServiceDirectoryApi.Api;
using FamilyHubs.ServiceDirectoryApi.Api.Endpoints;
using FamilyHubs.ServiceDirectoryApi.Core;
using FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Security.Identity;
using FamilyHubs.ServiceDirectoryApi.Infrastructure;
using FamilyHubs.ServiceDirectoryApi.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectoryApi.Infrastructure.Security.Identity;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer()
                .AddInfrastructureServices(builder.Configuration)
                .AddCoreServices();

builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<MinimalOrganisationEndPoints>();
builder.Services.AddScoped<MinimalGeneralEndPoints>();
builder.Services.AddScoped<MinimalServiceEndPoints>();
builder.Services.AddScoped<MinimalTaxonomyEndPoints>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1.00", new OpenApiInfo { Title = "Service Directory API", Version = "v1.00" });
    c.EnableAnnotations();
    c.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ServiceDirectoryDbContextInitialiser>();
        await initialiser.InitialiseAsync(builder.Configuration);
        await initialiser.SeedAsync();
    }
}

using (var scope = app.Services.CreateScope())
{
    var orgservice = scope.ServiceProvider.GetService<MinimalOrganisationEndPoints>();
    if (orgservice != null)
        orgservice.RegisterOrganisationEndPoints(app);

    var serservice = scope.ServiceProvider.GetService<MinimalServiceEndPoints>();
    if (serservice != null)
        serservice.RegisterServiceEndPoints(app);

    var taxonyservice = scope.ServiceProvider.GetService<MinimalTaxonomyEndPoints>();
    if (taxonyservice != null)
        taxonyservice.RegisterTaxonomyEndPoints(app);

    var genservice = scope.ServiceProvider.GetService<MinimalGeneralEndPoints>();
    if (genservice != null)
        genservice.RegisterMinimalGeneralEndPoints(app);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
