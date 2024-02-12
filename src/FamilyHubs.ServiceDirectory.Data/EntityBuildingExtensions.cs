using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data;

public static class EntityBuilderExtensions
{
    //todo: if works, call common private static method
    public static PropertyBuilder<TProperty> HasEnumProperty<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> entityBuilder,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        int? maxLength = null)
        where TEntity : class
        where TProperty : struct, Enum
    {
        int actualMaxLength = maxLength ?? Enum.GetValues(typeof(TProperty))
            .Cast<TProperty>()
            .Max(e => e.ToString().Length);

        return entityBuilder.Property(propertyExpression)
            .HasConversion
            (
                v => v.ToString(),
                v => (TProperty)Enum.Parse(typeof(TProperty), v)
            )
            .HasMaxLength(actualMaxLength);
    }

    public static PropertyBuilder<TProperty?> HasEnumProperty<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> entityBuilder,
        Expression<Func<TEntity, TProperty?>> propertyExpression,
        int? maxLength = null)
        where TEntity : class
        where TProperty : struct, Enum
    {
        int actualMaxLength = maxLength ?? Enum.GetValues(typeof(TProperty))
            .Cast<TProperty>()
            .Max(e => e.ToString().Length);

        return entityBuilder.Property(propertyExpression)
            .HasConversion
            (
                v => v.ToString(),
                v => (TProperty)Enum.Parse(typeof(TProperty), v)
            )
            .HasMaxLength(actualMaxLength);
    }
}