using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Imaging {

    /// <summary>
    /// An image.
    /// </summary>
    public class Image : Drawing {

        /// <summary>
        /// Make a new image from the path of a texture.
        /// </summary>
        /// <param name="texturePath">Path of the texture.</param>
        /// <param name="x">X-Position.</param>
        /// <param name="y">Y-Position.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Image(string texturePath, float x = 0, float y = 0, float width = -1, float height = -1) {
            Texture = new Texture(texturePath);
            X = x;
            Y = y;
            if (width != -1) {
                Width = width;
            } else {
                Width = Texture.TextureData.Width;
            }
            if (height != -1) {
               Height = height;
            } else {
                Height = Texture.TextureData.Height;
            }
        }

        /// <summary>
        /// Make a new image from a color.
        /// </summary>
        /// <param name="texturePath">Color.</param>
        /// <param name="x">X-Position.</param>
        /// <param name="y">Y-Position.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Image(Color color, float x = 0, float y = 0, float width = -1, float height = -1) {
            Texture = new Texture(color);
            X = x;
            Y = y;
            if (width != -1) {
                Width = width;
            } else {
                Width = Texture.TextureData.Width;
            }
            if (height != -1) {
                Height = height;
            } else {
                Height = Texture.TextureData.Height;
            }
        }

        /// <summary>
        /// Make a new image from a texture.
        /// </summary>
        /// <param name="texture">Texture.</param>
        public Image(Texture texture) {
            this.Texture = texture;
            Width = texture.TextureData.Width;
            Height = texture.TextureData.Height;
        }

        /// <summary>
        /// Make a new image from a texture and position it.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="x">X-Position.</param>
        /// <param name="y">Y-Position.</param>
        public Image(Texture texture, float x, float y) {
            this.Texture = texture;
            X = x;
            Y = y;
            Width = texture.TextureData.Width;
            Height = texture.TextureData.Height;
        }

        /// <summary>
        /// Make a new image from a texture, position it, and change the width and height.
        /// </summary>
        /// <param name="texture">Texture.</param>
        /// <param name="x">X-Position.</param>
        /// <param name="y">Y-Position.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Image(Texture texture, float x, float y, float width, float height) {
            this.Texture = texture;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Change the texture. THIS DOES NOT REMOVE TEXTURES FROM THE HEAP! It's recommended to use Texture.ChangeTexture() (which removes from heap) if you are not using pre-set textures.
        /// </summary>
        /// <param name="newTexture">New texture.</param>
        /// <param name="adjustWidthAndHeight">Whether or not to auto-adjust the image width and height.</param>
        public void ChangeTexture(Texture newTexture, bool adjustWidthAndHeight = true) {
            this.Texture = newTexture;
            if (adjustWidthAndHeight) {
                Width = newTexture.TextureData.Width;
                Height = newTexture.TextureData.Height;
            }
        }

        /// <summary>
        /// Set the width and height to the texture's.
        /// </summary>
        public void SetWidthAndHeightToTexture() {
            Width = TextureData.Width;
            Height = TextureData.Height;
        }

    }

}
