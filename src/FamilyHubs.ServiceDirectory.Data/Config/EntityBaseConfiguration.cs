using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class EntityBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase<long>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .HasMaxLength(MaxLength.EncryptedEmail)
            .IsRequired()
            .IsEncrypted();

        builder.Property(t => t.LastModifiedBy)
            .HasMaxLength(MaxLength.EncryptedEmail)
            .IsEncrypted();
    }
}