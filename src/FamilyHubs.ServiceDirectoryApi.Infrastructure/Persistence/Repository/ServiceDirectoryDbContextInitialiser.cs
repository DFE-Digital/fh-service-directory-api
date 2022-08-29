using FamilyHubs.ServiceDirectoryApi.Infrastructure.Security.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectoryApi.Infrastructure.Persistence.Repository;

public class ServiceDirectoryDbContextInitialiser
{
    private readonly ILogger<ServiceDirectoryDbContextInitialiser> _logger;
    private readonly ServiceDirectoryDbContext _context;
    private readonly UserManager<ServiceDirectoryUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ServiceDirectoryDbContextInitialiser(
        ILogger<ServiceDirectoryDbContextInitialiser> logger,
        ServiceDirectoryDbContext context,
        UserManager<ServiceDirectoryUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
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
        return;

        //if (_context.Classifications.Any())
        //    return;

        // Default roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ServiceDirectoryUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }



        // Default data
        // Seed, if necessary
        //List<Classification> classifications = new()
        //{
        //    new Classification("Autism", "Autism" ),
        //    new Classification("Brest Feeding", "Brest Feeding Support" ),
        //};

        //var organisationSeedData = new Infrastructure.Persistence.SeedData.Organisations.OrganisationSeedData(classifications);


        //_context.Classifications.AddRange(classifications);

        //await _context.SaveChangesAsync();

        //IReadOnlyCollection<Organisation> organisations = organisationSeedData.SeedOrganistions();

        //foreach (var organisation in organisations)
        //{
        //    _context.Organisations.Add(organisation);
        //}

        await _context.SaveChangesAsync();

        //var openReferralOrganisationSeedData = new Infrastructure.Persistence.SeedData.Organisations.OpenReferralOrganisationSeedData();

        //IReadOnlyCollection<OpenReferralOrganisation> openReferralOrganisations = openReferralOrganisationSeedData.SeedOpenReferralOrganistions();

        //foreach (var openReferralOrganisation in openReferralOrganisations)
        //{
        //    _context.OpenReferralOrganisation.Add(openReferralOrganisation);
        //}

        //await _context.SaveChangesAsync();

    }
}

