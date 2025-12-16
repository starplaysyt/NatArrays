using System.Numerics;

namespace NatLib.Core;

public static class Vector2Operations
{
    // ReSharper disable MemberCanBePrivate.Global
    
    // =========================
    // 1. INumber<T>
    // =========================

    public static Vector2<T> Add<T>(this Vector2<T> a, Vector2<T> b)
        where T : INumber<T>
        => new(a.X + b.X, a.Y + b.Y);

    public static Vector2<T> Add<T>(this Vector2<T> a, T scalar)
        where T : INumber<T>
        => new(a.X + scalar, a.Y + scalar);

    public static Vector2<T> Subtract<T>(this Vector2<T> a, Vector2<T> b)
        where T : INumber<T>
        => new(a.X - b.X, a.Y - b.Y);

    public static Vector2<T> Multiply<T>(this Vector2<T> a, T scalar)
        where T : INumber<T>
        => new(a.X * scalar, a.Y * scalar);

    public static Vector2<T> Divide<T>(this Vector2<T> a, T scalar)
        where T : INumber<T>
        => new(a.X / scalar, a.Y / scalar);

    public static Vector2<T> Abs<T>(this Vector2<T> v)
        where T : INumber<T>
        => new(T.Abs(v.X), T.Abs(v.Y));

    public static Vector2<T> Min<T>(this Vector2<T> a, Vector2<T> b)
        where T : INumber<T>
        => new(T.Min(a.X, b.X), T.Min(a.Y, b.Y));

    public static Vector2<T> Max<T>(this Vector2<T> a, Vector2<T> b)
        where T : INumber<T>
        => new(T.Max(a.X, b.X), T.Max(a.Y, b.Y));

    public static Vector2<T> Clamp<T>(this Vector2<T> v, Vector2<T> min, Vector2<T> max)
        where T : INumber<T>
        => new(T.Clamp(v.X, min.X, max.X), T.Clamp(v.Y, min.Y, max.Y));
    
    public static T Dot<T>(this Vector2<T> a, Vector2<T> b)
        where T : INumber<T>
        => a.X * b.X + a.Y * b.Y;

    public static Vector2<T> Lerp<T>(Vector2<T> a, Vector2<T> b, T t)
        where T : INumber<T>
        => new(a.X + (b.X - a.X) * t,
            a.Y + (b.Y - a.Y) * t);
    
    /// <summary>
    /// Returns the squared length (for comparisons).
    /// </summary>
    /// <param name="v"> Vector itself. </param>
    /// <typeparam name="T"> Any suitable datatype underneath Vector2. </typeparam>
    /// <returns> Squared vector length. </returns>
    public static T LengthSquared<T>(this Vector2<T> v)
        where T : INumber<T>
        => v.X * v.X + v.Y * v.Y;
    
    // =========================
    // 2. IFloatingPoint<T>
    // =========================

    /// <summary>
    /// Returns the Euclidean length (magnitude) of the vector.
    /// </summary>
    /// <param name="v"> Vector itself. </param>
    /// <typeparam name="T"> Any suitable datatype underneath Vector2. </typeparam>
    /// <returns> Euclidean vector length. </returns>
    public static T Length<T>(this Vector2<T> v)
        where T : IFloatingPoint<T>, IRootFunctions<T>
        => T.Sqrt(v.X * v.X + v.Y * v.Y);

    /// <summary>
    /// Returns a new vector pointing in the same direction with length 1.
    /// </summary>
    /// <param name="v"> Vector itself. </param>
    /// <typeparam name="T"> Any suitable datatype underneath Vector2. </typeparam>
    /// <returns> Vector pointing in the same direction with length 1. </returns>
    public static Vector2<T> Normalized<T>(this Vector2<T> v)
        where T : IFloatingPoint<T>, IRootFunctions<T>
        => v * (T.One / v.Length());
    
    /// <summary>
    /// Tries to normalize vector, performs zero-check to not get DivideByZeroException.
    /// </summary>
    /// <param name="v"> Vector itself. </param>
    /// <param name="result"> Normalized vector. </param>
    /// <typeparam name="T"> Any suitable datatype underneath Vector2. </typeparam>
    /// <returns> Result of operation - successful or not. </returns>
    public static bool TryNormalize<T>(this Vector2<T> v, out Vector2<T> result)
        where T : IFloatingPoint<T>, IRootFunctions<T>
    {
        var len = v.Length();
        if (len == T.Zero)
        {
            result = default;
            return false;
        }
        result = v * (T.One / len);
        return true;
    }

    /// <summary>
    /// Returns Euclidean distance between this vector and other.
    /// </summary>
    /// <param name="a"> First vector. </param>
    /// <param name="b"> Second vector. </param>
    /// <typeparam name="T"> Any suitable datatype underneath Vector2. </typeparam>
    /// <returns> Vector containing Euclidean distance. </returns>
    public static T Distance<T>(Vector2<T> a, Vector2<T> b)
        where T : IFloatingPoint<T>, IRootFunctions<T>
        => (b - a).Length();
    
