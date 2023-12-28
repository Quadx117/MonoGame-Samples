#define DEBUG_BOUNDING_RECTS

namespace Shooter_DX;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

/// <summary>
/// This is the main type for your game.
/// </summary>
public class ShooterGame : Game
{
#if DEBUG_BOUNDING_RECTS
    private Texture2D pixel;
    private bool showBoundingRectangles;
#endif

    private SpriteBatch spriteBatch;

    private Random random;

    private Player player;

    // Keyboard states used to determine key presses
    private KeyboardState currentKeyboardState;
    private KeyboardState previousKeyboardState;

    // Gamepad states used to determine button presses
    private GamePadState currentGamePadState;
    private GamePadState previousGamePadState;

    // TODO(PERE): Could be inside the Player class if we passed the inputs
    // to update the position from within the class.
    // A movement speed for the Player
    private float playerMoveSpeed;

    // Image used to display the static background
    private Texture2D mainBackground;

    // Parallaxing Layers
    private ParallaxingBackgrounds bgLayer1;
    private ParallaxingBackgrounds bgLayer2;

    // Enemies
    private Texture2D enemyTexture;
    private List<Enemy> enemies;

    // The rate at which the enemies appear
    private TimeSpan enemySpawnTime;
    private TimeSpan previousSpawnTime;

    // TODO(PERE): Moving this inside the Player class is more involved because
    // to the collision between projectiles and enemies. One simple way could be
    // to pass the list of projectiles to which to add if a projectile is fired.
    // Projectiles
    private Texture2D projectileTexture;
    private List<Projectile> projectiles;

    // The rate of fire of the Player's Laser
    private TimeSpan fireTime;
    private TimeSpan previousFireTime;

    // Explosion
    private Texture2D explosionTexture;
    private List<Animation> explosions;

    // The sound played when a laser is fired
    private SoundEffect laserSound;

    // The sound used for explosions
    private SoundEffect explosionSound;

    // Low Beep (Used in menu)
    private SoundEffect lowBeep;

    // The music played during Gameplay and Menus
    private Song gameplayMusic;
    private Song menuMusic;

    // Variables to check which song is playing
    private bool isPlayingMenuMusic;
    private bool isPlayingGameMusic;

    // Number that holds the player score
    private int score;

    // Background textures for the various screens in the game
    private Texture2D mainMenuScreenBackground;
    private Texture2D endMenuScreenBackground;

    // The current screen state
    private ScreenState currentScreen;

    // Font resources
    private Vector2 fontPos;
    private Vector2 fontOrigin;
    private SpriteFont gamefont;  // The font used to display UI elements
    private SpriteFont menufont;  // The font used in menus
    private Color playColor;
    private Color quitColor;
    private int elapsedTimeColor; // The time elapsed since the last fontColor change
    private int menuIndex;        // 0 means we are on Play; 1 means we are on Quit
    private float fontAlphaBlend; // Value for transparency.  0 = transparent ; 0.5 = 50% ; 1 = opaque;  BlendState.AlphaBlend must be set in spritebatch.begin

    // The elapsed time since the player died
    private int elapsedTimeDead;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShooterGame"/> class.
    /// </summary>
    public ShooterGame()
    {
        _ = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
        // Initialize the Player class
        player = new Player();

        // Set a constant player move speed
        playerMoveSpeed = 8.0f;

        // Initialize the enemies list
        enemies = new List<Enemy>();

        // Set the time keepers to 0
        previousSpawnTime = TimeSpan.Zero;

        // Used to determine how fast enemy respawns
        enemySpawnTime = TimeSpan.FromSeconds(1.0f);

        // Initialize our random number generator
        random = new Random();

        // Initalize the ParallaxingBackgrounds
        bgLayer1 = new ParallaxingBackgrounds();
        bgLayer2 = new ParallaxingBackgrounds();

        // Initialize the Projectiles list
        projectiles = new List<Projectile>();

        // Set the Laser to fire every quarter second
        fireTime = TimeSpan.FromSeconds(.15f);

        // Initialze the explosions list
        explosions = new List<Animation>();

        // Set player's score to 0
        score = 0;

        // Initialize the current screen state to the screen we want to display first
        currentScreen = ScreenState.MainMenu;

        // Initialize the Font position to be in the center of the screen
        fontPos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

        // Initialize our bool so that MenuMusic will play first
        isPlayingGameMusic = false;
        isPlayingMenuMusic = true;

        // Set the elapsedTimeColor and elapsedTimeDead to 0
        elapsedTimeColor = 0;
        elapsedTimeDead = 0;

        // Set the default color for the buttons in the main Menu
        playColor = Color.OrangeRed;
        quitColor = Color.White;
        menuIndex = 0;

        base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
#if DEBUG_BOUNDING_RECTS
        // Used to draw the Bounding Rectangles. Can be used to draw any primitives
        pixel = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        pixel.SetData(new[] { Color.White }); // So we can draw whatever color we want
#endif

        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load the Player resources
        Texture2D playerTexture = Content.Load<Texture2D>("Images/shipAnimation");
        Animation playerAnimation = new();
        playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 60, Color.White, 1f, true);

