using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PandaMonogame.UI;

namespace PandaMonogame
{
    public class GameState
    {
        public GameState() { }
        
        public virtual void Load(ContentManager Content, GraphicsDevice graphics) { }

        public virtual int Update(GameTime gameTime) { return -1; }

        public virtual void Draw(GameTime gameTime, GraphicsDevice graphics, SpriteBatch spriteBatch) { }

        public virtual void OnMouseMoved(Vector2 originalPosition, GameTime gameTime) { }
        public virtual void OnMouseDown(MouseButtonID button, GameTime gameTime) { }
        public virtual void OnMouseClicked(MouseButtonID button, GameTime gameTime) { }
        public virtual void OnMouseScroll(MouseScrollDirection direction, int scrollValue, GameTime gameTime) { }

        public virtual void OnKeyPressed(Keys key, GameTime gameTime, CurrentKeyState currentKeyState) { }
        public virtual void OnKeyReleased(Keys key, GameTime gameTime, CurrentKeyState currentKeyState) { }
        public virtual void OnKeyDown(Keys key, GameTime gameTime, CurrentKeyState currentKeyState) { }
        public virtual void OnTextInput(TextInputEventArgs e, GameTime gameTime, CurrentKeyState currentKeyState) { }
    }
}