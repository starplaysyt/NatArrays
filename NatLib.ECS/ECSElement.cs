namespace NatLib.ECS;

[AttributeUsage(AttributeTargets.Class)]
public class ECSElement(ECSElementType type) : Attribute
{
    public ECSElementType Type => type;
}