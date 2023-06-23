using GameEngine.Source;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Source
{
    public class Character
    {
        private RectangleShape sprite;
        private Vector2 position = new Vector2(30, 100);
        private float speed = 0.3f;

        public Character(GraphicsDevice graphicsDevice)
        {
            sprite = new RectangleShape(graphicsDevice, 50, Color.Red);
        }


        public void Update(GameTime gameTime)
        {
            if (position.X < 30 || position.X > 600)
            {
                speed *= -1;
            }

            position.X += speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite.GetTexture(), position, Color.White);
            spriteBatch.End();
        }
    }
}
