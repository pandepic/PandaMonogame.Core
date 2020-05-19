using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PandaMonogame
{
    public class AnimationManager
    {
        protected Dictionary<string, Animation> _animations = new Dictionary<string, Animation>();

        public AnimationManager()
        {

        }

        public Animation GetAnimation(string name)
        {
            if (_animations.ContainsKey(name))
                return _animations[name];

            return null;
        }

        public void AddAnimation(string name, Animation animation)
        {
            if (_animations.ContainsKey(name))
                return;

            _animations.Add(name, animation);
        }

        public void RemoveAnimation(string name)
        {
            if (_animations.ContainsKey(name))
                _animations.Remove(name);
        }

        public void Clear()
        {
            _animations.Clear();
        }
    }
}
