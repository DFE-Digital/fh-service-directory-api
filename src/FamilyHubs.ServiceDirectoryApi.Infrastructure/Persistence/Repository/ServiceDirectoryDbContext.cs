using FamilyHubs.ServiceDirectoryApi.Core.Entities;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.AccessibilityForDisabilitiess;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralContacts;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralCostOptions;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralEligibilitys;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralFundings;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralHolidaySchedules;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLanguages;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLinkTaxonomyCollections;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralLocations;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralOrganisations;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralParents;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralPhones;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralPhysicalAddresses;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralRegularSchedules;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralReviews;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAreas;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceAtLocations;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceDeliveries;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServices;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralServiceTaxonomies;
using FamilyHubs.ServiceDirectoryApi.Core.Entities.OpenReferralTaxonomies;
using FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Persistence.Repository;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FamilyHubs.ServiceDirectoryApi.Infrastructure.Persistence.Repository
{
    public class ServiceDirectoryDbContext : DbContext, IServiceDirectoryDbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        public ServiceDirectoryDbContext
        (
            DbContextOptions<ServiceDirectoryDbContext> options,
            IDomainEventDispatcher dispatcher
        )
        : base(options)
        {
            _dispatcher = dispatcher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OpenReferralServiceDelivery>().HasEnum(e => e.ServiceDelivery);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        #region Open Referral Entities
        public DbSet<AccessibilityForDisabilities> Accessibility_For_Disabilities => Set<AccessibilityForDisabilities>();
        public DbSet<OpenReferralContact> OpenReferralContacts => Set<OpenReferralContact>();
        public DbSet<OpenReferralCostOption> OpenReferralCost_Options => Set<OpenReferralCostOption>();
        public DbSet<OpenReferralEligibility> OpenReferralEligibilities => Set<OpenReferralEligibility>();
        public DbSet<OpenReferralFunding> OpenReferralFundings => Set<OpenReferralFunding>();
        public DbSet<OpenReferralHolidaySchedule> OpenReferralHoliday_Schedules => Set<OpenReferralHolidaySchedule>();
        public DbSet<OpenReferralLanguage> OpenReferralLanguages => Set<OpenReferralLanguage>();
        public DbSet<OpenReferralLinktaxonomycollection> OpenReferralLinktaxonomycollections => Set<OpenReferralLinktaxonomycollection>();
        public DbSet<OpenReferralLocation> OpenReferralLocations => Set<OpenReferralLocation>();
        public DbSet<OpenReferralOrganisation> OpenReferralOrganisations => Set<OpenReferralOrganisation>();
        public DbSet<OpenReferralParent> OpenReferralParents => Set<OpenReferralParent>();
        public DbSet<OpenReferralPhone> OpenReferralPhones => Set<OpenReferralPhone>();
        public DbSet<OpenReferralPhysicalAddress> OpenReferralPhysical_Addresses => Set<OpenReferralPhysicalAddress>();
        public DbSet<OpenReferralRegularSchedule> OpenReferralRegular_Schedules => Set<OpenReferralRegularSchedule>();
        public DbSet<OpenReferralReview> OpenReferralReviews => Set<OpenReferralReview>();
        public DbSet<OpenReferralService> OpenReferralServices => Set<OpenReferralService>();
        public DbSet<OpenReferralServiceArea> OpenReferralService_Areas => Set<OpenReferralServiceArea>();
        public DbSet<OpenReferralServiceTaxonomy> OpenReferralService_Taxonomies => Set<OpenReferralServiceTaxonomy>();
        public DbSet<OpenReferralServiceAtLocation> OpenReferralServiceAtLocations => Set<OpenReferralServiceAtLocation>();
        public DbSet<OpenReferralTaxonomy> OpenReferralTaxonomies => Set<OpenReferralTaxonomy>();
        public DbSet<OpenReferralServiceDelivery> OpenReferralServiceDeliveries => Set<OpenReferralServiceDelivery>();

        public DbSet<OpenReferralOrganisation> OpenReferralOrganisation => throw new NotImplementedException();
        #endregion

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // ignore events if no dispatcher provided
            if (_dispatcher == null) return result;

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<EntityBase<Guid>>()
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
