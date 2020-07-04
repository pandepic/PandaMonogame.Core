﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PandaMonogame
{
    public class Mod
    {
        public string Name { get; set; }
        public bool Ignore { get; set; }
    }

    public class ModManager
    {
        #region singleton boilerplate
        private static volatile ModManager _instance;
        private static readonly object _syncRoot = new Object();

        private ModManager() { }

        public static ModManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new ModManager();
                    }
                }

                return _instance;
            }
        }
        #endregion

        protected ContentManager Content { get; set; }

        protected string _modDirectory = "";
        public string ModDirectory { get => _modDirectory; set => _modDirectory = value; }

        protected string _assetsFileName = "";
        public string AssetsFileName { get => _assetsFileName; set => _assetsFileName = value; }

        protected string _modListFileName = "";
        public string ModListFileName { get => _modListFileName; set => _modListFileName = value; }

        protected List<Mod> _loadedMods = new List<Mod>();
        public List<Mod> LoadedMods { get => _loadedMods; }
        
        protected AssetManager _assetManager = new AssetManager();
        public AssetManager AssetManager { get => _assetManager; }
        
        protected SoundManager _soundManager = new SoundManager();
        public SoundManager SoundManager { get => _soundManager; }
        
        public void Init(ContentManager _Content)
        {
            Content = _Content;
            AssetManager.SoundManager = _soundManager;
        }

        public void LoadList(string modDirectory, string modListFileName, string assetsFileName)
        {
            _loadedMods.Clear();

            using (var fs = AssetManager.GetFileStream(modDirectory + "\\" + modListFileName))
            {
                XDocument modList = XDocument.Load(fs);

                List<Mod> xmlMods = (from XElement mod in modList.Root.Elements("mod")
                                     select new Mod()
                                     {
                                         Name = mod.Attribute("name").Value,
                                         Ignore = bool.Parse(mod.Attribute("ignore").Value)
                                     }).ToList();

                _modDirectory = modDirectory;
                _assetsFileName = assetsFileName;
                _modListFileName = modListFileName;

                List<string> modDirectories = Directory.GetDirectories(modDirectory).ToList();

                foreach (var mod in xmlMods)
                {
                    string modName = mod.Name.Replace(modDirectory + "\\", "");
                    string modPath = modDirectory + "\\" + modName;

                    if (modDirectories.Contains(modPath) == true)
                    {
                        _loadedMods.Add(mod);
                    } // if
                } // foreach

                foreach (var mod in modDirectories)
                {
                    string modName = mod.Replace(modDirectory + "\\", "");

                    if (_loadedMods.Where(m => m.Name == modName).Count() == 0)
                    {
                        _loadedMods.Add(new Mod()
                        {
                            Name = modName,
                            Ignore = false
                        });
                    } // if
                } // foreach
            }

            SaveList();

        } // loadList

        public void SaveList()
        {
            XDocument modListFile = new XDocument();

            modListFile.Add(new XElement("mods"));

            foreach (var mod in _loadedMods)
            {
                XElement modElement = new XElement("mod");
                modElement.SetAttributeValue("name", mod.Name);
                modElement.SetAttributeValue("ignore", mod.Ignore.ToString());

                modListFile.Root.Add(modElement);
            } // foreach

            modListFile.Save(_modDirectory + "\\" + _modListFileName);
        }

        public void ImportAssets()
        {
            if (PandaMonogameConfig.Logging)
                Console.WriteLine("Importing mods");

            foreach (var mod in _loadedMods.Where(m => m.Ignore == false))
            {
                _assetManager.Import(_modDirectory + "\\" + mod.Name + "\\" + _assetsFileName, _modDirectory + "\\" + mod.Name);
                if (PandaMonogameConfig.Logging)
                    Console.WriteLine("Mod imported: " + mod.Name);
            } // foreach
        } // importAssets
    }
}
