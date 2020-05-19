using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PandaMonogame
{
    public class AnimatedSprite : Sprite
    {
        public const int LOOP_FOREVER = -1;

        public int FrameWidth { get; set; } = 0;
        public int FrameHeight { get; set; } = 0;
        public int TotalFrames { get; set; } = 0;

        public Animation CurrentAnimation { get; set; } = null;
        public int CurrentFrame { get; set; } = 1;
        public int CurrentFrameIndex { get; set; } = 0;

        protected float _currentFrameTime = 0.0f;
        protected float _timePerFrame = 0.0f;
        protected int _animationLoopCount = 0;

        public AnimatedSprite() { }

        public AnimatedSprite(Texture2D texture, int frameWidth, int frameHeight, int defaultFrame = 1)
        {
            Init(texture, frameWidth, frameHeight, defaultFrame);
        }

        ~AnimatedSprite() { }

        public void Init(Texture2D texture, int frameWidth, int frameHeight, int defaultFrame = 1)
        {
            if (texture == null)
                return;

            base.LoadTexture(texture);

            FrameWidth = frameWidth;
            FrameHeight = frameHeight;

            _center = Vector2.Zero;

            TotalFrames = (texture.Width / frameWidth) * (texture.Height / frameHeight);

            SetFrame(defaultFrame);
        }

        public void PlayAnimation(Animation anim, int loopCount = -1)
        {
            if (anim.Frames.Count <= 0)
                return;

            CurrentAnimation = anim;

            CurrentFrameIndex = 0;
            _currentFrameTime = 0.0f;
            _timePerFrame = anim.Duration / (float)anim.Frames.Count;

            _animationLoopCount = loopCount;

            SetFrame((int)CurrentAnimation.Frames[CurrentFrameIndex]);

            this.SpriteEffectsFlip = anim.SpriteEffectsFlip;
        }

        public void StopAnimation()
        {
            CurrentAnimation = null;

            SetFrame(1);
            SpriteEffectsFlip = SpriteEffects.None;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (Texture == null)
                return;

            base.Update(gameTime);

            if (CurrentAnimation != null)
            {
                _currentFrameTime += (float)gameTime.ElapsedGameTime.Milliseconds;

                if (_currentFrameTime >= _timePerFrame)
                {
                    _currentFrameTime -= _timePerFrame;

                    CurrentFrameIndex++;

                    if (CurrentFrameIndex >= CurrentAnimation.Frames.Count)
                    {
                        if (_animationLoopCount == LOOP_FOREVER || _animationLoopCount > 0)
                        {
                            CurrentFrameIndex = 0;

                            if (_animationLoopCount != LOOP_FOREVER)
                                _animationLoopCount--;

                            SetFrame((int)CurrentAnimation.Frames[CurrentFrameIndex]);
                        }
                        else if (_animationLoopCount <= 0)
                        {
                            //setFrame(1);
                            CurrentAnimation = null;
                        }
                    }
                    else
                    {
                        SetFrame((int)CurrentAnimation.Frames[CurrentFrameIndex]);
                    }
                }
            }
        }

        public void SetFrame(int frame)
        {
            if (frame < 1 || frame > TotalFrames)
                return;

            _sourceRect.X = ((frame - 1) % (Texture.Width / FrameWidth)) * FrameWidth;
            _sourceRect.Y = ((frame - 1) / (Texture.Width / FrameWidth)) * FrameHeight;
            _sourceRect.Width = FrameWidth;
            _sourceRect.Height = FrameHeight;

            CurrentFrame = frame;
        }
    }
}
