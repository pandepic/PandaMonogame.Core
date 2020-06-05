﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml.Linq;

namespace PandaMonogame.UI
{
    public enum MouseButtonID
    {
        Left = 0,
        Right = 1,
        Middle = 2
    };

    public class WidgetPositionFlags
    {
        public bool CenterX;
        public bool CenterY;
        public bool AnchorLeft;
        public bool AnchorRight;
        public bool AnchorTop;
        public bool AnchorBottom;
        public int? SetX = null;
        public int? SetY = null;
    }

    public class PUIWidget
    {
        protected XElement _template = null;
        protected Rectangle _widgetRect = new Rectangle(0, 0, 0, 0);
        protected Vector2 _offset = Vector2.Zero;

        public XElement XmlRoot { get; set; } = null;
        public PUIFrame Parent { get; set; }
        public BASICEVENT_FUNC HandleEvents { get; set; }

        public string Name { get; set; }
        public bool Focused { get; private set; } = false;
        public int DrawOrder { get; set; }
        public bool Visible { get; set; }
        public bool Active { get; set; }

        protected WidgetPositionFlags _positionFlags = new WidgetPositionFlags();

        public Vector2 Position
        {
            get
            {
                return new Vector2(_widgetRect.X + _offset.X, _widgetRect.Y + _offset.Y);
            }
        }

        public int X
        {
            get
            {
                return _widgetRect.X + (int)_offset.X;
            }
            set
            {
                _widgetRect.X = value;
            }
        }

        public int Y
        {
            get
            {
                return _widgetRect.Y + (int)_offset.Y;
            }
            set
            {
                _widgetRect.Y = value;
            }
        }

        public int Width
        {
            get
            {
                return _widgetRect.Width;
            }
            set
            {
                _widgetRect.Width = value;
            }
        }

        public int Height
        {
            get
            {
                return _widgetRect.Height;
            }
            set
            {
                _widgetRect.Height = value;
            }
        }

        public PUIWidget() { DrawOrder = 0; Visible = true; Active = true; }
        public PUIWidget(int x, int y)
        {
            _widgetRect.X = x; _widgetRect.Y = y;
            DrawOrder = 0;
            Visible = true;
            Active = true;
        }
        public PUIWidget(int x, int y, int width, int height)
        {
            _widgetRect.X = x; _widgetRect.Y = y; _widgetRect.Height = height; _widgetRect.Width = width;
            DrawOrder = 0;
            Visible = true;
            Active = true;
        }

        public virtual void Load(PUIFrame parent, XElement el) { }

        public virtual void Update(GameTime gameTime) { }
        public virtual void OnMouseMoved(Vector2 originalPosition, Vector2 currentPosition, GameTime gameTime) { }
        public virtual void OnMouseDown(MouseButtonID button, Vector2 mousePosition, GameTime gameTime) { }
        public virtual void OnMouseClicked(MouseButtonID button, Vector2 mousePosition, GameTime gameTime) { }
        public virtual bool OnMouseScroll(MouseScrollDirection direction, int scrollValue, GameTime gameTime) { return false; }
        public virtual void OnKeyPressed(Keys key, GameTime gameTime, CurrentKeyState currentKeyState) { }
        public virtual void OnKeyReleased(Keys key, GameTime gameTime, CurrentKeyState currentKeyState) { }
        public virtual void OnKeyDown(Keys key, GameTime gameTime, CurrentKeyState currentKeyState) { }
        public virtual void OnTextInput(TextInputEventArgs e, GameTime gameTime, CurrentKeyState currentKeyState) { }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public void Init(PUIFrame parent, XElement el)
        {
            Parent = parent;
            HandleEvents = parent.CommonWidgetResources.HandleEvents;
            XmlRoot = el;
            LoadTemplate();
        } // init

        protected virtual void GrabFocus()
        {
            Parent.GrabFocus(this);
        }

        protected virtual void DropFocus()
        {
            Parent.DropFocus(this);
        }

        internal virtual void Focus()
        {
            Focused = true;
        }

        internal virtual void UnFocus()
        {
            Focused = false;
        }

        public void LoadTemplate()
        {
            if (Parent == null || Parent.Templates == null || Parent.Templates.Count == 0)
                return;

            var templateAttribute = XmlRoot.Attribute("Template");
            if (templateAttribute == null)
                return;

            var templateName = templateAttribute.Value;
            if (string.IsNullOrWhiteSpace(templateName))
                return;

            if (!Parent.Templates.ContainsKey(templateName))
                return;

            _template = Parent.Templates[templateName];
        } // loadTemplate

        public XElement GetXMLElement(string name)
        {
            XElement el = null;

            if (_template != null)
                el = _template.Element(name);

            if (el == null)
                el = XmlRoot.Element(name);

            return el;
        } // getXMLElement

        public XElement GetXMLElement(string parent, string name)
        {
            XElement el = null;

            if (_template != null)
            {
                var p = _template.Element(parent);
                if (p != null)
                    el = p.Element(name);
            }

            if (el == null)
            {
                var p = XmlRoot.Element(parent);
                if (p != null)
                    el = p.Element(name);
            }

            return el;
        } // getXMLElement

