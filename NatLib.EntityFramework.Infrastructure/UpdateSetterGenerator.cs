using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using NatLib.EntityFramework.Domain;

namespace NatLib.EntityFramework.Infrastructure;

internal static class UpdateSetterGenerator<TEntity, TKey, TUpdateMap>
    where TEntity : DomainEntity<TKey>
    where TKey : IComparable<TKey>
{
    private static readonly MethodInfo SetPropertyDefinition = ResolveSetPropertyDefinition();

    public static readonly Func<TUpdateMap, Action<UpdateSettersBuilder<TEntity>>> Compiled = Build();

    private static Func<TUpdateMap, Action<UpdateSettersBuilder<TEntity>>> Build()
    {
        var updateMapParam = Expression.Parameter(typeof(TUpdateMap), "updateMap");
        var builderParam   = Expression.Parameter(typeof(UpdateSettersBuilder<TEntity>), "builder");

        var statements = new List<Expression>();

        var mapProperties = typeof(TUpdateMap).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var mapProperty in mapProperties)
        {
            statements.Add(BuildPropertyAssignment(mapProperty, updateMapParam, builderParam));
        }

        Expression body = statements.Count > 0
            ? Expression.Block(statements)
            : Expression.Empty();

        var innerLambda = Expression.Lambda<Action<UpdateSettersBuilder<TEntity>>>(body, builderParam);
        var outerLambda = Expression.Lambda<Func<TUpdateMap, Action<UpdateSettersBuilder<TEntity>>>>(
            innerLambda, updateMapParam);

        return outerLambda.Compile();
    }

    private static Expression BuildPropertyAssignment(
        PropertyInfo mapProperty,
        ParameterExpression updateMapParam,
        ParameterExpression builderParam)
    {
        var entityProp = typeof(TEntity).GetProperty(mapProperty.Name, BindingFlags.Public | BindingFlags.Instance)
                         ?? throw new InvalidOperationException(
                             $"Unable to locate property '{mapProperty.Name}' in type '{typeof(TEntity).Name}'.");

        var mapPropType = mapProperty.PropertyType;
        if (!IsNullableAssignable(mapPropType))
            throw new InvalidOperationException(
                $"Property '{mapProperty.Name}' of type '{mapPropType.Name}' cannot be null-checked. " +
                "Use a reference type or Nullable<T> in the update map.");

        var mapPropAccess = Expression.Property(updateMapParam, mapProperty);
        var nullCheck = Expression.NotEqual(mapPropAccess, Expression.Constant(null, mapPropType));

        var delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), entityProp.PropertyType);
        
        var entityParam = Expression.Parameter(typeof(TEntity), "x");
        var propertySelector = Expression.Lambda(
            delegateType,
            Expression.Property(entityParam, entityProp),
            entityParam);

        Expression valueExpr = mapPropAccess;
        if (mapPropType != entityProp.PropertyType)
        {
            valueExpr = Expression.Convert(mapPropAccess, entityProp.PropertyType);
        }

        var expressionType = typeof(Expression<>).MakeGenericType(delegateType);
        var propertySelectorConstant = Expression.Constant(propertySelector, expressionType);
        
        var setPropertyGenMethod = SetPropertyDefinition.MakeGenericMethod(entityProp.PropertyType);
        
        var setPropertyCall = Expression.Call(
            builderParam,
            setPropertyGenMethod,
            propertySelectorConstant, 
            valueExpr);

        return Expression.IfThen(nullCheck, setPropertyCall);
    }

    private static MethodInfo ResolveSetPropertyDefinition()
    {
        var builderType = typeof(UpdateSettersBuilder<TEntity>);

        var methods = builderType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.Name == "SetProperty"
                        && m.IsGenericMethodDefinition
                        && m.GetParameters().Length == 2)
            .ToList();

        var method = methods.FirstOrDefault(m =>
        {
            var parameters = m.GetParameters();
            var genericArgs = m.GetGenericArguments();
            if (genericArgs.Length != 1) return false;

            var tProperty = genericArgs[0];
            return parameters[1].ParameterType == tProperty;
        });

        if (method is null) 
            throw new InvalidOperationException(
                $"Method SetProperty<TProperty>(Expression<Func<{builderType.Name}, TProperty>>, TProperty) not found.");

        return method;
    }

    private static bool IsNullableAssignable(Type type) =>
        !type.IsValueType || Nullable.GetUnderlyingType(type) is not null;
}