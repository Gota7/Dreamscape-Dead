using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// A timer that does something after a certain time interval.
    /// </summary>
    public class Timer : IComparable<Timer>, IUpdateable {

        /// <summary>
        /// Time remaining left in the stopwatch.
        /// </summary>
        private double timeRemaining;

        /// <summary>
        /// Initial time of the timer.
        /// </summary>
        private uint initialTime;

        /// <summary>
        /// Create a new timer.
        /// </summary>
        /// <param name="timeRemaining">Time remaining in milliseconds.</param>
        public Timer(uint timeRemaining) {
            this.timeRemaining = initialTime = timeRemaining;
        }

        /// <summary>
        /// Reset the timer.
        /// </summary>
        /// <param name="timeRemaining">Time to tick down to, null for intial value.</param>
        public void Reset(uint? timeRemaining) {
            if (timeRemaining == null) {
                this.timeRemaining = initialTime;
            } else {
                this.timeRemaining = timeRemaining.Value;
            }
        }

        /// <summary>
        /// Tick the stopwatch down. Put this in the proper update function.
        /// </summary>
        public void Update() {
            timeRemaining -= (uint)GameHelper.GameTime.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// If the timer has finished counting down, return the time past. Returns 0xFFFFFFFF if not done.
        /// </summary>
        /// <returns>-1 if the timer is still ticking, if not, then it is the negative milliseconds the timer past.</returns>
        public uint TimePast() {
            if (timeRemaining > 0) {
                return 0xFFFFFFFF;
            } else {
                return (uint)Math.Abs(timeRemaining);
            }
        }

        /// <summary>
        /// The time remaining in the timer. Returns 0xFFFFFFFF if the timer is past 0.
        /// </summary>
        /// <returns>The time remaining in the timer, or 0xFFFFFFFF if the timer is past 0.</returns>
        public uint TimeRemaining() {
            if (timeRemaining > 0) {
                return (uint)timeRemaining;
            } else {
                return 0xFFFFFFFF;
            }
        }

        /// <summary>
        /// Get the initial time.
        /// </summary>
        /// <returns>The initial timer time.</returns>
        public uint InitialTime() {
            return initialTime;
        }

        /// <summary>
        /// If the timer has finished.
        /// </summary>
        /// <returns>If the timer has finished.</returns>
        public bool Finished() {
            return timeRemaining <= 0;
        }

        /// <summary>
        /// Reset the timer back to the initial time.
        /// </summary>
        public void Reset() {
            timeRemaining = initialTime;
        }

        /// <summary>
        /// Compare to another timer.
        /// </summary>
        /// <param name="other">The other timer.</param>
        /// <returns>How much more time is on this timer than the other.</returns>
        public int CompareTo(Timer other) {
            return (int)(timeRemaining - other.timeRemaining);
        }

    }

}