        Vector2 playerPosition = new(GraphicsDevice.Viewport.TitleSafeArea.X,
                                     GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.TitleSafeArea.Height / 2));
        player.Initialize(playerAnimation, playerPosition);

        // Load the Background images
        mainBackground = Content.Load<Texture2D>("Images/mainbackground");
        bgLayer1.Initialize(Content, "Images/bgLayer1", GraphicsDevice.Viewport.Width, -1);
        bgLayer2.Initialize(Content, "Images/bgLayer2", GraphicsDevice.Viewport.Width, -2);
        mainMenuScreenBackground = Content.Load<Texture2D>("Images/mainMenu");
        endMenuScreenBackground = Content.Load<Texture2D>("Images/endMenu");

        // Load the enemy resources
        enemyTexture = Content.Load<Texture2D>("Images/mineAnimation");

        // Load the projectiles resources
        projectileTexture = Content.Load<Texture2D>("Images/laser");

        // Load the explosions resources
        explosionTexture = Content.Load<Texture2D>("Images/explosion");

        // Load the Laser, Explosion and LowBeep sound effect
        laserSound = Content.Load<SoundEffect>("Sound/laserFire");
        explosionSound = Content.Load<SoundEffect>("Sound/explosion");
        lowBeep = Content.Load<SoundEffect>("Sound/LowBeep");

        // Load the music
        gameplayMusic = Content.Load<Song>("Sound/gameMusic");
        menuMusic = Content.Load<Song>("Sound/menuMusic");

        // Start the music right away
        PlayMusic(menuMusic);

        // Load the UI font
        gamefont = Content.Load<SpriteFont>("Fonts/gameFont");
        menufont = Content.Load<SpriteFont>("Fonts/menuFont");
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
        // NOTE(PERE): Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">The elapsed time since the last call to <see cref="Update(GameTime)"/>.</param>
    protected override void Update(GameTime gameTime)
    {
        // Save the previous state of the Keyboard and GamePad so we can determine
        // single key/button presses
        previousGamePadState = currentGamePadState;
        previousKeyboardState = currentKeyboardState;

        // Read the current state of the Keyboard and GamePad and store it
        currentGamePadState = GamePad.GetState(PlayerIndex.One);
        currentKeyboardState = Keyboard.GetState();

        // Update mehtod associated with the current screen
        switch (currentScreen)
        {
            case ScreenState.MainMenu:
            {
                UpdateMainMenu(gameTime);
                if (!isPlayingMenuMusic)
                {
                    isPlayingMenuMusic = true;
                    isPlayingGameMusic = false;
                    PlayMusic(menuMusic);
                }

                break;
            }

            case ScreenState.MainGame:
            {
                elapsedTimeColor = 0;
                UpdateMainGame(gameTime);
                if (!isPlayingGameMusic)
                {
                    isPlayingMenuMusic = false;
                    isPlayingGameMusic = true;
                    PlayMusic(gameplayMusic);
                }

                break;
            }

            case ScreenState.EndMenu:
            {
                elapsedTimeColor = 0;
                UpdateEndMenu();
                if (!isPlayingMenuMusic)
                {
                    isPlayingMenuMusic = true;
                    isPlayingGameMusic = false;
                    PlayMusic(menuMusic);
                }

                break;
            }
        }

        base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Start drawing
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

        // Call the Draw method associated with the current screen
        switch (currentScreen)
        {
            case ScreenState.MainMenu:
            {
                DrawMainMenu();
                break;
            }

            case ScreenState.MainGame:
            {
                DrawMainGame();
                break;
            }

            case ScreenState.EndMenu:
            {
                DrawEndMenu();
                break;
            }
        }

        // Stop drawing
        spriteBatch.End();

        base.Draw(gameTime);
    }

    private static void PlayMusic(Song song)
    {
        // Due to the way the MediaPlayer plays music, we have to catch the exception.
        // Music will play when the game is not tethered
        try
        {
            // Play the music
            MediaPlayer.Play(song);

            // Loop the currently playing song
            MediaPlayer.IsRepeating = true;
        }
        catch
        {
            // If the song cannot be played, there is nothing the user can do about it
            // so we have nothing to do here.
        }
    }

    private void StartNewGame()
    {
        // Reset everything we need to start a new game
        player = new Player();
        enemies = new List<Enemy>();
        projectiles = new List<Projectile>();
        Animation playerAnimation = new();

        Texture2D playerTexture = Content.Load<Texture2D>("Images/shipAnimation");
        playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 60, Color.White, 1f, true);
        Vector2 playerPosition = new(GraphicsDevice.Viewport.TitleSafeArea.X,
                                     GraphicsDevice.Viewport.TitleSafeArea.Y + (GraphicsDevice.Viewport.TitleSafeArea.Height / 2));
        player.Initialize(playerAnimation, playerPosition);

        score = 0;

        // Set the time keepers to 0
        previousSpawnTime = TimeSpan.Zero;

        // Set the elapsedTimeColor and elapsedTimeDead to 0
        elapsedTimeColor = 0;
        elapsedTimeDead = 0;
    }

    private void UpdateMainMenu(GameTime gameTime)
    {
        if (menuIndex != 1 &&
            ((previousKeyboardState != currentKeyboardState && currentKeyboardState.IsKeyDown(Keys.Down)) ||
             (previousGamePadState != currentGamePadState && currentGamePadState.DPad.Down == ButtonState.Pressed)))
        {
            menuIndex = 1;
            elapsedTimeColor = 0;
            playColor = Color.White;
            quitColor = Color.OrangeRed;

            // Play the LowBeep sound effect
            _ = lowBeep.Play(0.7f, 0.0f, 0.0f);
        }

        if (menuIndex != 0 &&
            ((previousKeyboardState != currentKeyboardState && currentKeyboardState.IsKeyDown(Keys.Up)) ||
             (previousGamePadState != currentGamePadState && currentGamePadState.DPad.Up == ButtonState.Pressed)))
        {
            menuIndex = 0;
            elapsedTimeColor = 0;
            playColor = Color.OrangeRed;
            quitColor = Color.White;

            // Play the LowBeep sound effect
            _ = lowBeep.Play(0.7f, 0.0f, 0.0f);
        }

        if ((previousKeyboardState != currentKeyboardState && currentKeyboardState.IsKeyDown(Keys.Enter)) ||
             (previousGamePadState != currentGamePadState && currentGamePadState.Buttons.A == ButtonState.Pressed))
        {
            switch (menuIndex)
            {
                case 0:
                    currentScreen = ScreenState.MainGame;
                    break;
                case 1:
                    Exit();
                    break;
            }
        }

        // Update the elapsedTimeColor
        elapsedTimeColor += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

        if (elapsedTimeColor >= 500f)
        {
            elapsedTimeColor = 0;

            // NOTE(PERE): We make the active menu item blink, since it can be hard
            // to know which item is selected when there is only 2 options.
            if (menuIndex == 0)
            {
                playColor = playColor == Color.White ? Color.OrangeRed : Color.White;
            }
            else if (menuIndex == 1)
            {
                quitColor = quitColor == Color.White ? Color.OrangeRed : Color.White;
            }
        }
    }

    private void UpdateMainGame(GameTime gameTime)
    {
        // Allow the game to return to the main menu
        if (previousKeyboardState != currentKeyboardState && currentKeyboardState.IsKeyDown(Keys.Escape))
        {
            currentScreen = ScreenState.MainMenu;
            return;
        }

#if DEBUG_BOUNDING_RECTS
        if (previousKeyboardState != currentKeyboardState && currentKeyboardState.IsKeyDown(Keys.F5))
        {
            showBoundingRectangles = !showBoundingRectangles;
        }
#endif

        // Update the Player
        UpdatePlayer(gameTime);

        // Update the Enemies
        UpdateEnemies(gameTime);

        // Update the parallaxing background
        bgLayer1.Update();
        bgLayer2.Update();

        // Update Collision only if player is alive
        if (player.Active)
        {
            UpdateCollision();
        }

        // Update Projectiles
        UpdateProjectiles();

        // Update Explosions
        UpdateExplosions(gameTime);
    }

    private void UpdateEndMenu()
    {
        // Test if any key is pressed.
        // If a key is pressed .Length = 1 ; if two keys are pressed simultaneously .Length = 2, etc.
        if (previousKeyboardState != currentKeyboardState && currentKeyboardState.GetPressedKeys().Length > 0)
        {
            StartNewGame();
            currentScreen = ScreenState.MainMenu;
        }
    }

    private void UpdatePlayer(GameTime gameTime)
    {
        player.Update(gameTime);

        // Get Thumbstick Controls
        player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
        player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

        // Use the Keyboard / Dpad
        if (currentKeyboardState.IsKeyDown(Keys.Left) || currentGamePadState.DPad.Left == ButtonState.Pressed)
        {
            player.Position.X -= playerMoveSpeed;
        }

        if (currentKeyboardState.IsKeyDown(Keys.Right) || currentGamePadState.DPad.Right == ButtonState.Pressed)
        {
            player.Position.X += playerMoveSpeed;
        }

        if (currentKeyboardState.IsKeyDown(Keys.Up) || currentGamePadState.DPad.Up == ButtonState.Pressed)
        {
            player.Position.Y -= playerMoveSpeed;
        }

        if (currentKeyboardState.IsKeyDown(Keys.Down) || currentGamePadState.DPad.Down == ButtonState.Pressed)
        {
            player.Position.Y += playerMoveSpeed;
        }

        // Make sure the player does not go out of bounds. Because we've put the
        // center of the image of our ship over the top left corner (0, 0) of the
        // destinationRect, we have to set our clamping min. and max. value accordingly.
        player.Position.X = MathHelper.Clamp(player.Position.X,
                                             player.PlayerAnimation.FrameWidth / 2f,
                                             GraphicsDevice.Viewport.Width - (player.PlayerAnimation.FrameWidth / 2f));
        player.Position.Y = MathHelper.Clamp(player.Position.Y,
                                             player.PlayerAnimation.FrameHeight / 2f,
                                             GraphicsDevice.Viewport.Height - (player.PlayerAnimation.FrameHeight / 2f));

        // Fire if we press spacebar and only every interval we set as the fireTime
        if (player.Active &&
            currentKeyboardState.IsKeyDown(Keys.Space) &&
            (gameTime.TotalGameTime - previousFireTime > fireTime))
        {
            // Reset our current time
            previousFireTime = gameTime.TotalGameTime;

            // Add the projectile, but add it to the front and center of the player.
            // Because playerPosition is in the center of our ship, we have to set
            // our projectile position accordingly
            AddProjectile(player.Position + new Vector2(player.PlayerAnimation.FrameWidth / 2f, 0));

            // Play the laser sound effect
            _ = laserSound.Play();
        }

        // Display the Game Over screen after 3 seconds if the player is dead
        if ((!player.Active) && (elapsedTimeDead == 0))
        {
            // Add the explosion
            AddExplosion(player.Position);

            // Play the explosion sound effect
            _ = explosionSound.Play(0.3f, 0.0f, 0.0f);

            elapsedTimeDead += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        else if ((!player.Active) && (elapsedTimeDead <= 3000))
        {
            elapsedTimeDead += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        else if ((!player.Active) && (elapsedTimeDead > 3000))
        {
            currentScreen = ScreenState.EndMenu;
        }
    }

    private void AddEnemy()
    {
        Animation enemyAnimation = new();
        enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 60, Color.White, 1f, true);
        Vector2 position = new(GraphicsDevice.Viewport.Width + (enemyTexture.Width / 2f),
                               random.Next(100, GraphicsDevice.Viewport.Height - 100));

        Enemy enemy = new();
        enemy.Initialize(enemyAnimation, position);

        enemies.Add(enemy);
    }

    private void UpdateEnemies(GameTime gameTime)
    {
        // Spawn a new enemy every 1.5 seconds
        if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
        {
            previousSpawnTime = gameTime.TotalGameTime;
            AddEnemy();
        }

        // Update the enemies
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            enemies[i].Update(gameTime);
            if (!enemies[i].Active)
            {
                // If not Active and Health <= 0 (so we don't get an explosion
                // when the enemy goes out of the screen)
                if (enemies[i].Health <= 0)
                {
                    AddExplosion(enemies[i].Position);
                    _ = explosionSound.Play(0.3f, 0.0f, 0.0f);
                    score += enemies[i].Value;
                }

                enemies.RemoveAt(i);
            }
        }
    }

    private void AddProjectile(Vector2 position)
    {
        Projectile projectile = new(GraphicsDevice.Viewport, projectileTexture, position);
        projectiles.Add(projectile);
    }

    private void UpdateProjectiles()
    {
        // Update the projetctiles
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            projectiles[i].Update();

            if (!projectiles[i].Active)
            {
                projectiles.RemoveAt(i);
            }
        }
    }

    private void UpdateCollision()
    {
        // Use the Rectangle's built-in intersect function to determine if 2 objects are overlapping
        Rectangle rectangle1;
        Rectangle rectangle2;

        // Only create the rectangle for the player once
        rectangle1 = new Rectangle((int)player.Position.X - (player.Width / 2),
                                   (int)player.Position.Y - (player.Height / 2),
                                   player.Width,
                                   player.Height);

        // Check collision between Player and Enemies
        for (int i = 0; i < enemies.Count; i++)
        {
            rectangle2 = new Rectangle((int)enemies[i].Position.X - (enemies[i].Width / 2),
                                       (int)enemies[i].Position.Y - (enemies[i].Height / 2),
                                       enemies[i].Width,
                                       enemies[i].Height);

            // Determine if the two objects collided with each other
            if (rectangle1.Intersects(rectangle2))
            {
                player.Health -= enemies[i].Damage;
                enemies[i].Health = 0;
                player.Active = player.Health > 0;
            }
        }

        // Projectiles VS Enemies Collisions
        for (int i = 0; i < projectiles.Count; i++)
        {
            rectangle1 = new Rectangle((int)projectiles[i].Position.X - (projectiles[i].Width / 2),
                                       (int)projectiles[i].Position.Y - (projectiles[i].Height / 2),
                                       projectiles[i].Width,
                                       projectiles[i].Height);
            for (int j = 0; j < enemies.Count; j++)
            {
                rectangle2 = new Rectangle((int)enemies[j].Position.X - (enemies[j].Width / 2),
                                           (int)enemies[j].Position.Y - (enemies[j].Height / 2),
                                           enemies[j].Width,
                                           enemies[j].Height);

                if (rectangle1.Intersects(rectangle2))
                {
                    enemies[j].Health -= projectiles[i].Damage;
                    projectiles[i].Active = false;
                }
            }
        }

        // Player VS HUD collision
        // Check if the player is under the health and score text. If so, the
        // health and score text will be transparent so we can see the player.
        // We will check which line is longer and use that to test if the player
        // is under the text. We have use the offset of our player position since
        // it is in the middle of the ship.
        string txtHealth = $"Health : {player.Health}";
        string txtScore = $"Score : {score}";
        if (gamefont.MeasureString(txtHealth).X >=
            gamefont.MeasureString(txtScore).X)
        {
            fontOrigin = new Vector2(gamefont.MeasureString(txtHealth).X,
                                     gamefont.MeasureString(txtScore).Y + 25); // + 25 is the Height that we are using to set the position of the second Line (Health line)
        }
        else
        {
            fontOrigin = new Vector2(gamefont.MeasureString(txtScore).X,
                                     gamefont.MeasureString(txtScore).Y + 25); // + 25 is the Height that we are using to set the position of the second Line (Health line)
        }

        fontAlphaBlend = (player.Position.X - (player.Width / 2) <= fontOrigin.X) &&
                         (player.Position.Y - (player.Height / 2) <= fontOrigin.Y)
                         ? 0.25f
                         : 1f;
    }

    private void AddExplosion(Vector2 position)
    {
        Animation explosion = new();
        explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
        explosions.Add(explosion);
    }

    private void UpdateExplosions(GameTime gameTime)
    {
        for (int i = explosions.Count - 1; i >= 0; i--)
        {
            explosions[i].Update(gameTime);
            if (!explosions[i].Active)
            {
                explosions.RemoveAt(i);
            }
        }
    }

