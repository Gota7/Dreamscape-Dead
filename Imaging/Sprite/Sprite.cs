using Dreamscape.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Imaging {

    /// <summary>
    /// Sprite.
    /// </summary>
    public class Sprite : IOFile, IDrawable, IUpdateable {

        /// <summary>
        /// Texture.
        /// </summary>
        public Texture Texture;

        /// <summary>
        /// Texture path.
        /// </summary>
        public string TexturePath;

        /// <summary>
        /// Base path.
        /// </summary>
        public string BasePath;

        /// <summary>
        /// X position.
        /// </summary>
        public float X;

        /// <summary>
        /// Y position.
        /// </summary>
        public float Y;

        /// <summary>
        /// Sprite groups.
        /// </summary>
        public Dictionary<string, SpriteGroup> Groups = new Dictionary<string, SpriteGroup>();

        /// <summary>
        /// Current group.
        /// </summary>
        public SpriteGroup CurrentGroup => Groups[CurrentGroupName];

        /// <summary>
        /// Current group name.
        /// </summary>
        public string CurrentGroupName {
            get => m_CurrentGroupName;
            set { string bak = m_CurrentGroupName; m_CurrentGroupName = value; if (CurrentGroup.ResetInitialFrameOnChange && !bak.Equals(value)) { CurrentGroup.CurrentFrameName = CurrentGroup.InitialFrameName; } }
        }
        private string m_CurrentGroupName;

        /// <summary>
        /// Current group index.
        /// </summary>
        public int CurrentGroupIndex {
            get => Groups.Keys.ToList().IndexOf(CurrentGroupName);
            set { CurrentGroupName = Groups.Keys.ElementAt(value); }
        }

        /// <summary>
        /// If disposed.
        /// </summary>
        private bool Disposed;

        /// <summary>
        /// Blank constructor.
        /// </summary>
        public Sprite() {}

        /// <summary>
        /// Create a new sprite.
        /// </summary>
        /// <param name="texture">Sprite texture.</param>
        /// <param name="groups">Sprite groups.</param>
        /// <param name="x">Sprite X position.</param>
        /// <param name="y">Sprite Y position.</param>
        /// <param name="currentGroupName">The current group index.</param>
        public Sprite(Texture texture, Dictionary<string, SpriteGroup> groups, float x = 0, float y = 0, string currentGroupName = null) {
            X = x;
            Y = y;
            CurrentGroupName = currentGroupName;
            Texture = texture;
            Groups = groups;
            if (CurrentGroupName == null && groups != null) {
                CurrentGroupName = Groups.Keys.ElementAt(0);
            }
            foreach (var g in Groups) {
                g.Value.Texture = Texture;
            }
        }

        /// <summary>
        /// Create a new sprite.
        /// </summary>
        /// <param name="filePath">The path to the sprite.</param>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position./param>
        /// <param name="currentGroupName">The current group index.</param>
        public Sprite(string filePath, float x = 0, float y = 0, string currentGroupName = null) {
            BasePath = Path.GetDirectoryName(filePath);
            X = x;
            Y = y;
            CurrentGroupName = currentGroupName;
            Read(filePath);
            if (CurrentGroupName == null) {
                CurrentGroupName = Groups.Keys.ElementAt(0);
            }
            foreach (var g in Groups) {
                g.Value.Texture = Texture;
            }
        }

        /// <summary>
        /// Deconstructor.
        /// </summary>
        ~Sprite() {
            Dispose();
        }

        /// <summary>
        /// Draw the sprite.
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
        public void Draw(float xOffset = 0, float yOffset = 0, bool ignoreCamera = false, Color? color = null, Vector2? origin = null, Angle rotation = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0) {
            CurrentGroup.Draw(X + xOffset, Y + yOffset, ignoreCamera, color, origin, rotation, scale, spriteEffects, layerDepth);
        }

        /// <summary>
        /// Update the sprite.
        /// </summary>
        public void Update() {
            CurrentGroup.Update();
        }

        /// <summary>
        /// Read the file.
        /// </summary>
        /// <param name="r">The reader.</param>
        public override void Read(FileReader r) {
            r.ReadUInt32();
            TexturePath = r.ReadString();
            Texture = new Texture((BasePath == "" ? TexturePath : (BasePath + "/" + TexturePath)));
            uint numGroups = r.ReadUInt32();
            Groups = new Dictionary<string, SpriteGroup>();
            for (uint i = 0; i < numGroups; i++) {
                string name = r.ReadString();
                SpriteGroup g = r.Read<SpriteGroup>();
                Groups.Add(name, g);
            }
            CurrentGroupName = Groups.Keys.ElementAt(0);
        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="w">The writer.</param>
        public override void Write(FileWriter w) {
            w.Write("DSPR".ToCharArray());
            w.Write(TexturePath.Replace('\\', '/').Replace(BasePath + "/", ""));
            w.Write((uint)Groups.Count);
            foreach (var g in Groups) {
                w.Write(g.Key);
                w.Write(g.Value);
            }
        }

        /// <summary>
        /// Dispose the object.
        /// </summary>
        public void Dispose() {
            if (!Disposed) {
                if (Texture != null) {
                    Texture.Dispose();
                }
                Disposed = true;
            }
        }

    }

}
