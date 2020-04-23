using Dreamscape.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.UI {
    
    /// <summary>
    /// Coordinates.
    /// </summary>
    public class Coordinates : IOFile {

        /// <summary>
        /// X position.
        /// </summary>
        public float X;

        /// <summary>
        /// Y position.
        /// </summary>
        public float Y;

        /// <summary>
        /// Coordinates are relative to parent object.
        /// </summary>
        public bool RelativeX;

        /// <summary>
        /// Coordinates are relative to parent object.
        /// </summary>
        public bool RelativeY;

        /// <summary>
        /// Divide the area into N boxes.
        /// </summary>
        public byte DivisorX = 1;

        /// <summary>
        /// Start drawing from a multiple of a box specified earlier from the alignment specified.
        /// </summary>
        public byte BoxMultipleX;

        /// <summary>
        /// Divide the area into N boxes.
        /// </summary>
        public byte DivisorY = 1;

        /// <summary>
        /// Start drawing from a multiple of a box specified earlier from the alignment specified.
        /// </summary>
        public byte BoxMultipleY;

        /// <summary>
        /// Alignment of the object.
        /// </summary>
        public Alignment Alignment;

        /// <summary>
        /// The origin of where to draw the object.
        /// </summary>
        public Alignment Origin;

        /// <summary>
        /// Read the file.
        /// </summary>
        /// <param name="r">The reader.</param>
        public override void Read(FileReader r) {
            r.ReadUInt32();
            Alignment = (Alignment)r.ReadByte();
            Origin = (Alignment)r.ReadByte();
            X = r.ReadSingle();
            Y = r.ReadSingle(); 
        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="w">The writer.</param>
        public override void Write(FileWriter w) {
            
            //Alright, so this file is structured to fit in exactly 0x10 bytes so every bit is used.
            //char[4] DCRD Magic.
            //float X Offset.
            //float Y Offset.
            //0xAO. A - Alignment. O - Object alignment. The nibbles are scaled to bits. For alignment, 0b11 represents that the object is relative to the parent object and not the UI area.
            //Box multiples and divisors!

        }

        /// <summary>
        /// Get the horizontal position type for an alignment.
        /// </summary>
        /// <param name="a">Alignment.</param>
        /// <returns>The position type.</returns>
        public static PositionType HorizontalPositionType(Alignment a) {
            return (PositionType)((byte)a & 0x0F);
        }

        /// <summary>
        /// Get the vertical position type for an alignment.
        /// </summary>
        /// <param name="a">Alignment.</param>
        /// <returns>The position type.</returns>
        public static PositionType VerticalPositionType(Alignment a) {
            return (PositionType)(((byte)a & 0xF0) >> 4);
        }

    }

}
