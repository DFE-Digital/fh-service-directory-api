using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Interfaces.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<UICache> UICaches { get; }
        DbSet<AccessibilityForDisabilities> AccessibilityForDisabilities { get; }
        DbSet<Contact> Contacts { get; }
        DbSet<CostOption> CostOptions { get; }
        DbSet<Eligibility> Eligibilities { get; }
        DbSet<Funding> Fundings { get; }
        DbSet<HolidaySchedule> HolidaySchedules { get; }
        DbSet<Language> Languages { get; }
        DbSet<LinkTaxonomy> LinkTaxonomies { get; }
        DbSet<Location> Locations { get; }
        DbSet<Organisation> Organisations { get; }
        DbSet<Parent> Parents { get; }
        DbSet<PhysicalAddress> PhysicalAddresses { get; }
        DbSet<RegularSchedule> RegularSchedules { get; }
        DbSet<Review> Reviews { get; }
        DbSet<ServiceArea> ServiceAreas { get; }
        DbSet<ServiceTaxonomy> ServiceTaxonomies { get; }
        DbSet<ServiceAtLocation> ServiceAtLocations { get; }
        DbSet<ServiceDelivery> ServiceDeliveries { get; }
        DbSet<Service> Services { get; }
        DbSet<Taxonomy> Taxonomies { get; }
        DbSet<OrganisationType> OrganisationTypes { get; }
        DbSet<ServiceType> ServiceTypes { get; }
        DbSet<AdminArea> AdminAreas { get; }
        DbSet<RelatedOrganisation> RelatedOrganisations { get; }
        DbSet<LinkContact> LinkContacts { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}