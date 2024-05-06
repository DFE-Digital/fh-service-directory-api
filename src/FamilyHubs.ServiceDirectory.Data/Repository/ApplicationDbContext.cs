using System.Reflection;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.ManyToMany;
using FamilyHubs.ServiceDirectory.Data.Interceptors;
using FamilyHubs.ServiceDirectory.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Data.Repository
{
    public class ApplicationDbContext : DbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<Service>()
                .HasIndex(e => new { e.OrganisationId, e.Id })
                .IsUnique(false)
                .HasDatabaseName("IX_Services_OrganisationId_Id")
                .IncludeProperties(e => new { e.ServiceType, e.Status });

            modelBuilder.Entity<Contact>()
                .HasIndex(e => e.ServiceId)
                .IsUnique(false)
                .HasDatabaseName("IX_Contacts_ServiceId_Id")
                .IncludeProperties(e => new { e.Id, e.Title, e.Name, e.Telephone, e.TextPhone, e.Url, e.Email });

            modelBuilder.Entity<Service>()
                .HasIndex(e => new { e.ServiceType, e.Id, e.OrganisationId, e.Status })
                .IsUnique(false)
                .HasDatabaseName("IX_ServiceType_OrganisationId_Status");

            modelBuilder.Entity<ServiceDelivery>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.ServiceId, e.Id })
                    .HasDatabaseName("IX_ServiceDeliveryNonClustered")
                    .IsUnique(false)
                    .IsClustered(false);
            });

            modelBuilder.Entity<ServiceSearch>()
                .ToTable("ServiceSearches")
                .HasKey(e => e.Id);

            modelBuilder.Entity<ServiceSearch>()
                .Property(e => e.SearchTriggerEventId)
                .HasConversion<short>();
            
            modelBuilder.Entity<ServiceSearch>()
                .HasOne(e => e.SearchTriggerEvent)
                .WithMany(e => e.ServiceSearches)
                .HasForeignKey(e => e.SearchTriggerEventId)
                .IsRequired(false);

            modelBuilder.Entity<ServiceSearchResult>()
                .ToTable("ServiceSearchResults")
                .HasKey(e => e.Id);

            modelBuilder.Entity<ServiceSearchResult>()
                .HasOne(e => e.Service)
                .WithMany(e => e.ServiceSearchResults)
                .HasForeignKey(e => e.ServiceId)
                // Do not delete metrics if service deleted
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Event>()
                .ToTable("Events")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Event>()
                .Property(e => e.Id)
                .HasConversion<short>();

            modelBuilder.Entity<Event>()
                .HasData(
                    new()
                    {
                        Id = ServiceDirectorySearchEventType.ServiceDirectoryInitialSearch,
                        Name = nameof(ServiceDirectorySearchEventType.ServiceDirectoryInitialSearch),
                        Description = "Describes an initial, unfiltered search by a user."
                    },
                    new()
                    {
                        Id = ServiceDirectorySearchEventType.ServiceDirectorySearchFilter,
                        Name = nameof(ServiceDirectorySearchEventType.ServiceDirectorySearchFilter),
                        Description = "Describes a filtered search by a user."
                    }
                );

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        public DbSet<AccessibilityForDisabilities> AccessibilityForDisabilities => Set<AccessibilityForDisabilities>();
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<CostOption> CostOptions => Set<CostOption>();
        public DbSet<Eligibility> Eligibilities => Set<Eligibility>();
        public DbSet<Funding> Fundings => Set<Funding>();
        public DbSet<Language> Languages => Set<Language>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Organisation> Organisations => Set<Organisation>();
        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<ServiceAtLocation> ServiceAtLocations => Set<ServiceAtLocation>();
        public DbSet<ServiceArea> ServiceAreas => Set<ServiceArea>();
        public DbSet<ServiceDelivery> ServiceDeliveries => Set<ServiceDelivery>();
        public DbSet<Taxonomy> Taxonomies => Set<Taxonomy>();
        public DbSet<ServiceSearch> ServiceSearches => Set<ServiceSearch>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<ServiceSearchResult> ServiceSearchResult => Set<ServiceSearchResult>();
    }
}
