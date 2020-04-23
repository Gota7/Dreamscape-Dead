using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// Main camera.
    /// </summary>
    public class Camera {

        /// <summary>
        /// Camera X-Position.
        /// </summary>
        public float X;

        /// <summary>
        /// Camera Y-Position.
        /// </summary>
        public float Y;

        /// <summary>
        /// Area width.
        /// </summary>
        public float Width = -1;

        /// <summary>
        /// Area height.
        /// </summary>
        public float Height = -1;

        /// <summary>
        /// Camera stack, when there are multiple windows.
        /// </summary>
        private Stack<CameraStackEntry> CameraStack = new Stack<CameraStackEntry>();

        /// <summary>
        /// Reset camera position.
        /// </summary>
        public void ResetCameraPosition() {
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// Apply the camera position to render the object relative to the camera position.
        /// </summary>
        /// <param name="x">X to apply to.</param>
        public float ApplyCameraX(float x) {

            //Subtract the camera coordinates.
            return x - X;

        }

        /// <summary>
        /// Apply the camera position to render the object relative to the camera position.
        /// </summary>
        /// <param name="y">Y to apply to.</param>
        public float ApplyCameraY(float y) {

            //Subtract the camera coordinates.
            return y - Y;

        }

        /// <summary>
        /// Update the camera position with a velocity.
        /// </summary>
        /// <param name="velocity">Velocity to move the camera with.</param>
        public void Update(Vector2 velocity) {

            //Update position.
            var g = GameHelper.GameTime;
            X += (float)g.ElapsedGameTime.TotalMilliseconds * velocity.X;
            Y += (float)g.ElapsedGameTime.TotalMilliseconds * velocity.Y;

        }

        /// <summary>
        /// Calculate the midpoint of the camera.
        /// </summary>
        /// <returns>The midpoint of the camera.</returns>
        public Vector2 Midpoint() {

            //Simply is just the midpoint formula.
            return new Vector2((X + Width) / 2, (Y + Height) / 2);

        }

        /// <summary>
        /// Push the current camera to stack, and use new camera values.
        /// </summary>
        public void PushCameraStack(float x, float y, float width, float height, float targetWidth, float targetHeight, float xScale, float yScale, bool fullscreen) {

            //Push to camera stack.
            //cameraStack.Push(new CameraStackEntry { X = X, Y = Y, Width = ScreenWidth, Height = ScreenHeight, TargetWidth = Width, TargetHeight = Height, XScale = XScale, YScale = YScale, Fullscreen = Fullscreen });

            //Set camera values.
            X = x;
            Y = y;
            //ScreenWidth = width;
            //ScreenHeight = height;
            Width = targetWidth;
            Height = targetHeight;
            //XScale = xScale;
            //YScale = yScale;
            //Fullscreen = fullscreen;

        }

        /// <summary>
        /// Pop the camera stack.
        /// </summary>
        /// <returns>The camera stack entry.</returns>
        public CameraStackEntry PopCameraStack() {

            //Make a new camera stack entry from the current values.
            //CameraStackEntry oldCamera = new CameraStackEntry { X = X, Y = Y, Width = ScreenWidth, Height = ScreenHeight, TargetWidth = Width, TargetHeight = Height, XScale = XScale, YScale = YScale, Fullscreen = Fullscreen };

            //Set the camera values to what is in the stack.
            //CameraStackEntry newCamera = cameraStack.Pop();
            //X = newCamera.X;
            //Y = newCamera.Y;
            //ScreenWidth = newCamera.Width;
            //ScreenHeight = newCamera.Height;
            //Width = newCamera.TargetWidth;
            //Height = newCamera.TargetHeight;
            //XScale = newCamera.XScale;
            //YScale = newCamera.YScale;
            //Fullscreen = newCamera.Fullscreen;

            //Return the old camera stack entries.
            return null; //oldCamera;

        }

    }

}
