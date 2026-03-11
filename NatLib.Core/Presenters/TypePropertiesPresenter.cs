using System.Reflection;
using NatLib.Core.Interfaces;
using NatLib.Core.Unification;
using NatLib.Core.Utils;

namespace NatLib.Core.Presenters;

public class TypePropertiesPresenter : IStringPresenter
{
    public readonly PropertyInfo[] PropertyInfos;

    public readonly Type PresentedType;
    
    public PropertyInfo this[int id] => PropertyInfos[id];
    
    public int Count => PropertyInfos.Length;
    
    public TypePropertiesPresenter(Type type)
    {
        PropertyInfos = type.GetProperties();
        PresentedType = type;
    }

    public bool TrySetPropertyValue(object owner, int propertyId, object? value)
    {
        var propertyInfo = PropertyInfos[propertyId];

        var propSetter = 
            ReflectionUtils.GetPropertySetterDelegate(propertyInfo);
        
        propSetter.Invoke(owner, value);
        
        return true;
    }
    
    public string PresentString()
    {
        var width = StringStructuralConfiguration.Instance.PreferableWidth;
        var totalLines = PropertyInfos.Length + 2; // top + messages + bottom
        var length = totalLines * (width + 1);
        Span<char> result = stackalloc char[length];

        int offset = 0;
        
        StringStructuralUtils.WriteTopBorder(result.Slice(offset, width));
        offset += width;
        result[offset++] = '\n';
        
        for (int i = 0; i < PropertyInfos.Length; i++)
        {
            var message = $"{i + 1}. {PropertyInfos[i].Name}";
            StringStructuralUtils.WriteMessageInBounds(result.Slice(offset, width), message);
            offset += width;
            result[offset++] = '\n';
        }
        
        StringStructuralUtils.WriteBottomBorder(result.Slice(offset, width));
        offset += width;

        return new string(result[..offset]);
    }
}