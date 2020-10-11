using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    public enum KeyboardControlState
    {
        None = -1,
        Pressed,
        Released,
        Down,
    }

    public struct KeyboardGameControl<T> where T : IConvertible
    {
        public T Control;
        public Keys[] ControlKeys;
        public KeyboardControlState State;

        public KeyboardGameControl(T control, Keys[] controlKeys, KeyboardControlState state)
        {
            Control = control;
            ControlKeys = controlKeys;
            State = state;
        }
    }

    public delegate void KEYCONTROL_EVENT<T>(T control, GameTime gameTime, KeyboardControlState state) where T : IConvertible;

    public class GameControlsManager<T> where T : IConvertible
    {
        protected Dictionary<T, List<KeyboardGameControl<T>>> _keyboardControls = new Dictionary<T, List<KeyboardGameControl<T>>>();

        public KEYCONTROL_EVENT<T> OnGameKeyControl { get; set; } = null;

        public GameControlsManager()
        {
            
        }

        public void Clear()
        {
            _keyboardControls.Clear();
        }

        public void LoadFromSettings(string sectionName)
        {
            Clear();

            var keyboardSettings = SettingsManager.Instance.GetSettings(sectionName);

            foreach (var setting in keyboardSettings)
            {
                var controlType = setting.Name;
                var controls = setting.Value.Split(',');
                var state = setting.OtherAttributes["State"].ToEnum<KeyboardControlState>();

                foreach (var control in controls)
                {
                    var controlKeyList = new List<Keys>();
                    var controlKeys = control.Split('+');

                    foreach (var key in controlKeys)
                        controlKeyList.Add(key.ToEnum<Keys>());

                    AddKeysControl(controlType, controlKeyList.ToArray(), state);
                }
            }
        } // LoadFromSettings

        public void AddKeysControl(string name, Keys[] keys, KeyboardControlState state = KeyboardControlState.Released)
        {
            T nameKey = name.ToEnum<T>();

            if (!_keyboardControls.ContainsKey(nameKey))
                _keyboardControls.Add(nameKey, new List<KeyboardGameControl<T>>());

            _keyboardControls[nameKey].Add(new KeyboardGameControl<T>(name.ToEnum<T>(), keys, state));
        }

        public void OnKeyPressed(Keys key, GameTime gameTime, CurrentKeyState currentKeyState)
        {
            foreach (var kvp in _keyboardControls)
            {
                var controlFound = false;
                var gameControls = kvp.Value;
                for (var i = 0; i < gameControls.Count && !controlFound; i++)
                {
                    var gameControl = gameControls[i];
                    if (gameControl.State != KeyboardControlState.Pressed)
                        continue;

                    var containsControlKey = gameControl.ControlKeys.Length > 0 ? true : false;

                    for (var c  = 0; c < gameControl.ControlKeys.Length; c++)
                    {
                        var findKey = gameControl.ControlKeys[c];
                        var keyFound = false;

                        for (var k = 0; k < currentKeyState.HeldKeys.Count && !keyFound; k++)
                        {
                            if (currentKeyState.HeldKeys[k] == findKey)
                                keyFound = true;
                        }

                        containsControlKey = containsControlKey && keyFound;
                    }

                    if (containsControlKey)
                    {
                        controlFound = true;
                        OnGameKeyControl?.Invoke(gameControl.Control, gameTime, KeyboardControlState.Pressed);
                    }
                }
            }
        } // OnKeyPressed

        public void OnKeyReleased(Keys key, GameTime gameTime, CurrentKeyState currentKeyState)
        {
            foreach (var kvp in _keyboardControls)
            {
                var controlFound = false;
                var gameControls = kvp.Value;
                for (var i = 0; i < gameControls.Count && !controlFound; i++)
                {
                    var gameControl = gameControls[i];
                    if (gameControl.State != KeyboardControlState.Released)
                        continue;

                    var releasedKeyFound = false;
                    var containsControlKey = gameControl.ControlKeys.Length > 0 ? true : false;

                    for (var c = 0; c < gameControl.ControlKeys.Length; c++)
                    {
                        var findKey = gameControl.ControlKeys[c];
                        var keyFound = false;

                        if (findKey == key)
                        {
                            releasedKeyFound = true;
                            keyFound = true;
                        }
                        else
                        {
                            for (var k = 0; k < currentKeyState.HeldKeys.Count && !keyFound; k++)
                            {
                                if (currentKeyState.HeldKeys[k] == findKey)
                                    keyFound = true;
                            }
                        }

                        containsControlKey = containsControlKey && keyFound;
                    }

                    if (containsControlKey && releasedKeyFound)
                    {
                        controlFound = true;
                        OnGameKeyControl?.Invoke(gameControl.Control, gameTime, KeyboardControlState.Released);
                    }
                }
            }
        } // OnKeyReleased

        public void OnKeyDown(Keys key, GameTime gameTime, CurrentKeyState currentKeyState)
        {
            foreach (var kvp in _keyboardControls)
            {
                var controlFound = false;
                var gameControls = kvp.Value;
                for (var i = 0; i < gameControls.Count && !controlFound; i++)
                {
                    var gameControl = gameControls[i];
                    if (gameControl.State != KeyboardControlState.Down)
                        continue;

                    var containsControlKey = gameControl.ControlKeys.Length > 0 ? true : false;

                    for (var c = 0; c < gameControl.ControlKeys.Length; c++)
                    {
                        var findKey = gameControl.ControlKeys[c];
                        var keyFound = false;

                        for (var k = 0; k < currentKeyState.HeldKeys.Count && !keyFound; k++)
                        {
                            if (currentKeyState.HeldKeys[k] == findKey)
                                keyFound = true;
                        }

                        containsControlKey = containsControlKey && keyFound;
                    }

                    if (containsControlKey)
                    {
                        controlFound = true;
                        OnGameKeyControl?.Invoke(gameControl.Control, gameTime, KeyboardControlState.Down);
                    }
                }
            }
        } // OnKeyDown
    } // GameControlsManager
}