#if DEBUG_BOUNDING_RECTS
    // Used to draw the Bounding Rectangles
    private void DrawBoundingRect(Rectangle rectangleToDraw, int borderThickness, Color borderColor)
    {
        // Draw Top line
        spriteBatch.Draw(pixel,
                         new Rectangle(rectangleToDraw.X,
                                       rectangleToDraw.Y,
                                       rectangleToDraw.Width,
                                       borderThickness),
                         borderColor);

        // Draw Left line
        spriteBatch.Draw(pixel,
                         new Rectangle(rectangleToDraw.X,
                                       rectangleToDraw.Y,
                                       borderThickness,
                                       rectangleToDraw.Height),
                         borderColor);

        // Draw Right line
        spriteBatch.Draw(pixel,
                         new Rectangle(rectangleToDraw.X + rectangleToDraw.Width - borderThickness,
                                       rectangleToDraw.Y,
                                       borderThickness,
                                       rectangleToDraw.Height), borderColor);

        // Draw Bottom line
        spriteBatch.Draw(pixel,
                         new Rectangle(rectangleToDraw.X,
                                       rectangleToDraw.Y + rectangleToDraw.Height - borderThickness,
                                       rectangleToDraw.Width,
                                       borderThickness),
                         borderColor);
    }
#endif

    private void DrawMainMenu()
    {
        // Draw all the elements that are part of the Main Menu
        spriteBatch.Draw(mainMenuScreenBackground, Vector2.Zero, Color.White);

        fontOrigin = menufont.MeasureString("Play") / 2;
        spriteBatch.DrawString(menufont, "Play", new Vector2(fontPos.X, fontPos.Y + 10), playColor, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
        fontOrigin = menufont.MeasureString("Quit") / 2;
        spriteBatch.DrawString(menufont, "Quit", new Vector2(fontPos.X, fontPos.Y + 10 + (fontOrigin.Y * 2)), quitColor, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
    }

    private void DrawMainGame()
    {
        // Draw the static background
        spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

        // Draw the moving backgrounds
        bgLayer1.Draw(spriteBatch);
        bgLayer2.Draw(spriteBatch);

        // Draw the Player if player's health is above 0
        if (player.Health > 0)
        {
            player.Draw(spriteBatch);
        }

        // Draw the Enemies
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Draw(spriteBatch);
        }

        // Draw the projectiles
        for (int i = 0; i < projectiles.Count; i++)
        {
            projectiles[i].Draw(spriteBatch);
        }

        // Draw the Explosions
        for (int i = 0; i < explosions.Count; i++)
        {
            explosions[i].Draw(spriteBatch);
        }

        // Draw the score
        spriteBatch.DrawString(gamefont,
                               $"Score : {score}",
                               new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                                           GraphicsDevice.Viewport.TitleSafeArea.Y),
                               Color.White * fontAlphaBlend);

        // Draw the player's health
        spriteBatch.DrawString(gamefont,
                               $"Health : {player.Health}",
                               new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                                           GraphicsDevice.Viewport.TitleSafeArea.Y + 25),
                               Color.White * fontAlphaBlend);

