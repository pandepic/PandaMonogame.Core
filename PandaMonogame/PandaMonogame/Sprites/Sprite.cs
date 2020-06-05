using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PandaMonogame
{
    public enum FadeDirection
    {
        None = 0,
        FadeIn = 1,
        FadeOut = -1
    };

    public enum RotationDirection
    {
        None = 0,
        Clockwise = 1,
        CounterClockwise = -1
    };

    public enum ScaleDirection
    {
        None = 0,
        ScaleUp = 1,
        ScaleDown = -1
    }

    public class Sprite
    {
        public static readonly float FLASH_FOREVER = -1.0f;

        public Texture2D Texture { get; set; } = null;

        public float Scale { get; set; } = 1.0f;
        public float Rotation { get; set; } = 0.0f;
        public float Depth { get; set; } = 0.0f;

        public bool Visible { get; set; } = true;

        public SpriteEffects SpriteEffectsFlip { get; set; } = SpriteEffects.None;

        protected Color _colour = Color.White;
        public Color Colour
        {
            get { return _colour; }
            set { _colour = value; }
        }

        protected Vector2 _position = Vector2.Zero;
        public Vector2 Position { get => _position; set => _position = value; }

        protected Vector2 _drawPosition = Vector2.Zero;
        public Vector2 DrawPosition { get => _drawPosition; set => _drawPosition = value; }

        protected Vector2 _center = Vector2.Zero;
        public Vector2 Center { get => _center; set => _center = value; }

        protected Rectangle _sourceRect = Rectangle.Empty;
        public Rectangle SourceRect { get => _sourceRect; set => _sourceRect = value; }

        protected FadeDirection _fadeDirection = FadeDirection.None;
        protected float _targetTransparency;
        protected float _fadeTransparency = 0.0f;
        protected float _fadeChangePerSecond = 0.0f;
        public bool IsFading { get; set; } = false;

        protected RotationDirection _rotationDirection = RotationDirection.None;
        protected float _rotationChangePerSecond = 0.0f;
        protected float _rotationAmount = 0.0f;
        public bool IsRotating { get; set; } = false;

        protected ScaleDirection _scaleDirection = ScaleDirection.None;
        protected float _scaleChangePerSecond = 0.0f;
        protected float _targetScale;
        public bool IsScaling { get; set; } = false;

        public bool IsFlashing { get; set; } = false;
        protected bool _flashingDrawFlag = false;
        protected float _flashingDuration = 0.0f;
        protected float _totalFlashingTime = 0.0f;
        protected float _flashingInterval = 0.0f;
        protected float _currentFlashInterval = 0.0f;

        public int Width
        {
            get { return (Texture == null ? 0 : Texture.Width); }
        }

        public int Height
        {
            get { return (Texture == null ? 0 : Texture.Height); }
        }

        public Sprite() { }

        public Sprite(Texture2D texture)
        {
            LoadTexture(texture);
        }

        ~Sprite() { }

        public void LoadTexture(Texture2D texture)
        {
            this.Texture = texture;

            if (texture != null)
            {
                Center = new Vector2(texture.Width / 2, texture.Height / 2);
                SourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                if (Texture == null)
                    return;

                if (Visible == false)
                    return;

                if (IsFlashing == true && _flashingDrawFlag == false)
                    return;

                _drawPosition.X = (int)Position.X;
                _drawPosition.Y = (int)Position.Y;

                spriteBatch.Draw(
                        Texture,
                        DrawPosition,
                        SourceRect,
                        Colour,
                        Rotation,
                        Center,
                        Scale,
                        SpriteEffectsFlip,
                        Depth
                        );
            }
            catch (Exception e)
            {
                if (PandaMonogameConfig.Logging)
                    Console.WriteLine("Error drawing sprite: " + e.Message);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            _position = position;
            Draw(spriteBatch);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Texture == null)
                return;

            UpdateFade(gameTime);
            UpdateRotation(gameTime);
            UpdateScaling(gameTime);
            UpdateFlashingEffect(gameTime);
        }

        public void SetTransparency(byte alpha)
        {
            _colour.A = alpha;
        }

        public void BeginFlashingEffect(float interval, float duration)
        {
            if (interval <= 0.0f)
                return;

            if (duration <= 0 && duration != FLASH_FOREVER)
                return;

            _flashingInterval = interval;
            _flashingDuration = duration;
            _currentFlashInterval = 0.0f;
            _totalFlashingTime = 0.0f;
            IsFlashing = true;
            _flashingDrawFlag = false;
        }

        public void UpdateFlashingEffect(GameTime gameTime)
        {
            if (!IsFlashing)
                return;

            _currentFlashInterval += gameTime.ElapsedGameTime.Milliseconds;
            _totalFlashingTime += gameTime.ElapsedGameTime.Milliseconds;

            if (_currentFlashInterval >= _flashingInterval)
            {
                _currentFlashInterval -= _flashingInterval;

                if (_flashingDrawFlag == true)
                    _flashingDrawFlag = false;
                else
                    _flashingDrawFlag = true;
            }

            if (_totalFlashingTime >= _flashingDuration && _flashingDuration != FLASH_FOREVER)
            {
                IsFlashing = false;

                return;
            }
        }

        public void BeginScalingEffect(float targetScale, float duration)
        {
            if (targetScale == Scale)
                return;

            _scaleDirection = ScaleDirection.ScaleUp;

            if (targetScale < Scale)
                _scaleDirection = ScaleDirection.ScaleDown;

            float amount = Scale - targetScale;
            if (amount < 0)
                amount *= -1;

            _scaleChangePerSecond = amount / (duration / 1000.0f);

            _targetScale = targetScale;

            IsScaling = true;
        }

        private void UpdateScaling(GameTime gameTime)
        {
            if (_scaleDirection == ScaleDirection.None)
                return;

            Scale += (_scaleChangePerSecond * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f)) * (float)_scaleDirection;

            if (_scaleDirection == ScaleDirection.ScaleUp && Scale >= _targetScale)
            {
                _scaleDirection = ScaleDirection.None;
                Scale = _targetScale;
                IsScaling = false;
            }
            else if (_scaleDirection == ScaleDirection.ScaleDown && Scale <= _targetScale)
            {
                _scaleDirection = ScaleDirection.None;
                Scale = _targetScale;
                IsScaling = false;
            }
        }

        public void BeginRotationEffect(float rotationAmount, float duration)
        {
            if (rotationAmount == 0)
                return;

            _rotationDirection = RotationDirection.Clockwise;

            float amount = rotationAmount;
            if (amount < 0)
            {
                amount *= -1;

                _rotationDirection = RotationDirection.CounterClockwise;
            }

            _rotationChangePerSecond = amount / (duration / 1000.0f);

            _rotationAmount = amount;

            IsRotating = true;
        }

        private void UpdateRotation(GameTime gameTime)
        {
            if (_rotationDirection == RotationDirection.None)
                return;

            float change = _rotationChangePerSecond * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

            if (_rotationAmount - change < 0)
                change = _rotationAmount;

            Rotation += change * (float)_rotationDirection;

            _rotationAmount -= change;

            if (_rotationAmount <= 0)
            {
                _rotationDirection = RotationDirection.None;
                IsRotating = false;
            }
        }

        public void BeginFadeEffect(float targetTransparency, float duration)
        {
            if ((byte)targetTransparency == Colour.A)
                return;

            _fadeDirection = FadeDirection.FadeIn;

            if (targetTransparency < (float)Colour.A)
                _fadeDirection = FadeDirection.FadeOut;

            _fadeTransparency = Colour.A;

            float totalFadeChange = targetTransparency - (float)Colour.A;
            if (totalFadeChange < 0)
                totalFadeChange *= -1;

            _fadeChangePerSecond = totalFadeChange / (duration / 1000.0f);

            _targetTransparency = targetTransparency;

            IsFading = true;
        }

        private void UpdateFade(GameTime gameTime)
        {
            if (_fadeDirection == FadeDirection.None)
                return;

            _fadeTransparency += (_fadeChangePerSecond * ((float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f)) * (float)_fadeDirection;

            if (_fadeDirection == FadeDirection.FadeIn && _fadeTransparency >= _targetTransparency)
            {
                _fadeTransparency = _targetTransparency;
                _fadeDirection = FadeDirection.None;
                IsFading = false;
            }
            else if (_fadeDirection == FadeDirection.FadeOut && _fadeTransparency <= _targetTransparency)
            {
                _fadeTransparency = _targetTransparency;
                _fadeDirection = FadeDirection.None;
                IsFading = false;
            }

            _colour.A = (byte)_fadeTransparency;
        }

        public bool PointInsideSprite(Vector2 position, Vector2 point)
        {
            Rectangle boundingRect = new Rectangle((int)position.X, (int)position.Y, SourceRect.Width, SourceRect.Height);

            if (boundingRect.Width == 0 && boundingRect.Height == 0)
                return false;

            if (point.X < boundingRect.X)
                return false;
            if (point.Y < boundingRect.Y)
                return false;
            if (point.X > (boundingRect.X + boundingRect.Width))
                return false;
            if (point.Y > (boundingRect.Y + boundingRect.Height))
                return false;

            return true;
        }
    }
}