using System.Numerics;

namespace NatLib.Core.Structures;

public struct Shift4F : IEquatable<Shift4F>
{
    private Vector4 Value;
    
    public float Left { get => Value.X; set => Value.X = value; }
    
    public float Top { get => Value.Y; set => Value.Y = value; }
    
    public float Right { get => Value.Z; set => Value.Z = value; }
    
    public float Bottom { get => Value.W; set => Value.W = value; }

    public Shift4F(float dx, float dy)
    {
        Value = new Vector4(dx, dy, dx, dy);
    }

    public Shift4F(float left, float top, float right, float bottom)
    {
        Value = new Vector4(left, top, right, bottom);
    }

    public Shift4F()
    {
        Value = new Vector4(0, 0, 0, 0);
    }

    public RectangleF ShiftRect(RectangleF rect) => 
        new RectangleF(rect.X + Left, rect.Y + Top, 
            rect.Width - Left - Right, 
            rect.Height - Top - Bottom);
    
    public bool Equals(Shift4F other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is Shift4F other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();
    
    public override string ToString() => $"Shift4F({Left}, {Right}, {Top}, {Bottom})";

    public static bool operator ==(Shift4F left, Shift4F right) =>
        left.Equals(right);

    public static bool operator !=(Shift4F left, Shift4F right)
    {
        return !(left == right);
    }
}