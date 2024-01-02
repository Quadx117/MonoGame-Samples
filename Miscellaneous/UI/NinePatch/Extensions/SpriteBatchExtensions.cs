namespace NinePatch.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public static class SpriteBatchExtensions
{
    /// <summary>
    /// This method draws a texture as a series of 9 rectangles, scaling the appropriate
    /// sections accordingly. Horizontal scaling is only applied to the top and bottom
    /// rectangles, vertical scaling to the left and right rectangles, and the center
    /// rectangle is scaled in both directions as needed. The four corners are drawn
    /// without any scaling to keep them from being stretched or blurred.
    /// </summary>
    /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance used to draw the texture.</param>
    /// <param name="texture">The texture to draw.</param>
    /// <param name="destinationRectangle">The coordinates where to draw the texture on the screen.</param>
    /// <param name="border">The distance in pixels from the edges of the texture to the "middle" patch.
    /// This assumes that all 4 corner patches have the same width and height.</param>
    /// <param name="color">A <see cref="Color"/> used to tint the texture.</param>
    public static void DrawNinePatchRect(this SpriteBatch spriteBatch, Texture2D texture,
                                         Rectangle destinationRectangle, int border, Color color)
    {
        Point rectSizeCorner = new(border);

        // Top left, no scaling
        spriteBatch.Draw(
            texture,
            new Rectangle(destinationRectangle.Location,
                          rectSizeCorner),
            new Rectangle(0, 0, border, border),
            color);

        // Top, scaled horizontally if needed
        spriteBatch.Draw(
            texture,
            new Rectangle(destinationRectangle.Location + new Point(border, 0),
                          new Point(destinationRectangle.Width - (border * 2), border)),
            new Rectangle(border, 0, texture.Width - (border * 2), border),
            color);

        // Top right, no scaling
        spriteBatch.Draw(
            texture,
            new Rectangle(destinationRectangle.Location + new Point(destinationRectangle.Width - border, 0),
                          rectSizeCorner),
            new Rectangle(texture.Width - border, 0, border, border),
            color);

        // Middle left, scaled vertically if needed
        spriteBatch.Draw(
            texture,
            new Rectangle(destinationRectangle.Location + new Point(0, border),
                          new Point(border, destinationRectangle.Height - (border * 2))),
            new Rectangle(0, border, border, texture.Height - (border * 2)),
            color);

        // Middle, scaled in both directions if needed
        spriteBatch.Draw(
            texture,
            new Rectangle(destinationRectangle.Location + rectSizeCorner,
                          destinationRectangle.Size - new Point(border * 2)),
            new Rectangle(border, border, texture.Width - (border * 2), texture.Height - (border * 2)),
            color);

        // Middle right, scaled vertically if needed
        spriteBatch.Draw(
            texture,
            new Rectangle(destinationRectangle.Location + new Point(destinationRectangle.Width - border, border),
                          new Point(border, destinationRectangle.Height - (border * 2))),
            new Rectangle(texture.Width - border, border, border, texture.Height - (border * 2)),
            color);

        // Bottom left, no scaling
        spriteBatch.Draw(
            texture,
            new Rectangle(destinationRectangle.Location + new Point(0, destinationRectangle.Height - border),
                          rectSizeCorner),
            new Rectangle(0, texture.Height - border, border, border),
            color);

        // Bottom, scaled horizontally if needed
        spriteBatch.Draw(
            texture,
            new Rectangle(destinationRectangle.Location + new Point(border, destinationRectangle.Height - border),
                          new Point(destinationRectangle.Width - (border * 2), border)),
            new Rectangle(border, texture.Height - border, texture.Width - (border * 2), border),
            color);

        // Bottom right, no scaling
        spriteBatch.Draw(
            texture,
            new Rectangle(destinationRectangle.Location + destinationRectangle.Size - rectSizeCorner,
                          rectSizeCorner),
            new Rectangle(texture.Width - border, texture.Height - border, border, border),
            color);
    }
}
