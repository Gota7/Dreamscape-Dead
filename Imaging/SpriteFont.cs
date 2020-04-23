using Dreamscape.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFNT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Imaging {

    /// <summary>
    /// A sprite font.
    /// </summary>
    public class SpriteFont {

        /// <summary>
        /// Pages.
        /// </summary>
        private Dictionary<int, Texture> Pages;

        /// <summary>
        /// Bounds for the characters.
        /// </summary>
        private Dictionary<int, Rectangle> Bounds;

        /// <summary>
        /// Font data.
        /// </summary>
        public BitmapFont Font;

        /// <summary>
        /// Render target.
        /// </summary>
        private RenderTarget2D RenderTarget;

        /// <summary>
        /// Get the texture for a character.
        /// </summary>
        /// <param name="c">The character.</param>
        /// <returns>Texture for the character.</returns>
        private Texture CharTexture(char c) => Pages[Font.GetCharacter(c).Page];

        /// <summary>
        /// Create a sprite font.
        /// </summary>
        /// <param name="filePath">File path.</param>
        public SpriteFont(string filePath) {

            //Load the font.
            Font = BitmapFont.FromStream(FileSystem.OpenFileStream(filePath), false);

            //Load textures.
            Pages = new Dictionary<int, Texture>();
            string dirPath = Path.GetDirectoryName(filePath) + "/";
            if (dirPath.Equals("/")) { dirPath = ""; }
            foreach (var p in Font.Pages) {
                Pages.Add(p.Key, new Texture(dirPath + p.Value));
            }

            //For each character.
            Bounds = new Dictionary<int, Rectangle>();
            foreach (var c in Font.Characters) {
                Bounds.Add(c.Key, new Rectangle(c.Value.X, c.Value.Y, c.Value.Width, c.Value.Height));
            }

            //Render target.
            RenderTarget = new RenderTarget2D(GameHelper.Graphics, 1, 1);

        }

        /// <summary>
        /// Draw a character.
        /// </summary>
        /// <param name="c">The character to draw.</param>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="color">Font color.</param>
        /// <param name="previousChar">Previous character.</param>
        /// <param name="fontSize">Font size.</param>
        private void DrawChar(char c, float x, float y, Color color, char? previousChar = null, float fontSize = 32) {

            //Get scale.
            float scale = GetScale(fontSize);

            //Get the kerning.
            int kerning = 0;
            if (previousChar != null) { kerning = Font.GetKerningAmount(previousChar.Value, c); }

            //Draw the character.
            GameHelper.RenderBatch.Draw(CharTexture(c).TextureData, new Vector2(x + (kerning + Font.GetCharacter(c).XOffset) * scale, y + Font.GetCharacter(c).YOffset * scale), null, Bounds[c], color: color, scale: new Vector2(scale, scale));

        }

        /// <summary>
        /// Draw a string.
        /// </summary>
        /// <param name="str">String to draw.</param>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="fontSize">Font size.</param>
        /// <param name="color">Color.</param>
        /// <param name="alignment">Alignment.</param>
        /// <param name="charColors">Character colors.</param>
        /// <param name="charOffsets">Character offsets.</param>
        /// <param name="ignoreCamera">Ignore camera.</param>
        /// <param name="origin">Origin.</param>
        /// <param name="rotation">Rotation.</param>
        /// <param name="scale">Scale.</param>
        /// <param name="spriteEffects">Sprite effects.</param>
        /// <param name="layerDepth">Layer depth.</param>
        public void DrawString(string str, float x, float y, float fontSize = 32, Color? color = null, Alignment alignment = Alignment.TopLeft, Color[] charColors = null, Vector2[] charOffsets = null, bool ignoreCamera = false, Vector2? origin = null, Angle rotation = null, Vector2? scale = null, SpriteEffects spriteEffects = SpriteEffects.None, float layerDepth = 0) {

            //Get scale.
            float scl = GetScale(fontSize);

            //Color.
            if (color == null) {
                color = Color.White;
            }

            //Get the line widths.
            List<float> lineWidths = new List<float>();
            int lineNumber = 0;
            lineWidths.Add(0);
            char? prevChar = null;
            for (int i = 0; i < str.Length; i++) {

                //Add x.
                if (str[i] != '\n') {
                    char previousCharacter = i == 0 ? ' ' : str[i - 1];
                    lineWidths[lineNumber] += (Font.GetCharacter(str[i]).XAdvance + (prevChar == null ? 0 : Font.GetKerningAmount(prevChar.Value, str[i])) * scl);
                }

                //New line.
                else {
                    lineWidths.Add(0);
                    lineNumber++;
                }

            }

            //Save variables.
            float oldX = x;
            float oldY = y;
            x = 0;
            y = 0;

            //Get max length.
            float maxWidth = lineWidths.Max();
            float height = lineWidths.Count * Font.Common.LineHeight * scl;

            //Start render batch.
            GameHelper.RenderBatch.Begin(maxWidth.RoundUp(), height.RoundUp(), SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            //Figure out starting X.
            /*if (Coordinates.HorizontalPositionType(alignment) == PositionType.Center) {
                x -= lineWidths[0] / 2;
            } else if (Coordinates.HorizontalPositionType(alignment) == PositionType.BottomOrRight) {
                x -= lineWidths[0];
            }

            //Figure out starting Y.
            if (Coordinates.VerticalPositionType(alignment) == PositionType.Center) {
                y -= height / 2;
            } else if (Coordinates.VerticalPositionType(alignment) == PositionType.BottomOrRight) {
                y -= height;
            }*/

            //Draw each character.
            int lineNum = 0;
            for (int i = 0; i < str.Length; i++) {

                //Draw the character.
                if (str[i] != '\n') {

                    //Draw character.
                    DrawChar(str[i], x + (charOffsets != null ? charOffsets[i].X : 0), y + (charOffsets != null ? charOffsets[i].Y : 0), charColors == null ? color.Value : charColors[i], prevChar, fontSize);

                    //Advance the position.
                    x += Font.GetCharacter(str[i]).XAdvance * scl;

                } else {

                    //Advance the Y position.
                    y += Font.Common.LineHeight * scl;

                    //Set X to the original X position.
                    x = 0;

                    //Increment the line number.
                    lineNum++;

                    //X difference.
                    float diff = maxWidth - lineWidths[lineNum];

                    //Center position.
                    /*if (Coordinates.HorizontalPositionType(alignment) == PositionType.Center) {
                        x -= lineWidths[lineNum] / 2;
                    }

                    //Right position.
                    else if (Coordinates.HorizontalPositionType(alignment) == PositionType.BottomOrRight) {
                        x -= lineWidths[lineNum];
                    }*/

                }

            }

            //Draw the texture.
            x = oldX;
            y = oldY;
            if (Coordinates.HorizontalPositionType(alignment) == PositionType.Center) {
                x -= maxWidth / 2;
            } else if (Coordinates.HorizontalPositionType(alignment) == PositionType.BottomOrRight) {
                x -= maxWidth;
            }
            if (Coordinates.VerticalPositionType(alignment) == PositionType.Center) {
                y -= height / 2;
            } else if (Coordinates.VerticalPositionType(alignment) == PositionType.BottomOrRight) {
                y -= height;
            }
            GameHelper.RenderBatch.End(x, y, ignoreCamera, color, origin, rotation, scale, spriteEffects, layerDepth);

        }

        /// <summary>
        /// Get the scale of the text.
        /// </summary>
        /// <param name="textSize">The text size.</param>
        /// <returns>How much to scale the text by.</returns>
        private float GetScale(float textSize) {
            return textSize / Font.Info.Size;
        }

    }

}
