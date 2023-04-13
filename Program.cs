using Raylib_cs;

namespace thing;

internal static class Program
{
    public static List<Collider> Colliders = new()
    {
        new Collider(new Rectangle(0, 300, 110, 50)),
        new Collider(new Rectangle(100, 250, 500, 50)),
        new Collider(new Rectangle(150, 150, 100, 50)),
    };

    public static void Main()
    {
        Raylib.InitWindow(800, 480, "Hello World");
        Raylib.SetTargetFPS(60);

        var entities = new List<Entity> { new Player() };

        while (!Raylib.WindowShouldClose())
        {
            foreach (var e in entities) e.Update();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);

            Raylib.DrawFPS(12, 12);

            foreach (var c in Colliders)
            {
                Raylib.DrawRectangleRec(c.Bounds, Color.RED);
            }

            foreach (var e in entities)
            {
                e.Draw();
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
