using GameLibrary.Source;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace GameLibrary
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Character _hero;

        Texture2D _monkey;
        Texture2D _background;
        Texture2D _logo;
        SpriteFont _font;
        SoundEffect _hit;
        Song _title;
        List<GridCell> _grid = new List<GridCell>();

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // only support portait mode
            _graphics.SupportedOrientations = DisplayOrientation.Portrait;
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
            var gridWidth = (viewport.Width - (padding * 5)) / 4;
            var gridHeight = gridWidth;

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _hero.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.SaddleBrown);
            _spriteBatch.Begin();
            foreach (var square in _grid)
                _spriteBatch.Draw(_monkey, destinationRectangle: square.DisplayRectangle, color: Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
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
}