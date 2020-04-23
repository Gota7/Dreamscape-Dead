using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Audio {

    /// <summary>
    /// Dreamscape audio source.
    /// </summary>
    public class AudioSource : CSCore.IReadableAudioSource<byte>, CSCore.Linux.IReadableAudioSource<byte>, IWaveSource, CSCore.Linux.IWaveSource {

        /// <summary>
        /// Create a new audio source.
        /// </summary>
        /// <param name="source">Audio source.</param>
        /// <param name="loop">Loop information.</param>
        public AudioSource(IReadableAudioSource<byte> source, Loop loop = null) {
            IsLinux = false;
            AudioSourceWindows = source;
            Loop = loop;
        }

        /// <summary>
        /// Create a new audio source.
        /// </summary>
        /// <param name="source">Audio source.</param>
        /// <param name="loop">Loop information.</param>
        public AudioSource(CSCore.Linux.IReadableAudioSource<byte> source, Loop loop = null) {
            IsLinux = true;
            AudioSourceLinux = source;
            Loop = loop;
        }

        /// <summary>
        /// Loop information.
        /// </summary>
        public Loop Loop;

        /// <summary>
        /// Audio source.
        /// </summary>
        private IReadableAudioSource<byte> AudioSourceWindows;

        /// <summary>
        /// Audio source linux.
        /// </summary>
        private CSCore.Linux.IReadableAudioSource<byte> AudioSourceLinux;

        /// <summary>
        /// If linux.
        /// </summary>
        public bool IsLinux;

        /// <summary>
        /// Can seek.
        /// </summary>
        public bool CanSeek => IsLinux ? AudioSourceLinux.CanSeek : AudioSourceWindows.CanSeek;

        /// <summary>
        /// Wave format.
        /// </summary>
        public CSCore.WaveFormat WaveFormat => AudioSourceWindows.WaveFormat;

        /// <summary>
        /// Wave format linux.
        /// </summary>
        CSCore.Linux.WaveFormat CSCore.Linux.IAudioSource.WaveFormat => AudioSourceLinux.WaveFormat;

        /// <summary>
        /// Position.
        /// </summary>
        public long Position { get => IsLinux ? AudioSourceLinux.Position : AudioSourceWindows.Position; set { if (IsLinux) { AudioSourceLinux.Position = value; } else { AudioSourceWindows.Position = value; } } }

        /// <summary>
        /// Length.
        /// </summary>
        public long Length => IsLinux ? AudioSourceLinux.Length : AudioSourceWindows.Length;

        /// <summary>
        /// Read data.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        /// <returns>Number read.</returns>
        public int Read(byte[] buffer, int offset, int count) {

            //No loop info.
            if (Loop == null) {
                if (IsLinux) {
                    return AudioSourceLinux.Read(buffer, offset, count);
                } else {
                    return AudioSourceWindows.Read(buffer, offset, count);
                }
            }

            //Loops. TODO!!!
            else {
                return 0;
            }

        }

        /// <summary>
        /// Dispose the source.
        /// </summary>
        public void Dispose() {
            if (IsLinux) {
                AudioSourceLinux.Dispose();
            } else if (AudioSourceWindows != null) {
                AudioSourceWindows.Dispose();
            }
        }

    }

}
