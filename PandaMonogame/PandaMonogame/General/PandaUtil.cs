using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    public static class PandaUtil
    {
        public static void Cleanup()
        {
            LayeredSprite.Cleanup();
        }

        // https://stackoverflow.com/questions/3502493/is-there-any-generic-parse-function-that-will-convert-a-string-to-any-type-usi/3502523
        public static T ConvertTo<T>(this object value)
        {
            T returnValue;

            if (value is T variable)
                returnValue = variable;
            else
                try
                {
                    //Handling Nullable types i.e, int?, double?, bool? .. etc
                    if (Nullable.GetUnderlyingType(typeof(T)) != null)
                    {
                        TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                        returnValue = (T)conv.ConvertFrom(value);
                    }
                    else
                    {
                        returnValue = (T)Convert.ChangeType(value, typeof(T));
                    }
                }
                catch (Exception)
                {
                    returnValue = default;
                }
            
            return returnValue;
        } // ConvertTo

        public static bool Vector2IntCompare(Vector2 first, Vector2 second)
        {
            if ((int)first.X != (int)second.X)
                return false;

            if ((int)first.Y != (int)second.Y)
                return false;

            return true;
        } // Vector2IntCompare
    }
}
