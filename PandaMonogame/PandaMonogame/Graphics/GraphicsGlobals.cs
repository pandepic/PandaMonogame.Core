using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandaMonogame
{
    public class GraphicsGlobals
    {
        public static RenderTarget2D DefaultRenderTarget = null;
        public static int TargetResolutionWidth;
        public static int TargetResolutionHeight;
        public static FixedResolutionTarget CurrentFixedResolutionTarget = null;
        public static GraphicsDevice GraphicsDevice = null;

        public static void Setup(GraphicsDevice graphics)
        {
            GraphicsDevice = graphics;
            TargetResolutionWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            TargetResolutionHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
        }
    }
}
