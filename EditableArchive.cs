using Dreamscape.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// An archive that can be edited. This is for creating archives, whereas the main archive class should be used for efficiently loading game assets.
    /// </summary>
    public class EditableArchive : Archive {

        /// <summary>
        /// Files contained within the archive.
        /// </summary>
        public Dictionary<string, byte[]> Files = new Dictionary<string, byte[]>();

        /// <summary>
        /// Packed files.
        /// </summary>
        public List<string> PackedFiles = new List<string>();

        /// <summary>
        /// If the archive is disposed.
        /// </summary>
        private bool Disposed = false;

        /// <summary>
        /// Deconstructor.
        /// </summary>
        ~EditableArchive() {
            Dispose();
        }

        /// <summary>
        /// Fetch a file.
        /// </summary>
        /// <typeparam name="T">The type of file to fetch.</typeparam>
        /// <param name="filePath">The path to the file to fetch.</param>
        /// <returns>The file.</returns>
        public override IOFile FetchFile<T>(string filePath) {
            IOFile f = (IOFile)Activator.CreateInstance(typeof(T));
            f.Read(Files[filePath]);
            return f;
        }

        /// <summary>
        /// Fetch a file stream.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>The file stream.</returns>
        public override Stream FetchFileStream(string filePath) {
            return new MemoryStream(Files[filePath]);
        }

        /// <summary>
        /// Read the archive.
        /// </summary>
        /// <param name="r">The reader.</param>
        public override void Read(FileReader r) {

            //Read the archive.
            r.ReadUInt32();
            r.Align(0x10);

            //Info block.
            r.ReadUInt32();
            uint num = r.ReadUInt32();
            List<Tuple<string, uint, int>> offs = new List<Tuple<string, uint, int>>();
            Files = new Dictionary<string, byte[]>();
            PackedFiles = new List<string>();
            for (uint i = 0; i < num; i++) {
                offs.Add(new Tuple<string, uint, int>(r.ReadString(), r.ReadUInt32(), r.ReadInt32()));
                if (r.ReadBoolean()) { PackedFiles.Add(offs[(int)i].Item1); }
            }

            //Data block.
            foreach (var o in offs) {
                r.Jump(o.Item2);
                Files.Add(o.Item1, r.ReadBytes(o.Item3));
            }

        }

        /// <summary>
        /// Write the archive.
        /// </summary>
        /// <param name="w">The writer.</param>
        public override void Write(FileWriter w) {

            //Write the archive.
            w.Write("DARC".ToCharArray());
            w.Align(0x10);

            //Info block.
            w.Write("INFO".ToCharArray());
            w.Write((uint)Files.Count);
            for (int i = 0; i < Files.Count; i++) {
                w.Write(Files.Keys.ElementAt(i));
                w.InitOffset("Offset" + i);
                w.InitOffset("Size" + i);
                w.Write(PackedFiles.Contains(Files.Keys.ElementAt(i)));
            }

            //Data block.
            w.Align(0x10);
            w.Write("DATA".ToCharArray());
            for (int i = 0; i < Files.Count; i++) {
                w.CloseOffset("Offset" + i);
                w.StartStructure();
                w.Write(Files.Values.ElementAt(i));
                w.CloseOffset("Size" + i);
                w.EndStructure();
            }

        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public override void Dispose() {
            if (!Disposed) {
                Files.Clear();
                Disposed = true;
            }
        }

    }

}
