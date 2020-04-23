using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Imaging {

    /// <summary>
    /// A drawing.
    /// </summary>
    public abstract class Drawing : IDrawable, IDisposable {

        /// <summary>
        /// Texture. Take caution when dealing with this as it will not be removed from the heap!
        /// </summary>
        public Texture Texture;

        /// <summary>
        /// Texture data.
        /// </summary>
        public Texture2D TextureData => Texture.TextureData;

        /// <summary>
        /// Position.
        /// </summary>
        public Vector2 Position = Vector2.Zero;

        /// <summary>
        /// X position.
        /// </summary>
        public float X {
            get => Position.X;
            set { Position.X = value; }
        }

        /// <summary>
        /// Y position.
        /// </summary>
        public float Y {
            get => Position.Y;
            set { Position.Y = value; }
        }

        /// <summary>
        /// Drawing width.
        /// </summary>
        public float Width;

        /// <summary>
        /// Drawing height.
        /// </summary>
        public float Height;

        /// <summary>
        /// Draw position determined at draw time.
        /// </summary>
        protected Vector2 DrawPosition = Vector2.Zero;

        /// <summary>
        /// If disposed.
        /// </summary>
        private bool Disposed;

        /// <summary>
        /// Deconstructor.
        /// </summary>
        ~Drawing() {
            if (Texture != null) {
                Dispose();
            }
        }

        /// <summary>
        /// Draw the drawing.
        /// </summary>
        /// <param name="xOffset">X offset.</param>
        /// <param name="yOffset">Y offset.</param>
        /// <param name="ignoreCamera">Whether or not to ignore the camera.</param>
        /// <param name="color">Color to draw over.</param>
        /// <param name="origin">Origin of the drawing.</param>
        /// <param name="rotation">Rotation of the drawing.</param>
        /// <param name="scale">Scaling of the drawing.</param>
        /// <param name="spriteEffects">Sprite effects.</param>
        /// <param name="layerDepth">Depth to draw at.</param>
        public virtual void Draw(float xOffset = 0, float yOffset = 0, bool ignoreCamera = false, Color? color = null, Vector2? origin = null, Angle rotation = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0) {
            
            //Only draw if valid texture.
            if (Texture != null) {

                //Apply camera.
                if (!ignoreCamera) {

                    //New position.
                    DrawPosition.X = GameHelper.Camera.ApplyCameraX((float)X + xOffset);
                    DrawPosition.Y = GameHelper.Camera.ApplyCameraY((float)Y + yOffset);

                }

                //Don't apply camera.
                else {

                    //New position.
                    DrawPosition.X = (float)(X + xOffset);
                    DrawPosition.Y = (float)(Y + yOffset);

                }

                //Draw image.
                float rot = 0;
                if (rotation != null) { rot = (float)rotation.Radians; }
                Vector2? newScale = new Vector2(Width / Texture.TextureData.Width, Height / Texture.TextureData.Height);
                if (scale != null) { newScale *= scale; }
                if (origin != null) { DrawPosition += origin.Value; } //Draw from top-left anyways.
                GameHelper.SpriteBatch.Draw(TextureData, DrawPosition, null, null, origin, rot, newScale, color, spriteEffects, layerDepth);

            }

        }

        /// <summary>
        /// Dispose the texture.
        /// </summary>
        public virtual void Dispose() {
            if (!Disposed) {
                DisposeExtension();
                Texture.Dispose();
                Disposed = true;
            }
        }

        /// <summary>
        /// More to dispose.
        /// </summary>
        public virtual void DisposeExtension() {}

    }

}
