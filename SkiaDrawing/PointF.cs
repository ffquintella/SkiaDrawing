using System;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A structure that represents an x- and y-coordinate pair in floating-point.
    /// Mimics System.Drawing.PointF but uses SkiaSharp interop methods.
    /// </summary>
    public struct PointF : IEquatable<PointF>
    {
        /// <summary>
        /// The x-coordinate of this PointF.
        /// </summary>
        public float X;

        /// <summary>
        /// The y-coordinate of this PointF.
        /// </summary>
        public float Y;

        /// <summary>
        /// Initializes a new instance of the PointF struct with the specified coordinates.
        /// </summary>
        public PointF(float x, float y)
        {
            X = x;
            Y = y;
        }

        #region Static and Special Members

        /// <summary>
        /// A static property that returns a PointF with coordinates (0, 0).
        /// </summary>
        public static PointF Empty => new PointF(0f, 0f);

        /// <summary>
        /// Determines whether this PointF is empty (both X and Y are 0).
        /// </summary>
        public bool IsEmpty => (X == 0f && Y == 0f);

        #endregion

        #region Conversion to/from SkiaSharp

        /// <summary>
        /// Converts this PointF to an SKPoint (SkiaSharp).
        /// </summary>
        public SKPoint ToSKPoint()
        {
            return new SKPoint(X, Y);
        }

        /// <summary>
        /// Creates a PointF from an SKPoint.
        /// </summary>
        public static PointF FromSKPoint(SKPoint pt)
        {
            return new PointF(pt.X, pt.Y);
        }

        #endregion

        #region Equality and Overrides

        public bool Equals(PointF other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        public override bool Equals(object obj)
        {
            return (obj is PointF pf) && Equals(pf);
        }

        public override int GetHashCode()
        {
            // Combine X and Y into a simple hash
            unchecked
            {
                int hash = 17;
                hash = (hash * 31) + X.GetHashCode();
                hash = (hash * 31) + Y.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{{X={X}, Y={Y}}}";
        }

        public static bool operator ==(PointF left, PointF right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PointF left, PointF right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region Operators for Arithmetic (Optional)

        // You can optionally define operator +, -, etc.
        // Example:
        public static PointF operator +(PointF pt, PointF sz)
        {
            return new PointF(pt.X + sz.X, pt.Y + sz.Y);
        }

        public static PointF operator -(PointF pt, PointF sz)
        {
            return new PointF(pt.X - sz.X, pt.Y - sz.Y);
        }

        #endregion
    }
}
