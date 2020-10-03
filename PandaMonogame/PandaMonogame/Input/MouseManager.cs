using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PandaMonogame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PandaMonogame
{
    public enum MouseScrollDirection
    {
        Up,
        Down
    }

    public delegate void MOUSEBUTTON_EVENT(MouseButtonID button, GameTime gameTime);
    public delegate void MOUSEPOSITION_EVENT(Vector2 originalPosition, GameTime gameTime);
    public delegate void MOUSESCROLL_EVENT(MouseScrollDirection direction, int scrollValue, GameTime gameTime);

    public class MouseManager
    {
        protected MouseState _previousState = new MouseState();
        protected MouseState _currentState = new MouseState();

        public int X
        {
            get
            {
                return _currentState.X;
            }
        }

        public int Y
        {
            get
            {
                return _currentState.Y;
            }
        }

        public MOUSEBUTTON_EVENT OnMouseClicked { get; set; } = null;
        public MOUSEBUTTON_EVENT OnMouseDown { get; set; } = null;
        public MOUSEPOSITION_EVENT OnMouseMoved { get; set; } = null;
        public MOUSESCROLL_EVENT OnMouseScroll { get; set; } = null;

        public MouseManager() { }

        public static Vector2 GetMousePosition()
        {
            var mouseState = Mouse.GetState();
            return new Vector2(mouseState.Position.X, mouseState.Position.Y);
        }

        public void Update(GameTime gameTime)
        {
            _currentState = Mouse.GetState();
            
            #region mouse clicked

            if (_currentState.RightButton == ButtonState.Released
                && _previousState.RightButton == ButtonState.Pressed)
            {
                OnMouseClicked?.Invoke(MouseButtonID.Right, gameTime);
            }

            if (_currentState.LeftButton == ButtonState.Released
                && _previousState.LeftButton == ButtonState.Pressed)
            {
                OnMouseClicked?.Invoke(MouseButtonID.Left, gameTime);
            }

            if (_currentState.MiddleButton == ButtonState.Released
                && _previousState.MiddleButton == ButtonState.Pressed)
            {
                OnMouseClicked?.Invoke(MouseButtonID.Middle, gameTime);
            }

            #endregion

            #region mouse down

            if ((_previousState.LeftButton == ButtonState.Pressed
                && _previousState.Position != _currentState.Position)
                || _previousState.LeftButton == ButtonState.Released)
            {
                if (_currentState.LeftButton == ButtonState.Pressed)
                {
                    OnMouseDown?.Invoke(MouseButtonID.Left, gameTime);
                }
            }

            if ((_previousState.MiddleButton == ButtonState.Pressed
                && _previousState.Position != _currentState.Position)
                || _previousState.MiddleButton == ButtonState.Released)
            {
                if (_currentState.MiddleButton == ButtonState.Pressed)
                {
                    OnMouseDown?.Invoke(MouseButtonID.Middle, gameTime);
                }
            }

            if ((_previousState.RightButton == ButtonState.Pressed
                && _previousState.Position != _currentState.Position)
                || _previousState.RightButton == ButtonState.Released)
            {
                if (_currentState.RightButton == ButtonState.Pressed)
                {
                    OnMouseDown?.Invoke(MouseButtonID.Right, gameTime);
                }
            }

            #endregion

            if (_currentState.ScrollWheelValue != _previousState.ScrollWheelValue)
            {
                var scrollValue = _currentState.ScrollWheelValue - _previousState.ScrollWheelValue;
                var direction = MouseScrollDirection.Up;
                if ((_currentState.ScrollWheelValue - _previousState.ScrollWheelValue) < 0)
                    direction = MouseScrollDirection.Down;

                OnMouseScroll?.Invoke(direction, _currentState.ScrollWheelValue, gameTime);
            }

            if (_currentState.Position != _previousState.Position)
            {
                OnMouseMoved?.Invoke(new Vector2(_previousState.X, _previousState.Y), gameTime);
            }

            _previousState = _currentState;
        }
    }
}
