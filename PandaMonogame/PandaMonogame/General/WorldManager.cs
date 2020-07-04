using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PandaMonogame.WorldManager2DTile
{
    public class TileData
    {
        public string Name { get; set; }
        public int SheetIndex { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    } // TileData

    public class Tilesheet
    {
        public string Name { get; set; }
        public Dictionary<string, TileData> Tiles { get; set; }

        public AnimatedSprite TextureHandler { get; set; }

        public Tilesheet(Texture2D texture, int tileWidth, int tileHeight)
        {
            TextureHandler = new AnimatedSprite(texture, tileWidth, tileHeight);
            Tiles = new Dictionary<string, TileData>();
        }

    } // Tilesheet

    public class WorldManager2DTile
    {
        #region singleton boilerplate
        private static volatile WorldManager2DTile _instance;
        private static readonly object _syncRoot = new Object();

        private WorldManager2DTile() { }

        public static WorldManager2DTile Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new WorldManager2DTile();
                    }
                }

                return _instance;
            }
        }
        #endregion

        public int TileWidth { get; set; } = 0;
        public int TileHeight { get; set; } = 0;
        public Dictionary<string, Tilesheet> Tilesheets { get; set; } = new Dictionary<string, Tilesheet>();

        public void Load(GraphicsDevice graphics, ContentManager Content, string sheetsAssetName)
        {
            using (var fs = ModManager.Instance.AssetManager.GetFileStreamByAsset(sheetsAssetName))
            {
                XDocument doc = XDocument.Load(fs);

                XElement tilesheetsRoot = doc.Element("tilesheets");
                XElement tileDimensions = tilesheetsRoot.Element("tiledimensions");

                TileWidth = int.Parse(tileDimensions.Element("width").Value);
                TileHeight = int.Parse(tileDimensions.Element("height").Value);

                List<XElement> tilesheetElements = tilesheetsRoot.Elements("tilesheet").ToList();

                foreach (var tilesheetElement in tilesheetElements)
                {
                    int currentSheetIndex = 0;

                    Tilesheet newSheet = new Tilesheet(ModManager.Instance.AssetManager.LoadTexture2D(graphics, tilesheetElement.Element("assetname").Value),
                                                        TileWidth, TileHeight);

                    newSheet.Name = tilesheetElement.Element("name").Value;

                    List<XElement> tileElements = tilesheetElement.Element("tiles").Elements("tile").ToList();

                    int currentTileX = 0;
                    int currentTileY = 0;

                    foreach (var tileElement in tileElements)
                    {
                        TileData newTile = new TileData()
                        {
                            Name = tileElement.Attribute("name").Value,
                            X = currentTileX,
                            Y = currentTileY,
                            SheetIndex = currentSheetIndex
                        };

                        newSheet.Tiles.Add(newTile.Name, newTile);

                        currentSheetIndex += 1;
                        currentTileX += TileWidth;

                        if (currentTileX >= newSheet.TextureHandler.Width)
                        {
                            currentTileX = 0;
                            currentTileY += TileHeight;
                        }
                    } // foreach

                    Tilesheets.Add(newSheet.Name, newSheet);

                    if (PandaMonogameConfig.Logging)
                        Console.WriteLine("Loaded tilesheet " + newSheet.Name + " with " + newSheet.Tiles.Count.ToString() + " tiles.");
                } // foreach

                if (PandaMonogameConfig.Logging)
                    Console.WriteLine("WorldManager loaded tilesheets from tilesheets.xml");
            }
        } // load

        public void Generate(string scriptAssetName)
        {
            ScriptEngine engine = Python.CreateEngine();
            engine.Runtime.IO.RedirectToConsole();

            ScriptScope scope = engine.CreateScope();
            scope.SetVariable("WorldManager", this);
            scope.SetVariable("worldSeed", SettingsManager.Instance.Sections["world"].Settings["seed"].Value);

            ScriptSource source = engine.CreateScriptSourceFromFile(ModManager.Instance.AssetManager.GetAssetPath(scriptAssetName), Encoding.Unicode, SourceCodeKind.Statements);
            CompiledCode compiled = source.Compile();

            dynamic result = compiled.Execute(scope);

            if (PandaMonogameConfig.Logging)
                Console.WriteLine("WorldManager generated world from Asset " + scriptAssetName);
        } // generate
    } // WorldManager
}
