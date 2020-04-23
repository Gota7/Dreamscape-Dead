using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Input {

    /// <summary>
    /// Input Listener. This is used for button pushed/released events, just use input manager to see if button is down or up. This is mostly used for UI and menu related stuff.
    /// </summary>
    public interface IInputListener {

        /// <summary>
        /// Hooks whenever a button is pushed. Use this or button fired, not both.
        /// </summary>
        /// <param name="button">button that is pushed.</param>
        void ButtonPushed(string button);

        /// <summary>
        /// Hooks whenever a button is released.
        /// </summary>
        /// <param name="button">button that is released.</param>
        void ButtonReleased(string button);

        /// <summary>
        /// Hooks whenever a button is fired from being held. Use this or button pushed, not both.
        /// </summary>
        /// <param name="button">button that is fired.</param>
        void ButtonFired(string button);

        /// <summary>
        /// Left mouse is pressed.
        /// </summary>
        void LeftMousePressed();

        /// <summary>
        /// Left mouse is released.
        /// </summary>
        void LeftMouseReleased();

        /// <summary>
        /// Right mouse is pressed.
        /// </summary>
        void RightMousePressed();

        /// <summary>
        /// Right mouse is released.
        /// </summary>
        void RightMouseReleased();

    }

}
