using fh_service_directory_api.core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace fh_service_directory_api.infrastructure.Persistence.Repository;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private bool _isProduction = false;


    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;      
    }

    public async Task InitialiseAsync(IConfiguration configuration, bool isProduction)
    {
        try
        {
            _isProduction = isProduction;

            if (_context.Database.IsInMemory())
            {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
            }

            if (_context.Database.IsSqlServer() || _context.Database.IsNpgsql())
            {
                await _context.Database.MigrateAsync();
            }
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

        var openReferralOrganisationSeedData = new OpenReferralOrganisationSeedData(_isProduction);

        if (!_context.OrganisationTypes.Any())
        {
            _context.OrganisationTypes.AddRange(openReferralOrganisationSeedData.SeedOrganisationTypes());
        }

        if (!_context.ServiceTypes.Any())
        {
            _context.ServiceTypes.AddRange(openReferralOrganisationSeedData.SeedServiceTypes());
        }

        if (!_context.OpenReferralTaxonomies.Any())
        {
            _context.OpenReferralTaxonomies.AddRange(openReferralOrganisationSeedData.SeedOpenReferralTaxonomies());
        }

        await _context.SaveChangesAsync();

        IReadOnlyCollection<OpenReferralOrganisation> openReferralOrganisations = openReferralOrganisationSeedData.SeedOpenReferralOrganistions(_context.OrganisationTypes.FirstOrDefault(x => x.Name == "LA") ?? _context.OrganisationTypes.First());

        var taxonomies = _context.OpenReferralTaxonomies.ToList();

        foreach (var openReferralOrganisation in openReferralOrganisations)
        {
            if (openReferralOrganisation != null && openReferralOrganisation.Services != null)
            {
                foreach(var service in openReferralOrganisation.Services)
                {
                    var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == service.ServiceType.Id);
                    if (serviceType != null)
                    {
                        service.ServiceType = serviceType;
                    }

                    foreach(var serviceTaxonomy in service.Service_taxonomys)
                    {
                        if (serviceTaxonomy != null && serviceTaxonomy.Taxonomy != null)
                        {
                            var taxonomy = taxonomies.FirstOrDefault(x => x.Id == serviceTaxonomy.Taxonomy.Id);
                            if (taxonomy != null)
                            {
                                serviceTaxonomy.Taxonomy = taxonomy;
                            }
                        }
                    }
                }
            }

            if (openReferralOrganisation != null)
            {
                _context.OpenReferralOrganisations.Add(openReferralOrganisation);
            } 
        }

        if (!_isProduction) 
        {
            IReadOnlyCollection<OpenReferralOrganisation> familyHubs = openReferralOrganisationSeedData.GetSalfordFamilyHubOrganisations();

            foreach (var openReferralOrganisation in familyHubs)
            {
                openReferralOrganisation.OrganisationType = _context.OrganisationTypes.First(x => x.Id == openReferralOrganisation.OrganisationType.Id);

                if (openReferralOrganisation != null && openReferralOrganisation.Services != null)
                {
                    foreach (var service in openReferralOrganisation.Services)
                    {
                        var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == service.ServiceType.Id);
                        if (serviceType != null)
                        {
                            service.ServiceType = serviceType;
                        }

                        foreach (var serviceTaxonomy in service.Service_taxonomys)
                        {
                            if (serviceTaxonomy != null && serviceTaxonomy.Taxonomy != null)
                            {
                                var taxonomy = taxonomies.FirstOrDefault(x => x.Id == serviceTaxonomy.Taxonomy.Id);
                                if (taxonomy != null)
                                {
                                    serviceTaxonomy.Taxonomy = taxonomy;
                                }
                            }
                        }
                    }
                }

                if (openReferralOrganisation != null)
                {
                    _context.OpenReferralOrganisations.Add(openReferralOrganisation);
                }
            }

            if (!_context.RelatedOrganisations.Any())
            {
                _context.RelatedOrganisations.AddRange(openReferralOrganisationSeedData.SeedRelatedOrganisations());
            }
        }

        if (!_context.OrganisationAdminDistricts.Any())
        {
            _context.OrganisationAdminDistricts.AddRange(openReferralOrganisationSeedData.SeedOrganisationAdminDistrict());
        }

        await _context.SaveChangesAsync();

    }
}
