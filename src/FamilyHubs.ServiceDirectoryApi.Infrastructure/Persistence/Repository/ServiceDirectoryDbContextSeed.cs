//using fh_service_directory_api.core.Entities;
//using fh_service_directory_api.core.Interfaces.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//namespace fh_service_directory_api.infrastructure.Persistence.Repository
//{
//    public class ServiceDirectoryDbContextSeed
//    {
//        private DateTimeOffset _testDate = DateTime.UtcNow.Date;
//        private readonly ServiceDirectoryDbContextSeed _context;
//        private readonly ILogger<ServiceDirectoryDbContextSeed> _logger;

//        //private readonly IPhone _phone1 = new Phone("1234567890");
//        private readonly IOpenReferralOrganisation OpenReferralOrganisation1 = new OpenReferralOrganisation("Org1", "Description1", "Logo", "Uri", "Url");
//        private readonly IOpenReferralOrganisation OpenReferralOrganisation2 = new OpenReferralOrganisation("Org2", "Description2", string.Empty, string.Empty, string.Empty);
//        private readonly IOpenReferralOrganisation OpenReferralOrganisation3 = new OpenReferralOrganisation("Org3", "Description3", string.Empty, string.Empty, string.Empty);
//        private readonly IOpenReferralOrganisation OpenReferralOrganisation4 = new OpenReferralOrganisation("Org4", "Description4", string.Empty, string.Empty, string.Empty);

//        public ServiceDirectoryDbContextSeed
//        (
//            ServiceDirectoryDbContextSeed context,
//            ILogger<ServiceDirectoryDbContextSeed> logger
//        )
//        {
//            _context = context;
//            _logger = logger;
//        }

//        public async Task SeedAsync(DateTimeOffset testDate, int? retry = 0)
//        {
//            _logger.LogInformation($"Seeding data - testDate: {testDate}");
//            _logger.LogInformation($"DbContext Type: {_context.Database.ProviderName}");

//            _testDate = testDate;
//            int retryForAvailability = retry.Value;
//            try
//            {
//                if (_context.IsRealDatabase())
//                {
//                    // apply migrations if connecting to a SQL database
//                    _context.Database.Migrate();
//                }

//                if (_context != null && _context.OpenReferralOrganisations != null && CreateOrOpenReferralOrganisations != null)
//                {
//                    if (!await _context.OpenReferralOrganisations.AnyAsync())
//                    {
//                        var OpenReferralOrganisations = CreateOrOpenReferralOrganisations();
//                        await _context.OpenReferralOrganisations.AddRangeAsync((IEnumerable<OpenReferralOrganisation>)OpenReferralOrganisations);
//                        await _context.SaveChangesAsync();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                if (retryForAvailability < 1)
//                {
//                    retryForAvailability++;
//                    _logger.LogError(ex.Message);
//                    await SeedAsync(_testDate, retryForAvailability);
//                }
//                throw;
//            }

//            await _context.SaveChangesAsync();
//        }

//        private List<IOpenReferralOrganisation> CreateOrOpenReferralOrganisations()
//        {
//            var result = new List<IOpenReferralOrganisation>
//            {
//                OpenReferralOrganisation1,
//                OpenReferralOrganisation2,
//                OpenReferralOrganisation3,
//                OpenReferralOrganisation4
//            };

//            return result;
//        }
//    }
//}
