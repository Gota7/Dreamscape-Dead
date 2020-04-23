using Dreamscape.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// An archive containing files. Binary form is DARC (Dreamscape ARChive).
    /// </summary>
    public class Archive : IOFile, IDisposable {

        /// <summary>
        /// File reader.
        /// </summary>
        private FileReader FileReader;

        /// <summary>
        /// File information.
        /// </summary>
        private Dictionary<string, FileInfo> FileListing = new Dictionary<string, FileInfo>();

        /// <summary>
        /// Disposed.
        /// </summary>
        private bool Disposed = false;

        /// <summary>
        /// File information.
        /// </summary>
        public class FileInfo {

            /// <summary>
            /// File offset.
            /// </summary>
            public uint Offset;

            /// <summary>
            /// Size.
            /// </summary>
            public int Size;

            /// <summary>
            /// If the file is contained within a pack.
            /// </summary>
            public bool IsPacked;

        }

        /// <summary>
        /// Get the file paths.
        /// </summary>
        /// <returns>List of all the file paths.</returns>
        public List<string> GetFilePaths() => FileListing.Keys.ToList();

        /// <summary>
        /// Deconstructor.
        /// </summary>
        ~Archive() {
            Dispose();
        }

        /// <summary>
        /// Fetch an IO file.
        /// </summary>
        /// <typeparam name="T">The type of file.</typeparam>
        /// <param name="filePath">Path to the file.</param>
        /// <returns>The file fetched.</returns>
        public virtual IOFile FetchFile<T>(string filePath) {
            FileReader.Position = FileListing[filePath].Offset;
            if (FileListing[filePath].IsPacked) {
                using (Pack p = new Pack()) {
                    p.Read(FileReader.ReadBytes(FileListing[filePath].Size));
                    return p.FetchFile<T>();
                }
            } else {
                IOFile f = (IOFile)Activator.CreateInstance(typeof(T));
                f.Read(FileReader.ReadBytes(FileListing[filePath].Size));
                return f;
            }
        }

        /// <summary>
        /// Fetch a file stream.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>The file stream.</returns>
        public virtual Stream FetchFileStream(string filePath) {
            FileReader.Position = FileListing[filePath].Offset;
            if (FileListing[filePath].IsPacked) {
                using (Pack p = new Pack()) {
                    p.Read(FileReader.ReadBytes(FileListing[filePath].Size));
                    return p.FetchFileStream();
                }
            }
            return new MemoryStream(FileReader.ReadBytes(FileListing[filePath].Size));
        }

        /// <summary>
        /// Read the archive.
        /// </summary>
        /// <param name="r">The reader.</param>
        public override void Read(FileReader r) {

            //New file reader.
            MemoryStream src = new MemoryStream(r.ReadBytes((int)r.Length));
            FileReader = new FileReader(src);

            //Read INFO block.
            r.Position = 0;
            FileListing = new Dictionary<string, FileInfo>();
            r.ReadUInt32();
            r.Align(0x10);
            r.ReadUInt32();
            uint num = r.ReadUInt32();
            for (uint i = 0; i < num; i++) {
                string name = r.ReadString();
                FileInfo f = new FileInfo();
                f.Offset = r.ReadUInt32();
                f.Size = r.ReadInt32();
                f.IsPacked = r.ReadBoolean();
                FileListing.Add(name, f);
            }

        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="w">The writer.</param>
        public override void Write(FileWriter w) {

            //Write data.
            FileReader.Position = 0;
            w.Write(FileReader.ReadBytes((int)FileReader.Length));

        }

        /// <summary>
        /// Dispose of the archive.
        /// </summary>
        public virtual void Dispose() {
            if (!Disposed) {
                try {
                    FileReader.BaseStream.Dispose();
                    FileReader.Dispose();
                    FileListing.Clear();
                } catch { }
            }
        }

    }

}
