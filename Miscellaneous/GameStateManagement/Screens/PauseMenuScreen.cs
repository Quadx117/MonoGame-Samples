//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace GameStateManagement;

/// <summary>
/// The pause menu comes up over the top of the game,
/// giving the player options to resume or quit.
/// </summary>
class PauseMenuScreen : MenuScreen
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public PauseMenuScreen()
        : base("Paused")
    {
        // Create our menu entries.
        MenuEntry resumeGameMenuEntry = new("Resume Game");
        MenuEntry quitGameMenuEntry = new("Quit Game");

        // Hook up menu event handlers.
        resumeGameMenuEntry.Selected += OnCancel;
        quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

        // Add entries to the menu.
        MenuEntries.Add(resumeGameMenuEntry);
        MenuEntries.Add(quitGameMenuEntry);
    }

    /// <summary>
    /// Event handler for when the Quit Game menu entry is selected.
    /// </summary>
    void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        const string message = "Are you sure you want to quit this game?";

        MessageBoxScreen confirmQuitMessageBox = new(message);

        confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

        ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
    }

    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to quit" message box. This uses the loading screen to
    /// transition from the game back to the main menu screen.
    /// </summary>
    void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
    {
        LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                           new MainMenuScreen());
    }
}
