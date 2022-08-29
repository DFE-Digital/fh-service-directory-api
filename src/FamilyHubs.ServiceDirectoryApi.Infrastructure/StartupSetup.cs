using FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Security.Identity;
using FamilyHubs.ServiceDirectoryApi.Infrastructure.Persistence.Interceptors;
using FamilyHubs.ServiceDirectoryApi.Infrastructure.Persistence.Repository;
using FamilyHubs.ServiceDirectoryApi.Infrastructure.Security.Identity;
using FamilyHubs.ServiceDirectoryApi.Infrastructure.System;
using FamilyHubs.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

namespace FamilyHubs.ServiceDirectoryApi.Infrastructure;

public static class StartupSetup
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<EntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ServiceDirectoryDbContext>(options =>
                options.UseInMemoryDatabase("ServiceDirectoryDbContext"));
        }
        else if (configuration.GetValue<bool>("UseSqlServerDatabase"))
        {
            services.AddDbContext<ServiceDirectoryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ServiceDirectoryDbContext).Assembly.FullName)));
        }
        else
        {
            services.AddDbContext<ServiceDirectoryDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder.MigrationsAssembly(typeof(ServiceDirectoryDbContext).Assembly.FullName)));
        }

        services.AddScoped<IServiceDirectoryDbContext>(provider => provider.GetRequiredService<ServiceDirectoryDbContext>());

        services.AddScoped<ServiceDirectoryDbContextInitialiser>();

        // TODO: Sort out AddDefaultIdentity<ServiceDirectoryUser>()
        services
            .AddDefaultIdentity<ServiceDirectoryUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ServiceDirectoryDbContext>();

        //services.AddIdentityServer()
        //    .AddApiAuthorization<ApplicationUser, ServiceDirectoryDbContext>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthentication()
            .AddIdentityServerJwt();

        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}
