using System.Numerics;
using NatLib.Core.Structures;

namespace NatLib.Core.Operations;

public static class Point2VectorOperations
{
    /// <summary>
    /// Translates a point by the given vector.
    /// </summary>
    public static Point2<T> Translate<T>(this Point2<T> p, Vector2<T> v)
        where T : INumber<T>
        => new(p.X + v.X, p.Y + v.Y);
    
    /// <summary>
    /// Returns the vector set by two points.
    /// </summary>
    public static Vector2<T> VectorTo<T>(
        this Point2<T> p1, Point2<T> p2)
        where T : INumber<T>
        => new(p2.X - p1.X, p2.Y - p1.Y);
    
    /// <summary>
    /// Treats a vector as a point in space (from origin).
    /// </summary>
    /// <returns> Vector's point interpretation. </returns>
    public static Point2<T> FromVector<T>(this Vector2<T> v)
        where T : INumber<T>
        => new(v.X, v.Y);
    
    /// <summary>
    /// Computes the squared Euclidean distance between two points. 
    /// </summary>
    /// <returns> Squared distance from one point to another. </returns>
    public static T DistanceSquared<T>(this Point2<T> a, Point2<T> b)
        where T : INumber<T>
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return dx * dx + dy * dy;
    }

    /// <summary>
    /// Computes the squared distance from a point to a vector treated as a line
    /// passing through the origin.
    /// Uses cross product magnitude divided by vector length squared.
    /// </summary>
    public static T DistanceSquaredToVector<T>(this Point2<T> p, Vector2<T> v)
        where T : INumber<T>
    {
        var cross = p.X * v.Y - p.Y * v.X;
        return (cross * cross) / (v.X * v.X + v.Y * v.Y);
    }
    
    /// <summary>
    /// Projects a point onto a vector treated as a line through the origin.
    /// Returns the projection point in world coordinates.
    /// </summary>
    public static Point2<T> ProjectOntoVector<T>(this Point2<T> p, Vector2<T> v)
        where T : INumber<T>
    {
        var dot = p.X * v.X + p.Y * v.Y;
        var lenSq = v.X * v.X + v.Y * v.Y;

        var k = dot / lenSq;

        return new Point2<T>(v.X * k, v.Y * k);
    }
    
    /// <summary>
    /// Returns the shortest vector from the projection on the vector
    /// to the original point (perpendicular component).
    /// </summary>
    public static Vector2<T> PerpendicularVectorTo<T>(this Point2<T> p, Vector2<T> v)
        where T : INumber<T>
    {
        var proj = p.ProjectOntoVector(v);
        return new Vector2<T>(p.X - proj.X, p.Y - proj.Y);
    }
    
    /// <summary>
    /// Determines whether the point lies on the infinite line defined
    /// by the vector through the origin.
    /// </summary>
    public static bool IsOnVector<T>(this Point2<T> p, Vector2<T> v)
        where T : INumber<T>
        => (p.X * v.Y - p.Y * v.X) == T.Zero;
    
    /// <summary>
    /// Checks whether the point lies in the same general direction
    /// as the vector relative to the origin.
    /// </summary>
    public static bool IsOnSameDirection<T>(this Point2<T> p, Vector2<T> v)
        where T : INumber<T>
        => (p.X * v.X + p.Y * v.Y) >= T.Zero;
    
    /// <summary>
    /// Reflects a point across a vector treated as an axis through the origin.
    /// The vector defines the reflection axis direction.
    /// </summary>
    public static Point2<T> Reflect<T>(this Point2<T> p, Vector2<T> axis)
        where T : INumber<T>
    {
        var proj = p.ProjectOntoVector(axis);

        return new Point2<T>(proj.X * (T.One + T.One) - p.X, proj.Y * (T.One + T.One) - p.Y);
    }

}