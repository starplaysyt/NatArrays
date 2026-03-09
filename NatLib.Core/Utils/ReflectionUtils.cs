using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace NatLib.Core.Utils;

public static class ReflectionUtils
{
    private static readonly ConcurrentDictionary<string, PropertyInfo[]> PropertyInfos = new();

    private static readonly ConcurrentDictionary<MethodInfo, Delegate> MethodDelegates = new();

    public static PropertyInfo[] GetPropertyInfos(Type type)
    {
        if (PropertyInfos.TryGetValue(type.Name, out var infos)) return infos;
        
        var properties = type.GetProperties();
        PropertyInfos[type.Name] = properties;
        return properties;
    }

    public static PropertyInfo GetPropertyInfo(Type type, Func<PropertyInfo, bool> predicate)
    {
        var result = GetPropertyInfos(type).FirstOrDefault(predicate);
        
        return result ?? throw new InvalidOperationException($"Could not find property by got predicate in {type.Name}.");
    }

    /// <summary>
    /// Compiles a delegate that references a lambda that calls a getter of a property.
    /// </summary>
    /// <remarks>Uses buffering through a ConcurrentDictionary.</remarks>
    /// <returns><c>Func&lt;in T, out TResult&gt;</c>
    /// where T is the property owner and TResult is the value from property.</returns>
    /// <exception cref="InvalidOperationException">
    /// Throws when property is write-only, the property is not declared in the class, or property is a static field.
    /// </exception>
    public static Func<object, object?> GetPropertyGetterDelegate(PropertyInfo info)
    {
        if (!info.CanRead) throw new InvalidOperationException($"Property {info.Name} is write-only.");
        if (info.DeclaringType is null) throw new InvalidOperationException($"Property is not declared in the class.");
        
        var getMethod = info.GetMethod!;
        
        if (getMethod.IsStatic) throw new InvalidOperationException($"Static methods are not supported.");
        
        if (MethodDelegates.TryGetValue(getMethod, out var methodDelegate))
            return (methodDelegate as Func<object, object?>)!;
        
        // Declaring obj parameter for lambda
        var instanceDecl = Expression.Parameter(typeof(object), "obj");

        // Converting obj -> class type
        var convertExpr = Expression.Convert(instanceDecl, info.DeclaringType!);
        
        // Calling getter without args
        var callExpr = Expression.Call(
            convertExpr,
            getMethod);
        
        // Lambda declaring with the result (callExpr)
        var lambda = Expression.Lambda<Func<object, object?>>(
            callExpr,
            instanceDecl);
        
        // Compiling lambda, getting delegate
        var result = lambda.Compile();
        
        MethodDelegates[getMethod] = result;
        return result;
    }

    /// <summary>
    /// Compiles a delegate that references a lambda that calls a setter of a property.
    /// </summary>
    /// <remarks>Uses buffering through a ConcurrentDictionary.</remarks>
    /// <returns><c>Action&lt;in T1, in T2&gt;</c>
    /// where T1 is the property owner and T2 is new property value.</returns>
    /// <exception cref="InvalidOperationException">
    /// Throws when property is read-only, the property is not declared in the class, or property is a static field.
    /// </exception>
    public static Action<object, object?> GetPropertySetterDelegate(PropertyInfo info)
    {
        if (!info.CanWrite) throw new InvalidOperationException($"Property {info.Name} is read-only.");
        if (info.DeclaringType is null) throw new InvalidOperationException($"Property is not declared in the class.");
        
        var setMethod = info.SetMethod!;
        
        if (setMethod.IsStatic) throw new InvalidOperationException($"Static methods are not supported.");
        
        if (MethodDelegates.TryGetValue(setMethod, out var methodDelegate))
            return (methodDelegate as Action<object, object?>)!;
        
        // Declaring obj and value parameters for lambda
        var instanceDecl = Expression.Parameter(typeof(object), "obj");
        var valueDecl = Expression.Parameter(typeof(object), "value");
        
        // Converting obj -> class type, value -> property type
        var instanceConvertExpr = Expression.Convert(instanceDecl, info.DeclaringType!);
        var valueConvertExpr = Expression.Convert(valueDecl, info.PropertyType);
        
        // Calling setter with one arg
        var callExpr = Expression.Call(instanceConvertExpr, setMethod, valueConvertExpr);

        // Lambda declaring with the result (callExpr)
        var lambda = Expression.Lambda<Action<object, object?>>(
            callExpr, instanceDecl, valueDecl);
        
        // Compiling lambda, getting delegate
        var result = lambda.Compile();
        
        MethodDelegates[setMethod] = result;
        return result;
    }
    

    public static IEnumerable<string> GetPropertiesToString<T>(PropertyInfo[] properties, T value)
    {
        return properties.Select(propertyInfo => propertyInfo.GetValue(value, null)?.ToString() ?? string.Empty);
    }
    
    public static IEnumerable<string> GetPropertiesNames<T>(PropertyInfo[] properties) =>
        properties.Select(propertyInfo => propertyInfo.Name);
    
}