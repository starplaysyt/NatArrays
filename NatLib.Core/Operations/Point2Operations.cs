using System.Numerics;
using NatLib.Core.Structures;

namespace NatLib.Core.Operations;

public static class Point2Operations
{
    /// <summary>
    /// Adds two points component-wise.
    /// Primarily intended for utility and grid-based operations.
    /// </summary>
    public static Point2<T> Add<T>(this Point2<T> a, Point2<T> b)
        where T : INumber<T>
        => new(a.X + b.X, a.Y + b.Y);
    
    /// <summary>
    /// Subtracts one point from another component-wise.
    /// Useful for relative positioning and delta computations.
    /// </summary>
    public static Point2<T> Subtract<T>(this Point2<T> a, Point2<T> b)
        where T : INumber<T>
        => new(a.X - b.X, a.Y - b.Y);
    
    /// <summary>
    /// Scales the point coordinates by a scalar value.
    /// </summary>
    public static Point2<T> Multiply<T>(this Point2<T> p, T scalar)
        where T : INumber<T>
        => new(p.X * scalar, p.Y * scalar);
    
    /// <summary>
    /// Divides the point coordinates by a scalar value.
    /// </summary>
    public static Point2<T> Divide<T>(this Point2<T> p, T scalar)
        where T : INumber<T>
        => new(p.X / scalar, p.Y / scalar);
    
    /// <summary>
    /// Computes the squared Euclidean distance between two points.
    /// Avoids square root and is suitable for all numeric types.
    /// </summary>
    public static T DistanceSquared<T>(this Point2<T> a, Point2<T> b)
        where T : INumber<T>
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return dx * dx + dy * dy;
    }
    
    /// <summary>
    /// Returns the midpoint between two points.
    /// Uses arithmetic mean of coordinates.
    /// </summary>
    public static Point2<T> Midpoint<T>(this Point2<T> a, Point2<T> b)
        where T : INumber<T>
    {
        var two = T.One + T.One;
        return new Point2<T>((a.X + b.X) / two, (a.Y + b.Y) / two);
    }
    
    /// <summary>
    /// Checks whether the point lies at the origin (0, 0).
    /// </summary>
    public static bool IsOrigin<T>(this Point2<T> p)
        where T : INumber<T>
        => p.X == T.Zero && p.Y == T.Zero;
    
    /// <summary>
    /// Clamps the point coordinates to the given axis-aligned bounds.
    /// </summary>
    public static Point2<T> Clamp<T>(this Point2<T> p, Point2<T> min, Point2<T> max)
        where T : INumber<T>
        => new(
            T.Clamp(p.X, min.X, max.X),
            T.Clamp(p.Y, min.Y, max.Y)
        );
    
    /// <summary>
    /// Determines whether the point lies inside or on the boundary
    /// of an axis-aligned bounding box.
    /// </summary>
    public static bool IsInsideBounds<T>(
        this Point2<T> p,
        Point2<T> min,
        Point2<T> max)
        where T : INumber<T>
        => p.X >= min.X && p.X <= max.X &&
           p.Y >= min.Y && p.Y <= max.Y;
    
    /// <summary>
    /// Creates a new point with the specified X coordinate.
    /// </summary>
    public static Point2<T> WithX<T>(this Point2<T> p, T x)
        where T : INumber<T>
        => new(x, p.Y);

    /// <summary>
    /// Creates a new point with the specified Y coordinate.
    /// </summary>
    public static Point2<T> WithY<T>(this Point2<T> p, T y)
        where T : INumber<T>
        => new(p.X, y);
}