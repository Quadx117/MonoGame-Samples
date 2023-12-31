namespace ProgressBar.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal class ProgressBar
{
    /// <summary>
    /// The <see cref="Texture2D"/> instance used the draw the background of the prgress bar.
    /// </summary>
    protected readonly Texture2D _background;

    /// <summary>
    /// The <see cref="Texture2D"/> instance used the draw the foreground of the prgress bar.
    /// </summary>
    protected readonly Texture2D _foreground;

    /// <summary>
    /// The X and Y coordinate of the top left corner of the progress bar.
    /// </summary>
    protected readonly Vector2 _position;

    /// <summary>
    /// The maximum value of the progress bar.
    /// </summary>
    protected readonly float _maxValue;

    /// <summary>
    /// The current value of the progress bar.
    /// </summary>
    protected float _currentValue;

    /// <summary>
    /// The part of the foreground that is visible. This is what enables us to
    /// show the current value of the progress bar.
    /// </summary>
    protected Rectangle _visibleRect;

    public ProgressBar(Texture2D background, Texture2D foreground, float maxValue, Vector2 position)
    {
        _background = background;
        _foreground = foreground;
        _maxValue = maxValue;
        _currentValue = maxValue;
        _position = position;
        _visibleRect = new Rectangle(0, 0, _foreground.Width, _foreground.Height);
    }

    /// <summary>
    /// Update the progress bar.
    /// </summary>
    /// <param name="newValue">The new value to assign as the current value of the progress bar.</param>
    /// <param name="totalSecondsElapsed">The number of seconds elapsed since the
    /// last call to the <c>Update</c> method.</param>
    public virtual void Update(float newValue, float totalSecondsElapsed)
    {
        _currentValue = newValue;
        _visibleRect.Width = (int)(_currentValue / _maxValue * _foreground.Width);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_background, _position, Color.White);
        spriteBatch.Draw(_foreground, _position, _visibleRect, Color.White);
    }
}
