using System;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A brush that paints an area with a solid color,
    /// mimicking System.Drawing.SolidBrush using SkiaSharp.
    /// </summary>
    public sealed class SolidBrush : Brush, IDisposable
    {
        private bool disposed;

        /// <summary>
        /// Gets or sets the color of this SolidBrush.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Initializes a new instance of the SolidBrush class with the specified color.
        /// </summary>
        public SolidBrush(Color color): base(color)
        {
            Color = color;
        }

        /// <summary>
        /// Creates or returns a SkiaSharp SKPaint for filling with this brush's color.
        /// Typically used internally by Graphics methods.
        /// </summary>
        public SKPaint ToSKPaint()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SolidBrush));

            return new SKPaint
            {
                Color = Color.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };
        }

        /// <summary>
        /// Releases all resources used by the SolidBrush.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                // In GDI+, a SolidBrush might hold GDI objects. Here, we just set a flag.
                // If you had more resources, free them here.
                disposed = true;
            }
        }

        /// <summary>
        /// Checks if this SolidBrush is disposed.
        /// </summary>
        private void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SolidBrush));
        }

        /// <summary>
        /// Returns a string representation of this brush (e.g. the color).
        /// </summary>
        public override string ToString()
        {
            return $"SolidBrush [Color={Color}]";
        }
    }
}
