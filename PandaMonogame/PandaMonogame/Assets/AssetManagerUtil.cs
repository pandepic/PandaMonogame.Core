using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace PandaMonogame
{
    public static class AssetManagerUtil
    {
        public static Texture2D PremultiplyTextureAlpha(GraphicsDevice graphics, Stream fs)
        {
            var newTexture = Texture2D.FromStream(graphics, fs);

            Color[] colorBuffer = new Color[newTexture.Width * newTexture.Height];
            newTexture.GetData(colorBuffer);

            for (var i = 0; i < colorBuffer.Length; i++)
                colorBuffer[i] = Color.FromNonPremultiplied(colorBuffer[i].R, colorBuffer[i].G, colorBuffer[i].B, colorBuffer[i].A);

            newTexture.SetData(colorBuffer);
            
            return newTexture;
        }

        public static Texture2D ScaleTexture(GraphicsDevice graphics, Texture2D texture, float scale)
        {
            var scaledTexture = new RenderTarget2D(graphics, (int)(texture.Width * scale), (int)(texture.Height * scale));

            graphics.SetRenderTarget(scaledTexture);
            using (SpriteBatch spriteBatch = new SpriteBatch(graphics))
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
                graphics.Clear(Color.Transparent);
                spriteBatch.Draw(texture, Vector2.Zero, new Rectangle(0, 0, scaledTexture.Width, scaledTexture.Height), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                spriteBatch.End();
            }
            graphics.SetRenderTarget(null);

            return scaledTexture;
        }
    }
}
