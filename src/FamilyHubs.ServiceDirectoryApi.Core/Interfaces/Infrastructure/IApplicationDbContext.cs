using FamilyHubs.ServiceDirectory.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectory.Core.Interfaces.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<AccessibilityForDisabilities> AccessibilityForDisabilities { get; }
        DbSet<Contact> Contacts { get; }
        DbSet<CostOption> CostOptions { get; }
        DbSet<Eligibility> Eligibilities { get; }
        DbSet<Funding> Fundings { get; }
        DbSet<HolidaySchedule> HolidaySchedules { get; }
        DbSet<Language> Languages { get; }
        DbSet<Location> Locations { get; }
        DbSet<Organisation> Organisations { get; }
        DbSet<RegularSchedule> RegularSchedules { get; }
        DbSet<Review> Reviews { get; }
        DbSet<ServiceArea> ServiceAreas { get; }
        DbSet<ServiceDelivery> ServiceDeliveries { get; }
        DbSet<Service> Services { get; }
        DbSet<Taxonomy> Taxonomies { get; }
    }
}