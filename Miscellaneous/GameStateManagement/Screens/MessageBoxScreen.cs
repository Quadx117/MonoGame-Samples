//-----------------------------------------------------------------------------
// MessageBoxScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace GameStateManagement;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// A popup message box screen, used to display "are you sure?"
/// confirmation messages.
/// </summary>
class MessageBoxScreen : GameScreen
{
    private readonly string _message;
    private Texture2D _gradientTexture;

    public event EventHandler<PlayerIndexEventArgs> Accepted;
    public event EventHandler<PlayerIndexEventArgs> Cancelled;

    /// <summary>
    /// Constructor automatically includes the standard "A=ok, B=cancel"
    /// usage text prompt.
    /// </summary>
    public MessageBoxScreen(string message)
        : this(message, true)
    { }

    /// <summary>
    /// Constructor lets the caller specify whether to include the standard
    /// "A=ok, B=cancel" usage text prompt.
    /// </summary>
    public MessageBoxScreen(string msg, bool includeUsageText)
    {
        const string usageText = "\nA button, Space, Enter = ok" +
                                 "\nB button, Esc = cancel";

        _message = includeUsageText
                      ? msg + usageText
                      : msg;

        IsPopup = true;

        TransitionOnTime = TimeSpan.FromSeconds(0.2);
        TransitionOffTime = TimeSpan.FromSeconds(0.2);
    }

    /// <summary>
    /// Loads graphics content for this screen. This uses the shared ContentManager
    /// provided by the Game class, so the content will remain loaded forever.
    /// Whenever a subsequent MessageBoxScreen tries to load this same content,
    /// it will just get back another reference to the already loaded data.
    /// </summary>
    public override void LoadContent()
    {
        ContentManager content = ScreenManager.Game.Content;

        _gradientTexture = content.Load<Texture2D>("Images/gradient");
    }

    /// <summary>
    /// Responds to user input, accepting or cancelling the message box.
    /// </summary>
    public override void HandleInput(InputState input)
    {
        // We pass in our ControllingPlayer, which may either be null (to
        // accept input from any player) or a specific index. If we pass a null
        // controlling player, the InputState helper returns to us which player
        // actually provided the input. We pass that through to our Accepted and
        // Cancelled events, so they can tell which player triggered them.
        if (input.IsMenuSelect(ControllingPlayer, out PlayerIndex playerIndex))
        {
            // Raise the accepted event, then exit the message box.
            Accepted?.Invoke(this, new PlayerIndexEventArgs(playerIndex));

            ExitScreen();
        }
        else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
        {
            // Raise the cancelled event, then exit the message box.
            Cancelled?.Invoke(this, new PlayerIndexEventArgs(playerIndex));

            ExitScreen();
        }
    }

    /// <summary>
    /// Draws the message box.
    /// </summary>
    public override void Draw(GameTime gameTime)
    {
        SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
        SpriteFont font = ScreenManager.Font;

        // Darken down any other screens that were drawn beneath the popup.
        ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

        // Center the message text in the viewport.
        Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
        Vector2 viewportSize = new(viewport.Width, viewport.Height);
        Vector2 textSize = font.MeasureString(_message);
        Vector2 textPosition = (viewportSize - textSize) / 2;

        // The background includes a border somewhat larger than the text itself.
        const int hPad = 32;
        const int vPad = 16;

        Rectangle backgroundRectangle = new((int)textPosition.X - hPad,
                                            (int)textPosition.Y - vPad,
                                            (int)textSize.X + (hPad * 2),
                                            (int)textSize.Y + (vPad * 2));

        // Fade the popup alpha during transitions.
        Color color = Color.White * TransitionAlpha;

        spriteBatch.Begin();

        // Draw the background rectangle.
        spriteBatch.Draw(_gradientTexture, backgroundRectangle, color);

        // Draw the message box text.
        spriteBatch.DrawString(font, _message, textPosition, color);

        spriteBatch.End();
    }
}
