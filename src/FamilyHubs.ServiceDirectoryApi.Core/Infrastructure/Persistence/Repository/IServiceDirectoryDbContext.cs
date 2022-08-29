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
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.ServiceDirectoryApi.Core.Infrastructure.Persistence.Repository
{
    public interface IServiceDirectoryDbContext
    {
        DbSet<AccessibilityForDisabilities> Accessibility_For_Disabilities { get; }
        DbSet<OpenReferralContact> OpenReferralContacts { get; }
        DbSet<OpenReferralCostOption> OpenReferralCost_Options { get; }
        DbSet<OpenReferralEligibility> OpenReferralEligibilities { get; }
        DbSet<OpenReferralFunding> OpenReferralFundings { get; }
        DbSet<OpenReferralHolidaySchedule> OpenReferralHoliday_Schedules { get; }
        DbSet<OpenReferralLanguage> OpenReferralLanguages { get; }
        DbSet<OpenReferralLinktaxonomycollection> OpenReferralLinktaxonomycollections { get; }
        DbSet<OpenReferralLocation> OpenReferralLocations { get; }
        DbSet<OpenReferralOrganisation> OpenReferralOrganisations { get; }
        DbSet<OpenReferralParent> OpenReferralParents { get; }
        DbSet<OpenReferralPhone> OpenReferralPhones { get; }
        DbSet<OpenReferralPhysicalAddress> OpenReferralPhysical_Addresses { get; }
        DbSet<OpenReferralRegularSchedule> OpenReferralRegular_Schedules { get; }
        DbSet<OpenReferralReview> OpenReferralReviews { get; }
        DbSet<OpenReferralServiceArea> OpenReferralService_Areas { get; }
        DbSet<OpenReferralServiceTaxonomy> OpenReferralService_Taxonomies { get; }
        DbSet<OpenReferralServiceAtLocation> OpenReferralServiceAtLocations { get; }
        DbSet<OpenReferralServiceDelivery> OpenReferralServiceDeliveries { get; }
        DbSet<OpenReferralService> OpenReferralServices { get; }
        DbSet<OpenReferralTaxonomy> OpenReferralTaxonomies { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}