using System.Numerics;
using Raylib_cs;

namespace PBase;

public sealed class Collider : ICloneable
{
    public Rectangle Bounds;

    public Collider(Rectangle bounds)
    {
        Bounds = bounds;
    }

    /// <summary>
    ///     Gets the resolved position against the global list of colliders.
    /// </summary>
    /// <returns> A <see cref="Vector2" /> of the resolved positions. </returns>
    public Vector2 Resolved()
    {
        var finalPosition = new Vector2(Bounds.x, Bounds.y);

        foreach (var c in Program.Colliders)
        {
            if (c == this)
            {
                continue;
            }

            var resolution = GetResolution(Bounds, c.Bounds);
            finalPosition += resolution;
        }

        return finalPosition;
    }

    public void SetPosition(Vector2 newPosition)
    {
        Bounds.x = newPosition.X;
        Bounds.y = newPosition.Y;
    }

    public void SetPosition(float x, float y)
    {
        Bounds.x = x;
        Bounds.y = y;
    }

    private static Vector2 GetResolution(Rectangle a, Rectangle b)
    {
        var aLeft = a.x;
        var aRight = a.x + a.width;
        var bLeft = b.x;
        var bRight = b.x + b.width;

        if (bRight < aLeft || aRight < bLeft)
        {
            return Vector2.Zero;
        }

        var aTop = a.y;
        var aBottom = a.y + a.height;
        var bTop = b.y;
        var bBottom = b.y + b.height;

        if (bBottom < aTop || aBottom < bTop)
        {
            return Vector2.Zero;
        }

        var xOverlap = Math.Min(aRight, bRight) - Math.Max(aLeft, bLeft);
        var yOverlap = Math.Min(aBottom, bBottom) - Math.Max(aTop, bTop);

        var normal = xOverlap < yOverlap ? new Vector2(1, 0) : new Vector2(0, 1);
        var minOverlap = Math.Min(xOverlap, yOverlap);

        var resolution = normal * minOverlap;

        var aPosition = new Vector2(a.x, a.y);
        var bPosition = new Vector2(b.x, b.y);
        var difference = aPosition - bPosition;

        if (Vector2.Dot(difference, resolution) < 0)
        {
            resolution *= -1;
        }

        return resolution;
    }

    public object Clone()
    {
        return new Collider(new Rectangle(Bounds.x, Bounds.y, Bounds.width, Bounds.height));
    }
}
