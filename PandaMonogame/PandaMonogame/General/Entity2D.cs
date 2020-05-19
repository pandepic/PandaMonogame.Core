using Microsoft.Xna.Framework;

namespace PandaMonogame
{
    public class Entity2D
    {
        protected Vector2 _position;
        protected Vector2 _velocity;
        public int Width = 0;
        public int Height = 0;

        public Vector2 Position { get => _position; set => _position = value; }
        public Vector2 Velocity { get => _velocity; set => _velocity = value; }

        public Entity2D()
        {

        }

        public void UpdatePosition(GameTime gameTime)
        {
            float delta = (gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

            _position.X += _velocity.X * delta;
            _position.Y += _velocity.Y * delta;
        }
    }
}
