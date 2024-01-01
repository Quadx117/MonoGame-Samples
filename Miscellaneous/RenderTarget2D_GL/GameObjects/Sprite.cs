namespace RenderTarget2D_GL.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal class Sprite
{
    public Vector2 Position { get; set; }

    protected Texture2D _texture;

    public Sprite(Texture2D texture, Vector2 position)
    {
        _texture = texture;
        Position = position;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Color.White);
    }
}
