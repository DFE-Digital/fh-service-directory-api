using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data;

public static class EntityBuilderExtensions
{
    public static void HasEnum<TEntity, TProperty>
    (
        this EntityTypeBuilder<TEntity> entityBuilder,
        Expression<Func<TEntity, TProperty>> propertyExpression
    )
    where TEntity : class
    where TProperty : Enum
    {
        entityBuilder.Property(propertyExpression)
            .HasConversion
            (
                v => v.ToString(),
                v => (TProperty)Enum.Parse(typeof(TProperty), v)
            );
    }
}
