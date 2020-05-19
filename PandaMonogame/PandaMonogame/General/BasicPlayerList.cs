using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PandaMonogame
{
    public class BasicPlayerList
    {
        protected List<BasicPlayer> _players = new List<BasicPlayer>();

        public BasicPlayerList() { }

        public BasicPlayer this[int index]
        {
            get
            {
                return _players[index];
            }

            set
            {
                _players[index] = value;
            }
        }

        public void Add(BasicPlayer player)
        {
            _players.Add(player);
        }

        public void Remove(int index)
        {
            _players.Remove(_players[index]);
        }

        public void Remove(BasicPlayer player)
        {
            _players.Remove(player);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].Draw(spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].Update(gameTime);
            }
        }

        public void Clear()
        {
            _players.Clear();
        }
    }
}
