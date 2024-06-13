using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Shared.Enums;

namespace FamilyHubs.ServiceDirectory.Data.Repository;

#pragma warning disable S1075
public class OrganisationSeedData
{
    private readonly ApplicationDbContext _dbContext;

    public OrganisationSeedData(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedTaxonomies()
    {
        var activity = new Taxonomy { Name = "Activities, clubs and groups", TaxonomyType = TaxonomyType.ServiceCategory };
        var support = new Taxonomy { Name = "Family support", TaxonomyType = TaxonomyType.ServiceCategory };
        var health = new Taxonomy { Name = "Health", TaxonomyType = TaxonomyType.ServiceCategory };
        var earlyYear = new Taxonomy { Name = "Pregnancy, birth and early years", TaxonomyType = TaxonomyType.ServiceCategory };
        var send = new Taxonomy { Name = "Special educational needs and disabilities (SEND)", TaxonomyType = TaxonomyType.ServiceCategory };
        var transport = new Taxonomy { Name = "Transport", TaxonomyType = TaxonomyType.ServiceCategory };

        var parentTaxonomies = new List<Taxonomy>
        {
            activity,
            support,
            health,
            earlyYear,
            send,
            transport,
        };

        _dbContext.Taxonomies.AddRange(parentTaxonomies);
        await _dbContext.SaveChangesAsync();

        var taxonomies = new List<Taxonomy>
        {
            new Taxonomy { Name = "Activities", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Before and after school clubs", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Holiday clubs and schemes", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Music, arts and dance", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Parent, baby and toddler groups", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Pre-school playgroup", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },
            new Taxonomy { Name = "Sports and recreation", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = activity.Id },

            new Taxonomy { Name = "Bullying and cyber bullying", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Debt and welfare advice", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Domestic abuse", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Intensive targeted family support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Money, benefits and housing", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Parenting support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Reducing parental conflict", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Separating and separated parent support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Stopping smoking", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Substance misuse (including alcohol and drug)", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Targeted youth support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },
            new Taxonomy { Name = "Youth justice services", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = support.Id },

            new Taxonomy { Name = "Hearing and sight", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = health.Id },
            new Taxonomy { Name = "Mental health, social and emotional support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = health.Id },
            new Taxonomy { Name = "Nutrition and weight management", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = health.Id },
            new Taxonomy { Name = "Oral health", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = health.Id },
            new Taxonomy { Name = "Public health services", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = health.Id },

            new Taxonomy { Name = "Birth registration", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },
            new Taxonomy { Name = "Early years language and learning", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },
            new Taxonomy { Name = "Health visiting", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },
            new Taxonomy { Name = "Infant feeding support (including breastfeeding)", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },
            new Taxonomy { Name = "Midwife and maternity", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },
            new Taxonomy { Name = "Perinatal mental health support (pregnancy to one year post birth)", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = earlyYear.Id },

            new Taxonomy { Name = "Autistic Spectrum Disorder (ASD)", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Breaks and respite", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Early years support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Groups for parents and carers of children with SEND", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Hearing impairment", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Learning difficulties and disabilities", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Multi-sensory impairment", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Other difficulties or disabilities", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Physical disabilities", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Social, emotional and mental health support", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Speech, language and communication needs", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },
            new Taxonomy { Name = "Visual impairment", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = send.Id },

            new Taxonomy { Name = "Community transport", TaxonomyType = TaxonomyType.ServiceCategory, ParentId = transport.Id },

        };

        _dbContext.Taxonomies.AddRange(taxonomies);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SeedOrganisations()
    {
        const OrganisationType organisationType = OrganisationType.LA;
        var organisations = new List<Organisation>
        {
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Bristol County Council",
                Description =           "Bristol County Council",
                Uri =                   new Uri("https://www.bristol.gov.uk/").ToString(),
                Url =                   "https://www.bristol.gov.uk/",
                AdminAreaCode =         "E06000023",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Lancashire County Council",
                Description =           "Lancashire County Council",
                Uri =                   new Uri("https://www.lancashire.gov.uk/").ToString(),
                Url =                   "https://www.lancashire.gov.uk/",
                AdminAreaCode =         "E10000017",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "London Borough of Redbridge",
                Description =           "London Borough of Redbridge",
                Uri =                   new Uri("https://www.redbridge.gov.uk/").ToString(),
                Url =                   "https://www.redbridge.gov.uk/",
                AdminAreaCode =         "E09000026",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Salford City Council",
                Description =           "Salford City Council",
                Uri =                   new Uri("https://www.salford.gov.uk/").ToString(),
                Url =                   "https://www.salford.gov.uk/",
                AdminAreaCode =         "E08000006",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Suffolk County Council",
                Description =           "Suffolk County Council",
                Uri =                   new Uri("https://www.suffolk.gov.uk/").ToString(),
                Url =                   "https://www.suffolk.gov.uk/",
                AdminAreaCode =         "E10000029",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Tower Hamlets Council",
                Description =           "Tower Hamlets Council",
                Uri =                   new Uri("https://www.towerhamlets.gov.uk/").ToString(),
                Url =                   "https://www.towerhamlets.gov.uk/",
                AdminAreaCode =         "E09000030",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Lewisham Council",
                Description =           "Lewisham Council",
                Uri =                   new Uri("https://lewisham.gov.uk/").ToString(),
                Url =                   "https://lewisham.gov.uk/",
                AdminAreaCode =         "E09000023",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "North East Lincolnshire Council",
                Description =           "North East Lincolnshire Council",
                Uri =                   new Uri("https://www.nelincs.gov.uk/").ToString(),
                Url =                   "https://https://www.nelincs.gov.uk/",
                AdminAreaCode =         "E06000012",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "City of Wolverhampton Council",
                Description =           "City of Wolverhampton Council",
                Uri =                   new Uri("https://www.wolverhampton.gov.uk/").ToString(),
                Url =                   "https://www.wolverhampton.gov.uk/",
                AdminAreaCode =         "E08000031",
            },
            new Organisation
            {
                OrganisationType =      organisationType,
                Name =                  "Sheffield City Council",
                Description =           "Sheffield City Council",
                Uri =                   new Uri("https://www.sheffield.gov.uk/").ToString(),
                Url =                   "https://www.sheffield.gov.uk/",
                AdminAreaCode =         "E08000019",
            }
        };

        _dbContext.Organisations.AddRange(organisations);
        await _dbContext.SaveChangesAsync();
    }
}
#pragma warning restore S1075
