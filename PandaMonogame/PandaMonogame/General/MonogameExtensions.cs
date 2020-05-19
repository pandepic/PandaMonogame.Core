using Microsoft.Xna.Framework;

namespace PandaMonogame
{
    public static class MonogameExtensions
    {
        public static float DeltaTime(this GameTime t)
        {
            return (float)t.ElapsedGameTime.TotalSeconds;
        }
    }
}
