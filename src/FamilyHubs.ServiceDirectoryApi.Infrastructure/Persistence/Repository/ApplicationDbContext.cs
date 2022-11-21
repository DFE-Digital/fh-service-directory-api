using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using fh_service_directory_api.core.Entities;
using fh_service_directory_api.core.Interfaces.Infrastructure;
using fh_service_directory_api.infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace fh_service_directory_api.infrastructure.Persistence.Repository
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IDomainEventDispatcher dispatcher,AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) 
        : base(options)
        {
            _dispatcher = dispatcher;
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OpenReferralServiceDelivery>().HasEnum(e => e.ServiceDelivery);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        public DbSet<UICache> UICaches => Set<UICache>();

        #region Open Referral Entities
        public DbSet<Accessibility_For_Disabilities> Accessibility_For_Disabilities => Set<Accessibility_For_Disabilities>();
        public DbSet<OpenReferralContact> OpenReferralContacts => Set<OpenReferralContact>();
        public DbSet<OpenReferralCost_Option> OpenReferralCost_Options => Set<OpenReferralCost_Option>();
        public DbSet<OpenReferralEligibility> OpenReferralEligibilities => Set<OpenReferralEligibility>();
        public DbSet<OpenReferralFunding> OpenReferralFundings => Set<OpenReferralFunding>();
        public DbSet<OpenReferralHoliday_Schedule> OpenReferralHoliday_Schedules => Set<OpenReferralHoliday_Schedule>();
        public DbSet<OpenReferralLanguage> OpenReferralLanguages => Set<OpenReferralLanguage>();
        public DbSet<OpenReferralLinktaxonomycollection> OpenReferralLinktaxonomycollections => Set<OpenReferralLinktaxonomycollection>();
        public DbSet<OpenReferralLocation> OpenReferralLocations => Set<OpenReferralLocation>();
        public DbSet<OpenReferralOrganisation> OpenReferralOrganisations => Set<OpenReferralOrganisation>();
        public DbSet<OpenReferralParent> OpenReferralParents => Set<OpenReferralParent>();
        public DbSet<OpenReferralPhone> OpenReferralPhones => Set<OpenReferralPhone>();
        public DbSet<OpenReferralPhysical_Address> OpenReferralPhysical_Addresses => Set<OpenReferralPhysical_Address>();
        public DbSet<OpenReferralRegular_Schedule> OpenReferralRegular_Schedules => Set<OpenReferralRegular_Schedule>();
        public DbSet<OpenReferralReview> OpenReferralReviews => Set<OpenReferralReview>();
        public DbSet<OpenReferralService> OpenReferralServices => Set<OpenReferralService>();
        public DbSet<OpenReferralService_Area> OpenReferralService_Areas => Set<OpenReferralService_Area>();
        public DbSet<OpenReferralService_Taxonomy> OpenReferralService_Taxonomies => Set<OpenReferralService_Taxonomy>();
        public DbSet<OpenReferralServiceAtLocation> OpenReferralServiceAtLocations => Set<OpenReferralServiceAtLocation>();
        public DbSet<OpenReferralTaxonomy> OpenReferralTaxonomies => Set<OpenReferralTaxonomy>();
        public DbSet<OpenReferralServiceDelivery> OpenReferralServiceDeliveries => Set<OpenReferralServiceDelivery>();
        #endregion///

        public DbSet<ModelLink> ModelLinks => Set<ModelLink>();
        public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
        public DbSet<OrganisationType> OrganisationTypes => Set<OrganisationType>();
        public DbSet<OrganisationAdminDistrict> OrganisationAdminDistricts => Set<OrganisationAdminDistrict>();
        public DbSet<RelatedOrganisation> RelatedOrganisations => Set<RelatedOrganisation>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // ignore events if no dispatcher provided
            if (_dispatcher == null) return result;

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<EntityBase<string>>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
