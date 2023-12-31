namespace PixelShader.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PixelShader.GameObjects;

internal class GameManager
{
    private readonly Sprite _sprite;

    private readonly Vector2 _pos00;
    private readonly Vector2 _pos01;
    private readonly Vector2 _pos02;
    private readonly Vector2 _pos03;
    private readonly Vector2 _pos04;

    private readonly Effect _effect01;
    private readonly Effect _effect02;
    private readonly Effect _effect03;
    private readonly Effect _effect04;

    private float _amount = 1;
    private float _dir = -1;

    public GameManager(ContentManager content)
    {
        _sprite = new Sprite(content.Load<Texture2D>(@"Images\orb-red"));

        _pos00 = new Vector2(100, 100);
        _pos01 = new Vector2(200, 100);
        _pos02 = new Vector2(300, 100);
        _pos03 = new Vector2(400, 100);
        _pos04 = new Vector2(500, 100);

        _effect01 = content.Load<Effect>(@"Shaders\effect01");
        _effect02 = content.Load<Effect>(@"Shaders\effect02");
        _effect03 = content.Load<Effect>(@"Shaders\effect03");
        _effect04 = content.Load<Effect>(@"Shaders\effect04");
    }

    public void Update(float totalSecondsElapsed)
    {
        _amount += totalSecondsElapsed * _dir;
        if (_amount < 0 || _amount > 1)
        {
            _dir *= -1;
        }

        _effect04.Parameters["amount"].SetValue(_amount);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        _sprite.Draw(spriteBatch, _pos00);
        spriteBatch.End();

        spriteBatch.Begin(effect: _effect01);
        _sprite.Draw(spriteBatch, _pos01);
        spriteBatch.End();

        spriteBatch.Begin(effect: _effect02);
        _sprite.Draw(spriteBatch, _pos02);
        spriteBatch.End();

        spriteBatch.Begin(effect: _effect03);
        _sprite.Draw(spriteBatch, _pos03);
        spriteBatch.End();

        spriteBatch.Begin(effect: _effect04);
        _sprite.Draw(spriteBatch, _pos04);
        spriteBatch.End();
    }
}
