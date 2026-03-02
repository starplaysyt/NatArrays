using System.Linq.Expressions;
using System.Reflection;

namespace NatLib.Core.Utils;

public static class ReflectionUtils
{
    private static readonly Dictionary<string, PropertyInfo[]> _propertyInfos = new();
    
    private static readonly Dictionary<int, int> _propertyGetters = new Dictionary<int, int>(); 

    public static PropertyInfo[] GetPropertyInfos(Type type)
    {
        if (_propertyInfos.TryGetValue(type.Name, out var infos)) return infos;
        
        var properties = type.GetProperties();
        _propertyInfos[type.Name] = properties;
        return properties;
    }

    public static void GetPropertyGetterDelegate(PropertyInfo info)
    {
        var instance = Expression.Parameter(typeof(object), "obj");

        var convertInstance = Expression.Convert(instance, info.DeclaringType!);

        var propertyAccess = Expression.Property(convertInstance, info);

        var convertResult = Expression.Convert(propertyAccess, typeof(object));

        var lambda = Expression.Lambda<Func<object, object?>>(
            convertResult,
            instance);

        lambda.Compile();
    }
    

    public static IEnumerable<string> GetPropertiesToString<T>(PropertyInfo[] properties, T value)
    {
        return properties.Select(propertyInfo => propertyInfo.GetValue(value, null)?.ToString() ?? string.Empty);
    }
    
    public static IEnumerable<string> GetPropertiesNames<T> (PropertyInfo[] properties) =>
        properties.Select(propertyInfo => propertyInfo.Name);
    
}