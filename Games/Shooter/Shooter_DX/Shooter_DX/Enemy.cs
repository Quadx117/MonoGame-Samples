namespace Shooter_DX;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Represents an enemy.
/// </summary>
internal class Enemy
{
    /// <summary>
    /// The position of the enemy ship relative to the top left corner of the screen.
    /// </summary>
    private Vector2 position;

    /// <summary>
    /// The speed at which the enemy moves.
    /// </summary>
    private float speed;

    /// <summary>
    /// Gets the position of the enemy ship relative to the top left corner of the screen.
    /// </summary>
    public Vector2 Position => position;

    /// <summary>
    /// Gets or sets the hit points of the enemy. If it goes to zero the enemy dies.
    /// </summary>
    public int Health { get; set; }

    /// <summary>
    /// Gets the animation representing the enemy.
    /// </summary>
    public Animation EnemyAnimation { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the enemy ship is active or not.
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// Gets the amount of damage the enemy inflicts on the player ship.
    /// </summary>
    public int Damage { get; private set; }

    /// <summary>
    /// Gets the amount of score the enemy will give to the player when destroyed.
    /// </summary>
    public int Value { get; private set; }

    /// <summary>
    /// Gets the width of the enemy ship.
    /// </summary>
    public int Width => EnemyAnimation.FrameWidth;

    /// <summary>
    /// Gets the height of the enemy ship.
    /// </summary>
    public int Height => EnemyAnimation.FrameHeight;

    /// <summary>
    /// Initializes the enemy ship with full heatlh and in the active state.
    /// </summary>
    /// <param name="animation">The animation used to draw the enemy ship.</param>
    /// <param name="position">The starting position of the enemy ship.</param>
    public void Initialize(Animation animation, Vector2 position)
    {
        EnemyAnimation = animation;
        this.position = position;

        // We initialize the enemy to be active so it will update in the game
        Active = true;
        Health = 10;
        Damage = 10;
        Value = 100;
        speed = 6f;
    }

    /// <summary>
    /// Updates the enemy.
    /// </summary>
    /// <param name="gameTime">The elapsed time since the last call to <see cref="Update(GameTime)"/>.</param>
    public void Update(GameTime gameTime)
    {
        // The enemy always moves to the left, so decrement it's X position
        position.X -= speed;

        // Update the position of the Animation
        EnemyAnimation.Position = Position;

        // Update Animation
        EnemyAnimation.Update(gameTime);

        Active &= Position.X >= -Width &&
                  Health > 0;
    }

    /// <summary>
    /// Draws the enemy.
    /// </summary>
    /// <param name="spriteBatch">The spriteBatch used to draw the enemy.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        EnemyAnimation.Draw(spriteBatch);
    }
}
