using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngine.Source
{
    public class RectangleShape
    {
        private Texture2D texture;

        public RectangleShape(GraphicsDevice graphicsDevice, int size, Color color)
        {
            texture = new Texture2D(graphicsDevice, size, size);
            Color[] data = new Color[size * size];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = color;
            }
            texture.SetData(data);
        }

        public Texture2D GetTexture()
        {
            return texture;
        }
    }
}
