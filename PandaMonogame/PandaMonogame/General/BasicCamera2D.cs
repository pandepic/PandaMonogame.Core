using System;
using Microsoft.Xna.Framework;

namespace PandaMonogame
{
    public class BasicCamera2D
    {
        public float Rotation { get; set; } = 0f;
        public float Zoom { get; set; } = 1f;

        public Rectangle BoundingBox { get; set; } = Rectangle.Empty;
        protected Rectangle _view = Rectangle.Empty;

        protected Vector2 _origin = Vector2.Zero;
        protected Vector2 _position = Vector2.Zero;
        
        public Vector2 Velocity = Vector2.Zero;

        public BasicCamera2D() { }

        public BasicCamera2D(Rectangle view, Rectangle boundingBox)
        {
            if (view != null)
            {
                this._view = view;
                _origin = new Vector2((float)view.Width / 2f, (float)view.Height / 2f);

                _position.X = _view.X;
                _position.Y = _view.Y;
            }

            this.BoundingBox = boundingBox;
        }

        public void Update(GameTime gameTime)
        {
            if (Velocity != Vector2.Zero)
            {
                var delta = gameTime.DeltaTime();

                _position += Velocity * delta;
                _view.X = (int)_position.X;
                _view.Y = (int)_position.Y;

                CheckBoundingBox();
            }
        }

        public Rectangle GetViewRect()
        {
            return _view;
        }
        
        public Matrix GetViewMatrix(float z = 0f)
        {
            return  Matrix.CreateTranslation(new Vector3(-_position, z)) *
                    Matrix.CreateTranslation(new Vector3(-_origin, z)) *
                    Matrix.CreateScale(Zoom, Zoom, 1) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateTranslation(new Vector3(_origin, z));
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public Rectangle GetBoundingBox()
        {
            return BoundingBox;
        }

        public void SetPosition(Vector2 position)
        {
            if (position == null)
                return;

            SetViewPosition(
                (int)position.X,
                (int)position.Y
            );

            CheckBoundingBox();
        }

        public void OffsetPosition(int x, int y)
        {
            SetViewPosition(
                _view.X + x,
                _view.Y + y
            );

            CheckBoundingBox();
        }

        public void OffsetPosition(Vector2 vector)
        {
            OffsetPosition((int)vector.X, (int)vector.Y);
        }

        public void CenterPosition(Vector2 position)
        {
            if (position == null)
                return;

            SetViewPosition(
                (int)position.X - _view.Width / 2,
                (int)position.Y - _view.Height / 2
            );

            CheckBoundingBox();
        }

        public void CenterEntity(Entity2D entity)
        {
            if (entity == null)
                return;

            SetViewPosition(
                (int)entity.Position.X + entity.Width / 2 - _view.Width / 2,
                (int)entity.Position.Y + entity.Height / 2 - _view.Height / 2
            );

            CheckBoundingBox();
        }

        public void CheckBoundingBox()
        {
            if (BoundingBox.IsEmpty || BoundingBox == null)
                return;

            if (_view.X < BoundingBox.X)
                SetViewPositionX(BoundingBox.X);
            if (_view.X + _view.Width > BoundingBox.X + BoundingBox.Width)
                SetViewPositionX((BoundingBox.X + BoundingBox.Width) - _view.Width);
            if (_view.Y < BoundingBox.Y)
                SetViewPositionY(BoundingBox.Y);
            if ((_view.Y + _view.Height) > (BoundingBox.Y + BoundingBox.Height))
                SetViewPositionY((BoundingBox.Y + BoundingBox.Height) - _view.Height);
        }

        private void SetViewPositionX(int x)
        {
            SetViewPosition(x, _view.Y);
        }

        private void SetViewPositionY(int y)
        {
            SetViewPosition(_view.X, y);
        }

        private void SetViewPosition(int x, int y)
        {
            _view.X = x;
            _view.Y = y;
            _position.X = _view.X;
            _position.Y = _view.Y;
        }
    }
}
