namespace RenderTarget2D_GL.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RenderTarget2D_GL.GameObjects;

internal class GameManager
{
    private readonly Game _game;
    private readonly GraphicsDeviceManager _graphics;
    private readonly Sprite _sprite;
    private readonly Canvas _canvas;

    private Point _lastResolution;

    public GameManager(Game game, GraphicsDeviceManager graphics)
    {
        _game = game;
        _graphics = graphics;
        _canvas = new Canvas(_graphics.GraphicsDevice, 400, 300);
        _sprite = new Sprite(_game.Content.Load<Texture2D>(@"Images\screen"), new Vector2(0, 0));
        SetResolution(400, 300);
    }

    private void SetResolution(int width, int height)
    {
        _graphics.PreferredBackBufferWidth = width;
        _graphics.PreferredBackBufferHeight = height;
        _game.Window.IsBorderless = false;
        _graphics.ApplyChanges();
        _canvas.SetDestinationRectangle();
    }

    /// <summary>
    /// This method sets the game window to be full screen by setting IsBorderless
    /// to true and setting the back buffer to the full screen width and height.
    /// With the OpenGL backend, switching out of borderless fullscreen resets
    /// the window position in the center of the screen and switching to borderless
    /// resets the position to the top left of the screen. This is not the case with
    /// the WindowsDX backend, which means we need to reset it ourselves in both cases.
    /// </summary>
    /// <remarks>
    /// Fullscreen gives the game exclusive control of the monitor and may involve
    /// changing the screen resolution to match the application's requirements.
    /// This may result in desktop icons begin relocated if the game resolution
    /// is smaller than the desktop resolution, but has the potential to boost
    /// performance when compared to borderless windowed mode. Also, the mouse
    /// cursor remains locked to whichever screen is displaying the game. To
    /// navigate out of the game, the player would need to use the Alt+Tab shortcut.
    ///
    /// Borderless runs the game in a borderless window that covers the entire screen,
    /// giving the illusion of fullscreen while allowing the user to quickly switch to
    /// other programs or move the mouse seamlessly from one monitor to another.
    /// </remarks>
    private void SetBorderlessFullScreen()
    {
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _game.Window.IsBorderless = true;
        _graphics.ApplyChanges();
        _canvas.SetDestinationRectangle();
    }

    /// <summary>
    /// This method switches between exclusive fullscreen and windowed mode. We use
    /// "HardwareModeSwitch = false" in this demo since it is faster to switch.
    /// </summary>
    private void ToggleFullScreen()
    {
        if (!_graphics.IsFullScreen)
        {
            _lastResolution = new Point(_graphics.PreferredBackBufferWidth,
                                        _graphics.PreferredBackBufferHeight);
        }

        _graphics.IsFullScreen = !_graphics.IsFullScreen;

        // "Hard" mode (true) is slow to switch, but more effecient for performance,
        // while "soft" mode (false) is vice versa.
        _graphics.HardwareModeSwitch = false;

        if (_graphics.IsFullScreen)
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }
        else
        {
            _graphics.PreferredBackBufferWidth = _lastResolution.X;
            _graphics.PreferredBackBufferHeight = _lastResolution.Y;
        }

        _graphics.ApplyChanges();
        _canvas.SetDestinationRectangle();
    }

    public void Update()
    {
        InputManager.Update();
        if (InputManager.IsKeyPressed(Keys.F1))
        {
            SetResolution(400, 300);
        }

        if (InputManager.IsKeyPressed(Keys.F2))
        {
            SetResolution(1280, 720);
        }

        if (InputManager.IsKeyPressed(Keys.F3))
        {
            SetResolution(640, 720);
        }

        if (InputManager.IsKeyPressed(Keys.F4))
        {
            SetBorderlessFullScreen();
        }

        if (InputManager.IsKeyPressed(Keys.F5))
        {
            ToggleFullScreen();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _canvas.Activate();

        spriteBatch.Begin();
        _sprite.Draw(spriteBatch);
        spriteBatch.End();

        _canvas.Draw(spriteBatch);
    }
}
