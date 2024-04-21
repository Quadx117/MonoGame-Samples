namespace Pong_DX
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class PongGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// <see cref="RenderTarget2D"/> used to draw the game with the same
        /// aspect ratio regardless of the screen resolution.
        /// </summary>
        private RenderTarget2D _renderTarget;

        /// <summary>
        /// The drawing bounds on screen.
        /// </summary>
        private Rectangle _renderRect;

        /// <summary>
        /// A blank texture used to draw primitives with any color.
        /// </summary>
        private Texture2D _blankPixel;

        private GameState _gameState;

        // TODO(PERE): Use a round ball? (maybe toggle between both)
        private Ball _ball;
        private readonly Paddle[] _paddles;

        private bool _leftSideScored;
        private readonly Random _random;

        private SpriteFont _font;

        // TODO(PERE): Move this inside the paddle class?
        private int[] _scores;

        private SoundEffect _bounceSound;
        private SoundEffect _hitSound;
        private SoundEffect _scoreSound;

        private Texture2D _keyboard;
        private Texture2D _mouse;
        private Texture2D _gamepad;

        // TODO(PERE): Put the control code inside the Paddle class?
        private readonly PlayerPad[] _pads;

        public PongGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // TODO(PERE): Disable mouse when in the play state if we add menus using the mouse
            IsMouseVisible = false;
            // TODO(PERE): Do I want to keep this, which changes the refresh rate to 30fps instead of 60fps?
            TargetElapsedTime = new TimeSpan(333333);
            // NOTE(PERE): Used to show how letter-boxing/pillar-boxing works
            Window.AllowUserResizing = true;

            _gameState = GameState.Idle;

            _random = new Random();

            // TODO(PERE): Why here instead of Initialize like the ball?
            _paddles = new Paddle[2];

            // TODO(PERE): Why here instead of Initialize like the ball?
            // TODO(PERE): Rename to input instead (rename class too)
            _pads = new PlayerPad[2];
            _pads[0] = new PlayerPad(PadType.AI);
            _pads[1] = new PlayerPad(PadType.AI);
        }

        protected override void Initialize()
        {
            // TODO(PERE): Settle on a resolution for the base game.
            // This should be a multiple of 1920x1080 (or 1280x720) so we can make
            // it fit perfectly in standard resolution (perfect scaling ratio).
            _renderTarget = new RenderTarget2D(GraphicsDevice, 640, 480);

            // NOTE(PERE): This is the default window resolution at which the game is rendered.
            // It essentialy scales the _renderTarget by a factor of 2.
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            // TODO(PERE): Add configuration option
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            Window.ClientSizeChanged += OnWindowSizeChanged;
            OnWindowSizeChanged(null, null);

            _ball = new Ball(_random, _leftSideScored);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _blankPixel = new Texture2D(GraphicsDevice, 1, 1);
            Color[] data = new Color[1];
            data[0] = Color.White;
            _blankPixel.SetData(data);

            _font = Content.Load<SpriteFont>("ScoreFont");

            _bounceSound = Content.Load<SoundEffect>("Sounds/Click3");
            _hitSound = Content.Load<SoundEffect>("Sounds/Click7");
            _scoreSound = Content.Load<SoundEffect>("Sounds/Warning");

            _keyboard = Content.Load<Texture2D>("Sprites/Keyboard");
            _mouse = Content.Load<Texture2D>("Sprites/Mouse");
            _gamepad = Content.Load<Texture2D>("Sprites/GamePad");
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO(PERE): Check where to put this (i.e. add a menu to quit?)
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            switch (_gameState)
            {
                case GameState.Idle:
                {
                    (_, bool bounced) = _ball.Move(true);
                    if (bounced)
                    {
                        _ = _bounceSound.Play(0.15f, 0f, 0f);
                    }

                    (_, Button button) = PlayerPad.SelectionPoll();
                    if (button == Button.Click)
                    {
                        _gameState = GameState.ChooseInput;
                    }
                }
                break;
                case GameState.ChooseInput:
                {
                    (PadType type, Button button) = PlayerPad.SelectionPoll();
                    if (button == Button.Left && _pads[1].PadType == type)
                    {
                        // If we removed the input from a player, it is replaced by an AI player.
                        _pads[1] = new PlayerPad(PadType.AI);
                    }
                    else if (button == Button.Left && _pads[0].PadType == PadType.AI)
                    {
                        // If it was an AI Player, it is replaced by a human player with the selected input method.
                        _pads[0] = new PlayerPad(type);
                    }

                    if (button == Button.Right && _pads[0].PadType == type)
                    {
                        _pads[0] = new PlayerPad(PadType.AI);
                    }
                    else if (button == Button.Right && _pads[1].PadType == PadType.AI)
                    {
                        _pads[1] = new PlayerPad(type);
                    }

                    if (button == Button.Click)
                    {
                        _gameState = GameState.Start;
                    }
                }
                break;
                case GameState.Start:
                {
                    _ball.Reset(_random, _leftSideScored);
                    // TODO(PERE): Add a reset and only allocate memory in Initialize
                    _paddles[0] = new Paddle(false);
                    _paddles[1] = new Paddle(true);
                    _scores = new int[2];
                    _gameState = GameState.Play;
                }
                break;
                case GameState.Play:
                {
                    (int scored, bool bounced) = _ball.Move(false);
                    if (bounced)
                    {
                        _ = _bounceSound.Play(0.15f, 0f, 0f);
                    }

                    for (int playerIndex = 0; playerIndex < 2; ++playerIndex)
                    {
                        PlayerPad pad = _pads[playerIndex];
                        if (pad.PadType == PadType.AI)
                        {
                            _paddles[playerIndex].AIMove(_ball);
                        }
                        else
                        {
                            // TODO(PERE): Verify architecture of this move code
                            pad.PollInput();
                            _paddles[playerIndex].PlayerMove(pad.Y);
                        }
                    }

                    bool hit = _paddles[0].CollisionCheck(_ball);
                    hit |= _paddles[1].CollisionCheck(_ball);

                    // TODO(PERE): Change this
                    if (hit || scored == 0)
                    {
                        // Continue playing
                        if (hit)
                        {
                            _ = _hitSound.Play(0.15f, 0f, 0f);
                        }
                    }
                    else
                    {
                        _leftSideScored = scored == 1;
                        int index = _leftSideScored ? 0 : 1;
                        ++_scores[index];
                        _ = _scoreSound.Play(0.15f, 0f, 0f);
                        _gameState = GameState.CheckEnd;
                    }
                }
                break;

                // TODO(PERE): Rename CheckEnd to something else (maybe GameEnd?)
                // The name CheckEnd refers to the fact that we check for the ending
                // condition (i.e. win condition) here and either continue playing or
                // go to GameState.Idle. Maybe we don't need a separate state for this?
                case GameState.CheckEnd:
                {
                    _ball.Reset(_random, _leftSideScored);

                    _gameState = _scores[0] > 9 ||
                                 _scores[1] > 9
                                     ? GameState.Idle
                                     : GameState.Play;
                }
                break;

                default:
                {
                    _gameState = GameState.Idle;
                }
                break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            int halfRenderTargetWidth = _renderTarget.Width / 2;

            // Draw the net in the middle of the playing field
            for (int i = 0; i < 31; ++i)
            {
                _spriteBatch.Draw(_blankPixel,
                                  new Rectangle(halfRenderTargetWidth - 1,
                                                i * _renderTarget.Height / 31,
                                                2,
                                                _renderTarget.Height / 62),
                                  Color.White);
            }

            switch (_gameState)
            {
                case GameState.Idle:
                {
                    _spriteBatch.Draw(_blankPixel, _ball.BoundingBox, Color.White);
                    // TODO(PERE): Add some effect, like blinking or fading in and out
                    // TODO(PERE): Center text automatically based on string width
                    _spriteBatch.DrawString(_font,
                                            "Click to play",
                                            new Vector2(halfRenderTargetWidth - 225,
                                                        (_renderTarget.Height / 2) - 40),
                                            Color.White);
                }
                break;
                case GameState.ChooseInput:
                {
                    // TODO(PERE): We could put everything in the loop for
                    // game pads below if we don't mind having the gampad
                    // first followed by the keyboard and mouse (i.e. the
                    // order of the enum)
                    // TODO(PERE): Use a constant and simply negate the number (or * 1 or -1)
                    int displace = _pads[0].PadType == PadType.Mouse
                                       ? -128
                                       : _pads[1].PadType == PadType.Mouse
                                           ? 128
                                           : 0;
                    _spriteBatch.Draw(_mouse,
                                      new Rectangle(((_renderTarget.Width - _mouse.Width) / 2) + displace,
                                                    (_renderTarget.Height / 2) - 192,
                                                    _mouse.Width, _mouse.Height),
                                      Color.White);

                    displace = _pads[0].PadType == PadType.Keyboard
                                   ? -128
                                   : _pads[1].PadType == PadType.Keyboard
                                       ? 128
                                       : 0;
                    _spriteBatch.Draw(_keyboard,
                                      new Rectangle(((_renderTarget.Width - _keyboard.Width) / 2) + displace,
                                                    (_renderTarget.Height / 2) - 128,
                                                    98, 34),
                                      Color.White);

                    for (int playerIndex = 0; playerIndex < 4; ++playerIndex)
                    {
                        if (GamePad.GetState(playerIndex).IsConnected)
                        {
                            displace = _pads[0].PadType == (PadType)playerIndex
                                           ? -128
                                           : _pads[1].PadType == (PadType)playerIndex
                                               ? 128
                                               : 0;
                            _spriteBatch.Draw(_gamepad,
                                              new Rectangle(((_renderTarget.Width - _gamepad.Width) / 2) + displace,
                                                            (_renderTarget.Height / 2) - 64,
                                                            64, 64),
                                              Color.White);
                        }
                    }
                }
                break;
                case GameState.Start:
                    break;
                case GameState.Play:
                case GameState.CheckEnd:
                {
                    _spriteBatch.Draw(_blankPixel, _ball.BoundingBox, Color.White);

                    _spriteBatch.Draw(_blankPixel, _paddles[0].BoundingBox, Color.White);
                    _spriteBatch.Draw(_blankPixel, _paddles[1].BoundingBox, Color.White);

                    _spriteBatch.DrawString(_font, _scores[0].ToString(), new Vector2(128, 0), Color.White);
                    _spriteBatch.DrawString(_font, _scores[1].ToString(), new Vector2(_renderTarget.Width - 178, 0), Color.White);
                }
                break;
            }

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            // TODO(PERE): Choose a better color here
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_renderTarget, _renderRect, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // TODO(PERE): See if there is an event that would resize the buffer while dragging
        // instead of when releasing the mouse button. Could also pause the game and stop
        // rendering if cannot find resize on change.
        private void OnWindowSizeChanged(object sender, EventArgs e)
        {
            int width = Window.ClientBounds.Width;
            int height = Window.ClientBounds.Height;

            if (height < (float)width / _renderTarget.Width * _renderTarget.Height)
            {
                width = (int)((float)height / _renderTarget.Height * _renderTarget.Width);
            }
            else
            {
                height = (int)((float)width / _renderTarget.Width * _renderTarget.Height);
            }

            // NOTE(PERE): We divide each axis by 2, since we want to essentialy scale the final render
            // by a factor of 2.
            // TODO(PERE): Use constants for the scale factor or choose a base resolution and let the
            // scaling be dependant of the actual window size.
            // TODO(PERE): Add a minimum screen size so we can't shrink the window beyond a certain point.
            int x = (int)((Window.ClientBounds.Width - width) * 0.5f);
            int y = (int)((Window.ClientBounds.Height - height) * 0.5f);

            _renderRect = new Rectangle(x, y, width, height);
        }
    }
}

/*
  Architecure :
    - Rename PadType to ControllerType or InputType?
    - Check if the mouse si withing the bounds of the screen before accepting mouse input?
    - Find out why resizing the window is interpreted as a click event and starts the game
    - Make it so that it can't bounce twice at the same angle between both paddles (i.e. no
      straight lines between paddles)

  Gameplay :
    - Add power-ups to make the paddle grow/shrink?
    - Allow adjusting if the paddles can reach the top and bottom?
      It seems that the original arcade game had a potentiometer on the circuit board
      to adjust that. Don't konw for now what was the intended factory setting.
    - Make the ball accelerate in the same y-direction as the paddle when you hit it?

  paddle movement :
    - Use the mouse wheel
    - Use the mouse movement

  UI :
    - Find a nice free font

  Sound :
    - Bind a key or have a pause menu to set in-game volume
    - Use a random number to modify the pitch of the sound each time?
    - Pan the _hitSound according to its X position?
    - Pan the _scoreSound according to which player scored?
 */
