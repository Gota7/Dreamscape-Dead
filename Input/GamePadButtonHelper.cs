using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Input {

    /// <summary>
    /// Game pad button helper.
    /// </summary>
    public static class GamePadButtonHelper {

        /// <summary>
        /// Button is pressed.
        /// </summary>
        public const float PRESSED = 1f;

        /// <summary>
        /// Button is released.
        /// </summary>
        public const float RELEASED = 0f;

        /// <summary>
        /// Get the current value of a button.
        /// </summary>
        /// <param name="s">Gamepad state.</param>
        /// <param name="buttons">Button to test.</param>
        /// <returns>The value of the button.</returns>
        public static float ButtonsValue(GamePadState s, ulong buttons) {

            //Return release if disconnected.
            if (!s.IsConnected) {
                return RELEASED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.Guide) > 0 && s.Buttons.BigButton == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.A) > 0 && s.Buttons.A == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.B) > 0 && s.Buttons.B == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.X) > 0 && s.Buttons.X == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.Y) > 0 && s.Buttons.Y == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.Start) > 0 && s.Buttons.Start == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.Select) > 0 && s.Buttons.Back == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.Bumper_Left) > 0 && s.Buttons.LeftShoulder == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.Bumper_Right) > 0 && s.Buttons.RightShoulder == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.DPAD_Up) > 0 && s.DPad.Up == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.DPAD_Down) > 0 && s.DPad.Down == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.DPAD_Left) > 0 && s.DPad.Left == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.DPAD_Right) > 0 && s.DPad.Right == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.LeftStick_Button) > 0 && s.Buttons.LeftStick == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.RightStick_Button) > 0 && s.Buttons.RightStick == ButtonState.Pressed) {
                return PRESSED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.LeftStick_Up) > 0) {
                return s.ThumbSticks.Left.Y > 0 ? s.ThumbSticks.Left.Y : RELEASED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.LeftStick_Down) > 0) {
                return s.ThumbSticks.Left.Y < 0 ? Math.Abs(s.ThumbSticks.Left.Y) : RELEASED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.LeftStick_Left) > 0) {
                return s.ThumbSticks.Left.X < 0 ? Math.Abs(s.ThumbSticks.Left.X) : RELEASED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.LeftStick_Right) > 0) {
                return s.ThumbSticks.Left.X > 0 ? s.ThumbSticks.Left.X : RELEASED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.RightStick_Up) > 0) {
                return s.ThumbSticks.Right.Y > 0 ? s.ThumbSticks.Right.Y : RELEASED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.RightStick_Down) > 0) {
                return s.ThumbSticks.Right.Y < 0 ? Math.Abs(s.ThumbSticks.Right.Y) : RELEASED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.RightStick_Left) > 0) {
                return s.ThumbSticks.Right.X < 0 ? Math.Abs(s.ThumbSticks.Right.X) : RELEASED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.RightStick_Right) > 0) {
                return s.ThumbSticks.Right.X > 0 ? s.ThumbSticks.Right.X : RELEASED;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.Trigger_Left) > 0) {
                return s.Triggers.Left;
            }

            //See if the button is pressed.
            if ((buttons & (ulong)GamePadButton.Trigger_Right) > 0) {
                return s.Triggers.Right;
            }

            //Return released by default.
            return RELEASED;

        }

        /// <summary>
        /// If a button is down.
        /// </summary>
        /// <param name="s">Gamepad state.</param>
        /// <param name="buttons">Button to test.</param>
        /// <returns>If the button is down or not.</returns>
        public static bool Down(GamePadState s, ulong buttons) {
            return ButtonsValue(s, buttons) > RELEASED;
        }

    }

}