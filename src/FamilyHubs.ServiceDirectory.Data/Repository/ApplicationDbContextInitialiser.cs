﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.ServiceDirectory.Data.Repository;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;


    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync(bool isProduction, bool shouldRestDatabaseOnRestart)
    {
        try
        {
            if (!isProduction)
            {

                if (shouldRestDatabaseOnRestart) 
                    await _context.Database.EnsureDeletedAsync();

                if(_context.Database.IsSqlServer())
                    await _context.Database.MigrateAsync();
                else
                    await _context.Database.EnsureCreatedAsync();

                await SeedAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            var organisationSeedData = new OrganisationSeedData(_context);

            if (!_context.Taxonomies.Any())
                await organisationSeedData.SeedTaxonomies();

            if (!_context.Organisations.Any())
                await organisationSeedData.SeedOrganisations();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
