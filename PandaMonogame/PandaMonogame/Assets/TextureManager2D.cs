using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PandaMonogame
{
    public class TextureExistsException : Exception
    {
        public TextureExistsException(string name)
            : base("The texture (" + name + ") already exists in this texture manager.") { }

        public TextureExistsException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }
    }

    public class TextureNotExistsException : Exception
    {
        public TextureNotExistsException(string name)
            : base("The texture (" + name + ") does not exist in this texture manager.") { }

        public TextureNotExistsException(string errorMessage, Exception innerEx)
            : base(errorMessage, innerEx) { }
    }

    public class TextureManager2D
    {
        protected Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

        public TextureManager2D()
        {

        }

        ~TextureManager2D() { }

        public virtual void AddTexture(string name, Texture2D texture)
        {
            if (texture == null || name == "")
                return;

            if (_textures.ContainsKey(name))
                throw new TextureExistsException(name);

            _textures.Add(name, texture);
        }

        public virtual void RemoveTexture(string name)
        {
            if (!_textures.ContainsKey(name))
                throw new TextureNotExistsException(name);

            _textures.Remove(name);
        }

        public virtual Texture2D GetTexture(string name)
        {
            if (!_textures.ContainsKey(name))
                return null;

            return _textures[name];
        }
    }
}
