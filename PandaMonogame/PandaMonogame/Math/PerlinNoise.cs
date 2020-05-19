using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpNeat.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//https://stackoverflow.com/questions/8659351/2d-perlin-noise

namespace PandaMonogame
{
    public static class Noise2d
    {
        private static FastRandom _random = null;
        private static int[] _permutation;

        private static Vector2[] _gradients;

        static Noise2d()
        {
            //CalculatePermutation(out _permutation);
            //CalculateGradients(out _gradients);
        }

        public static void Init(int seed)
        {
            _random = new FastRandom(seed);

            CalculatePermutation(out _permutation);
            CalculateGradients(out _gradients);
        }

        private static void CalculatePermutation(out int[] p)
        {
            p = Enumerable.Range(0, 256).ToArray();

            /// shuffle the array
            for (var i = 0; i < p.Length; i++)
            {
                var source = _random.Next(p.Length);

                var t = p[i];
                p[i] = p[source];
                p[source] = t;
            }
        }

        /// <summary>
        /// generate a new permutation.
        /// </summary>
        public static void Reseed()
        {
            //CalculatePermutation(out _permutation);
        }

        private static void CalculateGradients(out Vector2[] grad)
        {
            grad = new Vector2[256];

            for (var i = 0; i < grad.Length; i++)
            {
                Vector2 gradient;

                do
                {
                    gradient = new Vector2((float)(_random.NextDouble() * 2 - 1), (float)(_random.NextDouble() * 2 - 1));
                }
                while (gradient.LengthSquared() >= 1);

                gradient.Normalize();

                grad[i] = gradient;
            }

        }

        private static float Drop(float t)
        {
            t = Math.Abs(t);
            return 1f - t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static float Q(float u, float v)
        {
            return Drop(u) * Drop(v);
        }

        public static float Noise(float x, float y)
        {
            var cell = new Vector2((float)Math.Floor(x), (float)Math.Floor(y));

            var total = 0f;

            var corners = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1) };

            foreach (var n in corners)
            {
                var ij = cell + n;
                var uv = new Vector2(x - ij.X, y - ij.Y);

                var index = _permutation[(int)ij.X % _permutation.Length];
                index = _permutation[(index + (int)ij.Y) % _permutation.Length];

                var grad = _gradients[index % _gradients.Length];

                total += Q(uv.X, uv.Y) * Vector2.Dot(grad, uv);
            }

            return Math.Max(Math.Min(total, 1f), -1f);
        }

        //public static float NoiseN(float x, float y)
        //{
        //    var n = Noise(x, y);
        //    var min = float.MaxValue;
        //    var max = float.MinValue;
        //    var norm = (n - min) / (max - min);
        //    return norm;
        //}

        public static float GenerateNoisePoint(int x, int y, int width, int height, int octaves, float frequency = 0.5f, float amplitude = 1f)
        {
            var min = float.MaxValue;
            var max = float.MinValue;

            float noise = 0;

            for (var octave = 0; octave < octaves; octave++)
            {
                var n = Noise2d.Noise(x * frequency * 1f / width, y * frequency * 1f / height);
                noise += n * amplitude;

                min = Math.Min(min, noise);
                max = Math.Max(max, noise);

                frequency *= 2;
                amplitude /= 2;
            }

            var norm = (noise - min) / (max - min);
            return norm;
        }

        public static float[,] GenerateNoiseMap(int width, int height, int octaves, float frequency = 0.5f, float amplitude = 1f)
        {
            var data = new float[width * height];

            /// Track min and max noise value. Used to normalize the result to the 0 to 1.0 range.
            var min = float.MaxValue;
            var max = float.MinValue;

            /// Rebuild the permutation table to get a different noise pattern. 
            /// Leave this out if you want to play with changing the number of octaves while 
            /// maintaining the same overall pattern.
            //Noise2d.Reseed();

            for (var octave = 0; octave < octaves; octave++)
            {
                /// parallel loop - easy and fast.
                Parallel.For(0
                    , width * height
                    , (offset) =>
                    {
                        var i = offset % width;
                        var j = offset / width;
                        var noise = Noise2d.Noise(i * frequency * 1f / width, j * frequency * 1f / height);
                        noise = data[j * width + i] += noise * amplitude;

                        min = Math.Min(min, noise);
                        max = Math.Max(max, noise);

                    }
                );

                frequency *= 2;
                amplitude /= 2;
            }

            var r = new float[width, height];
            var c = 0;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var norm = (data[c] - min) / (max - min);

                    r[x, y] = norm;

                    //r[x, y] = data[c];

                    c += 1;
                }
            }

            return r;
        }

        public static void GenerateNoiseMapTexture(GraphicsDevice Device, int width, int height, ref Texture2D noiseTexture, int octaves)
        {
            var data = new float[width * height];

            /// track min and max noise value. Used to normalize the result to the 0 to 1.0 range.
            var min = float.MaxValue;
            var max = float.MinValue;

            /// rebuild the permutation table to get a different noise pattern. 
            /// Leave this out if you want to play with changing the number of octaves while 
            /// maintaining the same overall pattern.
            Noise2d.Reseed();

            var frequency = 0.5f;
            var amplitude = 1f;

            for (var octave = 0; octave < octaves; octave++)
            {
                /// parallel loop - easy and fast.
                Parallel.For(0
                    , width * height
                    , (offset) =>
                    {
                        var i = offset % width;
                        var j = offset / width;
                        var noise = Noise2d.Noise(i * frequency * 1f / width, j * frequency * 1f / height);
                        noise = data[j * width + i] += noise * amplitude;

                        min = Math.Min(min, noise);
                        max = Math.Max(max, noise);

                    }
                );

                frequency *= 2;
                amplitude /= 2;
            }


            if (noiseTexture != null && (noiseTexture.Width != width || noiseTexture.Height != height))
            {
                noiseTexture.Dispose();
                noiseTexture = null;
            }
            if (noiseTexture == null)
            {
                noiseTexture = new Texture2D(Device, width, height, false, SurfaceFormat.Color);
            }

            var colors = data.Select(
                (f) =>
                {
                    var norm = (f - min) / (max - min);
                    return new Microsoft.Xna.Framework.Color(norm, norm, norm, 1);
                }
            ).ToArray();

            noiseTexture.SetData(colors);
        }
    }
}