        public XAttribute GetXMLAttribute(string name)
        {
            XAttribute att = null;

            if (_template != null)
                att = _template.Attribute(name);

            if (att == null)
                att = XmlRoot.Attribute(name);

            return att;
        } // getXMLAttribute

        public XAttribute GetXMLAttribute(string parent, string name)
        {
            XAttribute att = null;

            if (_template != null)
            {
                var p = _template.Element(parent);
                if (p != null)
                    att = p.Attribute(name);
            }

            if (att == null)
            {
                var p = XmlRoot.Element(parent);
                if (p != null)
                    att = p.Attribute(name);
            }

            return att;
        } // getXMLAttribute

        public T GetXMLAttribute<T>(string name)
        {
            return PandaUtil.ConvertTo<T>(GetXMLAttribute(name).Value);
        }

        public T GetXMLAttribute<T>(string parent, string name)
        {
            return PandaUtil.ConvertTo<T>(GetXMLAttribute(parent, name).Value);
        }

        public T GetXMLElement<T>(string name)
        {
            return PandaUtil.ConvertTo<T>(GetXMLElement(name).Value);
        }

        public T GetXMLElement<T>(string parent, string name)
        {
            return PandaUtil.ConvertTo<T>(GetXMLElement(parent, name).Value);
        }

        public void LoadStandardXML()
        {
            Name = GetXMLAttribute("Name").Value;

            var valueX = GetXMLElement("Position", "X").Value;

            if (valueX.ToUpper() == "CENTER")
                _positionFlags.CenterX = true;
            else if (valueX.ToUpper() == "LEFT")
                _positionFlags.AnchorLeft = true;
            else if (valueX.ToUpper() == "RIGHT")
                _positionFlags.AnchorRight = true;
            else
                _positionFlags.SetX = int.Parse(valueX);

            var valueY = GetXMLElement("Position", "Y").Value;

            if (valueY.ToUpper() == "CENTER")
                _positionFlags.CenterY = true;
            else if (valueY.ToUpper() == "TOP")
                _positionFlags.AnchorTop = true;
            else if (valueY.ToUpper() == "BOTTOM")
                _positionFlags.AnchorBottom = true;
            else
                _positionFlags.SetY = int.Parse(valueY);

            UpdatePositionFromFlags();

            var offsetElement = GetXMLElement("Offset");
            if (offsetElement != null)
            {
                _offset.X = int.Parse(GetXMLElement("Offset", "X").Value);
                _offset.Y = int.Parse(GetXMLElement("Offset", "Y").Value);
            }

            var sizeElement = GetXMLElement("Size");
            if (sizeElement != null)
            {
                Width = int.Parse(GetXMLElement("Size", "Width").Value);
                Height = int.Parse(GetXMLElement("Size", "Height").Value);
            }

            DrawOrder = GetXMLAttribute("DrawOrder") == null ? 0 : int.Parse(GetXMLAttribute("DrawOrder").Value);
            Visible = (GetXMLAttribute("Visible") != null ? Convert.ToBoolean(GetXMLAttribute("Visible").Value) : true);
            Active = (GetXMLAttribute("Active") != null ? Convert.ToBoolean(GetXMLAttribute("Active").Value) : true);

            if (PandaMonogameConfig.Logging)
                Console.WriteLine("New widget [type:" + this.GetType().Name + "] [active:" + Active + "] [visible:" + Visible + "]");
        } // loadStandardXML

        protected void UpdatePositionFromFlags()
        {
            if (_positionFlags.CenterX)
                CenterX();
            else if (_positionFlags.AnchorLeft)
                AnchorLeft();
            else if (_positionFlags.AnchorRight)
                AnchorRight();
            else if (_positionFlags.SetX.HasValue)
                X = _positionFlags.SetX.Value;

            if (_positionFlags.CenterY)
                CenterY();
            else if (_positionFlags.AnchorTop)
                AnchorTop();
            else if (_positionFlags.AnchorBottom)
                AnchorBottom();
            else if (_positionFlags.SetY.HasValue)
                Y = _positionFlags.SetY.Value;
        }

        public void AnchorTop() { Y = 0; }
        public void AnchorBottom() { Y = Parent.Height - Height; }
        public void AnchorLeft() { X = 0; }
        public void AnchorRight() { X = Parent.Width - Width; }
        public void CenterX() { X = (Parent.Width / 2) - (Width / 2); }
        public void CenterY() { Y = (Parent.Height / 2) - (Height / 2); }

        public bool PointInsideWidget(Vector2 point)
        {
            if (_widgetRect.Width == 0 && _widgetRect.Height == 0)
                return false;

            if (point.X < _widgetRect.X)
                return false;
            if (point.Y < _widgetRect.Y)
                return false;
            if (point.X > (_widgetRect.X + _widgetRect.Width))
                return false;
            if (point.Y > (_widgetRect.Y + _widgetRect.Height))
                return false;

            return true;
        } // pointInsideWidget
    }
}
