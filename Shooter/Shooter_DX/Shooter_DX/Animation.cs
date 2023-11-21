namespace Shooter_DX;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Handles spritesheet animations.
/// </summary>
internal class Animation
{
    /// <summary>
    /// The texture representing the collection of images used for animation.
    /// </summary>
    private Texture2D spriteStrip;

    /// <summary>
    /// The scale used to display the sprite strip.
    /// </summary>
    private float scale;

    /// <summary>
    /// The time since we last updated the frame.
    /// </summary>
    private int elapsedTime;

    /// <summary>
    /// The time we display a frame until the next one.
    /// </summary>
    private int frameTime;

    /// <summary>
    /// The number of frames that the animation contains.
    /// </summary>
    private int frameCount;

    /// <summary>
    /// The index of the current frame we are displaying.
    /// </summary>
    private int currentFrame;

    /// <summary>
    /// The color of the frame we will be displaying (i.e. tint color).
    /// </summary>
    private Color color;

    /// <summary>
    /// The area of the image strip we want to display.
    /// </summary>
    private Rectangle sourceRect = new();

    /// <summary>
    /// The area where we want to display the image from the strip in the game.
    /// </summary>
    private Rectangle destinationRect = new();

    /// <summary>
    /// Gets or sets the position of a given frame on the screen.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Gets the width of a given frame.
    /// </summary>
    public int FrameWidth { get; private set; }

    /// <summary>
    /// Gets the height of a given frame.
    /// </summary>
    public int FrameHeight { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the animation is active or not.
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the animation will keep playing or deactivate after one run.
    /// </summary>
    public bool Looping { get; private set; }

    /// <summary>
    /// Initializes an existing instance of this class.
    /// </summary>
    /// <param name="texture">The texture representing the collection of images used for animation.</param>
    /// <param name="position">The position where a given frame on the screen should be drawn.</param>
    /// <param name="frameWidth">The width of a given frame.</param>
    /// <param name="frameHeight">The height of a given frame.</param>
    /// <param name="frameCount">The number of frames that the animation contains.</param>
    /// <param name="frameTime">The time we display a frame until the next one.</param>
    /// <param name="tintColor">The color used to tint each frame.</param>
    /// <param name="scale">The scale used to display the sprite strip.</param>
    /// <param name="looping">Whether the animation will keep playing or deactivate after one run.</param>
    public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount,
                           int frameTime, Color tintColor, float scale, bool looping)
    {
        color = tintColor;
        FrameWidth = frameWidth;
        FrameHeight = frameHeight;
        this.frameCount = frameCount;
        this.frameTime = frameTime;
        this.scale = scale;

        Looping = looping;
        Position = position;
        spriteStrip = texture;

        elapsedTime = 0;
        currentFrame = 0;

        // Set the animation to active by default
        Active = true;
    }

    /// <summary>
    /// Updates the animation.
    /// </summary>
    /// <param name="gameTime">The elapsed time since the last call to <see cref="Update(GameTime)"/>.</param>
    public void Update(GameTime gameTime)
    {
        if (!Active)
        {
            return;
        }

        // Update the elapsed time
        elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

        if (elapsedTime > frameTime)
        {
            currentFrame++;

            if (currentFrame == frameCount)
            {
                currentFrame = 0;
                if (!Looping)
                {
                    Active = false;
                }
            }

            elapsedTime = 0;
        }

        // Grab the correct frame in the image strip by multiplying the currentFrame index by the FrameWidth
        sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

        // Where we want to draw our image from the strip on the screen.  Because all the images are not necessarilly the
        // same size, we will put the center of the image on the top left corner of the rectangle. That way if we put an
        // explosion in place of a destroyed ship it wil appear at the same place even if the two images are not the same size.
        destinationRect = new Rectangle(
            (int)Position.X - ((int)(FrameWidth * scale) / 2),
            (int)Position.Y - ((int)(FrameHeight * scale) / 2),
            (int)(FrameWidth * scale),
            (int)(FrameHeight * scale));
    }

    /// <summary>
    /// Draws the current animation frame.
    /// </summary>
    /// <param name="spriteBatch">The spriteBatch used to draw the animation.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        if (Active)
        {
            spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
        }
    }
}
