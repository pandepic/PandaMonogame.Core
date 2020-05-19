using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PandaMonogame
{
    public delegate void KEYBOARD_EVENT(Keys key, GameTime gameTime, CurrentKeyState currentKeyState);
    public delegate void TEXTINPUT_EVENT(TextInputEventArgs e, GameTime gameTime, CurrentKeyState currentKeyState);

    public class CurrentKeyState
    {
        public List<Keys> HeldKeys { get; set; } = new List<Keys>();

        public void Clear()
        {
            HeldKeys.Clear();
        }

        public bool IsKeyHeld(Keys key)
        {
            if (HeldKeys.Contains(key))
                return true;

            return false;
        } // isKeyHeld
    } // CurrentKeyState

    public class KeyboardManager
    {
        protected KeyboardState _previousState = new KeyboardState();
        protected KeyboardState _currentState = new KeyboardState();

        protected Keys[] _keysArray = null;

        public KEYBOARD_EVENT OnKeyPressed { get; set; } = null;
        public KEYBOARD_EVENT OnKeyReleased { get; set; } = null;
        public KEYBOARD_EVENT OnKeyDown { get; set; } = null;
        public TEXTINPUT_EVENT OnTextInput { get; set; } = null;

        public CurrentKeyState CurrentKeyState { get; set; } = null;

        public KeyboardManager()
        {
            CurrentKeyState = new CurrentKeyState();
        }

        public void Update(GameTime gameTime)
        {
            CurrentKeyState.Clear();
            _currentState = Keyboard.GetState();

            _keysArray = _currentState.GetPressedKeys();

            if (_keysArray != null)
            {
                for (int i = 0; i < _keysArray.Length; i++)
                {
                    if (!CurrentKeyState.HeldKeys.Contains(_keysArray[i]))
                        CurrentKeyState.HeldKeys.Add(_keysArray[i]);

                    if (_previousState.IsKeyUp(_keysArray[i]))
                        OnKeyPressed?.Invoke(_keysArray[i], gameTime, CurrentKeyState);
                    else
                        OnKeyDown?.Invoke(_keysArray[i], gameTime, CurrentKeyState);
                }
            }

            _keysArray = _previousState.GetPressedKeys();

            if (_keysArray != null)
            {
                for (int i = 0; i < _keysArray.Length; i++)
                {
                    if (_currentState.IsKeyUp(_keysArray[i]))
                    {
                        CurrentKeyState.HeldKeys.RemoveAll(k => { return k == _keysArray[i]; });
                        OnKeyReleased?.Invoke(_keysArray[i], gameTime, CurrentKeyState);
                    }
                }
            }

            _previousState = _currentState;
        }

        public void TextInput(TextInputEventArgs e, GameTime gameTime)
        {
            OnTextInput?.Invoke(e, gameTime, CurrentKeyState);
        }
    }
}
