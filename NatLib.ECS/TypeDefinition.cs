using System.Reflection;

namespace NatLib.ECS;

public static class TypeDefinitionService
{
    private static int _counter = 0;

    public static int AcquireTypeId() => ++_counter;
}

public struct TypeDefinition<T>
{
    // ReSharper disable once StaticMemberInGenericType
    public static readonly int TypeId = TypeDefinitionService.AcquireTypeId();
    
    public Type GetDefinitionType() => typeof(T);

    public ECSElementType GetElementType()
    {
        var attribute = typeof(T).GetCustomAttribute<ECSElement>()?.Type;

        if (attribute == null)
            throw new InvalidOperationException(
                "Tried to register non-ECS component to TypeDefinition");

        return attribute ?? throw new ArgumentOutOfRangeException();
    } 
}