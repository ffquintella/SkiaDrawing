using System;
using SkiaSharp;

namespace SkiaDrawing
{

        /// <summary>
    /// Mimics System.Drawing.BitmapData using SkiaSharp as the underlying image library.
    /// This class holds data such as the pointer to the pixel buffer (Scan0), stride (bytes per row),
    /// width, height, and pixel format.
    /// </summary>
    public class BitmapData : IDisposable
    {
        /// <summary>
        /// Gets a pointer to the beginning of the pixel data.
        /// </summary>
        public IntPtr Scan0 { get; private set; }

        /// <summary>
        /// Gets the stride (number of bytes per row).
        /// </summary>
        public int Stride { get; private set; }

        /// <summary>
        /// Gets the width (in pixels) of the locked region.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height (in pixels) of the locked region.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the pixel format of the image data.
        /// Here we use the SkiaSharp SKColorType to represent the pixel format.
        /// </summary>
        public SKColorType PixelFormat { get; private set; }

        // Optional: keep a reference to the SKBitmap if you wish to implement unlocking.
        private SKBitmap skBitmap;

        /// <summary>
        /// Creates a new BitmapData instance by "locking" the given SKBitmap.
        /// </summary>
        /// <param name="bitmap">The SKBitmap whose pixel data is to be accessed.</param>
        public BitmapData(SKBitmap bitmap)
        {
            skBitmap = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
            Width = bitmap.Width;
            Height = bitmap.Height;
            Stride = bitmap.RowBytes;
            PixelFormat = bitmap.ColorType;
            Scan0 = bitmap.GetPixels(); // Returns a pointer to the pixel data.
        }

        /// <summary>
        /// Releases any resources associated with the BitmapData.
        /// In this simple implementation, no explicit unlocking is performed.
        /// </summary>
        public void Dispose()
        {
            // In a more advanced implementation, you might need to signal that the
            // pixel data is no longer in use. Here we simply clear our reference.
            skBitmap = null;
        }
    }
    
}



