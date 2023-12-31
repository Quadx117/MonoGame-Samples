namespace ProgressBar.GameObjects;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal class ProgressBarAnimated : ProgressBar
{
    /// <summary>
    /// The new value of the progress bar, once the animation is finished.
    /// </summary>
    private float _targetValue;

    /// <summary>
    /// The speed at which the animation will run. For every secondes elapsed,
    /// this amount of the progress bar will be animated.
    /// </summary>
    private readonly float _animationSpeed = 20;

    /// <summary>
    /// The part of the foreground that is visible during the animation. This is
    /// what enables us to show the animation between the current value and the
    /// target value.
    /// </summary>
    private Rectangle _animationVisibleRect;

    /// <summary>
    /// The X and Y coordinate of the top left corner of the animated part of
    /// the progress bar. This is the part between the current value and the
    /// target value.
    /// </summary>
    private Vector2 _animationPosition;

    /// <summary>
    /// The <see cref="Color"/> used to show the progress bar animation.
    /// </summary>
    private Color _animationShade;

    public ProgressBarAnimated(Texture2D background, Texture2D foreground, float maxValue, Vector2 position)
        : base(background, foreground, maxValue, position)
    {
        _targetValue = maxValue;
        _animationVisibleRect = new Rectangle(foreground.Width, 0, 0, foreground.Height);
        _animationPosition = position;
        _animationShade = Color.DarkGray;
    }

    /// <summary>
    /// Update the progress bar.
    /// </summary>
    /// <param name="newValue">The new value to assign as the current value of the progress bar.</param>
    /// <param name="totalSecondsElapsed">The number of seconds elapsed since the
    /// last call to the <c>Update</c> method.</param>
    public override void Update(float newValue, float totalSecondsElapsed)
    {
        if (newValue == _currentValue)
        {
            return;
        }

        _targetValue = newValue;
        int x;

        if (_targetValue < _currentValue)
        {
            _currentValue -= _animationSpeed * totalSecondsElapsed;
            if (_currentValue < _targetValue)
            {
                _currentValue = _targetValue;
            }

            x = (int)(_targetValue / _maxValue * _foreground.Width);
            _animationShade = Color.Gray;
        }
        else
        {
            _currentValue += _animationSpeed * totalSecondsElapsed;
            if (_currentValue > _targetValue)
            {
                _currentValue = _targetValue;
            }

            x = (int)(_currentValue / _maxValue * _foreground.Width);
            _animationShade = Color.DarkGray * 0.5f;
        }

        _visibleRect.Width = x;
        _animationVisibleRect.X = x;
        _animationVisibleRect.Width = (int)(Math.Abs(_currentValue - _targetValue) / _maxValue * _foreground.Width);
        _animationPosition.X = _position.X + x;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // We start by drawing the progress bar.
        base.Draw(spriteBatch);

        // And then we draw the animated part.
        spriteBatch.Draw(_foreground, _animationPosition, _animationVisibleRect, _animationShade);
    }
}
