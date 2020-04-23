using Dreamscape.IO;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// Extensions.
    /// </summary>
    public static class Extensions {

        /// <summary>
        /// Swap two items in a list.
        /// </summary>
        /// <typeparam name="T">Type contained in list.</typeparam>
        /// <param name="l">List.</param>
        /// <param name="ind1">Index of the first item to swap.</param>
        /// <param name="ind2">Index of the second item to swap.</param>
        public static void Swap<T>(this IList<T> l, int ind1, int ind2) {

            //Not possible.
            if (ind1 < 0 || ind1 >= l.Count() || ind2 < 0 || ind2 >= l.Count()) {
                return;
            }

            //Switch data.
            T tmp = l[ind1];
            l[ind1] = l[ind2];
            l[ind2] = tmp;

        }

        /// <summary>
        /// Round.
        /// </summary>
        /// <param name="f">Float.</param>
        /// <returns>This rounded.</returns>
        public static int Round(this float f) {
            return (int)Math.Round(f, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Round up.
        /// </summary>
        /// <param name="f">Float.</param>
        /// <returns>This rounded.</returns>
        public static int RoundUp(this float f) {
            return (int)Math.Ceiling(f);
        }

        /// <summary>
        /// Get the sign of the number.
        /// </summary>
        /// <param name="f">The float.</param>
        /// <returns>The sign.</returns>
        public static Sign Sign(this float f) {
            if (f > 0) {
                return Dreamscape.Sign.Positive;
            } else if (f < 0) {
                return Dreamscape.Sign.Negative;
            } else {
                return Dreamscape.Sign.None;
            }
        }

        /// <summary>
        /// Get the direction of a vector.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <returns>The unit direction vector.</returns>
        public static Vector2 Direction(this Vector2 v) {

            //Divide the vector by its magnitude to remove all length.
            return v / v.Length();

        }

        /// <summary>
        /// Get an orthogonal vector to this vector.
        /// </summary>
        /// <param name="v">This vector.</param>
        /// <returns>The orthogonal vector.</returns>
        public static Vector2 Ortho(this Vector2 v) {

            //Returns an ortho vector by flipping the coordinates.
            return new Vector2(v.Y, -v.X);

        }

        /// <summary>
        /// Dot this vector with another.
        /// </summary>
        /// <param name="v">This vector.</param>
        /// <param name="other">The other vector to dot with.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static float Dot(this Vector2 v, Vector2 other) {

            //A * B = Sum of the products of the components.
            return v.X * other.X + v.Y + other.Y;

        }

        /// <summary>
        /// Project onto another vector.
        /// </summary>
        /// <param name="v">This vector.</param>
        /// <param name="other">The other vector to project onto.</param>
        /// <returns>The final projected vector.</returns>
        public static Vector2 ProjectOnto(this Vector2 v, Vector2 other) {

            //A||B = (A * A / (B * B))B. A vector dot itself is its magnitude squared.
            return v.Dot(other) / other.LengthSquared() * other;

        }

        /// <summary>
        /// Get the index of an item inside an array.
        /// </summary>
        /// <param name="a">Array of items.</param>
        /// <param name="item">Item to get the index of.</param>
        /// <returns>The index of the item.</returns>
        public static int IndexOf(this Array a, object item) {

            //For each item.
            for (int i = 0; i < a.Length; i++) {
                if (a.GetValue(i).Equals(item)) {
                    return i;
                }
            }

            //Default.
            return -1;

        }

        /// <summary>
        /// Get a list of normal axises.
        /// </summary>
        /// <param name="vertices">Vertices to get the vectors for.</param>
        /// <returns>Normal axises between each vertex./returns>
        public static List<Vector2> GetNormalAxises(this IEnumerable<Vector2> vertices) {

            //Axises for the shape.
            List<Vector2> axises = new List<Vector2>();

            //Get the vectors for each 
            for (int i = 0; i < vertices.Count(); i++) {

                //Current vertex.
                Vector2 p1 = vertices.ElementAt(i);

                //Next vertex.
                Vector2 p2 = vertices.ElementAt(i + 1 == vertices.Count() ? 0 : i + 1);

                //Get the edge vector.
                Vector2 edge = p1 - p2;

                //Get the perpendicular vector.
                Vector2 normal = edge.Ortho();

                //Add the normal to the axis list.
                normal.Normalize();
                axises.Add(normal);

            }

            //Return the axises.
            return axises;

        }

    }

    /// <summary>
    /// Sign.
    /// </summary>
    public enum Sign {
        Positive,
        Negative,
        None
    }

}
