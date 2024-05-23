using System.Reflection;
using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.ManyToMany;
using FamilyHubs.ServiceDirectory.Data.Interceptors;
using FamilyHubs.SharedKernel.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;

namespace FamilyHubs.ServiceDirectory.Data.Repository
{
    public class ApplicationDbContext : DbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        private readonly IEncryptionProvider _provider;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor,
            IKeyProvider keyProvider)
            : base(options)
        {
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;

            //todo: .Result could deadlock. how do we fix it? (is it really a problem, don't think it's bitten us in the referral api)
            _provider = GetEncryptionProvider(keyProvider).Result;
        }

        //todo: will need secrets added to the sd keyvault
        //todo: this code originated from the referral api - we should probably move it to shared kernel
        private async Task<IEncryptionProvider> GetEncryptionProvider(IKeyProvider keyProvider)
        {
            byte[]? byteencryptionKey;
            byte[]? byteencryptionIV;

            string? encryptionKey = await keyProvider.GetDbEncryptionKey();
            if (!string.IsNullOrEmpty(encryptionKey))
            {
                byteencryptionKey = ConvertStringToByteArray(encryptionKey);
            }
            else
            {
                throw new ArgumentException("EncryptionKey is missing");
            }
            string? encryptionIV = keyProvider.GetDbEncryptionIVKey().Result;
            if (!string.IsNullOrEmpty(encryptionIV))
            {
                byteencryptionIV = ConvertStringToByteArray(encryptionIV);
            }
            else
            {
                throw new ArgumentException("EncryptionIV is missing");
            }
            return new AesProvider(byteencryptionKey, byteencryptionIV);
        }

        private byte[] ConvertStringToByteArray(string value)
        {
            List<byte> bytes = new List<byte>();
            string[] parts = value.Split(',');
            foreach (string part in parts)
            {
                if (byte.TryParse(part, out byte b))
                {
                    bytes.Add(b);
                }
            }
            return bytes.ToArray();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //todo: move these index creators to the <Entity>Configuration classes
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

            modelBuilder.UseEncryption(this._provider);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (System.Diagnostics.Debugger.IsAttached) optionsBuilder.EnableSensitiveDataLogging();

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
    }
}
