using System.Numerics;
using Raylib_cs;

namespace Game;

public sealed class Player : Entity
{
    private const int Size = 20;
    private const float MoveForce = 10000;
    private const float StopForce = 10000;

    private const float JumpSpeed = 300;
    private const float MaxSpeed = 300;

    private PhysicsCollider _physicsCollider;

    public Player()
    {
        var physics = new Physics(10, Vector2.Zero, Vector2.Zero);
        var bounds = new Rectangle(this.X, this.Y, Size, Size);
        var collider = new Collider(bounds);
        _physicsCollider = new PhysicsCollider(physics, collider);
    }

    public override void SetPosition(float x, float y)
    {
        base.SetPosition(x, y);
        var newCollider = _physicsCollider.Collider.WithPosition(new Vector2(x, y));
        _physicsCollider = _physicsCollider with { Collider = newCollider };
    }

    public override void Update()
    {
        this.Input();
        _physicsCollider = _physicsCollider.Simulated(Program.Colliders);
        var newPosition = _physicsCollider.Collider.Position;
        this.SetPosition(newPosition.X, newPosition.Y);
        if (Math.Abs(_physicsCollider.Physics.Velocity.X) > MaxSpeed)
        {
            var newVelocityX = MaxSpeed * Math.Sign(_physicsCollider.Physics.Velocity.X);
            var newPhysics = _physicsCollider.Physics with
            {
                Velocity = new Vector2(_physicsCollider.Physics.Velocity.Y, newVelocityX)
            };

            _physicsCollider = _physicsCollider with { Physics = newPhysics };
        }
    }

    public override void Draw()
    {
        var size = new Vector2 { X = Size, Y = Size };
        Raylib.DrawRectangleV(this.Position, size, Color.BLUE);
    }

    /// <summary>
    ///     Logic to stop the player from moving.
    /// </summary>
    private void Stop()
    {
        const float leeway = 20;
        var newVelocity = _physicsCollider.Physics.Velocity;
        var newAppliedForce = _physicsCollider.Physics.AppliedForce;

        switch (_physicsCollider.Physics.Velocity.X)
        {
            case > leeway:
                newAppliedForce.X = -StopForce;
                break;
            case < -leeway:
                newAppliedForce.X = StopForce;
                break;
            default:
                newAppliedForce.X = 0.0f;
                newVelocity.X = 0.0f;
                break;
        }

        var newPhysics = _physicsCollider.Physics with { Velocity = newVelocity, AppliedForce = newAppliedForce };
        _physicsCollider = _physicsCollider with { Physics = newPhysics };
    }

    private void Input()
    {
        var newVelocity = _physicsCollider.Physics.Velocity;
        var newAppliedForce = _physicsCollider.Physics.AppliedForce;

        if (Raylib.IsKeyPressed(KeyboardKey.KEY_W) && true)
        {
            newVelocity.Y = -JumpSpeed;
            newAppliedForce.Y = 0;
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            newAppliedForce.X = -MoveForce;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            newAppliedForce.X = MoveForce;
        }

        var newPhysics = _physicsCollider.Physics with { Velocity = newVelocity, AppliedForce = newAppliedForce };
        _physicsCollider = _physicsCollider with { Physics = newPhysics };

        if (!Raylib.IsKeyDown(KeyboardKey.KEY_A) && !Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            this.Stop();
        }
    }
}
