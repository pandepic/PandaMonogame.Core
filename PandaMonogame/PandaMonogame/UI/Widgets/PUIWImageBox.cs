﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PandaMonogame.UI
{
    public class PUIWImageBox : PUIWidget
    {
        protected AnimatedSprite _image = null;

        public PUIWImageBox() { }

        public override void Load(PUIFrame parent, XElement el)
        {
            Init(parent, el);

            Texture2D texture = ModManager.Instance.AssetManager.LoadTexture2D(parent.CommonWidgetResources.Graphics, GetXMLElement("AssetName").Value);

            Width = texture.Width;
            Height = texture.Height;

            _image = new AnimatedSprite(texture, texture.Width, texture.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_image != null)
                _image.Draw(spriteBatch, Position + Parent.Position);
        }
    }
}
