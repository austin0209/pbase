using System.Numerics;
using Raylib_cs;

namespace thing;

public sealed class Player : Entity
{
    private const int Size = 20;
    private const float MoveForce = 10000;
    private const float StopForce = 10000;

    private const float JumpSpeed = 300;
    private const float MaxSpeed = 300;

    private readonly PhysicsCollider _physicsCollider;
    // private readonly Collider _collider;
    // private readonly Physics _physics;

    public Player()
    {
        // _physics = new Physics(10, Vector2.Zero, Vector2.Zero);
        // _collider = new Collider(this.X, this.Y, Size, Size);
        var bounds = new Rectangle(this.X, this.Y, Size, Size);
        _physicsCollider = new PhysicsCollider(10, Vector2.Zero, Vector2.Zero, bounds);
    }

    public override void SetPosition(float x, float y)
    {
        base.SetPosition(x, y);
        // _collider.SetPosition(x, y);
        _physicsCollider.SetPosition(new Vector2(x, y));
    }

    public override void Update()
    {
        this.Input();
        // this.ApplyPhysics();
        // this.ResolveCollision();
        var newPosition = _physicsCollider.Simulated();
        this.SetPosition(newPosition.X, newPosition.Y);
        _physicsCollider.Step();
        if (Math.Abs(_physicsCollider.Physics.Velocity.X) > MaxSpeed)
        {
            _physicsCollider.Physics.Velocity.X = MaxSpeed * Math.Sign(_physicsCollider.Physics.Velocity.X);
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
        var physics = _physicsCollider.Physics;

        switch (physics.Velocity.X)
        {
            case > leeway:
                physics.AppliedForce.X = -StopForce;
                break;
            case < -leeway:
                physics.AppliedForce.X = StopForce;
                break;
            default:
                physics.AppliedForce.X = 0.0f;
                physics.Velocity.X = 0.0f;
                break;
        }
    }

    private void Input()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_W) && true)
        {
            _physicsCollider.Physics.Velocity.Y = -JumpSpeed;
            _physicsCollider.Physics.AppliedForce.Y = 0;
        }

        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            _physicsCollider.Physics.AppliedForce.X = -MoveForce;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            _physicsCollider.Physics.AppliedForce.X = MoveForce;
        }

        if (!Raylib.IsKeyDown(KeyboardKey.KEY_A) && !Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            this.Stop();
        }
    }

    // private void ResolveCollision()
    // {
    //     var resolvedPosition = _collider.Resolved();
    //     var delta = this.Position - resolvedPosition;
    //
    //     this.SetPosition(resolvedPosition.X, resolvedPosition.Y);
    //
    //     if (delta.X != 0)
    //     {
    //         _physics.Velocity.X = 0;
    //         _physics.AppliedForce.X = 0;
    //     }
    //
    //     if (!_jumping && delta.Y != 0)
    //     {
    //         _physics.Velocity.Y = 0;
    //         _physics.AppliedForce.Y = 0;
    //     }
    // }
    //
    // private void ApplyPhysics()
    // {
    //     if (Math.Abs(_physics.Velocity.X) > MaxSpeed)
    //     {
    //         _physics.Velocity.X = MaxSpeed * Math.Sign(_physics.Velocity.X);
    //     }
    //
    //     var simulatedPosition = _physics.Simulated(this.Position);
    //     this.SetPosition(simulatedPosition.X, simulatedPosition.Y);
    //     _physics.Step();
    // }
}
