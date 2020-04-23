using CSCore.Linux;
using NVorbis;
using System;
using System.IO;

namespace Dreamscape.Audio {

    /// <summary>
    /// OGG file support for the game.
    /// </summary>
    public sealed class NVorbisSource : CSCore.ISampleSource, CSCore.Linux.ISampleSource {

        /// <summary>
        /// Sound stream that the player plays.
        /// </summary>
        private readonly Stream _stream;

        /// <summary>
        /// Reader to read the OGG file.
        /// </summary>
        private readonly VorbisReader _vorbisReader;

        /// <summary>
        /// Format info of the OGG file.
        /// </summary>
        private readonly CSCore.WaveFormat _waveFormat;

        /// <summary>
        /// Format info of the OGG file.
        /// </summary>
        private readonly CSCore.Linux.WaveFormat _waveFormatLinux;

        /// <summary>
        /// If the OGG has been disposed or not.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Create a new OGG source.
        /// </summary>
        /// <param name="stream">OGG stream to make the OGG source from.</param>
        public NVorbisSource(Stream stream) {

            //Stream can't be null.
            if (stream == null)
                throw new ArgumentNullException("stream");

            //Make sure stream can be read.
            if (!stream.CanRead)
                throw new ArgumentException("Stream is not readable.", "stream");

            //Set the stream.
            _stream = stream;

            //Set up the reader.
            _vorbisReader = new VorbisReader(stream, false);

            //Create the wave format.
            _waveFormat = new CSCore.WaveFormat(_vorbisReader.SampleRate, 32, _vorbisReader.Channels, CSCore.AudioEncoding.IeeeFloat);
            _waveFormatLinux = new CSCore.Linux.WaveFormat(_vorbisReader.SampleRate, 32, _vorbisReader.Channels, CSCore.Linux.AudioEncoding.IeeeFloat);

        }

        /// <summary>
        /// If the stream can seek.
        /// </summary>
        public bool CanSeek {
            get { return _stream.CanSeek; }
        }

        /// <summary>
        /// The wave format info of the OGG.
        /// </summary>
        public CSCore.WaveFormat WaveFormat {
            get { return _waveFormat; }
        }

        /// <summary>
        /// Linux wave format info of the OGG.
        /// </summary>
        CSCore.Linux.WaveFormat CSCore.Linux.IAudioSource.WaveFormat => _waveFormatLinux;

        /// <summary>
        /// The length of the OGG file.
        /// </summary>
        public long Length {
            get { return CanSeek ? (long)(_vorbisReader.TotalTime.TotalSeconds * _waveFormat.SampleRate * _waveFormat.Channels) : 0; }
        }

        /// <summary>
        /// The current position in the OGG file.
        /// </summary>
        public long Position {
            get {
                //Get position.
                return CanSeek ? (long)(_vorbisReader.DecodedTime.TotalSeconds * _vorbisReader.SampleRate * _vorbisReader.Channels) : 0;
            }
            set {

                //If can't seek, don't set position.
                if (!CanSeek)
                    throw new InvalidOperationException("NVorbisSource is not seekable.");

                //Make sure that the destination to seek to is accessible.
                if (value < 0 || value > Length)
                    throw new ArgumentOutOfRangeException("value");

                //Get the decoded time.
                _vorbisReader.DecodedTime = TimeSpan.FromSeconds((double)value / _vorbisReader.SampleRate / _vorbisReader.Channels);

            }
        }

        /// <summary>
        /// Read samples.
        /// </summary>
        /// <param name="buffer">Buffer of samples.</param>
        /// <param name="offset">Offset to read.</param>
        /// <param name="count">Number of samples.</param>
        /// <returns>Decoded samples.</returns>
        public int Read(float[] buffer, int offset, int count) {
            return _vorbisReader.ReadSamples(buffer, offset, count);
        }

        /// <summary>
        /// Dispose of the stream.
        /// </summary>
        public void Dispose() {

            //Dispose of stream if haven't already.
            if (!_disposed)
                _vorbisReader.Dispose();

            //Object has already been disposed.
            //else
                //throw new ObjectDisposedException("NVorbisSource");

            //Set disposed to true.
            _disposed = true;

        }

        /// <summary>
        /// Convert this to a linux wave source.
        /// </summary>
        /// <returns>The linux wave source.</returns>
        public CSCore.Linux.IWaveSource ToLinuxWaveSource() {
            return this.ToWaveSource();
        }

    }

}
