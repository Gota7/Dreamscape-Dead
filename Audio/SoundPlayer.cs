using CSCore;
using CSCore.Linux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Audio {
    
    /// <summary>
    /// Sound player.
    /// </summary>
    public class SoundPlayer : IDisposable {

        /// <summary>
        /// An audio buffer to store buffered.
        /// </summary>
        private AudioBuffer AudioBuffer = new AudioBuffer();

        /// <summary>
        /// Paused from window away.
        /// </summary>
        public bool PausedFromWindowAway = false;

        /// <summary>
        /// Sounds paused from window.
        /// </summary>
        private List<string> SoundsPausedFromWindowAway = new List<string>();

        /// <summary>
        /// If disposed.
        /// </summary>
        private bool Disposed;

        /// <summary>
        /// Deconstructor.
        /// </summary>
        ~SoundPlayer() {
            Dispose();
        }

        /// <summary>
        /// Load a sound.
        /// </summary>
        /// <param name="name">The sound name.</param>
        /// <param name="filePath">Sound file path.</param>
        /// <param name="loop">Loop for the song. If null, will load a loop file in the same path with the same name as the file if found.</param>
        public void LoadSound(string name, string filePath, Loop loop = null) {
            AudioBuffer.LoadAudioFile(name, filePath, loop);
        }

        /// <summary>
        /// Unload a sound.
        /// </summary>
        /// <param name="name">Sound name to unload.</param>
        public void UnloadSound(string name) {
            AudioBuffer.UnloadAudioFile(name);
        }

        /// <summary>
        /// Play a sound.
        /// </summary>
        /// <param name="name">Sound name.</param>
        /// <param name="volume">Sound volume.</param>
        /// <param name="unloadOnCompletion">If the sound should be unloaded on completion. TODO!!!</param>
        public void PlaySound(string name, float volume = 1f, bool unloadOnCompletion = false) {
            if (AudioBuffer.IsLinux) {
                var o = AudioBuffer.LinuxBuffer[name];
                o.WaveSource.Position = 0;
                o.Volume = volume;
                o.Play();
            } else {
                var o = AudioBuffer.WindowsBuffer[name];
                o.WaveSource.Position = 0;
                o.Volume = volume;
                o.Play();
            }
        }

        /// <summary>
        /// Pause playback.
        /// </summary>
        /// <param name="name">Sound name.</param>
        public void PauseSound(string name) {
            if (AudioBuffer.IsLinux) {
                AudioBuffer.LinuxBuffer[name].Pause();
            } else {
                AudioBuffer.WindowsBuffer[name].Pause();
            }
        }

        /// <summary>
        /// Resume playback.
        /// </summary>
        /// <param name="name">Sound name.</param>
        public void ResumeSound(string name) {
            if (AudioBuffer.IsLinux) {
                AudioBuffer.LinuxBuffer[name].Resume();
            } else {
                AudioBuffer.WindowsBuffer[name].Resume();
            }
        }

        /// <summary>
        /// Stop playback.
        /// </summary>
        /// <param name="name">Sound name.</param>
        public void StopSound(string name) {
            if (AudioBuffer.IsLinux) {
                AudioBuffer.LinuxBuffer[name].Stop();
            } else {
                AudioBuffer.WindowsBuffer[name].Stop();
            }
        }

        /// <summary>
        /// The volume.
        /// </summary>
        /// <param name="name">Sound name.</param>
        public float GetVolume(string name) {
            if (AudioBuffer.IsLinux) {
                return AudioBuffer.LinuxBuffer[name].Volume;
            } else {
                return AudioBuffer.WindowsBuffer[name].Volume;
            }
        }

        /// <summary>
        /// Set volume.
        /// </summary>
        /// <param name="name">Sound name.</param>
        /// <param name="vol">New volume.</param>
        public void SetVolume(string name, float vol) {
            if (AudioBuffer.IsLinux) {
                AudioBuffer.LinuxBuffer[name].Volume = vol;
            } else {
                AudioBuffer.WindowsBuffer[name].Volume = vol;
            }
        }

        /// <summary>
        /// Get the position.
        /// </summary>
        /// <param name="name">Sound name.</param>
        /// <returns>Posititon.</returns>
        public TimeSpan GetPosition(string name) {
            if (AudioBuffer.IsLinux) {
                return AudioBuffer.LinuxBuffer[name].WaveSource.GetPosition();
            } else {
                return AudioBuffer.WindowsBuffer[name].WaveSource.GetPosition();
            }
        }

        /// <summary>
        /// Set the position.
        /// </summary>
        /// <param name="name">Sound name.</param>
        /// <param name="pos">Position.</param>
        public void SetPosition(string name, TimeSpan pos) {
            if (AudioBuffer.IsLinux) {
                AudioBuffer.LinuxBuffer[name].WaveSource.SetPosition(pos);
            } else {
                AudioBuffer.WindowsBuffer[name].WaveSource.SetPosition(pos);
            }
        }

        /// <summary>
        /// Dispose of this player.
        /// </summary>
        public void Dispose() {
            if (!Disposed) {
                AudioBuffer.Dispose();
                Disposed = true;
            }
        }

        /// <summary>
        /// Pause all the active songs, to be used when the window is a way.
        /// </summary>
        public void PauseActiveSoundsOnWindowAway() {
            if (AudioBuffer.IsLinux) {
                foreach (var s in AudioBuffer.LinuxBuffer) {
                    if (s.Value.PlaybackState == CSCore.Linux.SoundOut.PlaybackState.Playing) {
                        SoundsPausedFromWindowAway.Add(s.Key);
                        s.Value.Pause();
                    }
                }
            } else {
                foreach (var s in AudioBuffer.WindowsBuffer) {
                    if (s.Value.PlaybackState == CSCore.SoundOut.PlaybackState.Playing) {
                        SoundsPausedFromWindowAway.Add(s.Key);
                        s.Value.Pause();
                    }
                }
            }
        }

        /// <summary>
        /// Resume all active sounds on window return.
        /// </summary>
        public void ResumeActiveSoundsOnWindowReturn() {
            foreach (var s in SoundsPausedFromWindowAway) {
                if (AudioBuffer.IsLinux) {
                    AudioBuffer.LinuxBuffer[s].Resume();
                } else {
                    AudioBuffer.WindowsBuffer[s].Resume();
                }
            }
            SoundsPausedFromWindowAway.Clear();
        }

    }

}
