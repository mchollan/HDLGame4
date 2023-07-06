using GameLibrary.Source;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace GameLibrary
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private MonoGame.Framework.Utilities.MonoGamePlatform? _platform;

        private Character _hero;

        Texture2D _monkey;
        Texture2D _background;
        Texture2D _logo;
        SpriteFont _font;
        SoundEffect _hit;
        Song _title;


        List<GridCell> _grid = new List<GridCell>();

        GameState currentState = GameState.Start;
        Random rnd = new Random();

        // Text to display to user
        string gameOverText = "Game Over";
        string tapToStartText = "Tap to Start";
        string scoreText = "Score : {0}";

        // Timers: Calculate when events should occur in our game
        TimeSpan gameTimer = TimeSpan.FromMilliseconds(0);
        // Define how often the level difficulty increases
        TimeSpan increaseLevelTimer = TimeSpan.FromMilliseconds(0);
        // Define the delay between game ending and new game beginning
        TimeSpan tapToRestartTimer = TimeSpan.FromSeconds(2);

        // How many cells should be altered in a level
        int cellsToChange = 0;
        int maxCells = 1;
        int maxCellsToChange = 14;
        int score = 0;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // only support portait mode
            _graphics.SupportedOrientations = DisplayOrientation.Portrait;

            _platform = Models.Platforms.DeterminePlatform();

            if(_platform == MonoGame.Framework.Utilities.MonoGamePlatform.DesktopGL ||
               _platform == MonoGame.Framework.Utilities.MonoGamePlatform.Windows) 
            {
                _graphics.PreferredBackBufferWidth = 600;
                _graphics.PreferredBackBufferHeight = 800;
                _graphics.ApplyChanges();
            }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _hero = new Character(_graphics.GraphicsDevice);

            _monkey = Content.Load<Texture2D>("monkey");
            _background = Content.Load<Texture2D>("background");
            _logo = Content.Load<Texture2D>("logo");
            _font = Content.Load<SpriteFont>("font");
            _hit = Content.Load<SoundEffect>("hit");
            _title = Content.Load<Song>("title");

            

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_title);

            // TODO: use this.Content to load your game content here

            var viewport = _graphics.GraphicsDevice.Viewport;
            var padding = (viewport.Width / 100);
            var paddingheight = (viewport.Height / 100);
            var gridWidth = (viewport.Width - (padding * 5)) / 4;
            var gridHeight = (viewport.Height - (paddingheight * 5)) / 5; ;

            for (int y = padding; y < gridHeight * 5; y += gridHeight + padding)
            {
                for (int x = padding; x < viewport.Width - gridWidth; x += gridWidth + padding)
                {
                    _grid.Add(new GridCell()
                    {
                        DisplayRectangle = new Rectangle(x, y, gridWidth, gridHeight)
                    });
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            // For mobile, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
            if(_platform == MonoGame.Framework.Utilities.MonoGamePlatform.iOS || _platform == MonoGame.Framework.Utilities.MonoGamePlatform.Android)
            {
                //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                //Keyboard.GetState().IsKeyDown(Keys.Escape))
               // {
               //     Exit();
               // }
            }

            // Custom logic from us
            var touchState = TouchPanel.GetState();
            switch (currentState)
            {
                case GameState.Start:
                    if (touchState.Count > 0)
                    {
                        currentState = GameState.Playing;
                    }
                    break;
                case GameState.Playing:
                    PlayGame(gameTime, touchState);
                    break;
                case GameState.GameOver:
                    tapToRestartTimer -= gameTime.ElapsedGameTime;
                    if (touchState.Count > 0 && tapToRestartTimer.TotalMilliseconds < 0)
                    {
                        currentState = GameState.Start;
                        score = 0;
                        increaseLevelTimer = TimeSpan.FromMilliseconds(0);
                        gameTimer = TimeSpan.FromMilliseconds(0);
                        cellsToChange = 1;
                        maxCells = 1;
                        for (int i = 0; i < _grid.Count; i++)
                        {
                            _grid[i].Reset();
                        }
                    }
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.SaddleBrown);

            // Calculate the center of the screen
            var center = _graphics.GraphicsDevice.Viewport.Bounds.Center.ToVector2();

            // Calculate half the width of the screen
            var half = _graphics.GraphicsDevice.Viewport.Width / 2;

            // Calculate aspect ratio of the MonkeyTap logo
            var aspect = (float)_logo.Height / _logo.Width;

            // Calculate position of logo on screen
            var rect = new Rectangle((int)center.X - (half / 2), 0, half, (int)(half * aspect));

            _spriteBatch.Begin();

            // Draw the background
            _spriteBatch.Draw(_background, destinationRectangle: _graphics.GraphicsDevice.Viewport.Bounds, color: Color.White);

            // Draw MonkeyTap logo
            _spriteBatch.Draw(_logo, destinationRectangle: rect, color: Color.White);

            // Draw a grid of squares
            foreach (var square in _grid)
            {
                _spriteBatch.Draw(_monkey, destinationRectangle: square.DisplayRectangle, color: Color.Lerp(Color.Transparent, square.Color, square.Transition));
            }

            // If the game is over, draw the score and game over text in the center of screen.
            if (currentState == GameState.GameOver)
            {
                // Measure the text so we can center it correctly
                var v = new Vector2(_font.MeasureString(gameOverText).X / 2, 0);
                _spriteBatch.DrawString(_font, gameOverText, center - v, Color.OrangeRed);

                var t = string.Format(scoreText, score);
                // Measure the text so we can center it correctly
                v = new Vector2(_font.MeasureString(t).X / 2, 0);
                // We can use the font.LineSpacing to draw on the line underneath the "Game Over" text
                _spriteBatch.DrawString(_font, t, center + new Vector2(-v.X, _font.LineSpacing), Color.White);
            }

            // If the game is starting over, add "Tap to Start" text
            if (currentState == GameState.Start)
            {
                // Measure the text so we can center it correctly
                var v = new Vector2(_font.MeasureString(tapToStartText).X / 2, 0);
                _spriteBatch.DrawString(_font, tapToStartText, center - v, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void ProcessTouches(TouchCollection touchState)
        {
            foreach (var touch in touchState)
            {
                if (touch.State != TouchLocationState.Released)
                    continue;
                for (int i = 0; i < _grid.Count; i++)
                {
                    if (_grid[i].DisplayRectangle.Contains(touch.Position) && _grid[i].Color == Color.White)
                    {
                        _hit.Play();
                        _grid[i].Reset();
                        score += 1;
                    }
                }
            }
        }

        void CheckForGameOver(GameTime gameTime)
        {
            for (int i = 0; i < _grid.Count; i++)
            {
                if (_grid[i].Update(gameTime))
                {
                    currentState = GameState.GameOver;
                    tapToRestartTimer = TimeSpan.FromSeconds(2);
                    break;
                }
            }
        }

        void CalculateCellsToChange(GameTime gameTime)
        {
            gameTimer += gameTime.ElapsedGameTime;
            if (gameTimer.TotalSeconds > 2)
            {
                gameTimer = TimeSpan.FromMilliseconds(0);
                cellsToChange = Math.Min(maxCells, maxCellsToChange);
            }
        }

        void IncreaseLevel(GameTime gameTime)
        {
            increaseLevelTimer += gameTime.ElapsedGameTime;
            if (increaseLevelTimer.TotalSeconds > 10)
            {
                increaseLevelTimer = TimeSpan.FromMilliseconds(0);
                maxCells++;
            }
        }

        void MakeMonkeysVisible()
        {
            if (cellsToChange > 0)
            {
                var idx = rnd.Next(_grid.Count);
                if (_grid[idx].Color == Color.Transparent)
                {
                    _grid[idx].Show();
                    cellsToChange--;
                }
            }
        }

        void PlayGame(GameTime gameTime, TouchCollection touchState)
        {
            ProcessTouches(touchState);
            CheckForGameOver(gameTime);
            CalculateCellsToChange(gameTime);
            MakeMonkeysVisible();
            IncreaseLevel(gameTime);
        }
    }

    public class GridCell
    {
        public Rectangle DisplayRectangle;
        public Color Color;
        public TimeSpan CountDown;
        public float Transition;

        public GridCell()
        {
            Reset();
        }

        public bool Update(GameTime gameTime)
        {
            if (Color == Color.White)
            {
                Transition += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 100f;
                CountDown -= gameTime.ElapsedGameTime;
                if (CountDown.TotalMilliseconds <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            Color = Color.Transparent;
            CountDown = TimeSpan.FromSeconds(5);
            Transition = 0f;
        }

        public void Show()
        {
            Color = Color.White;
            CountDown = TimeSpan.FromSeconds(5);
        }
    }

    enum GameState
    {
        Start,
        Playing,
        GameOver
    }
}