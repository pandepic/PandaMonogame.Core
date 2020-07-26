using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PandaMonogame
{
    public class Asset
    {
        public string Name { get; set; }
        public string Filepath { get; set; }
    }

    public class AssetManager
    {
        public int DefaultFontSize { get; set; } = 20;

        public SoundManager SoundManager { get; set; }
        protected Dictionary<string, Asset> _assets = new Dictionary<string, Asset>();
        protected Dictionary<string, object> _assetCache = new Dictionary<string, object>();

        protected Dictionary<string, IDisposable> _disposableAssets = new Dictionary<string, IDisposable>();

        public AssetManager() { }

        ~AssetManager()
        {
            DisposeAssets();
        }

        public void Import(string filepath, string modPath = "")
        {
            if (PandaMonogameConfig.Logging)
                Console.WriteLine("Importing assets: " + filepath);

            using (var fs = GetFileStream(filepath))
            {
                XDocument doc = XDocument.Load(fs);

                List<XElement> assetElements = doc.Root.Elements("Asset").ToList();

                foreach (var asset in assetElements)
                {
                    string assetName = asset.Attribute("Name").Value;
                    string assetPath = (modPath.Length > 0 ? (modPath + "\\") : "") + asset.Attribute("FilePath").Value;

                    if (_assets.ContainsKey(assetName) == false)
                    {
                        _assets.Add(assetName, new Asset() { Name = assetName, Filepath = assetPath });
                        if (PandaMonogameConfig.Logging)
                            Console.WriteLine("Asset imported: " + assetName + " - " + assetPath);
                    }
                }

                if (PandaMonogameConfig.Logging)
                    Console.WriteLine("Finished importing assets: " + filepath);
            }
        }

        public void Clear()
        {
            DisposeAssets();
            _assetCache.Clear();
        }

        public void DisposeAssets()
        {
            SoundManager?.StopAll();

            foreach (var t in _disposableAssets)
            {
                t.Value?.Dispose();
            }

            _disposableAssets.Clear();
        }

        public string GetAssetPath(string assetName)
        {
            return _assets[assetName].Filepath;
        }

        public FileStream GetFileStream(string path, FileMode mode = FileMode.Open)
        {
            // TODO : this doesn't currently support writing because of titlecontainer

            if (Path.IsPathRooted(path))
                return File.Open(path, mode);
            else
                return (FileStream)TitleContainer.OpenStream(path);
        }

        public Stream GetFileStreamByAsset(string assetName)
        {
            var path = GetAssetPath(assetName);
            return GetFileStream(path);
        }

        public void UnloadAsset(string assetName)
        {
            if (_disposableAssets.ContainsKey(assetName))
            {
                _disposableAssets[assetName]?.Dispose();
                _disposableAssets.Remove(assetName);
            }

            if (_assetCache.ContainsKey(assetName))
                _assetCache.Remove(assetName);
        }

        public SoundEffect LoadSoundEffect(string assetName)
        {
            if (!_assetCache.ContainsKey(assetName))
            {
                using (var fs = GetFileStreamByAsset(assetName))
                {
                    var sfx = SoundEffect.FromStream(fs);
                    _assetCache.Add(assetName, sfx);
                    _disposableAssets.Add(assetName, sfx);
                }
            }

            return (SoundEffect)_assetCache[assetName];
        }

        public Texture2D LoadTexture2D(GraphicsDevice graphics, string assetName, float scale = 1f, bool premultiplyAlpha = false)
        {
            if (!_assetCache.ContainsKey(assetName))
            {
                using (var fs = GetFileStreamByAsset(assetName))
                {
                    Texture2D loadTexture = null;

                    if (premultiplyAlpha)
                        loadTexture = AssetManagerUtil.PremultiplyTextureAlpha(graphics, fs);
                    else
                        loadTexture = Texture2D.FromStream(graphics, fs);

                    var newTexture = loadTexture;
                    if (scale != 1f)
                    {
                        newTexture = AssetManagerUtil.ScaleTexture(graphics, loadTexture, scale);
                        loadTexture.Dispose();
                    }

                    _assetCache.Add(assetName, newTexture);
                    _disposableAssets.Add(assetName, newTexture);
                }
            }

            return (Texture2D)_assetCache[assetName];
        }

        public Texture2D LoadTexture2D(GraphicsDevice graphics, string assetName, bool premultiplyAlpha = false)
        {
            return LoadTexture2D(graphics, assetName, 1f, premultiplyAlpha);
        }

        public Texture2D LoadTexture2D(GraphicsDevice graphics, string assetName)
        {
            return LoadTexture2D(graphics, assetName, 1f, false);
        }

        public DynamicSpriteFont LoadDynamicSpriteFont(string assetName)
        {
            if (!_assetCache.ContainsKey(assetName))
            {
                using (var fs = GetFileStreamByAsset(assetName))
                {
                    _assetCache.Add(assetName, DynamicSpriteFont.FromTtf(fs, DefaultFontSize));
                }
            }

            return (DynamicSpriteFont)_assetCache[assetName];
        }
    }
}
