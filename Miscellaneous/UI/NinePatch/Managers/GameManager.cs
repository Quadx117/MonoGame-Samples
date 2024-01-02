namespace NinePatch.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NinePatch.Extensions;

internal class GameManager
{
    private readonly Texture2D _ninePatchTexture;

    private readonly SpriteFont _font;

    public GameManager(ContentManager content)
    {
        _ninePatchTexture = content.Load<Texture2D>(@"Images\button_nine_patch");
        _font = content.Load<SpriteFont>(@"Fonts\gameFont");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int width = _ninePatchTexture.Width;
        int height = _ninePatchTexture.Height;
        int doubleWidth = _ninePatchTexture.Width * 2;
        int doubleHeight = _ninePatchTexture.Width * 2;
        int padding = 10;
        int xOrigin = 10;
        int yOrigin = 60;

        //
        // Normal scaling which results in blurry image.
        //

        spriteBatch.DrawString(_font,
                               "Normal scaling",
                               new Vector2(xOrigin + 68, 10),
                               Color.White);

        // Original texture, without any scaling
        spriteBatch.Draw(_ninePatchTexture,
                         new Rectangle(xOrigin, yOrigin,
                                       width, height),
                         Color.White);

        // Scaled horizontally
        spriteBatch.Draw(_ninePatchTexture,
                        new Rectangle(xOrigin + width + padding, yOrigin,
                                      doubleWidth, height),
                        Color.White);

        // Scaled vertically
        spriteBatch.Draw(_ninePatchTexture,
                        new Rectangle(xOrigin, yOrigin + height + padding,
                                      width, doubleHeight),
                        Color.White);

        // Scaled horizontally and vertically
        spriteBatch.Draw(_ninePatchTexture,
                        new Rectangle(xOrigin + width + padding, yOrigin + height + padding,
                                      doubleWidth, doubleHeight),
                        Color.White);

        //
        // Nine patch scaling which produces a pixel perfect result.
        //

        xOrigin += width + padding + doubleWidth + (padding * 5);

        spriteBatch.DrawString(_font,
                               "9-patch scaling",
                               new Vector2(xOrigin + 64, 10),
                               Color.White);

        // Original texture, without any scaling
        spriteBatch.Draw(_ninePatchTexture,
                         new Rectangle(xOrigin, yOrigin,
                                       width, height),
                         Color.White);

        // Nine patch, scaled horizontally
        spriteBatch.DrawNinePatchRect(_ninePatchTexture,
                                      new Rectangle(xOrigin + width + padding, yOrigin,
                                                    doubleWidth, height),
                                      50,
                                      Color.White);

        // Nine patch, scaled vertically
        spriteBatch.DrawNinePatchRect(_ninePatchTexture,
                                      new Rectangle(xOrigin, yOrigin + height + padding,
                                                    width, doubleHeight),
                                      50,
                                      Color.White);

        // Nine patch, scaled horizontally and vertically
        spriteBatch.DrawNinePatchRect(_ninePatchTexture,
                                      new Rectangle(xOrigin + width + padding, yOrigin + height + padding,
                                                    doubleWidth, doubleHeight),
                                      50,
                                      Color.White);
    }
}
