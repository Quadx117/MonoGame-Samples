namespace Shooter_DX;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Simulates a projecile in a 2D plane.
/// </summary>
internal class Projectile
{
    /// <summary>
    /// Determine how fast the projectile moves.
    /// </summary>
    private readonly float speed;

    /// <summary>
    /// The position of the projectile relative to the top left corner of the screen.
    /// </summary>
    private Vector2 position;

    /// <summary>
    /// Represents the viewable boundary of the game.
    /// </summary>
    private Viewport viewport;

    /// <summary>
    /// Initializes a new instance of the <see cref="Projectile"/> class and make the projectile active.
    /// </summary>
    /// <param name="viewport">The viewport where the projectile will be drawn.</param>
    /// <param name="texture">The texture used to draw the projectile.</param>
    /// <param name="position">The starting positon of the projectile.</param>
    public Projectile(Viewport viewport, Texture2D texture, Vector2 position)
    {
        Texture = texture;
        this.position = position;
        this.viewport = viewport;

        Active = true;
        Damage = 2;
        speed = 20f;
    }

    /// <summary>
    /// Gets the position of the projectile relative to the top left corner of the screen.
    /// </summary>
    public Vector2 Position => position;

    /// <summary>
    /// Gets the image representing the projectile.
    /// </summary>
    public Texture2D Texture { get; private set; }

    /// <summary>
    /// Gets the amount of damage the projectile infilcts to an enemy.
    /// </summary>
    public int Damage { get; private set; }

    /// <summary>
    /// Gets the width of the projectile.
    /// </summary>
    /// <remarks>
    /// The width is the <see cref="Texture"/>'s width.
    /// </remarks>
    public int Width => Texture.Width;

    /// <summary>
    /// Gets the height of the projectile.
    /// </summary>
    /// <remarks>
    /// The height is the <see cref="Texture"/>'s height.
    /// </remarks>
    public int Height => Texture.Height;

    /// <summary>
    /// Gets or sets a value indicating whether the projectile is active or not.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Update the projectile's position.
    /// </summary>
    public void Update()
    {
        // Projectiles always move to the right
        position.X += speed;

        // TODO(PERE): Why Width / 2
        // Deactivate the bullet if it goes out of screen
        Active &= position.X + (Texture.Width / 2) <= viewport.Width;
    }

    /// <summary>
    /// Draw the projectile to the screen.
    /// </summary>
    /// <param name="spriteBatch">The spriteBatch used to draw the projectile.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        // TODO(PERE): Why origin is in the center of the texture ?
        spriteBatch.Draw(Texture, position, null, Color.White, 0f, new Vector2(Width / 2, Height / 2),
                         1f, SpriteEffects.None, 0f);
    }
}
