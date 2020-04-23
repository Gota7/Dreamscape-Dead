using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// Angle.
    /// </summary>
    public class Angle {

        /// <summary>
        /// Real angle.
        /// </summary>
        private ushort realAngle = 0;

        /// <summary>
        /// Get the value.
        /// </summary>
        public ushort Value => realAngle;

        /// <summary>
        /// Blank constructor.
        /// </summary>
        public Angle() {}

        /// <summary>
        /// Create a new angle a given angle.
        /// </summary>
        /// <param name="units">Units of the angle.</param>
        public Angle(ushort units) {
            realAngle = units;
        }

        /// <summary>
        /// Create an angle from radians.
        /// </summary>
        /// <param name="radians">The amount in radians.</param>
        public Angle(double radians) {
            realAngle = (ushort)(radians * 10430.37835);
        }

        /// <summary>
        /// Create an angle from degrees.
        /// </summary>
        /// <param name="degrees">Degrees of the angle.</param>
        public Angle(float degrees) {
            realAngle = (ushort)(degrees * 182.04444444444444);
        }

        /// <summary>
        /// Get the amount in degrees.
        /// </summary>
        public double Degrees {
            get {
                return realAngle / 182.04444444444444;
            } set {
                realAngle = (ushort)(value * 182.04444444444444);
            }
        }

        /// <summary>
        /// Get the amount in radians.
        /// </summary>
        public double Radians {
            get {
                return realAngle / 10430.37835;
            } set {
                realAngle = (ushort)(value * 10430.37835);
            }
        }

        /// <summary>
        /// Add to angle.
        /// </summary>
        public static Angle operator +(Angle a, ushort amount) {
            return new Angle() { realAngle = (ushort)(a.realAngle + amount) };
        }

        /// <summary>
        /// Subtract from angle.
        /// </summary>
        public static Angle operator -(Angle a, ushort amount) {
            return new Angle() { realAngle = (ushort)(a.realAngle - amount) };
        }

        /// <summary>
        /// Add to angle.
        /// </summary>
        public static Angle operator +(Angle a, Angle b) {
            return new Angle() { realAngle = (ushort)(a.realAngle + b.realAngle) };
        }

        /// <summary>
        /// Subtract from angle.
        /// </summary>
        public static Angle operator -(Angle a, Angle b) {
            return new Angle() { realAngle = (ushort)(a.realAngle - b.realAngle) };
        }

        /// <summary>
        /// Set the angle.
        /// </summary>
        /// <param name="v">Value.</param>
        public static implicit operator Angle(ushort v) {
            return new Angle() { realAngle = v };
        }

    }

}
