using System.Reflection;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Interceptors;
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
        public DbSet<HolidaySchedule> HolidaySchedules => Set<HolidaySchedule>();
        public DbSet<Language> Languages => Set<Language>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<Organisation> Organisations => Set<Organisation>();
        public DbSet<RegularSchedule> RegularSchedules => Set<RegularSchedule>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<ServiceArea> ServiceAreas => Set<ServiceArea>();
        public DbSet<ServiceDelivery> ServiceDeliveries => Set<ServiceDelivery>();
        public DbSet<Taxonomy> Taxonomies => Set<Taxonomy>();
    }
}
