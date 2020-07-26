using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace PandaMonogame
{
    public class Animation
    {
        public static readonly int NO_ENDFRAME = -1;

        public List<int> Frames { get; set; } = new List<int>();
        public int EndFrame { get; set; } = NO_ENDFRAME;
        public float Duration { get; set; } = 0.0f;
        public string Name { get; set; } = "";
        public bool Paused { get; set; } = false;
        public SpriteEffects SpriteEffectsFlip { get; set; } = SpriteEffects.None;

        public Animation()
        {

        }

        public Animation(int min, int max, float duration)
        {
            Duration = duration;
            FrameSetRange(min, max);
        }

        public Animation(Animation anim)
        {
            Frames.AddRange(anim.Frames);
            Duration = anim.Duration;
            Name = anim.Name;
            Paused = anim.Paused;
        }

        public void FrameSetRange(int min, int max)
        {
            if (max < min)
                return;

            Frames.Clear();

            for (int i = min; i <= max; i++)
            {
                Frames.Add(i);
            }
        }
    }
}
