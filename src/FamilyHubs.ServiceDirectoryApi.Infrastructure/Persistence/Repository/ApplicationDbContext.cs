using System.Reflection;
using FamilyHubs.ServiceDirectory.Core.Entities;
using FamilyHubs.ServiceDirectory.Core.Interfaces.Infrastructure;
using FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Interceptors;
using FamilyHubs.SharedKernel;
using FamilyHubs.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Infrastructure.Persistence.Repository
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
            modelBuilder.Entity<ServiceDelivery>().HasEnum(e => e.Name);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        public DbSet<UiCache> UiCaches => Set<UiCache>();
        public DbSet<AccessibilityForDisabilities> AccessibilityForDisabilities => Set<AccessibilityForDisabilities>();
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<CostOption> CostOptions => Set<CostOption>();
        public DbSet<Eligibility> Eligibilities => Set<Eligibility>();
        public DbSet<Funding> Fundings => Set<Funding>();
        public DbSet<HolidaySchedule> HolidaySchedules => Set<HolidaySchedule>();
        public DbSet<Language> Languages => Set<Language>();
        public DbSet<LinkTaxonomy> LinkTaxonomies => Set<LinkTaxonomy>();
        public DbSet<LinkContact> LinkContacts => Set<LinkContact>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Organisation> Organisations => Set<Organisation>();
        public DbSet<Parent> Parents => Set<Parent>();
        public DbSet<PhysicalAddress> PhysicalAddresses => Set<PhysicalAddress>();
        public DbSet<RegularSchedule> RegularSchedules => Set<RegularSchedule>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<ServiceArea> ServiceAreas => Set<ServiceArea>();
        public DbSet<ServiceTaxonomy> ServiceTaxonomies => Set<ServiceTaxonomy>();
        public DbSet<ServiceAtLocation> ServiceAtLocations => Set<ServiceAtLocation>();
        public DbSet<Taxonomy> Taxonomies => Set<Taxonomy>();
        public DbSet<ServiceDelivery> ServiceDeliveries => Set<ServiceDelivery>();
        public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
        public DbSet<OrganisationType> OrganisationTypes => Set<OrganisationType>();
        public DbSet<AdminArea> AdminAreas => Set<AdminArea>();
        public DbSet<RelatedOrganisation> RelatedOrganisations => Set<RelatedOrganisation>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

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
