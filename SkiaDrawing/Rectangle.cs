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

        #region Static Properties

        /// <summary>
        /// Represents a Rectangle that has X, Y, Width, and Height values set to zero.
        /// </summary>
        public static Rectangle Empty => new Rectangle(0, 0, 0, 0);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the x-coordinate of the left edge of this Rectangle.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the x-coordinate of the right edge of this Rectangle.
        /// </summary>
        public int Right
        {
            get => X + Width;
            set => Width = value - X;
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the top edge of this Rectangle.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the y-coordinate of the bottom edge of this Rectangle.
        /// </summary>
        public int Bottom
        {
            get => Y + Height;
            set => Height = value - Y;
        }

        /// <summary>
        /// Gets a value indicating whether this Rectangle has Width and Height of 0.
        /// </summary>
        public bool IsEmpty => (Width <= 0) || (Height <= 0);

        /// <summary>
        /// Gets or sets the location of this Rectangle (X and Y).
        /// </summary>
        public Point Location
        {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the size of this Rectangle (Width and Height).
        /// </summary>
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

        #region Methods

        /// <summary>
        /// Determines whether the specified point is contained within this Rectangle.
        /// </summary>
        public bool Contains(int x, int y)
        {
            return (x >= X) && (x < X + Width) && (y >= Y) && (y < Y + Height);
        }

        /// <summary>
        /// Determines whether the specified point is contained within this Rectangle.
        /// </summary>
        public bool Contains(Point pt) => Contains(pt.X, pt.Y);

        /// <summary>
        /// Determines whether the specified Rectangle is contained within this Rectangle.
        /// </summary>
        public bool Contains(Rectangle rect)
        {
            return (rect.X >= X) &&
                   (rect.Right <= Right) &&
                   (rect.Y >= Y) &&
                   (rect.Bottom <= Bottom);
        }

        /// <summary>
        /// Inflates this Rectangle by the specified amount.
        /// </summary>
        public void Inflate(int x, int y)
        {
            X -= x;
            Y -= y;
            Width += 2 * x;
            Height += 2 * y;
        }

        /// <summary>
        /// Inflates this Rectangle by the specified Size.
        /// </summary>
        public void Inflate(Size size) => Inflate(size.Width, size.Height);

        /// <summary>
        /// Creates and returns an inflated copy of the specified Rectangle.
        /// </summary>
        public static Rectangle Inflate(Rectangle rect, int x, int y)
        {
            var r = rect;
            r.Inflate(x, y);
            return r;
        }

        /// <summary>
        /// Offsets the location of this Rectangle by the specified amount.
        /// </summary>
        public void Offset(int x, int y)
        {
            X += x;
            Y += y;
        }

        /// <summary>
        /// Offsets the location of this Rectangle by the specified Point.
        /// </summary>
        public void Offset(Point pos) => Offset(pos.X, pos.Y);

        /// <summary>
        /// Determines whether this Rectangle intersects with another.
        /// </summary>
        public bool IntersectsWith(Rectangle rect)
        {
            return (rect.X < X + Width) &&
                   (X < (rect.X + rect.Width)) &&
                   (rect.Y < Y + Height) &&
                   (Y < (rect.Y + rect.Height));
        }

        /// <summary>
        /// Returns the Rectangle that represents the intersection of this and another Rectangle.
        /// If there is no intersection, Rectangle.Empty is returned.
        /// </summary>
        public Rectangle Intersection(Rectangle rect)
        {
            if (!IntersectsWith(rect))
                return Empty;

            int x1 = Math.Max(X, rect.X);
            int y1 = Math.Max(Y, rect.Y);
            int x2 = Math.Min(Right, rect.Right);
            int y2 = Math.Min(Bottom, rect.Bottom);
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Returns the intersection of two Rectangles.
        /// </summary>
        public static Rectangle Intersect(Rectangle a, Rectangle b) => a.Intersection(b);

        /// <summary>
        /// Returns a new Rectangle that exactly contains the two input Rectangles (their union).
        /// If either Rectangle is empty, the union is the non-empty Rectangle.
        /// </summary>
        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            int x1 = Math.Min(a.X, b.X);
            int y1 = Math.Min(a.Y, b.Y);
            int x2 = Math.Max(a.Right, b.Right);
            int y2 = Math.Max(a.Bottom, b.Bottom);
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        #endregion

        #region Conversion to SkiaSharp

        /// <summary>
        /// Converts this Rectangle (integer-based) to an SKRect (float-based).
        /// </summary>
        public SKRect ToSKRect()
        {
            return new SKRect(X, Y, X + Width, Y + Height);
        }

        /// <summary>
        /// Creates a Rectangle from an SKRect by truncating float values to int.
        /// </summary>
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
                // Typical combination of fields for a hash code
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
