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

    public struct KeyboardGameControl
    {
        public string Name;
        public Keys[] ControlKeys;
        public KeyboardControlState State;

        public KeyboardGameControl(string name, Keys[] controlKeys, KeyboardControlState state)
        {
            Name = name;
            ControlKeys = controlKeys;
            State = state;
        }
    }

    public delegate void KEYCONTROL_EVENT(string name, GameTime gameTime, KeyboardControlState state);

    public class GameControlsManager
    {
        protected Dictionary<string, List<KeyboardGameControl>> _keyboardControls = new Dictionary<string, List<KeyboardGameControl>>();

        public KEYCONTROL_EVENT OnGameKeyControl { get; set; } = null;

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
            if (!_keyboardControls.ContainsKey(name))
                _keyboardControls.Add(name, new List<KeyboardGameControl>());

            _keyboardControls[name].Add(new KeyboardGameControl(name, keys, state));
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
                        OnGameKeyControl?.Invoke(gameControl.Name, gameTime, KeyboardControlState.Pressed);
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
                        OnGameKeyControl?.Invoke(gameControl.Name, gameTime, KeyboardControlState.Released);
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
                        OnGameKeyControl?.Invoke(gameControl.Name, gameTime, KeyboardControlState.Down);
                    }
                }
            }
        } // OnKeyDown
    } // GameControlsManager
}
