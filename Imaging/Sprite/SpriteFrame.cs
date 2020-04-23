using Dreamscape.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Imaging {
    
    /// <summary>
    /// A sprite frame.
    /// </summary>
    public class SpriteFrame : Drawing, IReadable, IWriteable {

        /// <summary>
        /// Texture bounds from the source texture.
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// Frame length if animated.
        /// </summary>
        public int FrameLength;

        /// <summary>
        /// True width.
        /// </summary>
        public float TrueWidth => Width == -1 ? Bounds.Width : Width;

        /// <summary>
        /// True height.
        /// </summary>
        public float TrueHeight => Height == -1 ? Bounds.Height : Height;

        /// <summary>
        /// Blank constructor.
        /// </summary>
        public SpriteFrame() {}

        /// <summary>
        /// Create a sprite frame.
        /// </summary>
        /// <param name="bounds">The bounds within the texture.</param>
        /// <param name="frameLength">Length of each frame.</param>
        /// <param name="xOffset">X offset.</param>
        /// <param name="yOffset">Y offset.</param>
        /// <param name="width">Adjusted texture width.</param>
        /// <param name="height">Adjusted texture height.</param>
        public SpriteFrame(Rectangle bounds, int frameLength = 1000, float xOffset = 0, float yOffset = 0, float width = -1, float height = -1) {
            Bounds = bounds;
            FrameLength = frameLength;
            X = xOffset;
            Y = yOffset;
            Width = width;
            Height = height;
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
        public override void Draw(float xOffset = 0, float yOffset = 0, bool ignoreCamera = false, Color? color = null, Vector2? origin = null, Angle rotation = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0) {

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
                Vector2? newScale = new Vector2(TrueWidth / Texture.TextureData.Width, TrueHeight / Texture.TextureData.Height);
                if (scale != null) { newScale *= scale; }
                if (origin != null) { DrawPosition += origin.Value; } //Draw from top-left anyways.
                GameHelper.SpriteBatch.Draw(TextureData, DrawPosition, null, Bounds, origin, rot, newScale, color, spriteEffects, layerDepth);

            }

        }

        /// <summary>
        /// Read the frame.
        /// </summary>
        /// <param name="r">The reader.</param>
        public void Read(FileReader r) {
            Bounds = new Rectangle(r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32());
            FrameLength = r.ReadInt32();
            X = r.ReadSingle();
            Y = r.ReadSingle();
            Width = r.ReadSingle();
            Height = r.ReadSingle();
        }

        /// <summary>
        /// Write the frame.
        /// </summary>
        /// <param name="w">The writer.</param>
        public void Write(FileWriter w) {
            w.Write(Bounds.X);
            w.Write(Bounds.Y);
            w.Write(Bounds.Width);
            w.Write(Bounds.Height);
            w.Write(FrameLength);
            w.Write(X);
            w.Write(Y);
            w.Write(Width);
            w.Write(Height);
        }

        /// <summary>
        /// Do nothing.
        /// </summary>
        public override void Dispose() {}

    }

}
