using Dreamscape.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Audio {

    /// <summary>
    /// A loop. Binary form is DBLI (Dreamscape Binary Loop Information).
    /// </summary>
    public class Loop : IOFile {

        /// <summary>
        /// Loop points.
        /// </summary>
        public Dictionary<string, LoopPoint> Points = new Dictionary<string, LoopPoint>();

        /// <summary>
        /// Read the file.
        /// </summary>
        /// <param name="r">The reader.</param>
        public override void Read(FileReader r) {

            //Get loops.
            int pointCount = r.ReadInt32();
            for (int i = 0; i < pointCount; i++) {
                LoopPoint p = r.Read<LoopPoint>();
                Points.Add(p.Name, p);
            }

        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="w">The writer.</param>
        public override void Write(FileWriter w) {

            //Prepare data.
            foreach (var p in Points) {
                p.Value.Name = p.Key;
            }

            //Write loops.
            w.Write(Points.Count);
            foreach (var p in Points) {
                w.Write(p.Value);
            }

        }

    }

    /// <summary>
    /// A loop point.
    /// </summary>
    public class LoopPoint : IReadable, IWriteable {

        //Common parameters.

        /// <summary>
        /// Loop type.
        /// </summary>
        public LoopType LoopType;

        /// <summary>
        /// Loop name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Position of the point.
        /// </summary>
        public TimeSpan Position;

        //Normal parameters.

        /// <summary>
        /// Target loop point.
        /// </summary>
        public string Target;

        /// <summary>
        /// How many loops. 0 means infinite.
        /// </summary>
        public int LoopCount;

        //Flag/Region parameter.

        /// <summary>
        /// Activation flag name.
        /// </summary>
        public string ActivationFlag;

        /// <summary>
        /// Read the loop point.
        /// </summary>
        /// <param name="r">The reader.</param>
        public void Read(FileReader r) {
            LoopType = (LoopType)r.ReadByte();
            Name = r.ReadString();
            Position = r.ReadTimespan();
            if (LoopType > LoopType.Marker) {
                Target = r.ReadString();
                LoopCount = r.ReadInt32();
            }
            if (LoopType > LoopType.Normal) {
                ActivationFlag = r.ReadString();
            }
        }

        /// <summary>
        /// Write the loop point.
        /// </summary>
        /// <param name="w">The writer.</param>
        public void Write(FileWriter w) {
            w.Write((byte)LoopType);
            w.Write(Name);
            w.Write(Position);
            if (LoopType > LoopType.Marker) {
                w.Write(Target);
                w.Write(LoopCount);
            }
            if (LoopType > LoopType.Normal) {
                w.Write(ActivationFlag);
            }
        }

    }

    /// <summary>
    /// Loop type.
    /// </summary>
    public enum LoopType : byte {

        /// <summary>
        /// Just a marker and does not point to anything.
        /// </summary>
        Marker,

        /// <summary>
        /// Go to a position.
        /// </summary>
        Normal,

        /// <summary>
        /// Loop is only active when a flag is set.
        /// </summary>
        FlagActivated,

        /// <summary>
        /// Only enabled if the flag name plus the region number is enabled.
        /// </summary>
        Regional,

    }

}
