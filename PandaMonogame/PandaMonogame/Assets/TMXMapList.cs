using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    public class TMXMapList
    {
        protected List<TMXMap> _maps;
        protected TMXMap _activeMap;

        public TMXMap this[int index]
        {
            get
            {
                return _maps[index];
            }

            set
            {
                _maps[index] = value;
            }
        }

        public TMXMapList()
        {
            _maps = new List<TMXMap>();
            _activeMap = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _activeMap?.Draw(spriteBatch);
        }
    }
}
