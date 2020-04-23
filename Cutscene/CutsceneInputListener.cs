using Dreamscape.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Cutscene {

    /// <summary>
    /// Cutscene input listener.
    /// </summary>
    public class CutsceneInputListener : IInputListener {

        /// <summary>
        /// Parent manager.
        /// </summary>
        private CutsceneManager Parent;

        /// <summary>
        /// Create a new input listener.
        /// </summary>
        /// <param name="parent">The parent listener.</param>
        public CutsceneInputListener(CutsceneManager parent) {
            Parent = parent;
            GameHelper.Input.InputListeners.Add(this);
        }

        public void ButtonFired(string button) {
            
        }

        public void ButtonPushed(string button) {

            //Any button.
            if (Parent.RunningCutscene && Parent.RunningCommand && Parent.CurrentCommand.CommandType == CommandType.WaitForAnyButton) {
                Parent.RunningCommand = false;
                return;
            }

            //Certain button.
            if (Parent.RunningCutscene && Parent.RunningCommand && Parent.CurrentCommand.CommandType == CommandType.WaitForButton) {
                if (Parent.CurrentCommand.StringParameters[0].Equals(button)) {
                    Parent.RunningCommand = false;
                    return;
                }
            }

        }

        public void ButtonReleased(string button) {
            
        }

        public void LeftMousePressed() {
            
        }

        public void LeftMouseReleased() {
            
        }

        public void RightMousePressed() {
            
        }

        public void RightMouseReleased() {
            
        }

    }

}
