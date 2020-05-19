using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PandaMonogame
{
    public class Camera3D
    {
        public Vector2 ViewRange = Vector2.Zero;
        public Vector3 CameraPosition = Vector3.Zero;
        public Vector3 CameraLookAt = Vector3.Zero;
        public Vector3 UpVector = Vector3.Up;

        public float AspectRatio = 1.0f;

        protected Matrix _cameraProjectionMatrix;
        protected Matrix _cameraViewMatrix;

        public Camera3D(GraphicsDevice graphicsDevice)
        {
            AspectRatio = graphicsDevice.Viewport.AspectRatio;

            UpdateViewMatrix();
            UpdateProjectionMatrix();
        }

        protected void UpdateProjectionMatrix()
        {
            _cameraProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                AspectRatio,
                ViewRange.X,
                ViewRange.Y);
        }

        protected void UpdateViewMatrix()
        {
            _cameraViewMatrix = Matrix.CreateLookAt(
                CameraPosition,
                CameraLookAt,
                UpVector);
        }
    }
}