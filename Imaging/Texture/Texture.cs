using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Imaging {

    /// <summary>
    /// A texture for an object to use.
    /// </summary>
    public class Texture : IDisposable {

        /// <summary>
        /// Md5Sum of the texture data.
        /// </summary>
        public string Md5Sum { get; private set; }

        /// <summary>
        /// Path to the PNG file.
        /// </summary>
        public string TexturePath { get; private set; }

        /// <summary>
        /// Actual texture data.
        /// </summary>
        public Texture2D TextureData => TextureHeap.GetTexture(Md5Sum);

        /// <summary>
        /// Disposed.
        /// </summary>
        private bool Disposed = false;

        /// <summary>
        /// Create a new texture.
        /// </summary>
        /// <param name="texturePath">Path to the texture.</param>
        public Texture(string texturePath) {

            //Set the texture path, and get the MD5SUM.
            Md5Sum = CalculateMD5(texturePath);
            TexturePath = texturePath;

            //Add the texture to the heap.
            TextureHeap.Add(this, Color.White);

        }

        /// <summary>
        /// Create a new texture from a color.
        /// </summary>
        /// <param name="color">Color to make the texture.</param>
        public Texture(Color color) {

            //Set the MD5 sum.
            MemoryStream colorTemp = new MemoryStream(new byte[] { color.A, color.R, color.G, color.B });
            Md5Sum = CalculateMD5(colorTemp);
            colorTemp.Dispose();

            //Add the texture to the heap.
            TextureHeap.Add(this, color);

        }

        /// <summary>
        /// Create a new texture from a texture 2d.
        /// </summary>
        /// <param name="tex">Texture 2D.</param>
        public Texture(Texture2D tex) {

            //Set the texture path, and get the MD5SUM.
            MemoryStream stream = new MemoryStream();
            tex.SaveAsPng(stream, tex.Width, tex.Height);
            stream.Position = 0;
            Md5Sum = CalculateMD5(stream);
            stream.Dispose();

            //Add the texture to the heap.
            TextureHeap.Add(this, tex);

        }

        /// <summary>
        /// Deconstructor.
        /// </summary>
        ~Texture() {
            Dispose();
        }

        /// <summary>
        /// Change the current texture to another one.
        /// </summary>
        /// <param name="texturePath">Path of the texture.</param>
        public void ChangeTexture(string texturePath) {

            //Remove texture data if needed.
            TextureHeap.Remove(this);

            //Set the texture path, and get the MD5SUM.
            Md5Sum = CalculateMD5(texturePath);
            TexturePath = texturePath;

            //Add this texture to the heap.
            TextureHeap.Add(this, Color.White);

        }

        /// <summary>
        /// Change the current texture to another one.
        /// </summary>
        /// <param name="color">Color to change to.</param>
        public void ChangeTexture(Color color) {

            //Remove texture data if needed.
            TextureHeap.Remove(this);

            //Get the Md5Sum, reset path.
            MemoryStream colorTemp = new MemoryStream(new byte[] { color.A, color.R, color.G, color.B });
            Md5Sum = CalculateMD5(colorTemp);
            TexturePath = null;
            colorTemp.Dispose();

            //Add this texture to the heap.
            TextureHeap.Add(this, color);

        }

        /// <summary>
        /// Change the texture data.
        /// </summary>
        /// <param name="tex">Texture data.</param>
        public void ChangeTexture(Texture2D tex) {

            //Remove texture data if needed.
            TextureHeap.Remove(this);

            //Get the Md5Sum, reset path.
            MemoryStream stream = new MemoryStream();
            tex.SaveAsPng(stream, tex.Width, tex.Height);
            stream.Position = 0;
            Md5Sum = CalculateMD5(stream);
            stream.Dispose();
            TexturePath = null;

            //Add this texture to the heap.
            TextureHeap.Add(this, tex);

        }

        /// <summary>
        /// Calculate MD5 hash of a file.
        /// </summary>
        /// <param name="filename">Name of the file.</param>
        /// <returns>The hash</returns>
        public static string CalculateMD5(string filename) {
            using (var stream = FileSystem.OpenFileStream(filename)) {
                return CalculateMD5(stream);
            }
        }

        /// <summary>
        /// Calculate MD5 of a stream.
        /// </summary>
        /// <param name="stream">Stream to calculate the hash from.</param>
        /// <returns>The MD5 string of the stream.</returns>
        public static string CalculateMD5(Stream stream) {
            using (var md5 = MD5.Create()) {
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose() {

            //Remove resources.
            if (!Disposed) {

                //Remove the texture from the heap.
                TextureHeap.Remove(this);

                //Disposed.
                Disposed = true;

            }

        }

    }

}
