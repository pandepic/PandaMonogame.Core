using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PandaMonogame
{
    public class Setting
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Dictionary<string, string> OtherAttributes { get; set; }
    } // Setting

    public class SettingsSection
    {
        public string Name { get; set; }
        public Dictionary<string, Setting> Settings { get; set; }

        public SettingsSection()
        {
            Settings = new Dictionary<string, Setting>();
        }
    } // SettingsSection

    public class SettingsManager
    {
        #region singleton boilerplate
        private static volatile SettingsManager _instance;
        private static readonly object _syncRoot = new Object();

        private SettingsManager() { }

        public static SettingsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new SettingsManager();
                    }
                }

                return _instance;
            }
        }
        #endregion

        public Dictionary<string, SettingsSection> Sections { get; set; } = new Dictionary<string, SettingsSection>();

        public void Load(string filepath)
        {
            if (PandaMonogameConfig.Logging)
                Console.WriteLine("Importing settings: " + filepath);

            Sections.Clear();

            using (var fs = ModManager.Instance.AssetManager.GetFileStream(filepath))
            {
                XDocument doc = XDocument.Load(fs);
                XElement settingsRoot = doc.Element("Settings");
                List<XElement> docSections = settingsRoot.Elements("Section").ToList();

                foreach (var docSection in docSections)
                {
                    SettingsSection section = new SettingsSection
                    {
                        Name = docSection.Attribute("Name").Value
                    };

                    if (PandaMonogameConfig.Logging)
                        Console.WriteLine("Settings section [" + section.Name + "]");

                    List<XElement> sectionSettings = docSection.Elements("Setting").ToList();

                    foreach (var sectionSetting in sectionSettings)
                    {
                        var newSetting = new Setting()
                        {
                            Name = sectionSetting.Attribute("Name").Value,
                            Value = sectionSetting.Attribute("Value").Value,
                            OtherAttributes = new Dictionary<string, string>(),
                        };

                        foreach (var att in sectionSetting.Attributes())
                        {
                            if (att.Name != "Name" && att.Name != "Value")
                                newSetting.OtherAttributes.Add(att.Name.ToString(), att.Value);
                        }

                        section.Settings.Add(sectionSetting.Attribute("Name").Value, newSetting);

                        if (PandaMonogameConfig.Logging)
                            Console.WriteLine("[" + section.Name + "] Setting added: " + newSetting.Name + " - " + newSetting.Value);
                    } // foreach

                    Sections.Add(section.Name, section);

                } // foreach

                if (PandaMonogameConfig.Logging)
                    Console.WriteLine("Finished importing settings: " + filepath);
            }
        } // load

        public void Save(string filepath)
        {
            if (PandaMonogameConfig.Logging)
                Console.WriteLine("Saving settings: " + filepath);

            XDocument doc = new XDocument();

            XElement root = new XElement("Settings");

            foreach (var section in Sections)
            {
                XElement xSection = new XElement("Section");
                xSection.SetAttributeValue("Name", section.Value.Name);

                foreach (var setting in section.Value.Settings)
                {
                    XElement xSetting = new XElement("Setting");
                    xSetting.SetAttributeValue("Name", setting.Value.Name);
                    xSetting.SetAttributeValue("Value", setting.Value.Value);

                    foreach (var kvp in setting.Value.OtherAttributes)
                        xSetting.SetAttributeValue(kvp.Key, kvp.Value);

                    xSection.Add(xSetting);
                } // foreach

                root.Add(xSection);
            } // foreach

            // root node
            doc.Add(root);

            doc.Save(filepath);
        } // save

        public T GetSetting<T>(string section, string name)
        {
            var setting = Sections[section].Settings[name].Value;

            return PandaUtil.ConvertTo<T>(setting);
        } // getSetting

        public string UpdateSetting(string section, string name, string value)
        {
            return Sections[section].Settings[name].Value = value;
        } // updateSetting

        public List<Setting> GetSettings(string section)
        {
            var settings = new List<Setting>();

            foreach (var kvp in Sections[section].Settings)
                settings.Add(kvp.Value);

            return settings;
        }

    } // SettingsManager
}
