//-----------------------------------------------------------------------------
// ScaleGameComponentAnimation.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

namespace CardsFramework;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// An animation which scales a component.
/// </summary>
public class ScaleGameComponentAnimation : AnimatedGameComponentAnimation
{
    float percent = 0;
    readonly float beginFactor;
    readonly float factorDelta;

    /// <summary>
    /// Initializes a new instance of the class.
    /// </summary>
    /// <param name="beginFactor">The initial scale factor.</param>
    /// <param name="endFactor">The eventual scale factor.</param>
    public ScaleGameComponentAnimation(float beginFactor, float endFactor)
    {
        this.beginFactor = beginFactor;
        factorDelta = endFactor - beginFactor;
    }

    /// <summary>
    /// Runs the scaling animation.
    /// </summary>
    /// <param name="gameTime">Game time information.</param>
    public override void Run(GameTime gameTime)
    {
        Texture2D texture;
        if (IsStarted())
        {
            texture = Component.CurrentFrame;
            if (texture != null)
            {
                // Calculate the completion percent of animation
                percent += (float)(gameTime.ElapsedGameTime.TotalSeconds /
                    Duration.TotalSeconds);

                // Inflate the component with an increasing delta. The eventual
                // delta will have the componenet scale to the specified target
                // scaling factor.
                Rectangle bounds = texture.Bounds;
                bounds.X = (int)Component.CurrentPosition.X;
                bounds.Y = (int)Component.CurrentPosition.Y;
                float currentFactor = beginFactor + (factorDelta * percent) - 1;
                bounds.Inflate((int)(bounds.Width * currentFactor),
                    (int)(bounds.Height * currentFactor));
                Component.CurrentDestination = bounds;
            }
        }
    }
}
