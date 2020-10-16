using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PandaMonogame
{
    public class SpriteList
    {
        protected List<Sprite> _sprites;

        public SpriteList()
        {
            _sprites = new List<Sprite>();
        }

        public Sprite this[int index]
        {
            get
            {
                return _sprites[index];
            }

            set
            {
                _sprites[index] = value;
            }
        }

        public void Add(Sprite sprite)
        {
            _sprites.Add(sprite);
        }

        public void Remove(int index)
        {
            _sprites.Remove(_sprites[index]);
        }

        public void Remove(Sprite sprite)
        {
            _sprites.Remove(sprite);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                _sprites[i].Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                _sprites[i].Update(gameTime);
            }
        }

        public void Clear()
        {
            _sprites.Clear();
        }
    }
}
