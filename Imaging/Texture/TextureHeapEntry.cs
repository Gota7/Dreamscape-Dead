using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Imaging {

    /// <summary>
    /// An entry for the texture heap.
    /// </summary>
    public class TextureHeapEntry {

        /// <summary>
        /// Actual texture data.
        /// </summary>
        public Texture2D TextureData;

        /// <summary>
        /// Textures that use this texture.
        /// </summary>
        public List<Texture> Users = new List<Texture>();

        /// <summary>
        /// Createe a new texture heap entry.
        /// </summary>
        /// <param name="textureData">Actual texture to use.</param>
        /// <param name="user">User to add to list.</param>
        public TextureHeapEntry(Texture2D textureData, Texture user) {
            TextureData = textureData;
            Users.Add(user);
        }

    }

}
