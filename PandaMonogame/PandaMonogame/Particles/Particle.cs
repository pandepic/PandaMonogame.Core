using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame.Particles
{
    public class Particle : IPoolable
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public double Duration;
        public int MaxDuration;

        public AnimatedSprite Sprite { get; set; } = null;

        #region IPoolable
        public int PoolIndex { get; set; }
        public bool IsAlive { get; set; }

        public virtual void Reset()
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Duration = 0;
        }
        #endregion

        public virtual void Start(int maxDuration)
        {
            MaxDuration = maxDuration;
            Duration = maxDuration;
        }

        public virtual void Update(GameTime gameTime)
        {
            Duration -= gameTime.ElapsedGameTime.TotalMilliseconds;
            Position += Velocity * gameTime.DeltaTime();
        }

        public virtual void Draw(GameTime gameTime, GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            Sprite.Position = Position;
            Sprite.Draw(spriteBatch);
        }
    }
}
