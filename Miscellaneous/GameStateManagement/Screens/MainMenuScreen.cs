//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace GameStateManagement;

using Microsoft.Xna.Framework;

/// <summary>
/// The main menu screen is the first thing displayed when the game starts up.
/// </summary>
class MainMenuScreen : MenuScreen
{
    /// <summary>
    /// Initializes a new instance of the screen.
    /// </summary>
    public MainMenuScreen()
        : base("Main Menu")
    {
        // Create our menu entries.
        MenuEntry playGameMenuEntry = new("Play Game");
        MenuEntry optionsMenuEntry = new("Options");
        MenuEntry exitMenuEntry = new("Exit");

        // Hook up menu event handlers.
        playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
        optionsMenuEntry.Selected += OptionsMenuEntrySelected;
        exitMenuEntry.Selected += OnCancel;

        // Add entries to the menu.
        MenuEntries.Add(playGameMenuEntry);
        MenuEntries.Add(optionsMenuEntry);
        MenuEntries.Add(exitMenuEntry);
    }

    /// <summary>
    /// Event handler for when the Play Game menu entry is selected.
    /// </summary>
    void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                           new GameplayScreen());
    }

    /// <summary>
    /// Event handler for when the Options menu entry is selected.
    /// </summary>
    void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
    }

    /// <summary>
    /// When the user cancels the main menu, ask if they want to exit the sample.
    /// </summary>
    protected override void OnCancel(PlayerIndex playerIndex)
    {
        const string message = "Are you sure you want to exit this sample?";

        MessageBoxScreen confirmExitMessageBox = new(message);

        confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

        ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
    }

    /// <summary>
    /// Event handler for when the user selects ok on the "are you sure
    /// you want to exit" message box.
    /// </summary>
    void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
    {
        ScreenManager.Game.Exit();
    }
}
