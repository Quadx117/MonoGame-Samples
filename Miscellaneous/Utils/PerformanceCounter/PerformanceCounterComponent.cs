namespace PerformanceCounter;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class PerformanceCounterComponent : DrawableGameComponent
{
    private readonly TimeSpan performanceRefreshDelay = TimeSpan.FromSeconds(1);

    private SpriteBatch spriteBatch;
    private SpriteFont spriteFont;

    private int frameRate = 0;
    private int frameCounter = 0;
    private int updateRate = 0;
    private int updateCounter = 0;
    private TimeSpan elapsedTime = TimeSpan.Zero;

    public PerformanceCounterComponent(Game game)
        : base(game)
    {
        // NOP
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteFont = Game.Content.Load<SpriteFont>("Fonts/gameFont");
    }

    public override void Update(GameTime gameTime)
    {
        elapsedTime += gameTime.ElapsedGameTime;
        updateCounter++;

        if (elapsedTime > performanceRefreshDelay)
        {
            elapsedTime -= performanceRefreshDelay;

            frameRate = frameCounter;
            frameCounter = 0;

            updateRate = updateCounter;
            updateCounter = 0;

            base.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        frameCounter++;

        string fps = $"FPS : {frameRate}";
        string ups = $"UPS : {updateRate}";

        spriteBatch.Begin();

        spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 33), Color.Black);
        spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 32), Color.White);
        spriteBatch.DrawString(spriteFont, ups, new Vector2(33, 63), Color.Black);
        spriteBatch.DrawString(spriteFont, ups, new Vector2(32, 62), Color.White);

        spriteBatch.End();

        base.Draw(gameTime);
    }
}
