using Dreamscape.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// File system.
    /// </summary>
    public static class FileSystem {

        /// <summary>
        /// List of files.
        /// </summary>
        private static List<string> FileListing = new List<string>();

        /// <summary>
        /// Maps files from path to an archive.
        /// </summary>
        private static Dictionary<string, Archive> FileMap = new Dictionary<string, Archive>();

        /// <summary>
        /// Archives.
        /// </summary>
        private static Dictionary<string, Archive> Archives = new Dictionary<string, Archive>();

        /// <summary>
        /// Load an archive from a file path.
        /// </summary>
        /// <param name="name">Name of the archive to load.</param>
        /// <param name="filePath">Path of the archive to load.</param>
        public static void LoadArchive(string name, string filePath) {
            Archive a;
            if (filePath.EndsWith(".dpck")) {
                Pack p = new Pack();
                p.Read(filePath);
                a = (Archive)p.FetchFile<Archive>();
                p.Dispose();
            } else {
                a = new Archive();
                a.Read(filePath);
            }
            Archives.Add(name, a);
            var l = a.GetFilePaths();
            FileListing.AddRange(l);
            foreach (var s in l) {
                if (FileMap.ContainsKey(s)) {
                    FileMap[s] = a;
                } else {
                    FileMap.Add(s, a);
                }
            }
        }

        /// <summary>
        /// Load an archive.
        /// </summary>
        /// <param name="name">Name of the archive to load.</param>
        /// <param name="archive">Archive to load.</param>
        public static void LoadArchive(string name, Archive archive) {
            Archives.Add(name, archive);
            var l = archive.GetFilePaths();
            FileListing.AddRange(l);
            foreach (var s in l) {
                if (FileMap.ContainsKey(s)) {
                    FileMap[s] = archive;
                } else {
                    FileMap.Add(s, archive);
                }
            }
        }

        /// <summary>
        /// Unload an archive.
        /// </summary>
        /// <param name="name">The archive to unload.</param>
        public static void UnloadArchive(string name) {
            Archive a = Archives[name];
            Archives.Remove(name);
            var l = a.GetFilePaths();
            foreach (var s in l) {
                FileListing.Remove(s);
                FileMap.Remove(s);
                if (FileListing.Contains(s)) {
                    foreach (var v in Archives.Values) {
                        if (v.GetFilePaths().Contains(s)) {
                            FileMap.Add(s, v);
                            break;
                        }
                    }
                }
            }
            a.Dispose();
        }

        /// <summary>
        /// If a file exists.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <returns>If the file exists.</returns>
        public static bool FileExists(string filePath) {
            return FileListing.Contains(filePath) || GameHelper.GameAssemby.GetManifestResourceNames().Contains(GetEmbeddedResourceName(filePath)) || File.Exists(filePath);
        }

        /// <summary>
        /// Open a file from a file path.
        /// </summary>
        /// <typeparam name="T">The type of IOFile.</typeparam>
        /// <param name="filePath">Path of the file.</param>
        /// <returns>The file openend.</returns>
        public static IOFile OpenFile<T>(string filePath) {
            if (FileListing.Contains(filePath)) {
                return FileMap[filePath].FetchFile<T>(filePath);
            } else if (GameHelper.GameAssemby.GetManifestResourceNames().Contains(GetEmbeddedResourceName(filePath))) {
                IOFile f = (IOFile)Activator.CreateInstance(typeof(T));
                f.Read(GameHelper.GameAssemby.GetManifestResourceStream(GetEmbeddedResourceName(filePath)));
                return f;
            } else {
                IOFile f = (IOFile)Activator.CreateInstance(typeof(T));
                f.Read(filePath);
                return f;
            }
        }

        /// <summary>
        /// Open a file stream from a file path.
        /// </summary>
        /// <param name="filePath">The file path to open a stream to.</param>
        /// <returns>The file stream.</returns>
        public static Stream OpenFileStream(string filePath) {
            if (FileListing.Contains(filePath)) {
                return FileMap[filePath].FetchFileStream(filePath);
            } else if (GameHelper.GameAssemby.GetManifestResourceNames().Contains(GetEmbeddedResourceName(filePath))) { 
                return GameHelper.GameAssemby.GetManifestResourceStream(GetEmbeddedResourceName(filePath));
            } else if (Assembly.GetExecutingAssembly().GetManifestResourceNames().Contains(GetEmbeddedResourceNameLocal(filePath))) {
                return Assembly.GetExecutingAssembly().GetManifestResourceStream(GetEmbeddedResourceNameLocal(filePath));
            } else {
                return new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
        }

        /// <summary>
        /// Get the embedded name for a resource.
        /// </summary>
        /// <param name="name">Local name.</param>
        /// <returns>The full embedded name.</returns>
        public static string GetEmbeddedResourceName(string name) {
            return GameHelper.GameAssemby.GetName().Name + "." + name.Replace("/", "\\").Replace("\\", ".");
        }

        /// <summary>
        /// Get the embedded local name for a resource.
        /// </summary>
        /// <param name="name">Local name.</param>
        /// <returns>The full embedded name.</returns>
        public static string GetEmbeddedResourceNameLocal(string name) {
            return Assembly.GetExecutingAssembly().GetName().Name + "." + name.Replace("/", "\\").Replace("\\", ".");
        }

        /// <summary>
        /// Open the file data from a file path.
        /// </summary>
        /// <param name="filePath">The file path to get the bytes of.</param>
        /// <returns>The file data.</returns>
        public static byte[] OpenFileData(string filePath) {
            using (Stream s = OpenFileStream(filePath)) {
                using (MemoryStream o = new MemoryStream()) {
                    using (FileReader r = new FileReader(s)) {
                        using (FileWriter w = new FileWriter(o)) {
                            w.Write(r.ReadBytes((int)s.Length));
                            return o.ToArray();
                        }
                    }
                }
            }
        }

    }

}
