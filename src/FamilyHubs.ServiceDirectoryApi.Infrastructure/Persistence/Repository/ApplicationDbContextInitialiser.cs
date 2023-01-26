using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private bool _isProduction;


    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync(bool isProduction)
    {
        try
        {
            _isProduction = isProduction;

            if (_context.Database.IsInMemory())
            {
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.EnsureCreatedAsync();
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
        if (_context.Organisations.Any())
            return;

        var organisationSeedData = new OrganisationSeedData(_isProduction);

        if (!_context.OrganisationTypes.Any())
        {
            _context.OrganisationTypes.AddRange(organisationSeedData.SeedOrganisationTypes());
        }

        if (!_context.ServiceTypes.Any())
        {
            _context.ServiceTypes.AddRange(organisationSeedData.SeedServiceTypes());
        }

        if (!_context.Taxonomies.Any())
        {
            _context.Taxonomies.AddRange(organisationSeedData.SeedTaxonomies());
        }

        await _context.SaveChangesAsync();

        var organisations = organisationSeedData.SeedOrganisations(_context.OrganisationTypes.FirstOrDefault(x => x.Name == "LA") ?? _context.OrganisationTypes.First());

        var taxonomies = _context.Taxonomies.ToList();

        foreach (var organisation in organisations)
        {
            if (organisation.Services != null)
            {
                foreach (var service in organisation.Services)
                {
                    var serviceType = _context.ServiceTypes.FirstOrDefault(x => x.Id == service.ServiceType.Id);
                    if (serviceType != null)
                    {
                        service.ServiceType = serviceType;
                    }

                    foreach (var serviceTaxonomy in service.ServiceTaxonomies)
                    {
                        if (serviceTaxonomy.Taxonomy != null)
                        {
                            var taxonomy = taxonomies.FirstOrDefault(x => x.Id == serviceTaxonomy.Taxonomy.Id);
                            if (taxonomy != null)
                            {
                                serviceTaxonomy.Taxonomy = taxonomy;
                            }
                        }
                    }
                    foreach (var serviceAtLocation in service.ServiceAtLocations)
                    {
                        if (serviceAtLocation.Location.LinkTaxonomies == null) continue;

                        foreach (var linkTaxonomy in serviceAtLocation.Location.LinkTaxonomies)
                        {
                            var taxonomy = taxonomies.FirstOrDefault(x => x.Id == linkTaxonomy.Taxonomy?.Id);
                            if (taxonomy != null)
                            {
                                linkTaxonomy.Taxonomy = taxonomy;
                            }
                        }
                    }
                }
            }

            _context.Organisations.Add(organisation);
        }

        if (!_context.AdminAreas.Any())
        {
            _context.AdminAreas.AddRange(organisationSeedData.SeedOrganisationAdminDistrict());
        }

        await _context.SaveChangesAsync();
    }
}
