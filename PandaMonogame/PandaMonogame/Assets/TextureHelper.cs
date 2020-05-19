using Microsoft.Xna.Framework.Graphics;
using System;

namespace PandaMonogame
{
    class TextureHelper
    {
        #region singleton boilerplate
        private static volatile TextureHelper instance;
        private static readonly object syncRoot = new Object();

        private TextureHelper() { }

        public static TextureHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new TextureHelper();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        //public void SaveTextureData(RenderTarget2D texture, string filename)
        //{
        //    byte[] imageData = new byte[4 * texture.Width * texture.Height];
        //    texture.GetData<byte>(imageData);

        //    using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(texture.Width, texture.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
        //    {
        //        System.Drawing.Imaging.BitmapData bmData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, texture.Width, texture.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
        //        IntPtr pnative = bmData.Scan0;
        //        System.Runtime.InteropServices.Marshal.Copy(imageData, 0, pnative, 4 * texture.Width * texture.Height);
        //        bitmap.UnlockBits(bmData);
        //        bitmap.Save(filename);
        //    }
        //}
    }
}
