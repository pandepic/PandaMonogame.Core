using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    public class LayeredSprite : AnimatedSprite
    {
        protected static Dictionary<string, Texture2D> _layeredCache = new Dictionary<string, Texture2D>();

        protected List<string> _layers = new List<string>();

        public LayeredSprite(GraphicsDevice graphics, List<string> layers, int frameWidth, int frameHeight, int defaultFrame = 1)
        {
            SetLayers(layers);
            BuildSprite(graphics);
            Init(Texture, frameWidth, frameHeight, defaultFrame);
        }

        ~LayeredSprite()
        {
            
        }

        public static void Cleanup()
        {
            foreach (var kvp in _layeredCache)
            {
                kvp.Value?.Dispose();
            }
        }

        public void SetLayers(List<string> _layers)
        {
            this._layers.Clear();

            foreach (var l in _layers)
            {
                if (!string.IsNullOrWhiteSpace(l))
                {
                    this._layers.Add(l);
                }
            }
        }

        public void BuildSprite(GraphicsDevice graphics)
        {
            var cacheName = "";

            foreach (var l in _layers)
            {
                cacheName += l;
            }

            if (_layeredCache.ContainsKey(cacheName))
            {
                Texture = _layeredCache[cacheName];
                return;
            }

            var layerTextures = new List<Texture2D>();

            foreach (var l in _layers)
            {
                layerTextures.Add(ModManager.Instance.AssetManager.LoadTexture2D(graphics, l));
            }

            if (layerTextures.Count <= 0)
                return;

            Texture = new RenderTarget2D(graphics, layerTextures[0].Width, layerTextures[0].Height);

            graphics.SetRenderTarget((RenderTarget2D)Texture);

            using (SpriteBatch spriteBatch = new SpriteBatch(graphics))
            {
                graphics.Clear(Color.Transparent);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                foreach (var t in layerTextures)
                {
                    spriteBatch.Draw(t, new Vector2(0, 0), Color.White);
                }

                spriteBatch.End();
            }

            graphics.SetRenderTarget(null);

            _layeredCache.Add(cacheName, Texture);
        }
    }
}
