namespace PixelShader.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal class Sprite
{
    protected readonly Texture2D _texture;

    public Sprite(Texture2D texture)
    {
        _texture = texture;
    }

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 pos)
    {
        spriteBatch.Draw(_texture, pos, Color.White);
    }
}
