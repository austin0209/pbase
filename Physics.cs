using System.Numerics;
using Raylib_cs;

namespace thing;

public sealed class Physics
{
    public const float Gravity = 5000; // Unit of acceleration.

    public float Mass;
    public Vector2 AppliedForce;
    public Vector2 Velocity;

    public Physics(float mass, Vector2 velocity, Vector2 appliedForce)
    {
        Mass = mass;
        Velocity = velocity;
        AppliedForce = appliedForce;
    }

    public Vector2 Simulated(in Vector2 initialPosition)
    {
        var dt = Raylib.GetFrameTime();
        return initialPosition + Velocity * dt;
    }

    public void Step()
    {
        var dt = Raylib.GetFrameTime();

        var forceGravity = Mass * Gravity;
        AppliedForce.Y = Math.Min(AppliedForce.Y + forceGravity * dt, forceGravity);
        Velocity += this.GetAcceleration(AppliedForce) * dt;
    }

    public Physics Clone()
    {
        return new Physics(Mass, AppliedForce, Velocity);
    }

    private Vector2 GetAcceleration(Vector2 force)
    {
        return force / Mass;
    }
}
