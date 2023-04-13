using System.Numerics;

namespace PBase;

public abstract class Entity
{
    private Vector2 _position;

    public Vector2 Position => _position;

    public float X => this.Position.X;

    public float Y => this.Position.Y;

    public virtual void SetPosition(float x, float y)
    {
        _position.X = x;
        _position.Y = y;
    }

    public abstract void Update();

    public abstract void Draw();
}
