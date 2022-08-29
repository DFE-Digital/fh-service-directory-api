//using FamilyHubs.SharedKernel;
//using FamilyHubs.SharedKernel.Interfaces;
//using fh_service_directory_api.core.Entities;
//using Microsoft.EntityFrameworkCore;
//using System.Reflection;

//namespace fh_service_directory_api.infrastructure.Persistence.Repository;

//public class ServiceDirectoryDbContext : DbContext, IServiceDirectoryDbContext
//{
//    private readonly IDomainEventDispatcher? _dispatcher;

//    public class TestContext : DbContext
//    {
//        public DbSet<OpenReferralOrganisation> openReferralOrganisations { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True");
//        }
//    }


//    public ServiceDirectoryDbContext(DbContextOptions<ServiceDirectoryDbContext> options,
//      IDomainEventDispatcher? dispatcher)
//        : base(options)
//    {
//        _dispatcher = dispatcher;
//    }

//    //public DbSet<Project> Projects => Set<Project>();
//    public DbSet<OpenReferralOrganisation> OpenReferralOrganisation => Set<OpenReferralOrganisation>();
//    //public DbSet<OpenReferralPhysical_Address> OpenReferralPhysical_Addresses => Set<OpenReferralPhysical_Address>();


//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);
//        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
//    }

//    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
//    {
//        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

//        // ignore events if no dispatcher provided
//        if (_dispatcher == null) return result;

//        // dispatch events only if save was successful
//        var entitiesWithEvents = ChangeTracker.Entries<EntityBase<Guid>>()
//            .Select(e => e.Entity)
//            .Where(e => e.DomainEvents.Any())
//            .ToArray();

//        await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

//        return result;
//    }

//    public override int SaveChanges()
//    {
//        return SaveChangesAsync().GetAwaiter().GetResult();
//    }
//}
