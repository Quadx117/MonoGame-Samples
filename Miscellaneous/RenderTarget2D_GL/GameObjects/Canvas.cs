namespace RenderTarget2D_GL.GameObjects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

internal class Canvas
{
    private readonly RenderTarget2D _renderTarget;
    private readonly GraphicsDevice _graphicsDevice;
    private Rectangle _destinationRectangle;

    public Canvas(GraphicsDevice graphicsDevice, int width, int height)
    {
        _graphicsDevice = graphicsDevice;
        _renderTarget = new RenderTarget2D(_graphicsDevice, width, height);
    }

    public void SetDestinationRectangle()
    {
        Rectangle screenSize = _graphicsDevice.PresentationParameters.Bounds;

        float scaleX = (float)screenSize.Width / _renderTarget.Width;
        float scaleY = (float)screenSize.Height / _renderTarget.Height;
        float scale = Math.Min(scaleX, scaleY);

        int newWidth = (int)(_renderTarget.Width * scale);
        int newHeight = (int)(_renderTarget.Height * scale);

        int posX = (screenSize.Width - newWidth) / 2;
        int posY = (screenSize.Height - newHeight) / 2;

        _destinationRectangle = new Rectangle(posX, posY, newWidth, newHeight);
    }

    public void Activate()
    {
        _graphicsDevice.SetRenderTarget(_renderTarget);
        _graphicsDevice.Clear(Color.DarkGray);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _graphicsDevice.SetRenderTarget(null);
        _graphicsDevice.Clear(Color.Black);
        spriteBatch.Begin();
        spriteBatch.Draw(_renderTarget, _destinationRectangle, Color.White);
        spriteBatch.End();
    }
}