    /// <summary>
    /// Returns squared distance between vectors(for comparisons)
    /// </summary>
    /// <param name="a"> First vector. </param>
    /// <param name="b"> Second vector. </param>
    /// <typeparam name="T"> Any suitable datatype underneath Vector2. </typeparam>
    /// <returns> Vector containing squared Euclidean distance. </returns>
    public static T DistanceSquared<T>(Vector2<T> a, Vector2<T> b)
        where T : IFloatingPoint<T> 
        => (b - a).LengthSquared();

    /// <summary>
    /// Returns angle between vectors in radians.
    /// </summary>
    /// <param name="a"> First vector. </param>
    /// <param name="b"> Second vector. </param>
    /// <typeparam name="T"> Any suitable datatype underneath Vector2. </typeparam>
    /// <returns> Angle between vectors. </returns>
    public static T Angle<T>(Vector2<T> a, Vector2<T> b)
        where T : IFloatingPoint<T>, IRootFunctions<T>, ITrigonometricFunctions<T>
    {
        var dot = a.Dot(b);
        var len = a.Length() * b.Length();
        return T.Acos(dot / len);
    }

    /// <summary>
    /// Reflects given vector about a given normal.
    /// </summary>
    /// <param name="v"> Vector itself. </param>
    /// <param name="normal"> Normal vector. </param>
    /// <typeparam name="T"> Any suitable datatype underneath Vector2. </typeparam>
    /// <returns> Reflection of given vector. </returns>
    public static Vector2<T> Reflect<T>(Vector2<T> v, Vector2<T> normal)
        where T : IFloatingPoint<T>
        => v - normal * (T.CreateChecked(2) * v.Dot(normal));
    
    /// <summary>
    /// Projects given vector on another.
    /// </summary>
    /// <param name="v"> First vector. </param>
    /// <param name="onto"> Second vector. </param>
    /// <typeparam name="T"> Any suitable datatype underneath Vector2. </typeparam>
    /// <returns> Projection of given vector on another. </returns>
    public static Vector2<T> Project<T>(Vector2<T> v, Vector2<T> onto)
        where T : IFloatingPoint<T>
        => onto * (v.Dot(onto) / onto.Dot(onto));

    /// <summary>
    /// Rotates vector to a specific angle.
    /// </summary>
    /// <param name="v"> Vector itself. </param>
    /// <param name="radians"> Given angle. </param>
    /// <typeparam name="T"> Any suitable datatype underneath Vector2. </typeparam>
    /// <returns> Rotated vector. </returns>
    public static Vector2<T> Rotate<T>(this Vector2<T> v, T radians)
        where T : IFloatingPoint<T>, ITrigonometricFunctions<T>
    {
        var cos = T.Cos(radians);
        var sin = T.Sin(radians);
        return new Vector2<T>(
            v.X * cos - v.Y * sin,
            v.X * sin + v.Y * cos
        );
    }
    
    // =========================
    // 3. IBinaryInteger<T>
    // =========================

    public static T ManhattanLength<T>(this Vector2<T> v)
        where T : IBinaryInteger<T>
        => T.Abs(v.X) + T.Abs(v.Y);


    public static Vector2<int> Sign<T>(this Vector2<T> v)
        where T : IBinaryInteger<T>, ISignedNumber<T>
        => new(T.Sign(v.X), T.Sign(v.Y));
    
    // =========================
    // 4. Geometry helpers
    // =========================

    public static Vector2<T> Perpendicular<T>(this Vector2<T> v)
        where T : INumber<T>
        => new(-v.Y, v.X);

    public static T Cross<T>(Vector2<T> a, Vector2<T> b)
        where T : INumber<T>
        => a.X * b.Y - a.Y * b.X;

    public static bool IsParallel<T>(Vector2<T> a, Vector2<T> b)
        where T : INumber<T>
        => Cross(a, b) == T.Zero;

    public static bool IsOrthogonal<T>(Vector2<T> a, Vector2<T> b)
        where T : INumber<T>
        => a.Dot(b) == T.Zero;

    // =========================
    // 5. Utility
    // =========================

    public static void Deconstruct<T>(this Vector2<T> v, out T x, out T y)
        where T : INumber<T>
    {
        x = v.X;
        y = v.Y;
    }

    public static Vector2<T> WithX<T>(this Vector2<T> v, T x)
        where T : INumber<T>
        => new(x, v.Y);


    public static Vector2<T> WithY<T>(this Vector2<T> v, T y)
        where T : INumber<T>
        => new(v.X, y);


    public static Vector2<T> Swap<T>(this Vector2<T> v)
        where T : INumber<T>
        => new(v.Y, v.X);
}