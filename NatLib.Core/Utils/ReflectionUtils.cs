using System.Reflection;

namespace NatLib.Core.Utils;

public static class ReflectionUtils
{
    private static readonly Dictionary<string, PropertyInfo[]> _propertyInfos = new();

    public static PropertyInfo[] GetPropertyInfos(Type type)
    {
        if (_propertyInfos.TryGetValue(type.Name, out var infos)) return infos;
        
        var properties = type.GetProperties();
        _propertyInfos[type.Name] = properties;
        return properties;
    }
}