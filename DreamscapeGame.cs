using Dreamscape.Audio;
using Dreamscape.Boot;
using Dreamscape.Cutscene;
using Dreamscape.Input;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// Dreamscape game.
    /// </summary>
    public abstract class DreamscapeGame : Game {

        /// <summary>
        /// Game assembly to get embedded resources from.
        /// </summary>
        public abstract Assembly GameAssembly();

        /// <summary>
        /// Enable this if you don't want to give my engine some advertising so that future devs are not inspired to consider using this to make their games.
        /// </summary>
        public bool DisableBootscreen;

        /// <summary>
        /// Have the boot screen run, even in debug mode.
        /// </summary>
        public bool ForceBootscreen;

        /// <summary>
        /// Pause the audio when window is not active.
        /// </summary>
        public bool PauseAudioWhenWindowIsNotActive = true;

        /// <summary>
        /// Whether or not the camera is in fullscreen.
        /// </summary>
        public bool Fullscreen { get; private set; } = false;

        /// <summary>
        /// Force the same screen resolution on resize.
        /// </summary>
        public bool ForceResolutionOnResize = false;

        /// <summary>
        /// Update resolution on resize.
        /// </summary>
        public bool UpdateResolutionOnResize = false;

        /// <summary>
        /// Width of the window.
        /// </summary>
        public int Width {
            get => GraphicsDeviceManager.PreferredBackBufferWidth;
            private set { GraphicsDeviceManager.PreferredBackBufferWidth = value; if (UpdateResolutionOnResize) { Resolution.X = value; } UpdateScale(); }
        }

        /// <summary>
        /// Height of the window.
        /// </summary>
        public int Height {
            get => GraphicsDeviceManager.PreferredBackBufferHeight;
            private set { GraphicsDeviceManager.PreferredBackBufferHeight = value; if (UpdateResolutionOnResize) { Resolution.Y = value; } UpdateScale(); }
        }

        /// <summary>
        /// Internal resolution.
        /// </summary>
        private Vector2 Resolution = new Vector2(1024, 576);

        /// <summary>
        /// Resolution X.
        /// </summary>
        public int ResolutionX => (int)Resolution.X;

        /// <summary>
        /// Resolution Y.
        /// </summary>
        public int ResolutionY => (int)Resolution.Y;

        /// <summary>
        /// Screen scale.
        /// </summary>
        private Vector2 ScreenScale = Vector2.One;

        /// <summary>
        /// Screen scale X.
        /// </summary>
        public float ScreenScaleX => ScreenScale.X;

        /// <summary>
        /// Screen scale Y.
        /// </summary>
        public float ScreenScaleY => ScreenScale.Y;

        /// <summary>
        /// Offset to draw the screen at.
        /// </summary>
        private Vector2 ScreenOffset = Vector2.Zero;

        /// <summary>
        /// Screen offset X.
        /// </summary>
        public float ScreenOffsetX => ScreenOffset.X;

        /// <summary>
        /// Screen offset Y.
        /// </summary>
        public float ScreenOffsetY => ScreenOffset.Y;

        /// <summary>
        /// Graphics device manager.
        /// </summary>
        public GraphicsDeviceManager GraphicsDeviceManager;

        /// <summary>
        /// Graphics device.
        /// </summary>
        public GraphicsDevice Graphics => GraphicsDeviceManager.GraphicsDevice;

        /// <summary>
        /// Spritebatch.
        /// </summary>
        public SpriteBatch SpriteBatch;

        /// <summary>
        /// Drawbatch.
        /// </summary>
        public DrawBatch DrawBatch;

        /// <summary>
        /// Renderbatch.
        /// </summary>
        public RenderBatch RenderBatch = new RenderBatch();

        /// <summary>
        /// Scenes.
        /// </summary>
        public Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>() { { "Default", new DefaultScene() } };

        /// <summary>
        /// Current scene.
        /// </summary>
        public Scene CurrentScene { get; private set; }

        /// <summary>
        /// Clear color.
        /// </summary>
        public Color ClearColor = Color.CornflowerBlue;

        /// <summary>
        /// Game time.
        /// </summary>
        public GameTime GameTime;

        /// <summary>
        /// Camera for the game
        /// </summary>
        public Camera Camera = new Camera();

        /// <summary>
        /// Input.
        /// </summary>
        public InputManager Input = new InputManager();

        /// <summary>
        /// Flags.
        /// </summary>
        private Dictionary<string, bool> Flags = new Dictionary<string, bool>();

        /// <summary>
        /// If a flag is set.
        /// </summary>
        /// <param name="name">Flag name.</param>
        /// <returns>If that flag is set.</returns>
        public bool FlagEnabled(string name) => Flags.ContainsKey(name) ? Flags[name] : false;

        /// <summary>
        /// Set a flag.
        /// </summary>
        /// <param name="name">Name of the flag to set.</param>
        public void SetFlag(string name) { if (Flags.ContainsKey(name)) { Flags[name] = true; } else { Flags.Add(name, true); } }

        /// <summary>
        /// Clear a flag.
        /// </summary>
        /// <param name="name">Name of the flag to clear.</param>
        public void ClearFlag(string name) { if (Flags.ContainsKey(name)) { Flags.Remove(name); } }

        /// <summary>
        /// Flip the value of a flag.
        /// </summary>
        /// <param name="name">The flag name.</param>
        public void FlipFlag(string name) { if (FlagEnabled(name)) { ClearFlag(name); } else { SetFlag(name); } }

        /// <summary>
        /// Audio.
        /// </summary>
        public SoundPlayer Audio = new SoundPlayer();

        /// <summary>
        /// Cutscene.
        /// </summary>
        public CutsceneManager Cutscene;

        /// <summary>
        /// Debug mode.
        /// </summary>
        public bool DebugMode;

        /// <summary>
        /// Load a scene.
        /// </summary>
        /// <param name="name">Scene name.</param>
        /// <param name="skipInitialization">If initialization should be skipped.</param>
        public void ChangeScene(string name, bool skipInitialization = false) {
            CurrentScene = Scenes[name];
            if (!skipInitialization) {
                CurrentScene.Initialize();
                if (CurrentScene as DefaultScene != null) {
                    DefaultSceneInitialize();
                }
            }
        }

        /// <summary>
        /// Create a new Dreamscape game.
        /// </summary>
        /// <param name="title">Title of the game.</param>
        /// <param name="initialWidth">Initial width of the game.</param>
        /// <param name="initialHeight">Initial height of the game.</param>
        /// <param name="forceResolutionOnResize">If the resoltution should be forced on window resize.</param>
        /// <param name="clearColor">Clear color.</param>
        /// <param name="debugMode">Debug mode.</param>
        public DreamscapeGame(string title, int initialWidth, int initialHeight, bool forceResolutionOnResize, Color clearColor, bool debugMode) {

            //Set game.
            GameHelper.CurrentGame = this;

            //Add this to the game helper list.
            GameHelper.Games.Add(this);

            //Default buttons.
            Input.GameButtons.Add("FULLSCREEN", new GameButton(PlayerIndex.One, Keys.F4));
            if (debugMode) { Input.GameButtons.Add("DEBUG_EXIT", new GameButton(PlayerIndex.One, Keys.Escape, GamePadButton.Select)); }

            //Load default scene.
            ChangeScene("Default", true);

            //Init stuff.
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.PreferMultiSampling = true;
            IsFixedTimeStep = false;
            //GraphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            Window.Title = title;
            ChangeResolution(initialWidth, initialHeight);
            ForceResolutionOnResize = forceResolutionOnResize;
            ClearColor = clearColor;
            DebugMode = debugMode;
            Cutscene = new CutsceneManager();

            //Boot scene.
            if ((!DebugMode && !DisableBootscreen) || ForceBootscreen) {
                BootScene boot = new BootScene();
                Scenes.Add("Boot", boot);
                ChangeScene("Boot");
            } else {
                ChangeScene("Default");
            }

        }

        /// <summary>
        /// Enable user resizing. This doesn't seem to like unusual scales...
        /// </summary>
        public void EnableUserResizing() {
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(OnUserResize);
        }

        /// <summary>
        /// Disable user resizing. TODO!!!
        /// </summary>
        public void DisableUserResizing() {
            Window.AllowUserResizing = false;
        }

        /// <summary>
        /// Screen is resized.
        /// </summary>
        public void OnUserResize(object sender, EventArgs e) {
            if (!UpdateResolutionOnResize) {
                ChangeSize(Window.ClientBounds.Width, Window.ClientBounds.Height);
            } else {
                ChangeResolution(Window.ClientBounds.Width, Window.ClientBounds.Height);
            }
        }

        /// <summary>
        /// Toggle fullscreen.
        /// </summary>
        public void ToggleFullscreen() {

            //Graphics.
            var graphics = GameHelper.GraphicsDeviceManager;

            //Disable fullscreen if already in fullscreen mode.
            if (Fullscreen) {

                //Disable fullscreen, set size to target resolution, and apply changes.
                graphics.IsFullScreen = false;
                Fullscreen = false;
                Width = (int)Resolution.X;
                Height = (int)Resolution.Y;
                graphics.ApplyChanges();

            //Enable fullscreen if not already in fullscreen mode.
            } else {

                //Set fullscreen, set the resolution to the screen, and apply changes.
                Fullscreen = true;
                graphics.IsFullScreen = true;
                Width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.ApplyChanges();

            }

            //Change size.
            if (UpdateResolutionOnResize) {
                Resolution.X = Width = graphics.PreferredBackBufferWidth;
                Resolution.Y = Height = graphics.PreferredBackBufferHeight;
                ScreenScale.X = 1;
                ScreenScale.Y = 1;
            }

        }

        /// <summary>
        /// Change the size of the camera. Use this to set up the screen resolution.
        /// </summary>
        /// <param name="width">Width of the new resolution.</param>
        /// <param name="height">Height of the new resolution.</param>
        /// <param name="changeSize">If the size should also change.</param>
        public void ChangeResolution(int width, int height, bool changeSize = true) {

            //Sizes different.
            bool sizesDifferent = false;
            if (changeSize) {
                sizesDifferent = width != Width || height != Height;
            }

            //Same resolution.
            if (width == Resolution.X && height == Resolution.Y && !sizesDifferent) {
                return;
            }

            //Fullscreen.
            bool wasFullscreen = false;
            if (Fullscreen) {
                ToggleFullscreen();
                wasFullscreen = true;
            }

            //Set the resolution and apply.
            var graphics = GraphicsDeviceManager;
            if (changeSize) {
                Width = width;
                Height = height;
            }
            Resolution.X = Camera.Width = width;
            Resolution.Y = Camera.Height = height;
            UpdateScale();
            graphics.ApplyChanges();
            if (wasFullscreen) { ToggleFullscreen(); }

        }

        /// <summary>
        /// Change the screen size.
        /// </summary>
        /// <param name="width">New width.</param>
        /// <param name="height">New height.</param>
        public void ChangeSize(int width, int height) {

            //Fullscreen.
            bool wasFullscreen = false;
            if (Fullscreen) {
                ToggleFullscreen();
                wasFullscreen = true;
            }

            //Set the resolution and apply.
            Width = width;
            Height = height;
            GraphicsDeviceManager.ApplyChanges();
            if (wasFullscreen) { ToggleFullscreen(); }

        }

        /// <summary>
        /// Update the scale of which the draw the screen when the resolution is changed.
        /// </summary>
        /// <param name="graphics">Graphics device manager.</param>
        private void UpdateScale() {

            //Graphics.
            var graphics = GraphicsDeviceManager;

            //Don't force resolution.
            if (!ForceResolutionOnResize) {

                //Divide the current resolution by the target resolution and create the scale.
                ScreenScale.X = Width / Resolution.X;
                ScreenScale.Y = Height / Resolution.Y;

                //No black bars.
                ScreenOffset = Vector2.Zero;

            }

            //Force resolution.
            else {

                //Divide the current resolution by the target resolution and create the scale.
                ScreenScale.X = Width / Resolution.X;
                ScreenScale.Y = Height / Resolution.Y;

                //X scale is the least.
                if (ScreenScale.X < ScreenScale.Y) {

                    //Set Y scale.
                    ScreenScale.Y = ScreenScale.X;

                    //Set black bar offsets.
                    ScreenOffset.X = 0;
                    ScreenOffset.Y = (Height - Resolution.Y * ScreenScale.Y) / 2;

                }

                //Y scale is the least.
                else {

                    //Set X scale.
                    ScreenScale.X = ScreenScale.Y;

                    //Set black bar offsets.
                    ScreenOffset.Y = 0;
                    ScreenOffset.X = (Width - Resolution.X * ScreenScale.X) / 2;

                }

            }

        }

        /// <summary>
        /// Load content.
        /// </summary>
        protected override void LoadContent() {

            //Set game.
            GameHelper.CurrentGame = this;

            //New spritebatch.
            SpriteBatch = new SpriteBatch(Graphics);

            //New drawbatch.
            DrawBatch = new DrawBatch(Graphics);

            //Load content for Dreamscape.
            LoadGameContent();

            //Load base content.
            base.LoadContent();

        }

        /// <summary>
        /// Load content for Dreamscape.
        /// </summary>
        public virtual void LoadGameContent() {}

        /// <summary>
        /// Unload content.
        /// </summary>
        protected override void UnloadContent() {

            //Set game.
            GameHelper.CurrentGame = this;

            //Unload content.
            base.UnloadContent();

            //Kill audio.
            Audio.Dispose();

            //Remove from game list.
            GameHelper.Games.Remove(this);

        }

        /// <summary>
        /// Draw the game.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void Draw(GameTime gameTime) {

            //Set game.
            GameHelper.CurrentGame = this;

            //Make sure game is active.
            if (!IsActive) {
                return;
            }

            //Set game time.
            GameTime = gameTime;

            //Start the draw.
            GameHelper.StartDraw();

            //Draw cutscene stuff.
            Cutscene.Draw();

            //Default scene.
            if ((CurrentScene as DefaultScene) != null) {
                DefaultSceneDraw();
            }

            //Normal scene.
            else {
                CurrentScene.Draw();
            }

            //End the draw.
            GameHelper.EndDraw();

            //Draw base.
            base.Draw(gameTime);

        }

        /// <summary>
        /// Initialize the default scene.
        /// </summary>
        public virtual void DefaultSceneInitialize() {}

        /// <summary>
        /// Draw the default scene.
        /// </summary>
        public virtual void DefaultSceneDraw() {}

        /// <summary>
        /// Update the default scene.
        /// </summary>
        public virtual void DefaultSceneUpdate() {}

        /// <summary>
        /// Update the game.
        /// </summary>
        /// <param name="gameTime">Game time.</param>
        protected override void Update(GameTime gameTime) {

            //Pause audio if away.
            if (!Audio.PausedFromWindowAway && !IsActive && PauseAudioWhenWindowIsNotActive) {
                Audio.PauseActiveSoundsOnWindowAway();
                Audio.PausedFromWindowAway = true;
            }

            //Make sure game is active.
            if (!IsActive) {
                return;
            }

            //Resume audio.
            if (Audio.PausedFromWindowAway) {
                Audio.ResumeActiveSoundsOnWindowReturn();
                Audio.PausedFromWindowAway = false;
            }

            //Set game.
            GameHelper.CurrentGame = this;

            //Set game time.
            GameTime = gameTime;

            //Update input.
            Input.Update();

            //Update cutscene.
            Cutscene.Update();

            //Debug exit.
            if (DebugMode) {
                if (Input.ButtonDown("DEBUG_EXIT")) {
                    Exit();
                }
            }

            //Update default scene.
            if ((CurrentScene as DefaultScene) != null) {
                DefaultSceneUpdate();
            }

            //Update other scene.
            else {
                CurrentScene.Update();
            }

            //Update base.
            base.Update(gameTime);

        }

    }

}
