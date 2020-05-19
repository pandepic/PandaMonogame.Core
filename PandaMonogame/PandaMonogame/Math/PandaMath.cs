using Microsoft.Xna.Framework;
using System;
using System.Security.Cryptography;
using System.Text;

namespace PandaMonogame
{
    public static class PandaMath
    {
        public static int GetSeedFromString(string str)
        {
            using (var mySHA256 = SHA256Managed.Create())
            {
                byte[] toHashBytes = Encoding.UTF8.GetBytes(str);
                var hashValue = mySHA256.ComputeHash(toHashBytes);
                var hashValueStr = "";

                foreach (byte b in hashValue)
                {
                    hashValueStr += b.ToString("x2");
                }

                int hashInt = Convert.ToInt32(hashValueStr.Substring(0, 8), 16);

                return hashInt;
            }
        } // getSeedFromString

        // https://denisrizov.com/2016/06/02/bezier-curves-unity-package-included/
        // https://javascript.info/bezier-curve
        public static Vector2 GetPointOnBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            float u = 1f - t;
            float t2 = t * t;
            float u2 = u * u;
            float u3 = u2 * u;
            float t3 = t2 * t;

            Vector2 result =
                (u3) * p0 +
                (3f * u2 * t) * p1 +
                (3f * u * t2) * p2 +
                (t3) * p3;

            return result;
        } // getPointOnBezierCurve

        public static Vector2 GetPointOnBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            float u = 1f - t;
            float t2 = t * t;
            float u2 = u * u;

            Vector2 result =
                (u2) * p0 +
                (2f * u * t) * p1 +
                (t2) * p2;

            return result;
        } // getPointOnBezierCurve
    }
}
