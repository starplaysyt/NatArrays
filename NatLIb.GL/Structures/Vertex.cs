using System.Runtime.InteropServices;

namespace NatLib.GL.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex
{
    public Vec2F Position;
    public Vec4F Color;
    public Vec2F Tex_coord;

    public Vertex() : this(new Vec2F(), new Vec4F(), new Vec2F())
    {
        
    }

    public Vertex(Vec2F position, Vec4F color) : this(position, color, new Vec2F())
    {
        
    }

    public Vertex(Vec2F position, Vec4F color, Vec2F texCoord)
    {
        Position = position;
        Color = color;
        Tex_coord = texCoord;
    }

    public override string ToString() => $"({Position}, {Color}, {Tex_coord})";
}