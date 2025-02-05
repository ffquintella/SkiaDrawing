using System;

namespace SkiaDrawing
{
    /// <summary>
    /// Represents the size of a rectangular region.
    /// This struct mimics the functionality of System.Drawing.Size.
    /// </summary>
    public struct Size : IEquatable<Size>
    {
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Initializes a new instance of the Size structure with the specified width and height.
        /// </summary>
        /// <param name="width">The width of the Size.</param>
        /// <param name="height">The height of the Size.</param>
        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Gets a value indicating whether this Size is empty.
        /// </summary>
        public bool IsEmpty => Width == 0 && Height == 0;

        /// <summary>
        /// Returns a string that represents the current Size.
        /// </summary>
        /// <returns>A string representation of the Size.</returns>
        public override string ToString() => $"{{Width={Width}, Height={Height}}}";

        /// <summary>
        /// Determines whether the specified object is equal to the current Size.
        /// </summary>
        public override bool Equals(object obj) => obj is Size size && Equals(size);

        /// <summary>
        /// Determines whether the specified Size is equal to the current Size.
        /// </summary>
        public bool Equals(Size other) => Width == other.Width && Height == other.Height;

        /// <summary>
        /// Returns a hash code for the Size.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                // Multiply width by a prime number and XOR with height.
                return (Width * 397) ^ Height;
            }
        }

        /// <summary>
        /// Determines whether two Size structures are equal.
        /// </summary>
        public static bool operator ==(Size left, Size right) => left.Equals(right);

        /// <summary>
        /// Determines whether two Size structures are not equal.
        /// </summary>
        public static bool operator !=(Size left, Size right) => !left.Equals(right);
    }
}

