using System.Numerics;
using Raylib_cs;

namespace thing;

public sealed class PhysicsCollider
{
    /// <summary>
    ///     Gets a boolean value representing if the entity is on the floor or not. Requires a prior call to Simulated()
    ///     to be updated properly.
    /// </summary>
    public bool OnFloor { get; private set; }

    // TODO: this should not be public. instead need to expose velocity and applied force indirectly.
    public readonly Physics Physics;

    private readonly Collider _collider;

    private bool _resetX;
    private bool _resetY;

    /// <summary>
    ///     A generic implementation for physics and collision together. Adds functionality to reset velocity
    ///     on collide.
    /// </summary>
    /// <param name="mass"> The mass of the entity. </param>
    /// <param name="velocity"> The initial velocity of the entity. </param>
    /// <param name="appliedForce"> The initial applied force on the entity. </param>
    /// <param name="bounds"> The collision bounds of the entity. </param>
    public PhysicsCollider(float mass, Vector2 velocity, Vector2 appliedForce, Rectangle bounds)
    {
        Physics = new Physics(mass, velocity, appliedForce);
        _collider = new Collider(bounds);
        Program.Colliders.Add(_collider);
    }

    /// <summary>
    ///  Gets a new position after collision and physics are simulated. Does not update physics or current position.
    /// </summary>
    /// <returns> Returns position after collision and physics are simulated. </returns>
    public Vector2 Simulated()
    {
        var initialPosition = new Vector2(_collider.Bounds.x, _collider.Bounds.y);
        var physicsSimulated = Physics.Simulated(initialPosition);

        _collider.SetPosition(physicsSimulated);

        var collisionResolved = _collider.Resolved();

        var collisionDelta = collisionResolved - physicsSimulated;

        _resetX = (collisionDelta.X > 0 && Physics.Velocity.X < 0) || (collisionDelta.X < 0 && Physics.Velocity.X > 0);
        _resetY = (collisionDelta.Y > 0 && Physics.Velocity.Y < 0) || (collisionDelta.Y < 0 && Physics.Velocity.Y > 0);
        this.OnFloor = collisionDelta.Y < 0;

        // Restore previous collider state.
        _collider.Bounds.x = initialPosition.X;
        _collider.Bounds.y = initialPosition.Y;

        return collisionResolved;
    }

    public void SetPosition(Vector2 position)
    {
        _collider.SetPosition(position);
    }

    /// <summary>
    ///     Executes a physics integration step. Requires a prior call to Simulated() for physics to be adjusted based
    ///     on collision.
    /// </summary>
    public void Step()
    {
        if (_resetX)
        {
            Physics.Velocity.X = 0;
            Physics.AppliedForce.X = 0;
        }

        if (_resetY)
        {
            Physics.Velocity.Y = 0;
            Physics.AppliedForce.Y = 0;
        }

        Physics.Step();
    }
}
