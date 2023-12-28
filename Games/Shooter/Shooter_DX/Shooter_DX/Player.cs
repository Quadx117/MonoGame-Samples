namespace Shooter_DX;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Represents the player's ship.
/// </summary>
internal class Player
{
    /// <summary>
    /// Gets or sets the position of the player relative to the top left corner of the screen.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1401:Fields should be private",
        Justification = "It would not be possible to modify the value of a member of the vector otherwise")]
    public Vector2 Position;

    /// <summary>
    /// Gets or sets a value indicating whether the player is active or not.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Gets or sets the amount of hit points (HP) that the player has.
    /// </summary>
    public int Health { get; set; }

    /// <summary>
    /// Gets the animation representing the player.
    /// </summary>
    public Animation PlayerAnimation { get; private set; }

    /// <summary>
    /// Gets the width of the player ship.
    /// </summary>
    /// <remarks>
    /// The width is the <see cref="Texture"/>'s width.
    /// </remarks>
    public int Width => PlayerAnimation.FrameWidth;

    /// <summary>
    /// Gets the height of the player ship.
    /// </summary>
    /// <remarks>
    /// The height is the <see cref="Texture"/>'s height.
    /// </remarks>
    public int Height => PlayerAnimation.FrameHeight;

    /// <summary>
    /// Initializes the player with full heatlh and in the active state.
    /// </summary>
    /// <param name="animation">The animation used to draw the player.</param>
    /// <param name="position">The initial position of the player.</param>
    public void Initialize(Animation animation, Vector2 position)
    {
        PlayerAnimation = animation;
        Position = position;

        Active = true;
        Health = 100;
    }

    /// <summary>
    /// Updates the player.
    /// </summary>
    /// <param name="gameTime">The elapsed time since the last call to <see cref="Update(GameTime)"/>.</param>
    public void Update(GameTime gameTime)
    {
        PlayerAnimation.Position = Position;
        PlayerAnimation.Update(gameTime);
    }

    /// <summary>
    /// Draws the player.
    /// </summary>
    /// <param name="spriteBatch">The spriteBatch used to draw the player.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        PlayerAnimation.Draw(spriteBatch);
    }
}
