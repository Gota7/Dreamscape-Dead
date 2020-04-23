using Dreamscape.IO;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// A compressed pack containing a file. Binary format is DPCK (Dreamscape PaCK).
    /// </summary>
    public class Pack : IOFile, IDisposable {

        /// <summary>
        /// File reader.
        /// </summary>
        public FileReader FileReader;

        /// <summary>
        /// Compression method.
        /// </summary>
        public PackCompression CompressionMethod;

        /// <summary>
        /// If the pack is disposed.
        /// </summary>
        private bool Disposed;

        /// <summary>
        /// Deconstructor.
        /// </summary>
        ~Pack() {
            Dispose();
        }

        /// <summary>
        /// Fetch the file contained in the pack.
        /// </summary>
        /// <typeparam name="T">Type of IOFile.</typeparam>
        /// <returns>The file contained in the pack.</returns>
        public IOFile FetchFile<T>() {
            IOFile f = (IOFile)Activator.CreateInstance(typeof(T));
            FileReader.Position = 0;
            f.Read(FileReader);
            return f;
        }

        /// <summary>
        /// Fetch the file stream.
        /// </summary>
        /// <returns>The stream for the file.</returns>
        public Stream FetchFileStream() {
            Stream s = new MemoryStream();
            FileReader.Position = 0;
            FileReader.BaseStream.CopyTo(s);
            return s;
        }

        /// <summary>
        /// Set the file used in the pack.
        /// </summary>
        /// <param name="b">The file bytes to put into the pack.</param>
        public void SetFile(byte[] b) {

            //Try disposing.
            try {
                FileReader.BaseStream.Dispose();
                FileReader.Dispose();
            } catch { }

            //Set the file data.
            MemoryStream src = new MemoryStream(b);
            FileReader = new FileReader(src);

        }

        /// <summary>
        /// Set the compression method to the best.
        /// </summary>
        public void SetBestCompression() {

            //Best method.
            PackCompression best = (PackCompression)0;
            uint least = uint.MaxValue;

            //Try each compression.
            foreach (var v in Enum.GetValues(typeof(PackCompression))) {
                CompressionMethod = (PackCompression)v;
                byte[] b = Write();
                if (b.Length < least) {
                    best = CompressionMethod;
                    least = (uint)b.Length;
                }
            }

            //Set method.
            CompressionMethod = best;

        }

        /// <summary>
        /// Read the pack.
        /// </summary>
        /// <param name="r">The reader.</param>
        public override void Read(FileReader r) {

            //Try to reset stream.
            try {
                FileReader.BaseStream.Dispose();
                FileReader.Dispose();
            } catch { }

            //Read the data.
            r.ReadUInt32();
            CompressionMethod = (PackCompression)r.ReadUInt32();
            uint size = r.ReadUInt32();
            r.ReadUInt32();
            MemoryStream src = new MemoryStream(r.ReadBytes((int)size));
            MemoryStream o = new MemoryStream();

            //Decompress data.
            switch (CompressionMethod) {
                case PackCompression.Gzip:
                    GZip.Decompress(src, o, false);
                    break;
                case PackCompression.Bzip2:
                    BZip2.Decompress(src, o, false);
                    break;
                case PackCompression.Lzma:
                    byte[] lzmaTemp = LZMA.Engine.Decompress(src.ToArray());
                    o.Write(lzmaTemp, 0, lzmaTemp.Length);
                    break;
            }

            //Clean up.
            FileReader = new FileReader(o);
            src.Dispose();

        }

        /// <summary>
        /// Write the pack.
        /// </summary>
        /// <param name="w">The writer.</param>
        public override void Write(FileWriter w) {

            //Write the data.
            w.Write("DPCK".ToArray());
            w.Write((uint)CompressionMethod);
            w.InitOffset("DataSize");
            w.Write((uint)FileReader.Length);
            w.StartStructure();

            //Switch compression.
            MemoryStream o = new MemoryStream();
            FileReader.Position = 0;
            switch (CompressionMethod) {
                case PackCompression.Gzip:
                    GZip.Compress(FileReader.BaseStream, o, false, 512, 9);
                    break;
                case PackCompression.Bzip2:
                    BZip2.Compress(FileReader.BaseStream, o, false, 9);
                    break;
                case PackCompression.Lzma:
                    MemoryStream m = (MemoryStream)FetchFileStream();
                    w.Write(LZMA.Engine.Compress(m.ToArray()));
                    m.Dispose();
                    break;
            }

            //Write buffer.
            w.Write(o.ToArray());
            w.CloseOffset("DataSize");
            w.EndStructure();
            o.Dispose();

        }

        /// <summary>
        /// Dispose of the pack.
        /// </summary>
        public void Dispose() {
            if (!Disposed) {
                try {
                    FileReader.BaseStream.Dispose();
                    FileReader.Dispose();
                } catch {}
                Disposed = true;
            }
        }

    }

    /// <summary>
    /// Pack compression type.
    /// </summary>
    public enum PackCompression {
        Gzip, Bzip2, Lzma
    }

}
