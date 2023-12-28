namespace Shooter_DX;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Represents a parallaxing background.
/// </summary>
internal class ParallaxingBackgrounds
{
    /// <summary>
    /// The image representing the parallaxing background.
    /// </summary>
    private Texture2D texture;

    /// <summary>
    /// An array of positions of the parallaxing background.
    /// </summary>
    private Vector2[] positions;

    /// <summary>
    /// The speed at wich the background is moving.
    /// </summary>
    private int speed;

    /// <summary>
    /// Initializes the background and load the required texture.
    /// </summary>
    /// <param name="content">The content manager used to load the texture.</param>
    /// <param name="texturePath">The relative path to the texture.</param>
    /// <param name="screenWidth">The screen width in pixels.</param>
    /// <param name="speed">The speed in pixels at which the background will scroll.</param>
    public void Initialize(ContentManager content, string texturePath, int screenWidth, int speed)
    {
        texture = content.Load<Texture2D>(texturePath);
        this.speed = speed;

        // If we divide the screen width by texture width, then we can determine the number of tiles needed.
        // We add 1 to the result so that we won't have a gap in the tiling.
        positions = new Vector2[(screenWidth / texture.Width) + 1];

        // Set the initial positions of the parallaxing background
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new Vector2(i * texture.Width, 0);
        }
    }

    /// <summary>
    /// Update the position of the background.
    /// </summary>
    public void Update()
    {
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i].X += speed;

            // If the speed has the background moving to the left
            if (speed <= 0)
            {
                // Check the texture is out of view then put that texture at the end of the screen
                if (positions[i].X <= -texture.Width)
                {
                    positions[i].X = texture.Width * (positions.Length - 1);
                }
            }

            // If the speed has the background moving to the right
            else
            {
                // Check if the texture is out of view then position it to the start of the screen
                if (positions[i].X >= texture.Width * (positions.Length - 1))
                {
                    positions[i].X = -texture.Width;
                }
            }
        }
    }

    /// <summary>
    /// Draw the background.
    /// </summary>
    /// <param name="spriteBatch">The spriteBatch used to draw the background.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            spriteBatch.Draw(texture, positions[i], Color.White);
        }
    }
}
