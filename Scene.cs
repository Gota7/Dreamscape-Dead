using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// A scene.
    /// </summary>
    public abstract class Scene : IUpdateable {

        /// <summary>
        /// Objects.
        /// </summary>
        //public List<GameObject> Objects = new List<GameObject>();

        /// <summary>
        /// Camara stack.
        /// </summary>
        protected CameraStackEntry CS = new CameraStackEntry();

        /// <summary>
        /// Render target.
        /// </summary>
        public RenderTarget2D RenderTarget;

        /// <summary>
        /// Render the scene instead of drawing it.
        /// </summary>
        public void Render() {
            GameHelper.Camera.PushCameraStack(CS.X, CS.Y, CS.Width, CS.Height, CS.Width, CS.Height, 1, 1, false);
            GameHelper.EndDraw();
            GameHelper.Graphics.SetRenderTarget(RenderTarget);
            GameHelper.StartDraw();
            Draw();
            GameHelper.EndDraw();
            GameHelper.Graphics.SetRenderTarget(null);
            GameHelper.Camera.PopCameraStack();
            GameHelper.StartDraw();
            GameHelper.Graphics.Clear(GameHelper.ClearColor);
        }

        /// <summary>
        /// Initialize the scene.
        /// </summary>
        public virtual void Initialize() {}

        /// <summary>
        /// Draw the scene.
        /// </summary>
        public virtual void Draw() {}

        /// <summary>
        /// Update the scene.
        /// </summary>
        public virtual void Update() {}

    }

    /// <summary>
    /// Default scene.
    /// </summary>
    public sealed class DefaultScene : Scene {

        /// <summary>
        /// Draw the scene.
        /// </summary>
        public override void Draw() {}

        /// <summary>
        /// Update the scene.
        /// </summary>
        public override void Update() {}

    }

}