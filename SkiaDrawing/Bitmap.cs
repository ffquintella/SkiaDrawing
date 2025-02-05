using System;
using System.IO;
using SkiaSharp;

namespace SkiaDrawing
{
 
    /// <summary>
    /// A simple Bitmap class that mimics basic functionality of System.Drawing.Bitmap,
    /// implemented using SkiaSharp.
    /// </summary>
    public class Bitmap : IDisposable
    {
        private SKBitmap skBitmap;

        #region Constructors

        /// <summary>
        /// Creates a new Bitmap with the specified width and height.
        /// The pixel format is SKColorType.Rgba8888 with premultiplied alpha.
        /// </summary>
        public Bitmap(int width, int height)
        {
            skBitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        }

        /// <summary>
        /// Loads a Bitmap from a file.
        /// </summary>
        /// <param name="fileName">The file path of the image.</param>
        public Bitmap(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            using (var stream = File.OpenRead(fileName))
            {
                skBitmap = SKBitmap.Decode(stream)
                    ?? throw new Exception("Failed to decode bitmap from file.");
            }
        }

        /// <summary>
        /// Loads a Bitmap from a stream.
        /// </summary>
        /// <param name="stream">A stream containing the encoded image data.</param>
        public Bitmap(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            skBitmap = SKBitmap.Decode(stream)
                ?? throw new Exception("Failed to decode bitmap from stream.");
        }

        /// <summary>
        /// Creates a Bitmap from an existing SKBitmap.
        /// </summary>
        /// <param name="bitmap">The SKBitmap instance to wrap.</param>
        public Bitmap(SKBitmap bitmap)
        {
            skBitmap = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
        }
        
        /// <summary>
        /// Creates a new Bitmap from an existing Bitmap 'b', scaled to the specified Size 's'.
        /// </summary>
        /// <param name="b">The source Bitmap.</param>
        /// <param name="s">The desired Size for the new Bitmap.</param>
        public Bitmap(Bitmap b, Size s)
        {
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            // Prepare a new SKBitmap with the desired size and default format.
            var newInfo = new SKImageInfo(s.Width, s.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var newSkBitmap = new SKBitmap(newInfo);

            // Scale the pixels from the original bitmap into the new one.
            // SKFilterQuality can be changed to Low, Medium, or High as desired.
            bool success = b.ToSKBitmap().ScalePixels(newSkBitmap, SKFilterQuality.High);
            if (!success)
                throw new Exception("Failed to scale the source bitmap to the specified size.");

            skBitmap = newSkBitmap;
        }
        

        #endregion

        #region Properties

        /// <summary>
        /// Gets the width of the bitmap.
        /// </summary>
        public int Width => skBitmap?.Width ?? 0;

        /// <summary>
        /// Gets the height of the bitmap.
        /// </summary>
        public int Height => skBitmap?.Height ?? 0;

        /// <summary>
        /// Mimics System.Drawing.Bitmap.Scan0 by returning a pointer to the beginning of the pixel data.
        /// </summary>
        public IntPtr Scan0
        {
            get
            {
                if (skBitmap == null)
                    throw new ObjectDisposedException(nameof(Bitmap));
                return skBitmap.GetPixels();  // Returns a pointer to the pixel data.
            }
        }

        /// <summary>
        /// Gets the stride (number of bytes per row) of the bitmap.
        /// This property mimics System.Drawing.Bitmap.Stride.
        /// </summary>
        public int Stride
        {
            get
            {
                if (skBitmap == null)
                    throw new ObjectDisposedException(nameof(Bitmap));
                return skBitmap.RowBytes;
            }
        }

        #endregion

        #region Pixel Access

        /// <summary>
        /// Gets the color of the pixel at the specified coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <returns>An SKColor representing the pixel color.</returns>
        public SKColor GetPixel(int x, int y)
        {
            if (skBitmap == null)
                throw new ObjectDisposedException(nameof(Bitmap));

            if (x < 0 || x >= skBitmap.Width || y < 0 || y >= skBitmap.Height)
                throw new ArgumentOutOfRangeException();

            return skBitmap.GetPixel(x, y);
        }

        /// <summary>
        /// Sets the color of the pixel at the specified coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="color">The SKColor to set.</param>
        public void SetPixel(int x, int y, SKColor color)
        {
            if (skBitmap == null)
                throw new ObjectDisposedException(nameof(Bitmap));

            if (x < 0 || x >= skBitmap.Width || y < 0 || y >= skBitmap.Height)
                throw new ArgumentOutOfRangeException();

            skBitmap.SetPixel(x, y, color);
        }

        #endregion

        #region Save Methods

        /// <summary>
        /// Saves the bitmap to a file in the specified encoded image format.
        /// </summary>
        /// <param name="fileName">The destination file name.</param>
        /// <param name="format">The encoded image format (e.g. SKEncodedImageFormat.Png).</param>
        /// <param name="quality">The quality (0-100) for encoding.</param>
        public void Save(string fileName, SKEncodedImageFormat format, int quality = 100)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            using (var data = skBitmap.Encode(format, quality))
            using (var stream = File.OpenWrite(fileName))
            {
                data.SaveTo(stream);
            }
        }

        /// <summary>
        /// Saves the bitmap to a stream in the specified encoded image format.
        /// </summary>
        /// <param name="stream">The destination stream.</param>
        /// <param name="format">The encoded image format (e.g. SKEncodedImageFormat.Png).</param>
        /// <param name="quality">The quality (0-100) for encoding.</param>
        public void Save(Stream stream, SKEncodedImageFormat format, int quality = 100)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var data = skBitmap.Encode(format, quality))
            {
                data.SaveTo(stream);
            }
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Returns the underlying SKBitmap.
        /// </summary>
        public SKBitmap ToSKBitmap()
        {
            if (skBitmap == null)
                throw new ObjectDisposedException(nameof(Bitmap));
            return skBitmap;
        }

        #endregion

        #region IDisposable Support

        /// <summary>
        /// Disposes the bitmap and releases associated resources.
        /// </summary>
        public void Dispose()
        {
            skBitmap?.Dispose();
            skBitmap = null;
        }

        #endregion

        /// <summary>
        /// Returns a string that represents the current Bitmap.
        /// </summary>
        public override string ToString()
        {
            return $"Bitmap: {Width} x {Height}, Stride: {Stride} bytes";
        }
    }
}