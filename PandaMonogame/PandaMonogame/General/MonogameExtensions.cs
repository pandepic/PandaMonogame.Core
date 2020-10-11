using Microsoft.Xna.Framework;
using System;

namespace PandaMonogame
{
    public static class MonogameExtensions
    {
        public static float DeltaTime(this GameTime t)
        {
            return (float)t.ElapsedGameTime.TotalSeconds;
        }

        public static float DeltaTimeMS(this GameTime t)
        {
            return (float)t.ElapsedGameTime.TotalMilliseconds;
        }

        public static float GetDistance(this Vector2 vec1, Vector2 vec2)
        {
            return MathF.Sqrt(MathF.Pow(vec2.X - vec1.X, 2) + MathF.Pow(vec2.Y - vec1.Y, 2));
        }
    }
}
