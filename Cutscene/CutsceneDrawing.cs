using Dreamscape.Imaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Cutscene {

    /// <summary>
    /// Cutscene drawing.
    /// </summary>
    public class CutsceneDrawing : Imaging.IDrawable {

        /// <summary>
        /// Parent manager.
        /// </summary>
        private CutsceneManager Parent;

        /// <summary>
        /// Drawing.
        /// </summary>
        public Drawing Drawing;

        /// <summary>
        /// If the drawing is visible.
        /// </summary>
        public bool Visible;

        /// <summary>
        /// Whether or not to ignore the camera.
        /// </summary>
        public bool IgnoreCamera;

        /// <summary>
        /// If the drawing survives after the cutscene.
        /// </summary>
        public bool Persistent;

        /// <summary>
        /// Rotation of the drawing.
        /// </summary>
        public Angle Rotation;

        /// <summary>
        /// Origin.
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// Layer depth.
        /// </summary>
        public float LayerDepth;

        /// <summary>
        /// Scale.
        /// </summary>
        public Vector2 Scale = new Vector2(1, 1);

        /// <summary>
        /// Sprite effects.
        /// </summary>
        public SpriteEffects SpriteEffects = SpriteEffects.None;

        /// <summary>
        /// Color.
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// Velocity.
        /// </summary>
        public Vector2 Velocity = Vector2.Zero;

        /// <summary>
        /// Moving right.
        /// </summary>
        public bool MovingRight => Velocity.X >= 0;

        /// <summary>
        /// Moving down.
        /// </summary>
        public bool MovingDown => Velocity.Y >= 0;

        /// <summary>
        /// Acceleration.
        /// </summary>
        public Vector2 Acceleration = Vector2.Zero;

        /// <summary>
        /// Opacity when fading.
        /// </summary>
        private float opacity = 1f;

        /// <summary>
        /// Fade timer.
        /// </summary>
        private Timer fadeTimer;

        /// <summary>
        /// Fading.
        /// </summary>
        private bool fading;

        /// <summary>
        /// Unfading.
        /// </summary>
        private bool unfading;

        /// <summary>
        /// Waiting for the drawing to reach a certain position.
        /// </summary>
        private bool waitingForPosition = false;

        /// <summary>
        /// X position to wait for.
        /// </summary>
        private float waitingForX;

        /// <summary>
        /// Y position to wait for.
        /// </summary>
        private float waitingForY;

        /// <summary>
        /// If disposed.
        /// </summary>
        private bool Disposed;

        /// <summary>
        /// Make a new cutscene drawing.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public CutsceneDrawing(CutsceneManager parent) {
            Parent = parent;
        }

        /// <summary>
        /// Deconstructor.
        /// </summary>
        ~CutsceneDrawing() {
            Dispose();
        }

        /// <summary>
        /// Wait till the drawing gets toward a certain position.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        public void WaitTillPosition(float x, float y) {
            waitingForPosition = true;
            waitingForX = x;
            waitingForY = y;
        }

        /// <summary>
        /// Fade the drawing.
        /// </summary>
        /// <param name="milliseconds">Milliseconds it takes to fade.</param>
        public void Fade(uint milliseconds) {
            fading = true;
            fadeTimer = new Timer(milliseconds);
        }

        /// <summary>
        /// Unfade the drawing.
        /// </summary>
        /// <param name="milliseconds">Milliseconds it takes to unfade.</param>
        public void Unfade(uint milliseconds) {
            unfading = true;
            fadeTimer = new Timer(milliseconds);
        }

        /// <summary>
        /// Draw the cutscene drawing. Everything except offsets is ignored.
        /// </summary>
        public void Draw(float xOffset = 0, float yOffset = 0, bool ignoreCamera = false, Color? color = null, Vector2? origin = null, Angle rotation = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0) {

            //Draw the drawing.
            Drawing.Draw(xOffset, yOffset, IgnoreCamera, Color * opacity, Origin, Rotation, Scale, SpriteEffects, LayerDepth);

        }

        /// <summary>
        /// Update the drawing.
        /// </summary>
        public void Update() {

            //Fading.
            if (fading) {
                fadeTimer.Update();
                uint time = fadeTimer.TimeRemaining();
                if (time == 0xFFFFFFFF) {
                    opacity = 0;
                    fading = false;
                } else {
                    opacity = (float)time / fadeTimer.InitialTime();
                }
            }

            //Unfading.
            if (unfading) {
                fadeTimer.Update();
                uint time = fadeTimer.TimeRemaining();
                if (time == 0xFFFFFFFF) {
                    opacity = 1;
                    unfading = false;
                } else {
                    opacity = 1 - (float)time / fadeTimer.InitialTime();
                }
            }

            //Update position.
            Drawing.X += (float)(Velocity.X * GameHelper.GameTime.ElapsedGameTime.TotalSeconds);
            Drawing.Y += (float)(Velocity.Y * GameHelper.GameTime.ElapsedGameTime.TotalSeconds);
            Velocity.X += (float)(Acceleration.X * GameHelper.GameTime.ElapsedGameTime.TotalSeconds);
            Velocity.Y += (float)(Acceleration.Y * GameHelper.GameTime.ElapsedGameTime.TotalSeconds);

            //Update drawing.
            if (Drawing as IUpdateable != null) {
                ((IUpdateable)Drawing).Update();
            }

            //Waiting for position.
            if (waitingForPosition) {

                //Past variables.
                bool pastX;
                bool pastY;

                //Right.
                if (MovingRight) {
                    pastX = Drawing.X >= waitingForX;
                }

                //Left.
                else {
                    pastX = Drawing.X <= waitingForX;
                }

                //Up.
                if (!MovingDown) {
                    pastY = Drawing.Y <= waitingForY;
                }
                
                //Down.
                else {
                    pastY = Drawing.Y >= waitingForY;
                }

                //If past both, then advance cutscene.
                if (pastX && pastY) {
                    Parent.RunningCommand = false;
                    waitingForPosition = false;
                }
            
            }

        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose() {
            if (!Disposed && Drawing as IDisposable != null) {
                (Drawing as IDisposable).Dispose();
                Disposed = true;
            }
        }  

    }

}
