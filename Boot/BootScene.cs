using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Boot {

    /// <summary>
    /// Boot scene.
    /// </summary>
    public class BootScene : Scene {

        /// <summary>
        /// Initialize the scene which just launches a cutscene.
        /// </summary>
        public override void Initialize() {
            GameHelper.Cutscene.StartCutscene("Boot/Boot.dream");
        }

    }

}
