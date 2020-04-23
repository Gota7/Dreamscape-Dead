using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Input {

    /// <summary>
    /// Updates states and raises events.
    /// </summary>
    public class InputManager {

        /// <summary>
        /// Game buttons.
        /// </summary>
        public Dictionary<string, GameButton> GameButtons = new Dictionary<string, GameButton>();

        /// <summary>
        /// Input listeners.
        /// </summary>
        public List<IInputListener> InputListeners = new List<IInputListener>();

        /// <summary>
        /// Keyboard state.
        /// </summary>
        private KeyboardState KS;

        /// <summary>
        /// Player 1 state.
        /// </summary>
        private GamePadState P1;

        /// <summary>
        /// Player 2 state.
        /// </summary>
        private GamePadState P2;

        /// <summary>
        /// Player 3 state.
        /// </summary>
        private GamePadState P3;

        /// <summary>
        /// Player 4 state.
        /// </summary>
        private GamePadState P4;

        /// <summary>
        /// Vibrating controllers;
        /// </summary>
        private VibratingController[] VibratingControllers = new VibratingController[4];

        /// <summary>
        /// Mouse state.
        /// </summary>
        private MouseState MouseState;

        /// <summary>
        /// Mouse down.
        /// </summary>
        public bool LeftMouseDown => MouseState.LeftButton == ButtonState.Pressed;

        /// <summary>
        /// Mouse up.
        /// </summary>
        public bool LeftMouseUp => !LeftMouseDown;

        /// <summary>
        /// Mouse down.
        /// </summary>
        public bool RightMouseDown => MouseState.RightButton == ButtonState.Pressed;

        /// <summary>
        /// Mouse up.
        /// </summary>
        public bool RightMouseUp => !RightMouseDown;

        /// <summary>
        /// Mouse X.
        /// </summary>
        public int MouseX => (MouseState.X / ((float)GameHelper.CurrentGame.ResolutionX / GameHelper.CurrentGame.Width)).Round();

        /// <summary>
        /// Mouse Y.
        /// </summary>
        public int MouseY => (MouseState.Y / ((float)GameHelper.CurrentGame.ResolutionY / GameHelper.CurrentGame.Height)).Round();

        /// <summary>
        /// Pushed buttons.
        /// </summary>
        private Dictionary<GameButton, bool> PushedButtons = new Dictionary<GameButton, bool>();

        /// <summary>
        /// Vibrating controller entry.
        /// </summary>
        public struct VibratingController {
            public float Left;
            public float Right;
            public bool Vibrating => Left > 0 || Right > 0;
        }

        /// <summary>
        /// Return if the key is down.
        /// </summary>
        /// <param name="key">The key to check if it's down.</param>
        /// <returns>If the key is down.</returns>
        public bool KeyDown(Keys key) {
            return KS.IsKeyDown(key);
        }

        /// <summary>
        /// Button value.
        /// </summary>
        /// <param name="button">Button name.</param>
        /// <returns>Button value.</returns>
        public float ButtonValue(string button) {
            return GameButtons[button].Value;
        }

        /// <summary>
        /// Button down.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns>If the button is down.</returns>
        public bool ButtonDown(string button) {
            return GameButtons[button].Down;
        }

        /// <summary>
        /// Button pushed.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns>If the button is pushed.</returns>
        public bool ButtonPushed(string button) {
            if (!PushedButtons.ContainsKey(GameButtons[button])) {
                return false;
            }
            bool ret = PushedButtons[GameButtons[button]];
            PushedButtons[GameButtons[button]] = false;
            return ret;
        }

        /// <summary>
        /// Button up.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns>If the button is up.</returns>
        public bool ButtonUp(string button) {
            return GameButtons[button].Up;
        }

        /// <summary>
        /// Get the gamepad buttons value.
        /// </summary>
        /// <param name="playerIndex">Player ID to get the value from.</param>
        /// <param name="buttons">Buttons to get the value from.</param>
        /// <returns>The button value for the specified player.</returns>
        public float GamepadButtonValue(PlayerIndex playerIndex, GamePadButton buttons) {

            //Switch the player number.
            switch (playerIndex) {

                case PlayerIndex.One:
                    return GamePadButtonHelper.ButtonsValue(P1, (ulong)buttons);

                case PlayerIndex.Two:
                    return GamePadButtonHelper.ButtonsValue(P2, (ulong)buttons);

                case PlayerIndex.Three:
                    return GamePadButtonHelper.ButtonsValue(P3, (ulong)buttons);

                case PlayerIndex.Four:
                    return GamePadButtonHelper.ButtonsValue(P4, (ulong)buttons);

            }

            //Idk how this happened, just return released.
            return GamePadButtonHelper.RELEASED;

        }

        /// <summary>
        /// Vibrate a controller.
        /// </summary>
        /// <param name="playerIndex">Player index.</param>
        /// <param name="leftMotor">Low frequency motor value (0 to 1).</param>
        /// <param name="rightMotor">High frequency motor value (0 to 1).</param>
        public void VibrateController(PlayerIndex playerIndex, float leftMotor, float rightMotor) {
            VibratingControllers[(int)playerIndex].Left = leftMotor;
            VibratingControllers[(int)playerIndex].Right = rightMotor;
        }

        /// <summary>
        /// End the vibration of a controller.
        /// </summary>
        /// <param name="playerIndex">Player to stop vibrating.</param>
        public void EndVibration(PlayerIndex playerIndex) {
            VibratingControllers[(int)playerIndex].Left = 0;
            VibratingControllers[(int)playerIndex].Right = 0;
            GamePad.SetVibration(playerIndex, 0, 0);
        }

        /// <summary>
        /// Update the states, and raise necessary events.
        /// </summary>
        /// <returns></returns>
        public void Update() {

            //Vibrate controllers.
            for (int i = 0; i < VibratingControllers.Length; i++) {
                if (VibratingControllers[i].Vibrating) {
                    GamePad.SetVibration(i, VibratingControllers[i].Left, VibratingControllers[i].Right);
                }
            }

            //Mouse events.
            MouseState newState = Mouse.GetState();
            if (LeftMouseDown != (newState.LeftButton == ButtonState.Pressed)) {
                if (newState.LeftButton == ButtonState.Pressed) {
                    foreach (var l in InputListeners) {
                        l.LeftMousePressed();
                    }
                } else {
                    foreach (var l in InputListeners) {
                        l.LeftMouseReleased();
                    }
                }
            }
            if (RightMouseDown != (newState.RightButton == ButtonState.Pressed)) {
                if (newState.RightButton == ButtonState.Pressed) {
                    foreach (var l in InputListeners) {
                        l.RightMousePressed();
                    }
                } else {
                    foreach (var l in InputListeners) {
                        l.RightMouseReleased();
                    }
                }
            }

            //Update the states.
            MouseState = newState;
            KS = Keyboard.GetState();
            P1 = GamePad.GetState(PlayerIndex.One);
            P2 = GamePad.GetState(PlayerIndex.Two);
            P3 = GamePad.GetState(PlayerIndex.Three);
            P4 = GamePad.GetState(PlayerIndex.Four);

            //Go through each game button.
            for (int i = 0; i < GameButtons.Count; i++) {

                //The button is now down.
                if (GameButtons[GameButtons.Keys.ElementAt(i)].Down) {

                    //If down and the previous value don't match.
                    if (GameButtons[GameButtons.Keys.ElementAt(i)].Down != GameButtons[GameButtons.Keys.ElementAt(i)].PrevDown || GameButtons[GameButtons.Keys.ElementAt(i)].CodeIsPush) {

                        //Fullscreen action is hardcoded.
                        if (GameButtons.ContainsKey("FULLSCREEN") && GameButtons.Keys.ElementAt(i).Equals("FULLSCREEN")) {
                            GameHelper.CurrentGame.ToggleFullscreen();
                        }

                        //Button pushed is raised.
                        foreach (var l in InputListeners) {
                            l.ButtonPushed(GameButtons.Keys.ElementAt(i));
                        }

                        //Button pushed list.
                        if (!PushedButtons.ContainsKey(GameButtons.Values.ElementAt(i))) {
                            PushedButtons.Add(GameButtons.Values.ElementAt(i), true);
                        } else {
                            PushedButtons[GameButtons.Values.ElementAt(i)] = true;
                        }

                    }

                    //Decrease the buffer.
                    GameButtons[GameButtons.Keys.ElementAt(i)].FiringBuffer -= (long)GameHelper.GameTime.ElapsedGameTime.TotalMilliseconds;

                    //Fire the button if needed.
                    if (GameButtons[GameButtons.Keys.ElementAt(i)].FiringBuffer <= 0) {

                        //Button released is raised.
                        foreach (var l in InputListeners) {
                            l.ButtonFired(GameButtons.Keys.ElementAt(i));
                        }

                        //Reset the buffer.
                        GameButtons[GameButtons.Keys.ElementAt(i)].FiringBuffer = GameButtons[GameButtons.Keys.ElementAt(i)].FireDelay;

                    }

                }

                //The button is now up.
                else {

                    //Button released is raised.
                    foreach (var l in InputListeners) {
                        l.ButtonReleased(GameButtons.Keys.ElementAt(i));
                    }

                    //Button pushed list.
                    if (!PushedButtons.ContainsKey(GameButtons.Values.ElementAt(i))) {
                        PushedButtons.Add(GameButtons.Values.ElementAt(i), false);
                    } else {
                        PushedButtons[GameButtons.Values.ElementAt(i)] = false;
                    }

                    //No firing buffer.
                    GameButtons[GameButtons.Keys.ElementAt(i)].FiringBuffer = 0;

                }

                //Set the previous state to the current state.
                GameButtons[GameButtons.Keys.ElementAt(i)].PrevDown = GameButtons[GameButtons.Keys.ElementAt(i)].Down;

                //If pushed by code, make sure to unpush it.
                if (GameButtons[GameButtons.Keys.ElementAt(i)].CodeIsPush) {
                    GameButtons[GameButtons.Keys.ElementAt(i)].CodeIsPush = false;
                    GameButtons[GameButtons.Keys.ElementAt(i)].SetValue(GamePadButtonHelper.RELEASED);
                }

            }

        }

    }

}
