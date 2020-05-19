using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;

namespace PandaMonogame
{
    public class SoundEffectPlaying
    {
        public SoundState State
        {
            get { return _instance.State; }
        }

        public float Volume
        {
            get { return _instance.Volume; }
            set { _instance.Volume = value; }
        }

        protected SoundEffectInstance _instance = null;
        public bool Looping { get; set; } = false;
        public int Type { get; set; }
        public string AssetName { get; set; }

        public SoundEffectPlaying(string assetName, bool looping, int type, float volume, float pitch = 0f, float pan = 0f)
        {
            var sfx = ModManager.Instance.AssetManager.LoadSoundEffect(assetName);
            _instance = sfx.CreateInstance();
            _instance.IsLooped = looping;
            _instance.Volume = volume;
            _instance.Pitch = pitch;
            _instance.Pan = pan;
            
            Looping = looping;
            Type = type;
            AssetName = assetName;
        }

        public void Play() { _instance.Play(); }
        public void Stop() { _instance.Stop(); }
    } // SoundEffectPlaying

    public class SoundManager
    {
        protected Dictionary<int, float> _volumeSettings = new Dictionary<int, float>();

        public float DefaultVolume { get; set; } = 1f;
        protected int _nextID = 0;

        protected Dictionary<int, SoundEffectPlaying> _soundEffectsPlaying = new Dictionary<int, SoundEffectPlaying>();
        protected List<int> _removeList = new List<int>();

        public SoundManager()
        {

        }

        public void Update()
        {
            for (var i = 0; i < _soundEffectsPlaying.Count; i++)
            {
                var sfx = _soundEffectsPlaying.ElementAt(i);

                if (sfx.Value.State == SoundState.Stopped)
                    _removeList.Add(sfx.Key);
            }

            for (var i = 0; i < _removeList.Count; i++)
            {
                _soundEffectsPlaying.Remove(_removeList[i]);
            }

            _removeList.Clear();
        } // update

        /// <summary>
        /// Play a sound using an asset name from the asset manager.
        /// </summary>
        /// <param name="assetName">The sound file's asset name from assets.xml</param>
        /// <param name="type">The sound type, this is used to set volume.</param>
        /// <param name="loop">True if you want the sound to loop until manually stopped.</param>
        /// <param name="allowDuplicate">Do you want to allow multiple sounds from the same asset at the same time?</param>
        /// <returns>An integer sound ID on success or -1 on failure.</returns>
        public int PlaySound(string assetName, int type, bool loop = false, bool allowDuplicate = false)
        {
            if (!allowDuplicate)
            {
                for (var i = 0; i < _soundEffectsPlaying.Count; i++)
                    if (_soundEffectsPlaying.ElementAt(i).Value.AssetName == assetName)
                        return -1;
            }

            if (!_volumeSettings.ContainsKey(type))
                _volumeSettings.Add(type, DefaultVolume);

            var sfx = new SoundEffectPlaying(assetName, loop, type, _volumeSettings[type]);
            sfx.Play();

            var sfxID = _nextID;
            _nextID += 1;

            // god knows how long a game would need to run to trigger this
            if (_nextID >= (int.MaxValue - 1))
                _nextID = 0;

            _soundEffectsPlaying.Add(sfxID, sfx);

            return sfxID;
        } // playSound

        public void StopByID(int id)
        {
            var sfx = _soundEffectsPlaying[id];
            sfx.Stop();

            _soundEffectsPlaying.Remove(id);
        } // stopByID

        public void StopAll()
        {
            for (var i = 0; i < _soundEffectsPlaying.Count; i++)
            {
                var sfx = _soundEffectsPlaying.ElementAt(i).Value;
                sfx.Stop();
            }

            _soundEffectsPlaying.Clear();
        } // stopAll

        public void SetVolume(int type, float volume)
        {
            if (volume < 0)
                volume = 0;
            if (volume > 1)
                volume = 1;

            if (!_volumeSettings.ContainsKey(type))
                _volumeSettings.Add(type, volume);

            _volumeSettings[type] = volume;

            for (var i = 0; i < _soundEffectsPlaying.Count; i++)
            {
                var sfx = _soundEffectsPlaying.ElementAt(i).Value;

                if (sfx.Type == type)
                    sfx.Volume = volume;
            }
        } // setVolume

    } // SoundManager
}
