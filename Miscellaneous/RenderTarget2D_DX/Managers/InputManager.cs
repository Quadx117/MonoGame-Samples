﻿namespace RenderTarget2D_DX.Managers;

using Microsoft.Xna.Framework.Input;

internal class InputManager
{
    private static KeyboardState _lastKeyboard;
    private static KeyboardState _currentKeyboard;

    public static bool IsKeyPressed(Keys key)
    {
        return _currentKeyboard.IsKeyDown(key) &&
               _lastKeyboard.IsKeyUp(key);
    }

    public static void Update()
    {
        _lastKeyboard = _currentKeyboard;
        _currentKeyboard = Keyboard.GetState();
    }
}
