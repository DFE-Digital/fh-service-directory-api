﻿using FamilyHubs.ServiceDirectory.Data.Entities;
using FamilyHubs.ServiceDirectory.Data.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

//todo: we need to revisit the cascade delete behaviours
// we have that set on optional relationships and more crucially on entities
// that can be optionally referenced by more than one different parent entity
// we don't want to delete the referenced entity if it is still being referenced by another entity
public class ServiceConfiguration : EntityBaseConfiguration<Service>
{
    public override void Configure(EntityTypeBuilder<Service> builder)
    {
        base.Configure(builder);

        builder.Navigation(e => e.ServiceDeliveries).AutoInclude();
        builder.Navigation(e => e.CostOptions).AutoInclude();
        builder.Navigation(e => e.Eligibilities).AutoInclude();
        builder.Navigation(e => e.Fundings).AutoInclude();
        builder.Navigation(e => e.Languages).AutoInclude();
        builder.Navigation(e => e.ServiceAreas).AutoInclude();
        builder.Navigation(e => e.Taxonomies).AutoInclude();
        builder.Navigation(e => e.Locations).AutoInclude();
        builder.Navigation(e => e.Contacts).AutoInclude();
        builder.Navigation(e => e.Schedules).AutoInclude();
        //builder.Navigation(e => e.ServiceAtLocations).AutoInclude();

        builder.Property(t => t.Name)
            .HasMaxLength(255);

        // 200 char limit in the front end, but line endings are allowed, and they take 2 chars
        // so we make it big enough to accomodate 200 line endings as the max length
        builder.Property(t => t.Summary)
            .HasMaxLength(200*2);

        builder.HasEnumProperty(s => s.DeliverableType, 50);
        builder.HasEnumProperty(s => s.ServiceType);
        builder.HasEnumProperty(s => s.Status, 50);

        builder.Property(t => t.InterpretationServices)
            .HasMaxLength(512);

        builder.HasMany(s => s.Fundings)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.CostOptions)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.Eligibilities)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.Languages)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.ServiceAreas)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.ServiceDeliveries)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.Contacts)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(s => s.Schedules)
            .WithOne()
            .HasForeignKey(lc => lc.ServiceId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade)
            ;

        builder.HasMany(p => p.Locations)
            .WithMany()
            .UsingEntity<ServiceAtLocation>();

        builder.HasMany(p => p.Taxonomies)
            .WithMany()
            .UsingEntity<ServiceTaxonomy>("ServiceTaxonomies",
                lt => lt
                    .HasOne<Taxonomy>()
                    .WithMany()
                    .HasForeignKey(s => s.TaxonomyId)
                    .OnDelete(DeleteBehavior.Cascade),
                rt => rt
                    .HasOne<Service>()
                    .WithMany()
                    .HasForeignKey(s => s.ServiceId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade)
                );
    }
}