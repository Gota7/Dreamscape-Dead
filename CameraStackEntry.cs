using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// A stack entry for the camera.
    /// </summary>
    public class CameraStackEntry {

        /// <summary>
        /// Camera X-Position.
        /// </summary>
        public float X;

        /// <summary>
        /// Camera Y-Position.
        /// </summary>
        public float Y;

        /// <summary>
        /// Original width.
        /// </summary>
        public float TargetWidth = 1024;

        /// <summary>
        /// Original height.
        /// </summary>
        public float TargetHeight = 576;

        /// <summary>
        /// Camera width.
        /// </summary>
        public float Width = 1024;

        /// <summary>
        /// Camera height.
        /// </summary>
        public float Height = 576;

        /// <summary>
        /// X scale.
        /// </summary>
        public float XScale = 1;

        /// <summary>
        /// Y scale.
        /// </summary>
        public float YScale = 1;

        /// <summary>
        /// Whether or not the camera is in fullscreen.
        /// </summary>
        public bool Fullscreen = false;

    }

}
