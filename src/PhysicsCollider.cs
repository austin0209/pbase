using System.Numerics;
using Raylib_cs;

namespace Game;

/// <summary>
///     A generic implementation for physics and collision together. Adds functionality to reset velocity
///     on collide.
/// </summary>
public record PhysicsCollider(Physics Physics, Collider Collider, bool OnFloor=false)
{
    /// <summary>
    ///  Gets a new collider after collision and physics are simulated. Does not update physics or current position.
    /// </summary>
    /// <returns> Returns collider after collision and physics are simulated. </returns>
    public PhysicsCollider Simulated(List<Collider> allColliders)
    {
        var initialPosition = new Vector2(this.Collider.Bounds.x, this.Collider.Bounds.y);
        var finalPosition = this.Physics.Delta + initialPosition;

        var movedCollider = this.Collider.WithPosition(finalPosition);

        var colliderResolved = movedCollider.Resolved(allColliders);

        var collisionDelta = colliderResolved.Position - this.Collider.Position;

        var resetX = (collisionDelta.X > 0 && this.Physics.Velocity.X < 0) || (collisionDelta.X < 0 && this.Physics.Velocity.X > 0);
        var resetY = (collisionDelta.Y > 0 && this.Physics.Velocity.Y < 0) || (collisionDelta.Y < 0 && this.Physics.Velocity.Y > 0);
        var onFloor = collisionDelta.Y < 0;

        var newVelocity = new Vector2();
        var newAppliedForce = new Vector2();

        if (resetX)
        {
            newVelocity.X = 0;
            newAppliedForce.X = 0;
        }

        if (resetY)
        {
            newVelocity.Y = 0;
            newAppliedForce.Y = 0;
        }

        var newPhysics = this.Physics with { Velocity = newVelocity, AppliedForce = newAppliedForce };
        newPhysics = newPhysics.Simulated();

        return new PhysicsCollider(newPhysics, colliderResolved, onFloor);
    }
}
