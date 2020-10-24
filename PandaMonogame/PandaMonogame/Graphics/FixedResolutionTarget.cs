using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandaMonogame
{
    public class FixedResolutionTarget
    {
        protected int _targetWidth;
        protected int _targetHeight;
        protected GraphicsDevice _graphics;
        protected RenderTarget2D _renderTarget;
        protected Rectangle _targetRect;
        protected bool _pixelPerfect = false;

        public Rectangle TargetRect { get => _targetRect; }

        public FixedResolutionTarget(int width, int height, GraphicsDevice graphics, bool pixelPerfect = false)
        {
            _graphics = graphics;
            _pixelPerfect = pixelPerfect;

            SetSize(width, height);
        }

        ~FixedResolutionTarget()
        {
            _renderTarget?.Dispose();
        }

        public void SetSize(int width, int height)
        {
            _targetWidth = width;
            _targetHeight = height;

            _renderTarget?.Dispose();

            _renderTarget = new RenderTarget2D(_graphics, _targetWidth, _targetHeight);
            _graphics.SetRenderTarget(_renderTarget);
            _graphics.Clear(Color.Transparent);
            _graphics.SetRenderTarget(null);

            var windowWidth = _graphics.PresentationParameters.BackBufferWidth;
            var windowHeight = _graphics.PresentationParameters.BackBufferHeight;

            _targetRect.Width = _targetWidth;
            _targetRect.Height = _targetHeight;

            if (!_pixelPerfect)
            {
                var resolutionScaleX = (float)windowWidth / (float)_targetWidth;
                var resolutionScaleY = (float)windowHeight / (float)_targetHeight;
                var scale = Math.Min(resolutionScaleX, resolutionScaleY);

                _targetRect.Width = (int)((float)_targetWidth * scale);
                _targetRect.Height = (int)((float)_targetHeight * scale);
            }
            else
            {
                var windowAspectRatio = windowWidth / windowHeight;
                var pixelPerfectScale = 1;

                if ((float)_targetWidth / (float)_targetHeight > windowAspectRatio)
                    pixelPerfectScale = windowWidth / _targetWidth;
                else
                    pixelPerfectScale = windowHeight / _targetHeight;

                if (pixelPerfectScale == 0)
                    pixelPerfectScale = 1;

                _targetRect.Width = (int)((float)_targetWidth * (float)pixelPerfectScale);
                _targetRect.Height = (int)((float)_targetHeight * (float)pixelPerfectScale);
            }

            var offset = new Vector2(0);

            if (_targetRect.Width < windowWidth)
                offset.X = windowWidth / 2 - _targetRect.Width / 2;
            if (_targetRect.Height < windowHeight)
                offset.Y = windowHeight / 2 - _targetRect.Height / 2;

            _targetRect.Location = offset.ToPoint();
        }

        public void Enable()
        {
            GraphicsGlobals.TargetResolutionWidth = _targetWidth;
            GraphicsGlobals.TargetResolutionHeight = _targetHeight;
            GraphicsGlobals.CurrentFixedResolutionTarget = this;
        }

        public void Disable()
        {
            GraphicsGlobals.TargetResolutionWidth = _graphics.PresentationParameters.BackBufferWidth;
            GraphicsGlobals.TargetResolutionHeight = _graphics.PresentationParameters.BackBufferHeight;
            GraphicsGlobals.CurrentFixedResolutionTarget = null;
        }

        public void Begin()
        {
            _graphics.SetRenderTarget(null);
            _graphics.Clear(Color.Black);
            GraphicsGlobals.DefaultRenderTarget = _renderTarget;
            _graphics.SetRenderTarget(_renderTarget);
            _graphics.Clear(Color.Black);
        }

        public void End()
        {
            GraphicsGlobals.DefaultRenderTarget = null;
            _graphics.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(_renderTarget, _targetRect, null, Color.White);
            spriteBatch.End();
        }
    }
}
