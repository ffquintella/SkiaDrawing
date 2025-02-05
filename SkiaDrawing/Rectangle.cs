using System;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// Represents an integer-based rectangle, similar to System.Drawing.Rectangle, 
    /// but augmented with conversion helpers for SkiaSharp SKRect.
    /// </summary>
    public struct Rectangle : IEquatable<Rectangle>
    {
        #region Fields

        public int X;
        public int Y;
        public int Width;
        public int Height;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Rectangle struct with the specified coordinates and size.
        /// </summary>
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the Rectangle struct with the specified location and size.
        /// </summary>
        public Rectangle(Point location, Size size)
        {
            X = location.X;
            Y = location.Y;
            Width = size.Width;
            Height = size.Height;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a Rectangle from the specified left, top, right, and bottom edges.
        /// </summary>
        /// <param name="left">The x-coordinate of the left edge.</param>
        /// <param name="top">The y-coordinate of the top edge.</param>
        /// <param name="right">The x-coordinate of the right edge.</param>
        /// <param name="bottom">The y-coordinate of the bottom edge.</param>
        /// <returns>A new Rectangle that spans from (left, top) to (right, bottom).</returns>
        public static Rectangle FromLTRB(int left, int top, int right, int bottom)
        {
            return new Rectangle(left, top, right - left, bottom - top);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Represents a Rectangle that has X, Y, Width, and Height values set to zero.
        /// </summary>
        public static Rectangle Empty => new Rectangle(0, 0, 0, 0);

        #endregion

        #region Properties

        public int Left
        {
            get => X;
            set
            {
                int oldRight = Right;
                X = value;
                Width = oldRight - X;
            }
        }

        public int Right
        {
            get => X + Width;
            set => Width = value - X;
        }

        public int Top
        {
            get => Y;
            set
            {
                int oldBottom = Bottom;
                Y = value;
                Height = oldBottom - Y;
            }
        }

        public int Bottom
        {
            get => Y + Height;
            set => Height = value - Y;
        }

        public bool IsEmpty => (Width <= 0) || (Height <= 0);

        public Point Location
        {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Size Size
        {
            get => new Size(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        #endregion

        #region Conversion to SkiaSharp

        public SKRect ToSKRect()
        {
            return new SKRect(X, Y, X + Width, Y + Height);
        }

        public static Rectangle FromSKRect(SKRect rect)
        {
            int x = (int)rect.Left;
            int y = (int)rect.Top;
            int w = (int)(rect.Right - rect.Left);
            int h = (int)(rect.Bottom - rect.Top);
            return new Rectangle(x, y, w, h);
        }

        #endregion

        #region Equality and Overrides

        public bool Equals(Rectangle other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            return obj is Rectangle rect && Equals(rect);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 31) + X.GetHashCode();
                hash = (hash * 31) + Y.GetHashCode();
                hash = (hash * 31) + Width.GetHashCode();
                hash = (hash * 31) + Height.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"Rectangle [ X={X}, Y={Y}, Width={Width}, Height={Height} ]";
        }

        #endregion
    }
}
