//-----------------------------------------------------------------------------
// InputState.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace GameStateManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Helper for reading input from keyboard, gamepad, and touch input. This class 
/// tracks both the current and previous state of the input devices, and implements 
/// query methods for high level input actions such as "move up through the menu"
/// or "pause the game".
/// </summary>
public class InputState
{
    public const int MaxInputs = 4;

    public KeyboardState CurrentKeyboardStates;
    public KeyboardState LastKeyboardStates;

    public readonly GamePadState[] CurrentGamePadStates;
    public readonly GamePadState[] LastGamePadStates;

    public readonly bool[] GamePadWasConnected;

#if WINDOWS_PHONE
    public TouchCollection TouchState;

    public readonly List<GestureSample> Gestures = new List<GestureSample>();
#endif

    /// <summary>
    /// Constructs a new input state.
    /// </summary>
    public InputState()
    {
        CurrentKeyboardStates = new KeyboardState();
        LastKeyboardStates = new KeyboardState();

        CurrentGamePadStates = new GamePadState[MaxInputs];
        LastGamePadStates = new GamePadState[MaxInputs];

        GamePadWasConnected = new bool[MaxInputs];
    }

    /// <summary>
    /// Reads the latest state of the keyboard and gamepad.
    /// </summary>
    public void Update()
    {
        LastKeyboardStates = CurrentKeyboardStates;

        // NOTE(PERE): MonoGame doesn't support multiple keyboards plugged
        // into the same PC anymore.
        CurrentKeyboardStates = Keyboard.GetState();

        for (int i = 0; i < MaxInputs; i++)
        {
            LastGamePadStates[i] = CurrentGamePadStates[i];
            CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

            // Keep track of whether a gamepad has ever been
            // connected, so we can detect if it is unplugged.
            if (CurrentGamePadStates[i].IsConnected)
            {
                GamePadWasConnected[i] = true;
            }
        }
#if WINDOWS_PHONE
        TouchState = TouchPanel.GetState();

        Gestures.Clear();
        while (TouchPanel.IsGestureAvailable)
        {
            Gestures.Add(TouchPanel.ReadGesture());
        }
#endif
    }

    /// <summary>
    /// Helper for checking if a key was newly pressed during this update. When a
    /// keypress is detected, the output playerIndex reports which player pressed
    /// it.
    /// </summary>
    public bool IsNewKeyPress(Keys key,
                              out PlayerIndex playerIndex)
    {
        // NOTE(PERE): We default to one in the case of keyboard input since
        // the game is not really local multiplier enabled so we can minimize
        // the changes to the code for now. A lot more thoughts would be needed
        // if we ever want to mak local multiplayer work.
        playerIndex = PlayerIndex.One;

        return CurrentKeyboardStates.IsKeyDown(key) &&
               LastKeyboardStates.IsKeyUp(key);
    }

    /// <summary>
    /// Helper for checking if a button was newly pressed during this update.
    /// The controllingPlayer parameter specifies which player to read input for.
    /// If this is null, it will accept input from any player. When a button press
    /// is detected, the output playerIndex reports which player pressed it.
    /// </summary>
    public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer,
                                 out PlayerIndex playerIndex)
    {
        if (controllingPlayer.HasValue)
        {
            // Read input from the specified player.
            playerIndex = controllingPlayer.Value;

            int i = (int)playerIndex;

            return CurrentGamePadStates[i].IsButtonDown(button) &&
                    LastGamePadStates[i].IsButtonUp(button);
        }
        else
        {
            // Accept input from any player.
            return IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Four, out playerIndex);
        }
    }

    /// <summary>
    /// Checks for a "menu select" input action.
    /// The controllingPlayer parameter specifies which player to read input for.
    /// If this is null, it will accept input from any player.
    /// </summary>
    public bool IsMenuSelect(PlayerIndex? controllingPlayer)
    {
        return IsNewKeyPress(Keys.Space, out _) ||
               IsNewKeyPress(Keys.Enter, out _) ||
               IsNewButtonPress(Buttons.A, controllingPlayer, out _) ||
               IsNewButtonPress(Buttons.Start, controllingPlayer, out _);
    }

    /// <summary>
    /// Checks for a "menu cancel" input action.
    /// The controllingPlayer parameter specifies which player to read input for.
    /// If this is null, it will accept input from any player.
    /// </summary>
    public bool IsMenuCancel(PlayerIndex? controllingPlayer)
    {
        return IsNewKeyPress(Keys.Escape, out _) ||
               IsNewButtonPress(Buttons.B, controllingPlayer, out _) ||
               IsNewButtonPress(Buttons.Back, controllingPlayer, out _);
    }

    /// <summary>
    /// Checks for a "menu up" input action.
    /// The controllingPlayer parameter specifies which player to read
    /// input for. If this is null, it will accept input from any player.
    /// </summary>
    public bool IsMenuUp(PlayerIndex? controllingPlayer)
    {
        return IsNewKeyPress(Keys.Up, out _) ||
               IsNewKeyPress(Keys.Left, out _) ||
               IsNewButtonPress(Buttons.DPadLeft, controllingPlayer, out _) ||
               IsNewButtonPress(Buttons.LeftThumbstickLeft, controllingPlayer, out _);
    }

    /// <summary>
    /// Checks for a "menu down" input action.
    /// The controllingPlayer parameter specifies which player to read
    /// input for. If this is null, it will accept input from any player.
    /// </summary>
    public bool IsMenuDown(PlayerIndex? controllingPlayer)
    {
        return IsNewKeyPress(Keys.Down, out _) ||
               IsNewKeyPress(Keys.Right, out _) ||
               IsNewButtonPress(Buttons.DPadRight, controllingPlayer, out _) ||
               IsNewButtonPress(Buttons.LeftThumbstickRight, controllingPlayer, out _);
    }

    /// <summary>
    /// Checks for a "pause the game" input action.
    /// The controllingPlayer parameter specifies which player to read
    /// input for. If this is null, it will accept input from any player.
    /// </summary>
    public bool IsPauseGame(PlayerIndex? controllingPlayer)
    {
        return IsNewKeyPress(Keys.Escape, out _) ||
               IsNewButtonPress(Buttons.Back, controllingPlayer, out _) ||
               IsNewButtonPress(Buttons.Start, controllingPlayer, out _);
    }
}
