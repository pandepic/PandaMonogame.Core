using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace PandaMonogame
{
    public struct LayerTexture
    {
        public RenderTarget2D Texture { get; set; }
        public int DrawOrder { get; set; }
        public Rectangle SourceRect { get; set; }
        public Vector2 DrawPosition { get; set; }
    }

    public class TMXTilesheet
    {
        public AnimatedSprite Sheet { get; set; }
        public int FirstGID { get; set; }
        public int LastGID { get; set; }
        public string Name { get; set; }
    }

    public class TMXMap
    {
        public TmxMap Map { get; set; }
        public List<LayerTexture> LayerTextures { get; set; }
        protected GraphicsDevice _graphics;
        protected List<TMXTilesheet> _tilesheets;

        public TMXMap(GraphicsDevice graphics, string file, ContentManager Content)
        {
            _graphics = graphics;

            Map = new TmxMap(file);

            _tilesheets = new List<TMXTilesheet>();

            foreach (var ts in Map.Tilesets)
            {
                _tilesheets.Add(new TMXTilesheet()
                {
                    Sheet = new AnimatedSprite(ModManager.Instance.AssetManager.LoadTexture2D(graphics, ts.Name), Map.TileWidth, Map.TileHeight),
                    FirstGID = ts.FirstGid,
                    Name = ts.Name,
                });
            }

            _tilesheets = _tilesheets.OrderBy(ts => ts.FirstGID).ToList();

            for (int i = 0; i < _tilesheets.Count(); i++)
            {
                if (i == (_tilesheets.Count() - 1))
                {
                    _tilesheets[i].LastGID = -1; // magic number for last sheet
                }
                else
                {
                    _tilesheets[i].LastGID = _tilesheets[i + 1].FirstGID - 1;
                }
            }

            LayerTextures = new List<LayerTexture>();

            foreach (var layer in Map.Layers)
            {
                RenderTarget2D texture = BuildLayerTexture(layer);

                LayerTextures.Add(new LayerTexture()
                {
                    Texture = texture,
                    DrawOrder = (layer.Properties.ContainsKey("drawOrder") ? int.Parse(layer.Properties["drawOrder"]) : Map.Layers.IndexOf(layer)),
                    SourceRect = new Rectangle(0, 0, texture.Width, texture.Height),
                    DrawPosition = new Vector2(0, 0)
                });
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Vector2.One, false);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPosition, bool useCamera = true)
        {
            foreach (var texture in LayerTextures.Where(t => t.Texture != null).OrderBy(t => t.DrawOrder))
            {
                spriteBatch.Draw(texture.Texture, (useCamera ? texture.DrawPosition + cameraPosition : texture.DrawPosition), texture.SourceRect, Color.White);
            }
        }

        protected RenderTarget2D BuildLayerTexture(TmxLayer layer)
        {
            RenderTarget2D texture = new RenderTarget2D(_graphics, Map.Width * Map.TileWidth, Map.Height * Map.TileHeight);

            _graphics.SetRenderTarget(texture);

            using (var spriteBatch = new SpriteBatch(_graphics))
            {
                _graphics.Clear(Color.Transparent);

                int x = 0;
                int y = 0;

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                foreach (var tile in layer.Tiles)
                {
                    TMXTilesheet tilesheet = null;

                    foreach (var ts in this._tilesheets)
                    {
                        if (tile.Gid >= ts.FirstGID && (ts.LastGID == -1 || tile.Gid <= ts.LastGID))
                        {
                            tilesheet = ts;
                        }
                    }

                    tilesheet.Sheet.SetFrame(tile.Gid);
                    tilesheet.Sheet.Draw(spriteBatch, new Vector2(x, y));

                    x += Map.TileWidth;

                    if (x >= (Map.Width * Map.TileWidth))
                    {
                        x = 0;
                        y += Map.TileHeight;
                    }
                }

                spriteBatch.End();

                _graphics.SetRenderTarget(null);

                return texture;
            }
        } // BuildLayerTexture
    }
}
