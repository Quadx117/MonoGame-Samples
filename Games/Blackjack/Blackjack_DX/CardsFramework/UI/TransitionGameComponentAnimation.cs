//-----------------------------------------------------------------------------
// TransitionGameComponentAnimation.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace CardsFramework;

using Microsoft.Xna.Framework;

/// <summary>
/// An animation which moves a component from one point to the other.
/// </summary>
public class TransitionGameComponentAnimation : AnimatedGameComponentAnimation
{
    Vector2 sourcePosition;
    Vector2 positionDelta;
    float percent = 0;
    Vector2 destinationPosition;

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="sourcePosition">The source position.</param>
    /// <param name="destinationPosition">The destination position.</param>
    public TransitionGameComponentAnimation(Vector2 sourcePosition,
        Vector2 destinationPosition)
    {
        this.destinationPosition = destinationPosition;
        this.sourcePosition = sourcePosition;
        positionDelta = destinationPosition - sourcePosition;
    }

    /// <summary>
    /// Runs the transition animation.
    /// </summary>
    /// <param name="gameTime">Game time information.</param>
    public override void Run(GameTime gameTime)
    {
        if (IsStarted())
        {
            // Calculate the animation's completion percentage.
            percent += (float)(gameTime.ElapsedGameTime.TotalSeconds /
                Duration.TotalSeconds);

            // Move the component towards the destination as the animation
            // progresses
            Component.CurrentPosition = sourcePosition + (positionDelta * percent);

            if (IsDone())
            {
                Component.CurrentPosition = destinationPosition;
            }
        }
    }
}
