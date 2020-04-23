using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Input {

    /// <summary>
    /// Game button.
    /// </summary>
    public class GameButton {

        /// <summary>
        /// Keyboard key, if assigned.
        /// </summary>
        public Keys Key;

        /// <summary>
        /// Player number for the key.
        /// </summary>
        public PlayerIndex PlayerIndex;

        /// <summary>
        /// Game pad buttons, if assigned.
        /// </summary>
        public GamePadButton GamePadButtons;

        /// <summary>
        /// How long to wait before firing each input.
        /// </summary>
        public long FireDelay = 1000;

        /// <summary>
        /// Time left before firing again.
        /// </summary>
        public long FiringBuffer = 0;

        /// <summary>
        /// If the user input has been blocked.
        /// </summary>
        public bool InputBlocked;

        /// <summary>
        /// If the button is pushed by code.
        /// </summary>
        public bool CodeIsPush;

        /// <summary>
        /// Button pressed via code.
        /// </summary>
        private float codeButtonValue = 0;

        /// <summary>
        /// Make a new game button from the player index and key.
        /// </summary>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="key">Key to define.</param>
        /// <param name="fireDelay">How long to wait before raising the fired event.</param>
        public GameButton(PlayerIndex playerIndex, Keys key, long fireDelay = 1000) {
            PlayerIndex = playerIndex;
            Key = key;
            GamePadButtons = (ulong)GamePadButton.NULL;
        }

        /// <summary>
        /// Make a new game button from the player index and game pad button.
        /// </summary>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="gamePadButtons">Game pad button.</param>
        /// <param name="fireDelay">How long to wait before raising the fired event.</param>
        public GameButton(PlayerIndex playerIndex, GamePadButton gamePadButtons, long fireDelay = 1000) {
            PlayerIndex = playerIndex;
            Key = (Keys)(-1);
            GamePadButtons = gamePadButtons;
        }

        /// <summary>
        /// Make a new game button from the player index, key, and game pad button.
        /// </summary>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="key">Key to define.</param>
        /// <param name="gamePadButtons">Game pad button.</param>
        /// <param name="fireDelay">How long to wait before raising the fired event.</param>
        public GameButton(PlayerIndex playerIndex, Keys key, GamePadButton gamePadButtons, long fireDelay = 1000) {
            PlayerIndex = playerIndex;
            Key = key;
            GamePadButtons = gamePadButtons;
            FireDelay = fireDelay;
        }

        /// <summary>
        /// The value of the button.
        /// </summary>
        public float Value {

            get {

                //Get key value.
                float val = GamePadButtonHelper.RELEASED;
                if ((int)Key != -1) {
                    val = Math.Max(InputBlocked ? 0 : (GameHelper.Input.KeyDown(Key) ? GamePadButtonHelper.PRESSED : GamePadButtonHelper.RELEASED), codeButtonValue);
                }

                //Get gamepad value.
                float gamepadVal = GamePadButtonHelper.RELEASED;
                if (GamePadButtons != (ulong) GamePadButton.NULL) {
                    gamepadVal = Math.Max(InputBlocked ? 0 : (GameHelper.Input.GamepadButtonValue(PlayerIndex, GamePadButtons)), codeButtonValue);
                }

                //The value is the max of the two inputs.
                return Math.Max(val, gamepadVal);

            }

        }

        /// <summary>
        /// Whether or not the game key is active.
        /// </summary>
        public bool Down => Value > 0f;

        /// <summary>
        /// A game key is up if it is not down.
        /// </summary>
        public bool Up => !Down;

        /// <summary>
        /// Whether or not the button was previously down, only to be used by Input Manager.
        /// </summary>
        public bool PrevDown = false;

        /// <summary>
        /// Set the value of the game button.
        /// </summary>
        /// <param name="inputValue">Value to set the button to.</param>
        public void SetValue(float inputValue) {
            codeButtonValue = inputValue;
        }

        /// <summary>
        /// Set the button to up.
        /// </summary>
        public void SetButtonUp() {
            codeButtonValue = GamePadButtonHelper.RELEASED;
        }

        /// <summary>
        /// Set the button to down.
        /// </summary>
        public void SetButtonDown() {
            codeButtonValue = GamePadButtonHelper.PRESSED;
        }

        /// <summary>
        /// Push the button.
        /// </summary>
        public void PushButton() {
            if (codeButtonValue <= 0) { codeButtonValue = GamePadButtonHelper.PRESSED; }
            CodeIsPush = true;
        }

    }
    
}
