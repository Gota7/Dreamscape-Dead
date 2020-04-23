using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Imaging
{

    /// <summary>
    /// Store textures.
    /// </summary>
    public static class TextureHeap {

        /// <summary>
        /// Cache of hidden images.
        /// </summary>
        private static Dictionary<string, TextureHeapEntry> TextureCache = new Dictionary<string, TextureHeapEntry>();

        /// <summary>
        /// Add a texture to the heap, if it doesn't exist.
        /// </summary>
        /// <param name="texture">Texture to add to the heap.</param>
        /// <param name="color">Color.</param>
        public static void Add(Texture texture, Color color) {

            //If the cache already has a texture for that hash.
            if (TextureCache.ContainsKey(texture.Md5Sum)) {

                //Add the reference.
                TextureCache[texture.Md5Sum].Users.Add(texture);

            }

            //Add the hash.
            else {

                //Texture is a color.
                if (texture.TexturePath == null) {

                    //New entry for the color.
                    Texture2D colorTex = new Texture2D(GameHelper.Graphics, 1, 1);
                    colorTex.SetData(new Color[] { color });

                    //Add color entry.
                    TextureCache.Add(texture.Md5Sum, new TextureHeapEntry(colorTex, texture));

                }

                //Texture exists.
                else {

                    //New entry for the hash.
                    Stream f = FileSystem.OpenFileStream(texture.TexturePath);
                    TextureCache.Add(texture.Md5Sum, new TextureHeapEntry(Texture2D.FromStream(GameHelper.Graphics, f), texture));
                    f.Dispose();

                }

            }

        }

        /// <summary>
        /// Add a texture to the heap, if it doesn't exist. This is to be used with Texture2D created textures.
        /// </summary>
        /// <param name="texture">Texture to add to the heap. MUST HAVE PROPER MD5SUM SET!</param>
        /// <param name="texture2D">Texture 2D to add.</param>
        public static void Add(Texture texture, Texture2D texture2D) {

            //If the cache already has a texture for that hash.
            if (TextureCache.ContainsKey(texture.Md5Sum)) {

                //Add the reference.
                TextureCache[texture.Md5Sum].Users.Add(texture);

            }

            //Add the hash.
            else {

                //Add to cache.
                TextureCache.Add(texture.Md5Sum, new TextureHeapEntry(texture2D, texture));

            }

        }

        /// <summary>
        /// Get the actual texture data for a texture.
        /// </summary>
        /// <param name="hash">Hash entry to get the data for.</param>
        /// <returns>Reference to the 2d texture.</returns>
        public static Texture2D GetTexture(string hash) {
            return TextureCache[hash].TextureData;
        }

        /// <summary>
        /// Remove a texture from the image cache, but only if no other textures use it.
        /// </summary>
        /// <param name="texture">Texture to use.</param>
        public static void Remove(Texture texture) {

            //Remove the texture reference from the hash.
            TextureCache[texture.Md5Sum].Users.Remove(texture);

            //If there are no more users of the texture.
            if (TextureCache[texture.Md5Sum].Users.Count <= 0) {

                //Dispose of the texture.
                TextureCache[texture.Md5Sum].TextureData.Dispose();

                //Remove the hash, since no textures are active with the hash.
                TextureCache.Remove(texture.Md5Sum);

            }

        }

    }

}
