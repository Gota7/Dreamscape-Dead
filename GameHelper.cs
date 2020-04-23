using Dreamscape.Audio;
using Dreamscape.Cutscene;
using Dreamscape.Input;
using LilyPath;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// Game helper. This thing will be your friend for getting any game-wide properties.
    /// </summary>
    public static class GameHelper {

        /// <summary>
        /// Dreamscape games.
        /// </summary>
        public static List<DreamscapeGame> Games = new List<DreamscapeGame>();

        /// <summary>
        /// Current game.
        /// </summary>
        public static DreamscapeGame CurrentGame;

        /// <summary>
        /// Game assembly.
        /// </summary>
        public static Assembly GameAssemby => CurrentGame.GameAssembly();

        /// <summary>
        /// Graphics device manager.
        /// </summary>
        public static GraphicsDeviceManager GraphicsDeviceManager => CurrentGame.GraphicsDeviceManager;

        /// <summary>
        /// Graphics.
        /// </summary>
        public static GraphicsDevice Graphics => CurrentGame.Graphics;

        /// <summary>
        /// Game time.
        /// </summary>
        public static GameTime GameTime => CurrentGame.GameTime;

        /// <summary>
        /// Window.
        /// </summary>
        public static GameWindow Window => CurrentGame.Window;

        /// <summary>
        /// Clear color.
        /// </summary>
        public static Color ClearColor => CurrentGame.ClearColor;

        /// <summary>
        /// Camera.
        /// </summary>
        public static Camera Camera => CurrentGame.Camera;

        /// <summary>
        /// Sprite batch.
        /// </summary>
        public static SpriteBatch SpriteBatch => CurrentGame.SpriteBatch;

        /// <summary>
        /// Current scene.
        /// </summary>
        public static Scene CurrentScene => CurrentGame.CurrentScene;

        /// <summary>
        /// Draw batch.
        /// </summary>
        public static DrawBatch DrawBatch => CurrentGame.DrawBatch;

        /// <summary>
        /// Render batch.
        /// </summary>
        public static RenderBatch RenderBatch => CurrentGame.RenderBatch;

        /// <summary>
        /// Get input.
        /// </summary>
        public static InputManager Input => CurrentGame.Input;

        /// <summary>
        /// Get cutscene.
        /// </summary>
        public static CutsceneManager Cutscene => CurrentGame.Cutscene;

        /// <summary>
        /// Get audio.
        /// </summary>
        public static SoundPlayer Audio => CurrentGame.Audio;

        /// <summary>
        /// If a flag is set.
        /// </summary>
        /// <param name="name">Flag name.</param>
        /// <returns>If that flag is set.</returns>
        public static bool FlagEnabled(string name) => CurrentGame.FlagEnabled(name);

        /// <summary>
        /// Set a flag.
        /// </summary>
        /// <param name="name">Name of the flag to set.</param>
        public static void SetFlag(string name) => CurrentGame.SetFlag(name);

        /// <summary>
        /// Clear a flag.
        /// </summary>
        /// <param name="name">Name of the flag to clear.</param>
        public static void ClearFlag(string name) => CurrentGame.ClearFlag(name);

        /// <summary>
        /// Flip the value of a flag.
        /// </summary>
        /// <param name="name">The flag name.</param>
        public static void FlipFlag(string name) => CurrentGame.FlipFlag(name);

        /// <summary>
        /// Debug mode.
        /// </summary>
        public static bool DebugMode => CurrentGame.DebugMode;

        /// <summary>
        /// Black texture.
        /// </summary>
        public static Imaging.Texture Black;

        /// <summary>
        /// Start drawing.
        /// </summary>
        public static void StartDraw() {

            //Render stuff.
            RenderBatch.Render();

            //Clear the screen.
            Graphics.Clear(ClearColor);

            //Start the draw.
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, transformMatrix: Matrix.CreateScale(CurrentGame.ScreenScaleX, CurrentGame.ScreenScaleY, 1) * Matrix.CreateTranslation(CurrentGame.ScreenOffsetX, CurrentGame.ScreenOffsetY, 0));
            DrawBatch.Begin(DrawSortMode.Deferred, BlendState.NonPremultiplied);

        }

        /// <summary>
        /// End drawing.
        /// </summary>
        public static void EndDraw() {

            //End the draw.
            DrawBatch.End();
            SpriteBatch.End();

            //Load texture.
            if (Black == null) {
                Black = new Imaging.Texture(Color.Black);
            }

            //Draw the black bars.
            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            SpriteBatch.Draw(Black.TextureData, new Vector2(0, 0), scale: new Vector2(CurrentGame.ScreenOffsetX, CurrentGame.Height));
            SpriteBatch.Draw(Black.TextureData, new Vector2(0, 0), scale: new Vector2(CurrentGame.Width, CurrentGame.ScreenOffsetY));
            SpriteBatch.Draw(Black.TextureData, new Vector2(CurrentGame.Width - CurrentGame.ScreenOffsetX, 0), scale: new Vector2(CurrentGame.ScreenOffsetX, CurrentGame.Height));
            SpriteBatch.Draw(Black.TextureData, new Vector2(0, CurrentGame.Height - CurrentGame.ScreenOffsetY), scale: new Vector2(CurrentGame.Width, CurrentGame.ScreenOffsetY));
            SpriteBatch.End();

        }

    }

}
