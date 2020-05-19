using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame.Particles
{
    public class ParticleManager
    {
        public const int DefaultPoolSize = 500;

        public ObjectPool<Particle> ParticlePool { get; set; }
        public List<ParticleEmitter> Emitters { get; set; }

        public ParticleManager(int poolSize = DefaultPoolSize)
        {
            Emitters = new List<ParticleEmitter>();
            ParticlePool = new ObjectPool<Particle>(poolSize);
        }

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < Emitters.Count; i++)
            {
                Emitters[i]?.Update(gameTime);
            }

            for (var i = 0; i < ParticlePool.Size; i++)
            {
                ParticlePool[i]?.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            for (var i = 0; i < ParticlePool.Size; i++)
            {
                ParticlePool[i]?.Draw(gameTime, graphics, spriteBatch);
            }
        }
    }
}
