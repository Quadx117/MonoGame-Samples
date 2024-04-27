namespace Pong_DX;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// This class handles inputs for all human players across all supported
/// input devices (keyboard, mouse and game pads).
/// </summary>
public class PlayerPad
{
    private const int SPEED_FACTOR = 16;
    private const int HALF_SPEED_FACTOR = 8;

    private static int _oldXPos;
    // TODO(PERE): Use Button instead of int?
    private static int _oldDirection;
    private static bool _latch;

    public readonly PadType PadType;

    private int _oldYPos;

    public int Y { get; private set; }

    public PlayerPad(PadType padType)
    {
        PadType = padType;
        MouseState mouseState = Mouse.GetState();
        _oldYPos = mouseState.Y;
    }

    // TODO(PERE): Find better name
    // TODO(PERE): Implement a better system for buttonPresses (see Shooter)?
    public static (PadType, Button) SelectionPoll()
    {
        int latchDir = _oldDirection;
        int dir;
        bool buttonLatch = _latch;
        bool clicked;

        // Check the gamepads
        for (int playerIndex = 0; playerIndex < 4; ++playerIndex)
        {
            GamePadState padState = GamePad.GetState(playerIndex);
            if (!padState.IsConnected)
            {
                // We latch the select button, because you have to hit the select button to get to the controller menu
                clicked = padState.IsButtonDown(Buttons.A);
                _latch = clicked;
                if (clicked && !buttonLatch)
                {
                    return ((PadType)playerIndex, Button.Click);
                }
                else
                {
                    // Direction is latched so when unselecting you don't full go to the other (i.e you stop in the middle position
                    dir = padState.IsButtonDown(Buttons.DPadLeft) || padState.IsButtonDown(Buttons.LeftThumbstickLeft)
                            ? -1
                            : padState.IsButtonDown(Buttons.DPadRight) || padState.IsButtonDown(Buttons.LeftThumbstickRight)
                                ? 1
                                : 0;
                    _oldDirection = dir;
                    if (dir != 0)
                    {
                        return ((PadType)playerIndex, dir != latchDir ? (Button)dir : 0);
                    }
                }
            }
        }

        // Check the keyboard
        KeyboardState keyboard = Keyboard.GetState();
        clicked = keyboard.IsKeyDown(Keys.Space);
        _latch = clicked;
        if (clicked && !buttonLatch)
        {
            return (PadType.Keyboard, Button.Click);
        }
        else
        {
            dir = keyboard.IsKeyDown(Keys.Left)
                      ? -1
                      : keyboard.IsKeyDown(Keys.Right)
                          ? 1
                          : 0;
            _oldDirection = dir;
            if (dir != 0)
            {
                return (PadType.Keyboard, dir != latchDir ? (Button)dir : 0);
            }
        }

        // Check the mouse
        MouseState mouse = Mouse.GetState();
        int xPos = mouse.X;
        clicked = mouse.LeftButton == ButtonState.Pressed;
        _latch = clicked;
        if (clicked && !buttonLatch)
        {
            return (PadType.Mouse, Button.Click);
        }
        else
        {
            int deltaX = xPos - _oldXPos;
            _oldXPos = xPos;
            dir = Math.Sign(deltaX);
            _oldDirection = dir;
            if (dir != 0)
            {
                return (PadType.Mouse, dir != latchDir ? (Button)dir : 0);
            }
        }

        // If no input were selected, return a default tuple
        return (PadType.AI, 0);
    }

    public void PollInput()
    {
        Y = PadType switch
        {
            PadType.One => GetPad((PlayerIndex)PadType),
            PadType.Two => GetPad((PlayerIndex)PadType),
            PadType.Three => GetPad((PlayerIndex)PadType),
            PadType.Four => GetPad((PlayerIndex)PadType),
            PadType.Keyboard => GetKeyboard(),
            PadType.Mouse => GetMouse(),
            _ => 0
        };
    }

    private int GetPad(PlayerIndex index)
    {
        GamePadState state = GamePad.GetState(index);
        if (!state.IsConnected)
        {
            return 0;
        }
        else
        {
            int dPad = state.IsButtonDown(Buttons.DPadUp)
                           ? -1
                           : state.IsButtonDown(Buttons.DPadDown)
                               ? 1
                               : 0;
            if (dPad != 0)
            {
                // TODO(PERE): Const for 16 which is a speedFactor
                return dPad * SPEED_FACTOR;
            }
            else
            {
                float stick = state.ThumbSticks.Left.Y;
                return (int)(stick * SPEED_FACTOR);
            }
        }
    }

    private int GetKeyboard()
    {
        // TODO(PERE): Use other keys for 2 player games on the same keyboard?
        KeyboardState keyboard = Keyboard.GetState();
        int dPad = keyboard.IsKeyDown(Keys.Up)
                       ? -1
                       : keyboard.IsKeyDown(Keys.Down)
                           ? 1
                           : 0;

        return dPad * (keyboard.IsKeyDown(Keys.LeftControl)
                          ? SPEED_FACTOR
                          : HALF_SPEED_FACTOR);
    }

    private int GetMouse()
    {
        MouseState mouse = Mouse.GetState();
        // TODO(PERE): use the mouse wheel instead? (or both?)
        int deltaY = mouse.Y - _oldYPos;
        _oldYPos = mouse.Y;

        return deltaY;
    }
}
