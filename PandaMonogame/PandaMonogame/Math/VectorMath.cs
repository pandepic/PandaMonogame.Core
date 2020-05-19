using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PandaMonogame
{
    public enum BasicDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public sealed class VectorMath
    {
        #region singleton boilerplate
        private static volatile VectorMath _instance;
        private static readonly object _syncRoot = new Object();

        private VectorMath() { }

        public static VectorMath Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new VectorMath();
                    }
                }

                return _instance;
            }
        }
        #endregion

        public float DistanceBetweenVectors2D(Vector2 vector1, Vector2 vector2)
        {
            return DistanceBetweenVectors2D(vector1.X, vector1.Y, vector2.X, vector2.Y);
        }

        public float DistanceBetweenVectors2D(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow((double)x2 - (double)x1, 2) + Math.Pow((double)y2 - (double)y1, 2));
        }

        public BasicDirection GetBasicHorizontalDirection2D(Vector2 vector1, Vector2 vector2)
        {
            if (vector1.X < vector2.X)
                return BasicDirection.Right;

            if (vector1.X > vector2.X)
                return BasicDirection.Left;

            return BasicDirection.None;
        }

        public BasicDirection GetBasicVerticalDirection2D(Vector2 vector1, Vector2 vector2)
        {
            if (vector1.Y > vector2.Y)
                return BasicDirection.Up;

            if (vector1.Y < vector2.Y)
                return BasicDirection.Down;

            return BasicDirection.None;
        }
    }
}
