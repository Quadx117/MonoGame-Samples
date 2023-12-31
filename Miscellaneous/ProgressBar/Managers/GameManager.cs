namespace ProgressBar.Managers;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProgressBar.GameObjects;

internal class GameManager
{
    private readonly Hero _hero;

    public GameManager(ContentManager content)
    {
        _hero = new(content);
    }

    public void Update(float totalSecondsElapsed)
    {
        InputManager.Update();
        _hero.Update(totalSecondsElapsed);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _hero.Draw(spriteBatch);
    }
}
