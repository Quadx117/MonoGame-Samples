//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace Blackjack_DX;

using System;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class BackgroundScreen : GameScreen
{
    Texture2D background;

    /// <summary>
    /// Initializes a new instance of the screen.
    /// </summary>
    public BackgroundScreen()
    {
        TransitionOnTime = TimeSpan.FromSeconds(0.0);
        TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    /// <summary>
    /// Load graphics content for the screen.
    /// </summary>
    public override void LoadContent()
    {
        background = ScreenManager.Game.Content.Load<Texture2D>(@"Images\titlescreen");
        base.LoadContent();
    }

    /// <summary>
    /// Allows the screen to run logic, such as updating the transition position.
    /// Unlike HandleInput, this method is called regardless of whether the screen
    /// is active, hidden, or in the middle of a transition.
    /// </summary>
    /// <param name="gameTime"></param>
    /// <param name="otherScreenHasFocus"></param>
    /// <param name="coveredByOtherScreen"></param>
    public override void Update(GameTime gameTime, bool otherScreenHasFocus,
        bool coveredByOtherScreen)
    {
        base.Update(gameTime, otherScreenHasFocus, false);
    }

    /// <summary>
    /// This is called when the screen should draw itself.
    /// </summary>
    /// <param name="gameTime"></param>
    public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
    {
        ScreenManager.SpriteBatch.Begin();

        ScreenManager.SpriteBatch.Draw(background, ScreenManager.GraphicsDevice.Viewport.Bounds,
            Color.White * TransitionAlpha);

        ScreenManager.SpriteBatch.End();

        base.Draw(gameTime);
    }
}
