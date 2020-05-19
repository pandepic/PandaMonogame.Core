using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    public class IntXY
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IntXY() { }
        public IntXY(int x, int y) { X = x; Y = y; }
        public IntXY(Vector2 vector) { X = (int)vector.X; Y = (int)vector.Y; }

        public static bool operator ==(IntXY a, IntXY b) { return (a.X == b.X) && (a.Y == b.Y); }
        public static bool operator !=(IntXY a, IntXY b) { return (a.X != b.X) || (a.Y != b.Y); }
        public static bool operator ==(IntXY a, Vector2 b) { return (a.X == (int)b.X) && (a.Y == (int)b.Y); }
        public static bool operator !=(IntXY a, Vector2 b) { return (a.X != (int)b.X) || (a.Y != (int)b.Y); }
        public static IntXY operator +(IntXY a, IntXY b) { return new IntXY(a.X + b.X, a.Y + b.Y); }
        public static IntXY operator +(IntXY a, Vector2 b) { return new IntXY(a.X + (int)b.X, a.Y + (int)b.Y); }
        public static IntXY operator -(IntXY a, IntXY b) { return new IntXY(a.X - b.X, a.Y - b.Y); }
        public static IntXY operator -(IntXY a, Vector2 b) { return new IntXY(a.X - (int)b.X, a.Y - (int)b.Y); }
        public static IntXY operator *(IntXY a, IntXY b) { return new IntXY(a.X * b.X, a.Y * b.Y); }
        public static IntXY operator *(IntXY a, Vector2 b) { return new IntXY(a.X * (int)b.X, a.Y * (int)b.Y); }
        public static IntXY operator /(IntXY a, IntXY b) { return new IntXY(a.X / b.X, a.Y / b.Y); }
        public static IntXY operator /(IntXY a, Vector2 b) { return new IntXY(a.X / (int)b.X, a.Y / (int)b.Y); }

        public override bool Equals(object obj)
        {
            if (!(obj is IntXY))
                return false;

            return this == (IntXY)obj;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return "{" + X + "," + Y + "}";
        }

        public static IntXY Parse(string str)
        {
            var split = str.Replace("{", "").Replace("}", "").Split(',');
            return new IntXY(int.Parse(split[0]), int.Parse(split[1]));
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public Rectangle ToRectangle(int width = 0, int height = 0)
        {
            return new Rectangle(X, Y, width, height);
        }

        public void Set(int x, int y) { X = x; Y = y; }
        public void Set(Vector2 vector) { X = (int)vector.X; Y = (int)vector.Y; }
    }
}
