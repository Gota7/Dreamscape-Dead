using CSCore;
using CSCore.Codecs;
using CSCore.Linux.SoundOut;
using CSCore.SoundOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Audio {
    
    /// <summary>
    /// A buffer containing cached files.
    /// </summary>
    public class AudioBuffer {

        /// <summary>
        /// If the buffer is linux.
        /// </summary>
        public bool IsLinux;

        /// <summary>
        /// Windows buffer.
        /// </summary>
        public Dictionary<string, WasapiOut> WindowsBuffer = new Dictionary<string, WasapiOut>();

        /// <summary>
        /// Linux buffer.
        /// </summary>
        public Dictionary<string, ALSoundOut> LinuxBuffer = new Dictionary<string, ALSoundOut>();

        /// <summary>
        /// Windows codecs.
        /// </summary>
        private Dictionary<string, GetCodecAction> WindowsCodecs = new Dictionary<string, GetCodecAction>();

        /// <summary>
        /// Linux codecs.
        /// </summary>
        private Dictionary<string, CSCore.Linux.Codecs.GetCodecAction> LinuxCodecs = new Dictionary<string, CSCore.Linux.Codecs.GetCodecAction>();

        /// <summary>
        /// If disposed.
        /// </summary>
        private bool Disposed;

        /// <summary>
        /// Create a new audio buffer.
        /// </summary>
        public AudioBuffer() {
            IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            if (IsLinux) {
                if (!CSCore.Linux.Codecs.CodecFactory.SupportedFilesFilterEn.Contains(".ogg")) {
                    CSCore.Linux.Codecs.CodecFactory.Instance.Register("ogg-vorbis", new CSCore.Linux.Codecs.CodecFactoryEntry(s => new NVorbisSource(s).ToLinuxWaveSource(), ".ogg"));
                }
                LinuxCodecs.Add(".ogg", new CSCore.Linux.Codecs.GetCodecAction(s => new NVorbisSource(s).ToLinuxWaveSource()));
            } else {
                if (!CodecFactory.SupportedFilesFilterEn.Contains(".ogg")) {
                    CodecFactory.Instance.Register("ogg-vorbis", new CodecFactoryEntry(s => new NVorbisSource(s).ToWaveSource(), ".ogg"));
                }
                WindowsCodecs.Add(".ogg", new GetCodecAction(s => new NVorbisSource(s).ToWaveSource()));
            }
        }

        /// <summary>
        /// Deconstructor.
        /// </summary>
        ~AudioBuffer() {
            Dispose();
        }

        /// <summary>
        /// Load an audio file. Unfortunately, Linux can't load from the file system YET.
        /// </summary>
        /// <param name="name">The audio name.</param>
        /// <param name="filePath">Path to the file.</param>
        /// <param name="loop">Loop. Will try and find one with the same name and in the same location if null.</param>
        public void LoadAudioFile(string name, string filePath, Loop loop = null) {
            if (loop == null && FileSystem.FileExists(Path.GetFileNameWithoutExtension(filePath) + ".dbli")) {
                loop = (Loop)FileSystem.OpenFile<Loop>(Path.GetFileNameWithoutExtension(filePath) + ".dbli");
            }
            if (IsLinux) {
                var c = CSCore.Linux.Codecs.CodecFactory.Instance.GetCodec(filePath);
                var src = new AudioSource(c, loop);
                ALSoundOut o = new ALSoundOut();
                o.Initialize(src);
                if (!LinuxBuffer.ContainsKey(name)) {
                    LinuxBuffer.Add(name, o);
                } else {
                    LinuxBuffer[name] = o;
                }
            } else {
                IWaveSource c = null;
                if (WindowsCodecs.ContainsKey(Path.GetExtension(filePath))) {
                    c = WindowsCodecs[Path.GetExtension(filePath)](FileSystem.OpenFileStream(filePath));
                }
                if (c == null) { c = new CSCore.MediaFoundation.MediaFoundationDecoder(FileSystem.OpenFileStream(filePath)); }
                var src = new AudioSource(c, loop);
                WasapiOut o = new WasapiOut();
                o.Initialize(src);
                if (!WindowsBuffer.ContainsKey(name)) {
                    WindowsBuffer.Add(name, o);
                } else {
                    WindowsBuffer[name] = o;
                }
            }
        }

        /// <summary>
        /// Unload an audio file.
        /// </summary>
        /// <param name="name">Name of the audio file to unload.</param>
        public void UnloadAudioFile(string name) {
            if (IsLinux) {
                LinuxBuffer[name].Dispose();
                LinuxBuffer.Remove(name);
            } else {
                WindowsBuffer[name].Dispose();
                WindowsBuffer.Remove(name);
            }
        }

        /// <summary>
        /// Dispose of the audio buffer.
        /// </summary>
        public void Dispose() {
            if (!Disposed) {
                if (IsLinux) {
                    foreach (var v in LinuxBuffer) {
                        v.Value.Dispose();
                    }
                    LinuxBuffer.Clear();
                } else {
                    foreach (var v in WindowsBuffer) {
                        v.Value.Dispose();
                    }
                    WindowsBuffer.Clear();
                }
            }
        }

    }

}
