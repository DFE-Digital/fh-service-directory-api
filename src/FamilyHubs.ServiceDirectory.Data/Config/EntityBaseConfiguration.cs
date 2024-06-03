using FamilyHubs.ServiceDirectory.Data.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data.Config;

public class EntityBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : EntityBase<long>
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
    }
}