#if DEBUG_BOUNDING_RECTS
        if (showBoundingRectangles)
        {
            // Draw Bounding Rectangles
            Rectangle boundingRect = new((int)player.Position.X - (player.Width / 2),
                                         (int)player.Position.Y - (player.Height / 2),
                                         player.Width,
                                         player.Height);

            DrawBoundingRect(boundingRect, 2, Color.BlueViolet);

            for (int i = 0; i < enemies.Count; i++)
            {
                boundingRect = new Rectangle((int)enemies[i].Position.X - (enemies[i].Width / 2),
                                             (int)enemies[i].Position.Y - (enemies[i].Height / 2),
                                             enemies[i].Width,
                                             enemies[i].Height);
                DrawBoundingRect(boundingRect, 2, Color.SpringGreen);
            }

            for (int i = 0; i < projectiles.Count; i++)
            {
                boundingRect = new Rectangle((int)projectiles[i].Position.X - (projectiles[i].Width / 2),
                                             (int)projectiles[i].Position.Y - (projectiles[i].Height / 2),
                                             projectiles[i].Width,
                                             projectiles[i].Height);
                DrawBoundingRect(boundingRect, 2, Color.Black);
            }
        }
#endif
    }

    private void DrawEndMenu()
    {
        // Draw all the elements that are part of the End Menu
        // Draw the EndMenu Background
        spriteBatch.Draw(endMenuScreenBackground, Vector2.Zero, Color.White);

        // Draw the score
        string txt = $"Score : {score}";
        fontOrigin = menufont.MeasureString(txt) / 2;
        spriteBatch.DrawString(menufont, txt, fontPos, Color.White, 0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
    }
}

/*
 * TODO(PERE) :
 * - Architecture :
 *   - Use an enum for the menu buttons instead of an index ?
 *   - Create an InputManager class
 *   - Use a scene graph/screen manager ?
 *   - Add pixel perfect collision ?
 *   - Should the bounding rect of lasers be a bit smaller ?
 *   - Add a base class for Projectiles, Player, Enemy (Active, Width, Height, etc) ?
 * - Gameplay :
 * - Keep highscores ? (ex: last 10)
 *   - Make user add his name to the leaderborads ?
 * - Add a new type of enemy who fires at the player ?
 */
