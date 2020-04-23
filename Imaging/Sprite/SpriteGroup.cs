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
    /// A sprite group.
    /// </summary>
    public class SpriteGroup : Drawing, IReadable, IWriteable, IUpdateable {

        /// <summary>
        /// Frames.
        /// </summary>
        public Dictionary<string, SpriteFrame> Frames = new Dictionary<string, SpriteFrame>();

        /// <summary>
        /// Current frame index.
        /// </summary>
        public string CurrentFrameName;

        /// <summary>
        /// Current frame index.
        /// </summary>
        public int CurrentFrameIndex {
            get => Frames.Keys.ToList().IndexOf(CurrentFrameName);
            set { CurrentFrameName = Frames.Keys.ElementAt(value); }
        }

        /// <summary>
        /// Current frame.
        /// </summary>
        public SpriteFrame CurrentFrame => Frames[CurrentFrameName];

        /// <summary>
        /// If the sprite group is animated.
        /// </summary>
        public bool Animated;

        /// <summary>
        /// Initial frame index.
        /// </summary>
        public string InitialFrameName;

        /// <summary>
        /// Reset the initial frame on change.
        /// </summary>
        public bool ResetInitialFrameOnChange;

        /// <summary>
        /// Animation timer.
        /// </summary>
        private Timer AnimationTimer;

        /// <summary>
        /// A blank constructor.
        /// </summary>
        public SpriteGroup() {}

        /// <summary>
        /// Create a sprite group from frames.
        /// </summary>
        /// <param name="frames">Frames.</param>
        /// <param name="animated">If the group is animated/</param>
        /// <param name="initialFrameName">Initial frame name.</param>
        /// <param name="resetInitialFrameOnChange">If to reset the initial frame on change.</param>
        public SpriteGroup(Dictionary<string, SpriteFrame> frames, bool animated = false, string initialFrameName = "", bool resetInitialFrameOnChange = false) {
            Animated = animated;
            Frames = frames;
            InitialFrameName = initialFrameName.Equals("") ? frames.Keys.ElementAt(0) : initialFrameName;
            CurrentFrameName = initialFrameName.Equals("") ? frames.Keys.ElementAt(0) : initialFrameName;
            AnimationTimer = new Timer((uint)CurrentFrame.FrameLength);
            ResetInitialFrameOnChange = resetInitialFrameOnChange;
            foreach (var f in frames) {
                f.Value.Texture = Texture;
            }
        }

        /// <summary>
        /// Show the next frame.
        /// </summary>
        public void ShowNextFrame() {
            int currIndex = Frames.Keys.ToList().IndexOf(CurrentFrameName);
            currIndex++;
            if (currIndex >= Frames.Count) { currIndex = 0; }
            CurrentFrameName = Frames.Keys.ElementAt(currIndex);
            AnimationTimer = new Timer((uint)CurrentFrame.FrameLength);
        }

        /// <summary>
        /// Draw the current frame.
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
            CurrentFrame.Draw(X + xOffset, Y + yOffset, ignoreCamera, color, origin, rotation, scale, spriteEffects, layerDepth);
        }

        /// <summary>
        /// Update.
        /// </summary>
        public void Update() {

            //Only update if animated.
            if (!Animated) { return; }

            //Tick animation timer.
            AnimationTimer.Update();

            //If it's time for the next frame.
            if (AnimationTimer.Finished()) {
                ShowNextFrame();
            }

        }

        /// <summary>
        /// Read the group.
        /// </summary>
        /// <param name="r">The reader.</param>
        public void Read(FileReader r) {
            Frames = new Dictionary<string, SpriteFrame>();
            Animated = r.ReadBoolean();
            ResetInitialFrameOnChange = r.ReadBoolean();
            InitialFrameName = r.ReadString();
            X = r.ReadSingle();
            Y = r.ReadSingle();
            uint numFrames = r.ReadUInt32();
            for (uint i = 0; i < numFrames; i++) {
                Frames.Add(r.ReadString(), r.Read<SpriteFrame>());
                Frames.Last().Value.Texture = Texture;
            }
            CurrentFrameName = InitialFrameName;
            AnimationTimer = new Timer((uint)CurrentFrame.FrameLength);
        }

        /// <summary>
        /// Write the group.
        /// </summary>
        /// <param name="w">The writer.</param>
        public void Write(FileWriter w) {
            w.Write(Animated);
            w.Write(ResetInitialFrameOnChange);
            w.Write(InitialFrameName);
            w.Write(X);
            w.Write(Y);
            w.Write(Frames.Count);
            foreach (var f in Frames) {
                w.Write(f.Key);
                w.Write(f.Value);
            }
        }

        /// <summary>
        /// Do nothing.
        /// </summary>
        public override void Dispose() {}

    }
    
}
