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

        var organisationSeedData = new OrganisationSeedData(_isProduction, _context);

        if (!_context.Taxonomies.Any())
        {
            await organisationSeedData.SeedTaxonomies();
        }

        await organisationSeedData.SeedOrganisations();
    }
}
