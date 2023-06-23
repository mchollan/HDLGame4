using GameLibrary.Source;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_monkey, position, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}