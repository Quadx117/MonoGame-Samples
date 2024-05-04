namespace GameStateManagement;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Sample showing how to manage different game states, with transitions
/// between menu screens, a loading screen, the game itself, and a pause
/// menu. This main game class is extremely simple: all the interesting
/// stuff happens in the ScreenManager component.
/// </summary>
public class GameStateManagementDemo : Game
{
    private readonly ScreenManager _screenManager;

    // By preloading any assets used by UI rendering, we avoid framerate glitches
    // when they suddenly need to be loaded in the middle of a menu transition.
    private static readonly string[] _preloadAssets =
    {
        "Images/gradient",
    };

    public GameStateManagementDemo()
    {
        _ = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = 853,
            PreferredBackBufferHeight = 480
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Create the screen manager component.
        _screenManager = new ScreenManager(this);
        Components.Add(_screenManager);

        // Activate the first screens.
        _screenManager.AddScreen(new BackgroundScreen(), null);
        _screenManager.AddScreen(new MainMenuScreen(), null);
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        foreach (string asset in _preloadAssets)
        {
            Content.Load<object>(asset);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // The real drawing happens inside the screen manager component.
        base.Draw(gameTime);
    }
}
