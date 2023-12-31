namespace ProgressBar.GameObjects;

using global::ProgressBar.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

internal class Hero
{
    private readonly float _maxHealth;
    private float _health;
    private readonly ProgressBar _healthBar;
    private readonly ProgressBarAnimated _healthBarAnimated;

    private readonly SpriteFont _font;
    private readonly Vector2 _textPosition;

    public Hero(ContentManager content, float health = 100f)
    {
        Texture2D back = content.Load<Texture2D>(@"Images\background");
        Texture2D front = content.Load<Texture2D>(@"Images\foreground");
        
        _font = content.Load<SpriteFont>(@"Fonts\gameFont");
        _textPosition = new Vector2(110, 450);

        _maxHealth = health;
        _health = health;
        _healthBar = new ProgressBar(back, front, _maxHealth, new Vector2(100, 100));
        _healthBarAnimated = new ProgressBarAnimated(back, front, _maxHealth, new Vector2(100, 300));
    }

    public void TakeDamage(float dmg)
    {
        _health -= dmg;
        if (_health < 0)
        {
            _health = 0;
        }
    }

    public void Heal(float heal)
    {
        _health += heal;
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public void Update(float totalSecondsElapsed)
    {
        if (InputManager.MouseLeftClicked)
        {
            TakeDamage(10);
        }

        if (InputManager.MouseRightClicked)
        {
            Heal(10);
        }

        _healthBar.Update(_health, totalSecondsElapsed);
        _healthBarAnimated.Update(_health, totalSecondsElapsed);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _healthBar.Draw(spriteBatch);
        _healthBarAnimated.Draw(spriteBatch);

        spriteBatch.DrawString(_font,
                               "Left click to decrease, right click to increase",
                               _textPosition,
                               Color.White);
    }
}
