//-----------------------------------------------------------------------------
// MenuEntry.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace GameStateManagement;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Helper class represents a single entry in a MenuScreen. By default this
/// just draws the entry text string, but it can be customized to display menu
/// entries in different ways. This also provides an event that will be raised
/// when the menu entry is selected.
/// </summary>
class MenuEntry
{
    /// <summary>
    /// Tracks a fading selection effect on the entry.
    /// </summary>
    /// <remarks>
    /// The entries transition out of the selection effect when they are deselected.
    /// </remarks>
    private float _selectionFade;

    /// <summary>
    /// Gets or sets the text of this menu entry.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the position at which to draw this menu entry.
    /// This is set by the MenuScreen each frame in Update.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Event raised when the menu entry is selected.
    /// </summary>
    public event EventHandler<PlayerIndexEventArgs> Selected;

    /// <summary>
    /// Method for raising the Selected event.
    /// </summary>
    protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
    {
        Selected?.Invoke(this, new PlayerIndexEventArgs(playerIndex));
    }

    /// <summary>
    /// Constructs a new menu entry with the specified text.
    /// </summary>
    public MenuEntry(string text)
    {
        Text = text;
    }

    /// <summary>
    /// Updates the menu entry.
    /// </summary>
    public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
    {
        // there is no such thing as a selected item on Windows Phone, so we always
        // force isSelected to be false
#if WINDOWS_PHONE
        isSelected = false;
#endif

        // When the menu selection changes, entries gradually fade between
        // their selected and deselected appearance, rather than instantly
        // popping to the new state.
        float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

        _selectionFade = isSelected
                            ? Math.Min(_selectionFade + fadeSpeed, 1)
                            : Math.Max(_selectionFade - fadeSpeed, 0);
    }

    /// <summary>
    /// Draws the menu entry. This can be overridden to customize the appearance.
    /// </summary>
    public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
    {
        // there is no such thing as a selected item on Windows Phone, so we always
        // force isSelected to be false
#if WINDOWS_PHONE
        isSelected = false;
#endif

        // Draw the selected entry in yellow, otherwise white.
        Color color = isSelected
                          ? Color.Yellow
                          : Color.White;

        // Pulsate the size of the selected menu entry.
        double time = gameTime.TotalGameTime.TotalSeconds;

        float pulsate = (float)Math.Sin(time * 6) + 1;

        float scale = 1 + (pulsate * 0.05f * _selectionFade);

        // Modify the alpha to fade text out during transitions.
        color *= screen.TransitionAlpha;

        // Draw text, centered on the middle of each line.
        ScreenManager screenManager = screen.ScreenManager;
        SpriteBatch spriteBatch = screenManager.SpriteBatch;
        SpriteFont font = screenManager.Font;

        Vector2 origin = new(0, font.LineSpacing / 2);

        spriteBatch.DrawString(font, Text, Position, color, 0,
                               origin, scale, SpriteEffects.None, 0);
    }

    /// <summary>
    /// Queries how much space this menu entry requires.
    /// </summary>
    public virtual int GetHeight(MenuScreen screen)
    {
        return screen.ScreenManager.Font.LineSpacing;
    }

    /// <summary>
    /// Queries how wide the entry is, used for centering on the screen.
    /// </summary>
    public virtual int GetWidth(MenuScreen screen)
    {
        return (int)screen.ScreenManager.Font.MeasureString(Text).X;
    }
}
