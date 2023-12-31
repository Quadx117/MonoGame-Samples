namespace ProgressBar.Managers;

using Microsoft.Xna.Framework.Input;

internal static class InputManager
{
    private static MouseState _lastMouseState;

    public static bool MouseLeftClicked { get; private set; }

    public static bool MouseRightClicked { get; private set; }

    public static void Update()
    {
        MouseState mouse = Mouse.GetState();

        MouseLeftClicked = mouse.LeftButton == ButtonState.Pressed &&
                          _lastMouseState.LeftButton == ButtonState.Released;

        MouseRightClicked = mouse.RightButton == ButtonState.Pressed &&
                            _lastMouseState.RightButton == ButtonState.Released;

        _lastMouseState = mouse;
    }
}
