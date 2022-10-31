using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace fh_service_directory_api.infrastructure.Persistence.Repository;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;      
    }

    public async Task InitialiseAsync(IConfiguration configuration)
    {
        try
        {
            if (_context.Database.IsSqlServer() || _context.Database.IsNpgsql())
            {
                if (configuration.GetValue<bool>("RecreateDbOnStartup"))
                {
                    _context.Database.EnsureDeleted();
                    _context.Database.EnsureCreated();
                }
                else
                    await _context.Database.MigrateAsync();
            }
            //else
            //{
            //    _context.Database.EnsureDeleted();
            //}
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        if (_context.OpenReferralOrganisations.Any())
            return;

        var openReferralOrganisationSeedData = new OpenReferralOrganisationSeedData();

        if (!_context.OrganisationTypes.Any())
        {
            _context.OrganisationTypes.AddRange(openReferralOrganisationSeedData.SeedOrganisationTypes());
        }

        if (!_context.ServiceTypes.Any())
        {
            _context.ServiceTypes.AddRange(openReferralOrganisationSeedData.SeedServiceTypes());
        }

        await _context.SaveChangesAsync();

        var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Name == "Information Sharing");

        IReadOnlyCollection<OpenReferralOrganisation> openReferralOrganisations = openReferralOrganisationSeedData.SeedOpenReferralOrganistions(_context.OrganisationTypes.FirstOrDefault(x => x.Name == "LA") ?? _context.OrganisationTypes.First());

        foreach (var openReferralOrganisation in openReferralOrganisations)
        {
            if (serviceType != null && openReferralOrganisation != null && openReferralOrganisation.Services != null)
            {
                foreach(var service in openReferralOrganisation.Services)
                {
                    service.ServiceType = serviceType;
                }
                
            }

            if (openReferralOrganisation != null)
            {
                _context.OpenReferralOrganisations.Add(openReferralOrganisation);
            } 
        }

        if (!_context.OrganisationAdminDistricts.Any())
        {
            _context.OrganisationAdminDistricts.AddRange(openReferralOrganisationSeedData.SeedOrganisationAdminDistrict());
        }

        await _context.SaveChangesAsync();

        if (!_context.ModelLinks.Any())
        {
            _context.OpenReferralLocations.AddRange(openReferralOrganisationSeedData.SeedFamilyHubs());
            _context.OpenReferralTaxonomies.AddRange(openReferralOrganisationSeedData.SeedTaxonomies());
            _context.ModelLinks.AddRange(openReferralOrganisationSeedData.SeedModelLinks());

            await _context.SaveChangesAsync();
        }

    }
}
