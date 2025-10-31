using System;
using System.Linq;
using System.Reflection;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace _2b_ecommerce.Infrastructure.Persistence;

public partial class AppDbContext
{
    private static readonly Type[] DomainEnumTypes = typeof(DiscountMode).Assembly
        .GetTypes()
        .Where(t => t.IsEnum && t.Namespace == typeof(DiscountMode).Namespace)
        .ToArray();

    private static readonly MethodInfo HasPostgresEnumGeneric = typeof(NpgsqlModelBuilderExtensions)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(m => m.Name == nameof(NpgsqlModelBuilderExtensions.HasPostgresEnum)
                    && m.IsGenericMethodDefinition
                    && m.GetParameters().Length >= 2
                    && m.GetParameters()[0].ParameterType == typeof(ModelBuilder))
        .OrderBy(m => m.GetParameters().Length)
        .First();
    private static readonly int HasPostgresEnumParameterCount = HasPostgresEnumGeneric.GetParameters().Length;

    static AppDbContext()
    {
        RegisterGlobalEnumMappings();
    }

    private static void RegisterGlobalEnumMappings()
    {
#pragma warning disable CS0618 // Global mapper is still used for shared enum mappings
        var mapper = NpgsqlConnection.GlobalTypeMapper;
#pragma warning restore CS0618
        var mapEnumGeneric = mapper
            .GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.Name == "MapEnum" && m.IsGenericMethodDefinition)
            .OrderBy(m => m.GetParameters().Length)
            .First();
        var parameterCount = mapEnumGeneric.GetParameters().Length;

        foreach (var enumType in DomainEnumTypes)
        {
            try
            {
                var args = parameterCount switch
                {
                    0 => Array.Empty<object?>(),
                    1 => new object?[] { null },
                    2 => new object?[] { null, null },
                    _ => Enumerable.Repeat<object?>(null, parameterCount).ToArray()
                };
                mapEnumGeneric.MakeGenericMethod(enumType).Invoke(mapper, args);
            }
            catch (TargetInvocationException ex) when (ex.InnerException is ArgumentException)
            {
                // enum already mapped, nothing to do
            }
        }
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        foreach (var enumType in DomainEnumTypes)
        {
            var args = HasPostgresEnumParameterCount switch
            {
                2 => new object?[] { modelBuilder, null },
                3 => new object?[] { modelBuilder, "public", null },
                4 => new object?[] { modelBuilder, "public", null, null },
                _ => Enumerable.Repeat<object?>(null, HasPostgresEnumParameterCount).ToArray()
            };
            args[0] = modelBuilder;

            HasPostgresEnumGeneric.MakeGenericMethod(enumType).Invoke(null, args);
        }
    }
}
