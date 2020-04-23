using Dreamscape.Audio;
using Dreamscape.Imaging;
using Dreamscape.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Cutscene {

    /// <summary>
    /// Cutscene manager.
    /// </summary>
    public class CutsceneManager {

        /// <summary>
        /// Commands.
        /// </summary>
        public List<CutsceneCommand> Commands = new List<CutsceneCommand>();

        /// <summary>
        /// Current command index.
        /// </summary>
        public int CurrentCommandIndex = 0;

        /// <summary>
        /// Get the current command.
        /// </summary>
        public CutsceneCommand CurrentCommand => Commands[CurrentCommandIndex];

        /// <summary>
        /// Custom command interpreter.
        /// </summary>
        public InterpretCustomCommand CustomCommandInterpreter = null;

        /// <summary>
        /// Custom command updater.
        /// </summary>
        public UpdateCustomCommand CustomCommandUpdater = null;

        /// <summary>
        /// Listen to input.
        /// </summary>
        public CutsceneInputListener InputListener;

        /// <summary>
        /// If the cutscene is running.
        /// </summary>
        public bool RunningCutscene { get; private set; }

        /// <summary>
        /// If a command is running.
        /// </summary>
        public bool RunningCommand {
            get {
                return m_RunningCommand;
            } set {
                if (!value) {
                    CurrentCommandIndex++;
                }
                m_RunningCommand = value;
            }
        }
        private bool m_RunningCommand;

        /// <summary>
        /// Wait timer.
        /// </summary>
        private Timer waitTimer = new Timer(0);

        /// <summary>
        /// Cutscene drawings.
        /// </summary>
        private Dictionary<string, CutsceneDrawing> drawings = new Dictionary<string, CutsceneDrawing>();

        /// <summary>
        /// Interpret a custom command.
        /// </summary>
        /// <param name="commandType">The command type.</param>
        /// <param name="command">Command to interpret.</param>
        public delegate void InterpretCustomCommand(string commandType, CutsceneCommand command);

        /// <summary>
        /// Update a custom command.
        /// </summary>
        /// <param name="commandType">The command type.</param>
        /// <param name="command">The command to update.</param>
        public delegate void UpdateCustomCommand(string commandType, CutsceneCommand command);

        /// <summary>
        /// Cutscene manager.
        /// </summary>
        public CutsceneManager() {
            InputListener = new CutsceneInputListener(this);
        }

        /// <summary>
        /// Start a cutscene from a file.
        /// </summary>
        /// <param name="filePath">Path to the cutscene file.</param>
        /// <param name="customCommandInterpreter">The interpreter to interpret custom commands.</param>
        /// <param name="customCommandUpdater">The updater to execute the update cycle of custom commands.</param>
        /// <param name="currentCommandIndex">The command index to start with.</param>
        public void StartCutscene(string filePath, InterpretCustomCommand customCommandInterpreter = null, UpdateCustomCommand customCommandUpdater = null, int currentCommandIndex = 0) {
            StartCutscene(CutsceneFile.RetrieveCommands(filePath), customCommandInterpreter, customCommandUpdater, currentCommandIndex);
        }

        /// <summary>
        /// Start the cutscene.
        /// </summary>
        /// <param name="commands">Commands to start.</param>
        /// <param name="customCommandInterpreter">The interpreter to interpret custom commands.</param>
        /// <param name="customCommandUpdater">The updater to execute the update cycle of custom commands.</param>
        /// <param name="currentCommandIndex">The command index to start with.</param>
        public void StartCutscene(List<CutsceneCommand> commands, InterpretCustomCommand customCommandInterpreter = null, UpdateCustomCommand customCommandUpdater = null, int currentCommandIndex = 0) {
            Commands = commands;
            CustomCommandInterpreter = customCommandInterpreter;
            CustomCommandUpdater = customCommandUpdater;
            RunningCutscene = true;
            RunningCommand = false;
            CurrentCommandIndex = currentCommandIndex;
        }

        /// <summary>
        /// Execute the next. command.
        /// </summary>
        public void ExecuteCommand() {

            //Get the command.
            CutsceneCommand c = CurrentCommand;

            //Switch the type of command.
            switch (c.CommandType) {

                //Custom.
                case CommandType.Custom:
                    CustomCommandInterpreter?.Invoke(c.CustomCommandType, c);
                    if (CustomCommandInterpreter == null) { RunningCommand = false; }
                    break;

                //Wait.
                case CommandType.Wait:
                    waitTimer = new Timer((uint)c.GetNumber(0));
                    break;

                //Load audio.
                case CommandType.LoadSound:
                    GameHelper.Audio.LoadSound(c.StringParameters[0], c.StringParameters[1], c.StringParameters.Length > 2 ? (Loop)FileSystem.OpenFile<Loop>(c.StringParameters[3]) : null);
                    RunningCommand = false;
                    break;

                //Unload audio.
                case CommandType.UnloadSound:
                    GameHelper.Audio.UnloadSound(c.StringParameters[0]);
                    RunningCommand = false;
                    break;

                //Play sound.
                case CommandType.PlaySound:
                    GameHelper.Audio.PlaySound(c.StringParameters[0], c.FloatParameters.Length > 0 ? c.FloatParameters[0] : 1f, c.BooleanParameters.Length > 0 ? c.BooleanParameters[0] : false);
                    RunningCommand = false;
                    break;

                //Play an independent sound.
                case CommandType.PlayIndependentSound:
                    //GameHelper.Audio.PlayIndependentSound(c.StringParameters[0], c.StringParameters[1], c.FloatParameters.Length > 0 ? c.FloatParameters[0] : 1f, c.StringParameters.Length > 2 ? (Loop)FileSystem.OpenFile<Loop>(c.StringParameters[1]) : null);
                    RunningCommand = false;
                    break;

                //Resume a sound.
                case CommandType.ResumeSound:
                    GameHelper.Audio.ResumeSound(c.StringParameters[0]);
                    RunningCommand = false;
                    break;

                //Pause a sound.
                case CommandType.PauseSound:
                    GameHelper.Audio.PauseSound(c.StringParameters[0]);
                    RunningCommand = false;
                    break;

                //Stop a sound.
                case CommandType.StopSound:
                    GameHelper.Audio.StopSound(c.StringParameters[0]);
                    RunningCommand = false;
                    break;

                //Sound position.
                case CommandType.SoundPosition:
                    GameHelper.Audio.SetPosition(c.StringParameters[0], new TimeSpan((int)(c.DecimalParameters.Length > 0 ? c.DecimalParameters[0] : 0), (int)(c.DoubleParameters.Length > 0 ? c.DoubleParameters[0] : 0), (int)(c.FloatParameters.Length > 0 ? c.FloatParameters[0] : 0), c.IntParameters.Length > 0 ? c.IntParameters[0] : 0, (int)(c.UIntParameters.Length > 0 ? c.UIntParameters[0] : 0)));
                    RunningCommand = false;
                    break;

                //Sound volume.
                case CommandType.SoundVolume:
                    GameHelper.Audio.SetVolume(c.StringParameters[0], c.FloatParameters[0]);
                    RunningCommand = false;
                    break;

                //Load drawing.
                case CommandType.LoadDrawing:
                    Image image = new Image(c.StringParameters[1]);
                    if (!drawings.ContainsKey(c.StringParameters[0])) {
                        drawings.Add(c.StringParameters[0], new CutsceneDrawing(this));
                    }
                    drawings[c.StringParameters[0]].Drawing = image;
                    if (c.BooleanParameters.Length > 0) { drawings[c.StringParameters[0]].Persistent = c.BooleanParameters[0]; }
                    RunningCommand = false;
                    break;

                //Load image from a color.
                case CommandType.LoadImageFromColor:
                    Drawing color = new Image(new Microsoft.Xna.Framework.Color(c.IntParameters[0], c.IntParameters[1], c.IntParameters[2], c.IntParameters.Length > 3 ? c.IntParameters[3] : 255));
                    color.Width = c.FloatParameters[0];
                    color.Height = c.FloatParameters[0];
                    if (!drawings.ContainsKey(c.StringParameters[0])) {
                        drawings.Add(c.StringParameters[0], new CutsceneDrawing(this));
                    }
                    drawings[c.StringParameters[0]].Drawing = color;
                    if (c.BooleanParameters.Length > 0) { drawings[c.StringParameters[0]].Persistent = c.BooleanParameters[0]; }
                    RunningCommand = false;
                    break;

                //Drawing persistence.
                case CommandType.DrawingPersistence:
                    bool persistence = !drawings[c.StringParameters[0]].Persistent;
                    if (c.BooleanParameters.Length > 0) { persistence = c.BooleanParameters[0]; }
                    drawings[c.StringParameters[0]].Persistent = persistence;
                    RunningCommand = false;
                    break;

                //Drawing ignore camera.
                case CommandType.DrawingIgnoreCamera:
                    bool ignoreCamera = !drawings[c.StringParameters[0]].IgnoreCamera;
                    if (c.BooleanParameters.Length > 0) { ignoreCamera = c.BooleanParameters[0]; }
                    drawings[c.StringParameters[0]].IgnoreCamera = ignoreCamera;
                    RunningCommand = false;
                    break;

                //Drawing X position.
                case CommandType.DrawingX:
                    drawings[c.StringParameters[0]].Drawing.X = (float)c.GetNumber(0);
                    RunningCommand = false;
                    break;

                //Drawing Y position.
                case CommandType.DrawingY:
                    drawings[c.StringParameters[0]].Drawing.Y = (float)c.GetNumber(0);
                    RunningCommand = false;
                    break;

                //Drawing width.
                case CommandType.DrawingWidth:
                    drawings[c.StringParameters[0]].Drawing.Width = (float)c.GetNumber(0);
                    RunningCommand = false;
                    break;

                //Drawing height.
                case CommandType.DrawingHeight:
                    drawings[c.StringParameters[0]].Drawing.Height = (float)c.GetNumber(0);
                    RunningCommand = false;
                    break;

                //Drawing velocity X.
                case CommandType.DrawingVelocityX:
                    drawings[c.StringParameters[0]].Velocity.X = (float)c.GetNumber(0);
                    RunningCommand = false;
                    break;

                //Drawing velocity Y.
                case CommandType.DrawingVelocityY:
                    drawings[c.StringParameters[0]].Velocity.Y = (float)c.GetNumber(0);
                    RunningCommand = false;
                    break;

                //Drawing acceleration X.
                case CommandType.DrawingAccelerationX:
                    drawings[c.StringParameters[0]].Acceleration.X = (float)c.GetNumber(0);
                    RunningCommand = false;
                    break;

                //Drawing acceleration Y.
                case CommandType.DrawingAccelerationY:
                    drawings[c.StringParameters[0]].Acceleration.Y = (float)c.GetNumber(0);
                    RunningCommand = false;
                    break;

                //Drawing wait till position.
                case CommandType.DrawingWaitTillPosition:
                    drawings[c.StringParameters[0]].WaitTillPosition((float)c.GetNumber(0), (float)c.GetNumber(1));
                    break;

                //Drawing rotation.
                case CommandType.DrawingRotation:
                    Angle a = new Angle();
                    if (c.UIntParameters.Length > 0) {
                        a = (ushort)c.UIntParameters[0];
                    } else if (c.IntParameters.Length > 0) {
                        a = (ushort)c.IntParameters[0];
                    } else if (c.DoubleParameters.Length > 0) {
                        a.Radians = c.DoubleParameters[0];
                    } else if (c.FloatParameters.Length > 0) {
                        a.Degrees = c.FloatParameters[0];
                    }
                    drawings[c.StringParameters[0]].Rotation = a;
                    RunningCommand = false;
                    break;

                //Drawing origin.
                case CommandType.DrawingOrigin:
                    if (c.StringParameters.Length > 1) {
                        switch (c.StringParameters[1].ToLower()) {
                            case "center":
                                drawings[c.StringParameters[0]].Origin = new Microsoft.Xna.Framework.Vector2(drawings[c.StringParameters[0]].Drawing.Width / 2, drawings[c.StringParameters[0]].Drawing.Height / 2);
                                break;
                            case "topleft":
                                drawings[c.StringParameters[0]].Origin = new Microsoft.Xna.Framework.Vector2(0, 0);
                                break;
                            case "bottomleft":
                                drawings[c.StringParameters[0]].Origin = new Microsoft.Xna.Framework.Vector2(0, drawings[c.StringParameters[0]].Drawing.Height);
                                break;
                            case "topright":
                                drawings[c.StringParameters[0]].Origin = new Microsoft.Xna.Framework.Vector2(drawings[c.StringParameters[0]].Drawing.Width, 0);
                                break;
                            case "bottomright":
                                drawings[c.StringParameters[0]].Origin = new Microsoft.Xna.Framework.Vector2(drawings[c.StringParameters[0]].Drawing.Width, drawings[c.StringParameters[0]].Drawing.Height);
                                break;
                        }
                    } else {
                        drawings[c.StringParameters[0]].Origin = new Microsoft.Xna.Framework.Vector2((float)c.GetNumber(0), (float)c.GetNumber(1));
                    }
                    RunningCommand = false;
                    break;

                //Drawing layer depth.
                case CommandType.DrawingLayerDepth:
                    drawings[c.StringParameters[0]].LayerDepth = (float)c.GetNumber(0);
                    RunningCommand = false;
                    break;

                //Drawing sprite effects.
                case CommandType.DrawingSpriteEffects:
                    switch (c.StringParameters[1].ToLower()) {
                        case "fliphorizontal":
                            drawings[c.StringParameters[0]].SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                            break;
                        case "flipvertical":
                            drawings[c.StringParameters[0]].SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically;
                            break;
                        case "both":
                            drawings[c.StringParameters[0]].SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally | Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically;
                            break;
                        default:
                            drawings[c.StringParameters[0]].SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                            break;
                    }
                    RunningCommand = false;
                    break;

                //Drawing scale X.
                case CommandType.DrawingScaleX:
                    drawings[c.StringParameters[0]].Scale.X = (float)c.GetNumber(0);
                    RunningCommand = false;
                    break;

                //Drawing scale Y.
                case CommandType.DrawingScaleY:
                    drawings[c.StringParameters[0]].Scale.Y = (float)c.GetNumber(0);
                    RunningCommand = false;
                    break;

                //Drawing color.
                case CommandType.DrawingColor:
                    byte alpha = 255;
                    if (c.IntParameters.Length > 3) { alpha = (byte)c.IntParameters[3]; }
                    drawings[c.StringParameters[0]].Color = new Microsoft.Xna.Framework.Color((byte)c.IntParameters[0], (byte)c.IntParameters[1], (byte)c.IntParameters[2], alpha);
                    RunningCommand = false;
                    break;

                //Fade drawing.
                case CommandType.FadeDrawing:
                    drawings[c.StringParameters[0]].Fade((uint)c.GetNumber(0));
                    RunningCommand = false;
                    break;

                //Unfade drawing.
                case CommandType.UnfadeDrawing:
                    drawings[c.StringParameters[0]].Unfade((uint)c.GetNumber(0));
                    RunningCommand = false;
                    break;

                //Show drawing.
                case CommandType.ShowDrawing:
                    drawings[c.StringParameters[0]].Visible = true;
                    RunningCommand = false;
                    break;

                //Hide drawing.
                case CommandType.HideDrawing:
                    drawings[c.StringParameters[0]].Visible = false;
                    RunningCommand = false;
                    break;

                //Maximize drawing.
                case CommandType.MaximizeDrawing:
                    float origWidth = drawings[c.StringParameters[0]].Drawing.Width;
                    float origHeight = drawings[c.StringParameters[0]].Drawing.Height;
                    drawings[c.StringParameters[0]].Drawing.Width = GameHelper.Camera.Width;
                    drawings[c.StringParameters[0]].Drawing.Height = GameHelper.Camera.Height;
                    drawings[c.StringParameters[0]].Drawing.X = drawings[c.StringParameters[0]].Drawing.Y = 0;
                    RunningCommand = false;
                    break;

                //Change the sprite group.
                case CommandType.ChangeDrawingSpriteGroup:
                    //if (drawings[c.StringParameters[0]] as Sprite != null) {
                    //    (drawings[c.StringParameters[0]] as Sprite).CurrentGroupIndex = c.StringParameters[1];
                    //}
                    RunningCommand = false;
                    break;

                //Unload drawing.
                case CommandType.UnloadDrawing:
                    drawings[c.StringParameters[0]].Dispose();
                    drawings.Remove(c.StringParameters[0]);
                    RunningCommand = false;
                    break;

                //Wait for button.
                case CommandType.WaitForButton:
                    break;

                //Wait for any button.
                case CommandType.WaitForAnyButton:
                    break;

                //Button down.
                case CommandType.ButtonDown:
                    GameHelper.Input.GameButtons[c.StringParameters[0]].SetButtonDown();
                    RunningCommand = false;
                    break;

                //Button up.
                case CommandType.ButtonUp:
                    GameHelper.Input.GameButtons[c.StringParameters[0]].SetButtonUp();
                    RunningCommand = false;
                    break;

                //Push button.
                case CommandType.PushButton:
                    GameHelper.Input.GameButtons[c.StringParameters[0]].PushButton();
                    RunningCommand = false;
                    break;

                //Block input.
                case CommandType.BlockInput:
                    GameHelper.Input.GameButtons[c.StringParameters[0]].InputBlocked = true;
                    RunningCommand = false;
                    break;

                //Allow input.
                case CommandType.AllowInput:
                    GameHelper.Input.GameButtons[c.StringParameters[0]].InputBlocked = false;
                    RunningCommand = false;
                    break;

                //Block all inputs.
                case CommandType.BlockAllInputs:
                    foreach (var b in GameHelper.Input.GameButtons) {
                        b.Value.InputBlocked = true;
                    }
                    RunningCommand = false;
                    break;

                //Allow all inputs.
                case CommandType.AllowAllInputs:
                    foreach (var b in GameHelper.Input.GameButtons) {
                        b.Value.InputBlocked = false;
                    }
                    RunningCommand = false;
                    break;

                //Signal an object.
                case CommandType.Signal:
                    //foreach (var o in GameHelper.CurrentScene.Objects.Where(x => x.CutsceneObjectGroup.Equals(c.StringParameters[0]))) {
                    //    o.OnCutsceneCall(c);
                    //}
                    RunningCommand = false;
                    break;

                //Exit the game.
                case CommandType.ExitGame:
                    GameHelper.CurrentGame.Exit();
                    RunningCommand = false;
                    break;

                //Change the scene.
                case CommandType.ChangeScene:
                    GameHelper.CurrentGame.ChangeScene(c.StringParameters[0], c.BooleanParameters.Count() > 0 ? c.BooleanParameters[0] : false);
                    RunningCommand = false;
                    break;

                //Change size.
                case CommandType.ChangeSize:
                    GameHelper.CurrentGame.ChangeSize((int)c.GetNumber(0), (int)c.GetNumber(1));
                    RunningCommand = false;
                    break;

                //Change resolution.
                case CommandType.ChangeResolution:
                    bool changeSize = true;
                    if (c.BooleanParameters.Length > 0) { changeSize = c.BooleanParameters[0]; }
                    GameHelper.CurrentGame.ChangeResolution((int)c.GetNumber(0), (int)c.GetNumber(1), changeSize);
                    RunningCommand = false;
                    break;

                //Fullscreen.
                case CommandType.Fullscreen:
                    bool fullscreen = !GameHelper.CurrentGame.Fullscreen;
                    if (c.BooleanParameters.Length > 0) { fullscreen = c.BooleanParameters[0]; }
                    if (GameHelper.CurrentGame.Fullscreen != fullscreen) {
                        GameHelper.CurrentGame.ToggleFullscreen();
                    }
                    RunningCommand = false;
                    break;

                //Force resize resolution.
                case CommandType.ForceResizeResolution:
                    if (c.BooleanParameters.Length > 0) { GameHelper.CurrentGame.ForceResolutionOnResize = c.BooleanParameters[0]; } else { GameHelper.CurrentGame.ForceResolutionOnResize = !GameHelper.CurrentGame.ForceResolutionOnResize; }
                    RunningCommand = false;
                    break;

                //Set window title.
                case CommandType.WindowTitle:
                    GameHelper.CurrentGame.Window.Title = c.StringParameters[0];
                    RunningCommand = false;
                    break;

                //Fin.
                case CommandType.Fin:
                    EndCutscene();
                    break;

            }

        }

        /// <summary>
        /// Draw stuff.
        /// </summary>
        public void Draw() {

            //Draw images.
            foreach (var d in drawings.Where(x => x.Value.Visible)) {
                d.Value.Draw();
            }

        }

        /// <summary>
        /// Update the interpreter.
        /// </summary>
        public void Update() {

            //Update drawings.
            foreach (var d in drawings) {
                d.Value.Update();
            }

            //Make sure the cutscene is running.
            if (!RunningCutscene) { return; }

            //If a command is running, update it and return.
            if (RunningCommand) {
                UpdateCommand();
                return;
            }

            //End cutscene if the command index is out of bounds.
            if (CurrentCommandIndex > Commands.Count - 1) {
                EndCutscene();
                return;
            }

            //Execute the next command.
            RunningCommand = true;
            ExecuteCommand();

        }

        /// <summary>
        /// Update the current command.
        /// </summary>
        private void UpdateCommand() {

            //Switch the command type.
            switch (CurrentCommand.CommandType) {

                //Custom.
                case CommandType.Custom:
                    CustomCommandUpdater?.Invoke(CurrentCommand.CustomCommandType, CurrentCommand);
                    if (CustomCommandUpdater == null) { RunningCommand = false; }
                    break;

                //Wait.
                case CommandType.Wait:
                    waitTimer.Update();
                    if (waitTimer.TimePast() != 0xFFFFFFFF) {
                        RunningCommand = false;
                    }
                    break;

                //Wait for a button.
                case CommandType.WaitForButton:
                    if (GameHelper.Input.GameButtons[CurrentCommand.StringParameters[0]].Down) {
                        RunningCommand = false;
                    }
                    break;

                //Wait for any button.
                case CommandType.WaitForAnyButton:
                    foreach (var b in GameHelper.Input.GameButtons) {
                        if (b.Value.Down) {
                            RunningCommand = false;
                            return;
                        }
                    }
                    break;

            }

        }

        /// <summary>
        /// End the cutscene.
        /// </summary>
        public void EndCutscene() {

            //Cutscene is not running.
            RunningCutscene = false;

            //Command is not running.
            RunningCommand = false;

            //Remove non-persistent drawings.
            for (int i = drawings.Count - 1; i >= 0; i--) {
                if (!drawings.Values.ElementAt(i).Persistent) {
                    drawings.Values.ElementAt(i).Dispose();
                    drawings.Remove(drawings.Keys.ElementAt(i));
                }
            }

        }

    }

}
