using System.Numerics;
using Raylib_cs;

namespace Game;

public record Collider(Rectangle Bounds)
{
    public Vector2 Position => new Vector2(this.Bounds.x, this.Bounds.y);

    /// <summary>
    ///     Gets the collider against a list of colliders.
    /// </summary>
    public Collider Resolved(List<Collider> allColliders)
    {
        var finalPosition = new Vector2(Bounds.x, Bounds.y);

        foreach (var c in allColliders)
        {
            if (c == this)
            {
                continue;
            }

            var resolution = GetResolution(Bounds, c.Bounds);
            finalPosition += resolution;
        }

        return this.WithPosition(finalPosition);
    }

    public Collider WithPosition(Vector2 newPosition)
    {
        var bounds = new Rectangle(newPosition.X, newPosition.Y, this.Bounds.width, this.Bounds.height);
        return new Collider(bounds);
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
}
