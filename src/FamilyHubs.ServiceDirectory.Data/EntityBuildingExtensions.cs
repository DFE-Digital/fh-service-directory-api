﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FamilyHubs.ServiceDirectory.Data;

public static class EntityBuilderExtensions
{
    //todo: call common private static method(s)?
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
            .HasMaxLength(actualMaxLength)
            .IsUnicode(false);
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
                // a null value will never be passed to a value converter, so we can safely forgive null
                v => (TProperty)Enum.Parse(typeof(TProperty), v!)
            )
            .HasMaxLength(actualMaxLength)
            .IsUnicode(false);
    }
}