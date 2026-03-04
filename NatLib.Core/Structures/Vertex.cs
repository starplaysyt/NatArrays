using System.Numerics;
using System.Runtime.InteropServices;

namespace NatLib.Core.Structures;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex
{
    public Vector2 Position;
    public Vector4 Color;
    public Vector2 Tex_coord;

    public Vertex() 
        : this(new Vector2(), new Vector4())
    {
        
    }

    public Vertex
        (Vector2 position, Vector4 color, Vector2 texCoord = new())
    {
        Position = position;
        Color = color;
        Tex_coord = texCoord;
    }

    public override string ToString() => 
        $"({Position}, {Color}, {Tex_coord})";
}