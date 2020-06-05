using PandaMonogame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    public static class PandaMonogameConfig
    {
        public static bool Logging = false;
        public static int UISoundType = -1;

        public static Dictionary<Type, string> UIWidgetTypes { get; set; } = new Dictionary<Type, string>()
        {
            { typeof(PUIWBasicButton), "Button" },
            { typeof(PUIWLabel), "Label" },
            { typeof(PUIWTextBox), "Textbox" },
            { typeof(PUIWImageBox), "ImageBox" },
            { typeof(PUIWHScrollBar), "HScrollBar" },
            { typeof(PUIWHProgressBar), "HProgressBar" },
        };

        public static void RegisterWidgetType(Type type, string elementName)
        {
            if (UIWidgetTypes.ContainsKey(type))
                throw new Exception("Type already exists.");

            if (UIWidgetTypes.ContainsValue(elementName))
                throw new Exception("Element name already exists.");

            UIWidgetTypes.Add(type, elementName);
        }
    }
}
