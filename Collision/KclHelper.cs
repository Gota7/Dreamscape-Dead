using MathNet.Numerics.LinearAlgebra;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Collision {

    /// <summary>
    /// Helper that provides useful collision functions.
    /// </summary>
    public static class KclHelper {

        /// <summary>
        /// Get the raycast to where it intersects a line made by Point A and Point B.
        /// </summary>
        /// <param name="raycastOrigin">Raycast origin.</param>
        /// <param name="raycastDirection">Raycast direction.</param>
        /// <param name="a">Point A.</param>
        /// <param name="b">Point B.</param>
        /// <returns>The length of the raycast. Null if doesn't intersect.</returns>
        public static Vector2? Raycast(Vector2 raycastOrigin, Vector2 raycastDirection, Vector2 a, Vector2 b) {

            //Get the raycasts to the vertices.
            Vector2 aCast = a - raycastOrigin;
            Vector2 bCast = b - raycastOrigin;

            //Get the slopes to the vertices.
            float slopeA = aCast.Y / aCast.X;
            float slopeB = bCast.Y / bCast.X;

            //If the slope of the raycast is not inbetween the two vertices, it doesn't intersect the line formed by them!
            float raycastSlope = raycastDirection.Y / raycastDirection.X;
            if (!((slopeA <= raycastSlope && raycastSlope <= slopeB) || (slopeB <= raycastSlope && raycastSlope <= slopeA))) {
                return null;
            }

            //Easy exit if the raycast lands on a point.
            if (slopeA == raycastSlope && aCast.X.Sign() == raycastDirection.X.Sign() && aCast.Y.Sign() == raycastDirection.Y.Sign()) {
                return aCast;
            } else if (slopeB == raycastSlope && bCast.X.Sign() == raycastDirection.X.Sign() && bCast.Y.Sign() == raycastDirection.Y.Sign()) {
                return bCast;
            }

            //Get the line vector between the two points.
            Vector2 lineVector = b - a;

            //Set up a matrix to solve.
            var m = Matrix<float>.Build.Dense(2, 2);
            m[0, 0] = raycastDirection.X;
            m[1, 0] = raycastDirection.Y;
            m[0, 1] = -lineVector.X;
            m[1, 1] = -lineVector.Y;

            //Solve the matrix.
            Vector<float> input = Vector<float>.Build.DenseOfArray(new float[] { a.X - raycastOrigin.X, a.Y - raycastOrigin.Y });
            var solution = m.Solve(input);

            //Solution must not be negative!
            if (solution[0] < 0) { return null; }

            //Return the solution, which is the solved weight for the raycast times the raycast.
            return solution[0] * raycastDirection;

        }

    }

}
