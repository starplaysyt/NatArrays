namespace NatLib.EntityFramework.Domain;

public readonly struct NullableField<T> where T : notnull
{
    public bool IsSet { get; }
    public T? Value { get; }

    public NullableField(T? value)
    {
        IsSet = true;
        Value = value;
    }
    
    public static implicit operator NullableField<T>(T? value) => new(value);
}