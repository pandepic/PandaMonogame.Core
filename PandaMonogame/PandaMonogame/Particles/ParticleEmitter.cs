using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame.Particles
{
    public class ParticleEmitter
    {
        protected ParticleManager _parent;
        public ObjectPool<Particle> ParticlePool { get; set; }

        public ParticleEmitter(ParticleManager parent)
        {
            _parent = parent;
            ParticlePool = new ObjectPool<Particle>(5000);
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }
    }
}
