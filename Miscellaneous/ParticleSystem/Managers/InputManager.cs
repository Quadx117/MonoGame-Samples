namespace ParticleSystem.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

internal static class InputManager
{
    public static Vector2 MousePosition { get; private set; }

    public static void Update()
    {
        MousePosition = Mouse.GetState().Position.ToVector2();
    }
}
