using System.Numerics;
using Raylib_cs;

namespace Game;

public record Physics(float Mass, Vector2 AppliedForce, Vector2 Velocity)
{
    public const float Gravity = 5000; // Unit of acceleration.

    public Vector2 Delta
    {
        get
        {
            var dt = Raylib.GetFrameTime();
            return this.Velocity * dt;
        }
    }

    public Physics Simulated()
    {
        var dt = Raylib.GetFrameTime();

        var forceGravity = this.Mass * Gravity;
        var appliedForceY = Math.Min(AppliedForce.Y + forceGravity * dt, forceGravity);
        var appliedForce = new Vector2(this.AppliedForce.X, appliedForceY);
        var velocity = this.Velocity + this.GetAcceleration(this.AppliedForce) * dt;

        return this with { AppliedForce = appliedForce, Velocity = velocity };
    }

    private Vector2 GetAcceleration(Vector2 force)
    {
        return force / this.Mass;
    }
}
