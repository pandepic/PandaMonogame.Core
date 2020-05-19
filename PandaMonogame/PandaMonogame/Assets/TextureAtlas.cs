using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    //internal class TextureAtlasEntry
    //{
    //    public string AssetName { get; set; }
    //    public IntXY Offset { get; set; }
    //    public IntXY GridPosition { get; set; }
    //}

    //public class TextureAtlas
    //{
    //    private Dictionary<string, TextureAtlasEntry> _entries = new Dictionary<string, TextureAtlasEntry>();

    //    protected int _maxWidth;
    //    protected float _scale = 1f;

    //    public Texture2D Texture { get; protected set; } = null;

    //    public TextureAtlas(GraphicsDevice graphics, string[] assets, float scale = 1f, int maxWidth = 4000)
    //    {
    //        var textures = new Dictionary<string, Texture2D>();

    //        _scale = scale;
    //        _maxWidth = maxWidth;

    //        var totalWidth = 0;
    //        var totalHeight = 0;

    //        var currentWidth = 0;
    //        var currentHeight = 0;
    //        var currentX = -1;
    //        var currentY = 0;

    //        for (var i = 0; i < assets.Length; i++)
    //        {
    //            var assetName = assets[i];
    //            var newTexture = ModManager.Instance.AssetManager.LoadTexture2D(graphics, assetName, scale);
    //            textures.Add(assetName, newTexture);

    //            currentWidth += newTexture.Width;
    //            if (currentWidth != newTexture.Width && currentWidth >= _maxWidth)
    //            {
    //                if (currentWidth > totalWidth)
    //                    totalWidth = currentWidth;

    //                totalHeight += currentHeight;

    //                currentWidth = 0;
    //                currentX = 0;
    //                currentY += 1;
    //            }
    //            else
    //            {
    //                if (newTexture.Height > currentHeight)
    //                    currentHeight = newTexture.Height;

    //                currentX += 1;
    //            }

    //            var newEntry = new TextureAtlasEntry()
    //            {
    //                AssetName = assetName,
    //                GridPosition = new IntXY(currentX, currentY),
    //            };

    //            _entries.Add(assetName, newEntry);
    //        }

    //        for (var i = 0; i < assets.Length; i++)
    //        {
    //            var entry = _entries[assets[i]];
    //            var texture = textures[entry.AssetName];
    //        }

    //        for (var i = 0; i < assets.Length; i++)
    //            ModManager.Instance.AssetManager.UnloadAsset(assets[i]);
    //    }

    //    ~TextureAtlas()
    //    {
    //        Texture?.Dispose();
    //    }
    //}
}
