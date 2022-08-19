using fh_service_directory_api.core.Concretions.Entities;
using fh_service_directory_api.core.Interfaces.Entities;
using fh_service_directory_api.core.OrganisationAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace fh_service_directory_api.infrastructure.Persistence.Repository
{
    public class ApplicationDbContextSeed
    {
        private DateTimeOffset _testDate = DateTime.UtcNow.Date;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationDbContextSeed> _logger;

        private readonly IPhone _phone1 = new Phone("1234567890");
        private readonly IOrganisation organisation1 = new Organisation("Org1", "Description1", "Logo", "Uri", "Url");
        private readonly IOrganisation organisation2 = new Organisation("Org2", "Description2", string.Empty, string.Empty, string.Empty);
        private readonly IOrganisation organisation3 = new Organisation("Org3", "Description3", string.Empty, string.Empty, string.Empty);
        private readonly IOrganisation organisation4 = new Organisation("Org4", "Description4", string.Empty, string.Empty, string.Empty);

        public ApplicationDbContextSeed
        (
            ApplicationDbContext context,
            ILogger<ApplicationDbContextSeed> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync(DateTimeOffset testDate, int? retry = 0)
        {
            _logger.LogInformation($"Seeding data - testDate: {testDate}");
            _logger.LogInformation($"DbContext Type: {_context.Database.ProviderName}");

            _testDate = testDate;
            int retryForAvailability = retry.Value;
            try
            {
                if (_context.IsRealDatabase())
                {
                    // apply migrations if connecting to a SQL database
                    _context.Database.Migrate();
                }

                if (_context != null && _context.Organizations != null && CreateOrOrganisations != null)
                {
                    if (!await _context.Organizations.AnyAsync())
                    {
                        var organisations = CreateOrOrganisations();
                        await _context.Organizations.AddRangeAsync((IEnumerable<Organisation>)organisations);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 1)
                {
                    retryForAvailability++;
                    _logger.LogError(ex.Message);
                    await SeedAsync(_testDate, retryForAvailability);
                }
                throw;
            }

            await _context.SaveChangesAsync();
        }

        private List<IOrganisation> CreateOrOrganisations()
        {
            var result = new List<IOrganisation>
            {
                organisation1,
                organisation2,
                organisation3,
                organisation4
            };

            return result;
        }
    }
}
