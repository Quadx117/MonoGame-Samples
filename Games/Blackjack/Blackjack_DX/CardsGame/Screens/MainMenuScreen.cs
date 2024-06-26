//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace Blackjack_DX;

using System;
using GameStateManagement;
using Microsoft.Xna.Framework;

class MainMenuScreen : MenuScreen
{
    public static string Theme = "Red";

    /// <summary>
    /// Initializes a new instance of the screen.
    /// </summary>
    public MainMenuScreen()
        : base("")
    {
    }

    public override void LoadContent()
    {
        // Create our menu entries.
        MenuEntry startGameMenuEntry = new("Play");
        MenuEntry themeGameMenuEntry = new("Theme");
        MenuEntry exitMenuEntry = new("Exit");

        // Hook up menu event handlers.
        startGameMenuEntry.Selected += StartGameMenuEntrySelected;
        themeGameMenuEntry.Selected += ThemeGameMenuEntrySelected;
        exitMenuEntry.Selected += OnCancel;

        // Add entries to the menu.
        MenuEntries.Add(startGameMenuEntry);
        MenuEntries.Add(themeGameMenuEntry);
        MenuEntries.Add(exitMenuEntry);

        base.LoadContent();
    }

    /// <summary>
    /// Respond to "Play" Item Selection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void StartGameMenuEntrySelected(object sender, EventArgs e)
    {
        foreach (GameScreen screen in ScreenManager.GetScreens())
        {
            screen.ExitScreen();
        }

        ScreenManager.AddScreen(new GameplayScreen(Theme), null);
    }

    /// <summary>
    /// Respond to "Theme" Item Selection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void ThemeGameMenuEntrySelected(object sender, EventArgs e)
    {
        ScreenManager.AddScreen(new OptionsMenu(), null);
    }

    /// <summary>
    /// Respond to "Exit" Item Selection
    /// </summary>
    /// <param name="playerIndex"></param>
    protected override void OnCancel(PlayerIndex playerIndex)
    {
        ScreenManager.Game.Exit();
    }
}
