﻿//-----------------------------------------------------------------------------
// BlackjackGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace Blackjack_DX;

using GameStateManagement;
using Microsoft.Xna.Framework;

/// <summary>
/// This is the main game type.
/// </summary>
public class BlackjackGame : Game
{
    private readonly GraphicsDeviceManager graphics;
    private readonly ScreenManager screenManager;

    public static float HeightScale = 1.0f;
    public static float WidthScale = 1.0f;

    /// <summary>
    /// Initializes a new instance of the game.
    /// </summary>
    public BlackjackGame()
    {
        graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";

        screenManager = new ScreenManager(this);

        screenManager.AddScreen(new BackgroundScreen(), null);
        screenManager.AddScreen(new MainMenuScreen(), null);

        Components.Add(screenManager);

#if WINDOWS
        IsMouseVisible = true;
#elif WINDOWS_PHONE
        // Frame rate is 30 fps by default for Windows Phone.
        TargetElapsedTime = TimeSpan.FromTicks(333333);
        graphics.IsFullScreen = true;
#else
        Components.Add(new GamerServicesComponent(this));
#endif

        // Initialize sound system
        AudioManager.Initialize(this);
    }

    protected override void Initialize()
    {
        base.Initialize();

#if XBOX
        graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
        graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width; 
#elif WINDOWS
        graphics.PreferredBackBufferHeight = 480;
        graphics.PreferredBackBufferWidth = 800;
#endif
        graphics.ApplyChanges();

        Rectangle bounds = graphics.GraphicsDevice.Viewport.TitleSafeArea;
        HeightScale = bounds.Height / 480f;
        WidthScale = bounds.Width / 800f;

    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
        AudioManager.LoadSounds();

        base.LoadContent();
    }
}
