using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PandaMonogame
{
    public class SpriteList
    {
        protected List<Sprite> sprites;

        public SpriteList()
        {
            sprites = new List<Sprite>();
        }

        public Sprite this[int index]
        {
            get
            {
                return sprites[index];
            }

            set
            {
                sprites[index] = value;
            }
        }

        public void Add(Sprite sprite)
        {
            sprites.Add(sprite);
        }

        public void Remove(int index)
        {
            sprites.Remove(sprites[index]);
        }

        public void Remove(Sprite sprite)
        {
            sprites.Remove(sprite);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var sprite in sprites)
            {
                sprite.Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Update(gameTime);
            }
        }

        public void Clear()
        {
            sprites.Clear();
        }
    }
}
