
using System;
using System.IO;
using SkiaSharp;

namespace SkiaDrawing
{
    public class Bitmap : IDisposable
    {
        private SKBitmap skBitmap;

        #region Constructors

        public Bitmap(int width, int height)
        {
            skBitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
        }

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

        public Bitmap(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            skBitmap = SKBitmap.Decode(stream)
                ?? throw new Exception("Failed to decode bitmap from stream.");
        }

        public Bitmap(SKBitmap bitmap)
        {
            skBitmap = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
        }

        public Bitmap(Bitmap b, Size s)
        {
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            var newInfo = new SKImageInfo(s.Width, s.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var newSkBitmap = new SKBitmap(newInfo);

            bool success = b.ToSKBitmap().ScalePixels(newSkBitmap, SKFilterQuality.High);
            if (!success)
                throw new Exception("Failed to scale the source bitmap to the specified size.");

            skBitmap = newSkBitmap;
        }

        #endregion

        #region Properties

        public int Width => skBitmap?.Width ?? 0;
        public int Height => skBitmap?.Height ?? 0;

        public IntPtr Scan0
        {
            get
            {
                if (skBitmap == null)
                    throw new ObjectDisposedException(nameof(Bitmap));
                return skBitmap.GetPixels();
            }
        }

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

        public SKColor GetPixel(int x, int y)
        {
            if (skBitmap == null)
                throw new ObjectDisposedException(nameof(Bitmap));
            if (x < 0 || x >= skBitmap.Width || y < 0 || y >= skBitmap.Height)
                throw new ArgumentOutOfRangeException();

            return skBitmap.GetPixel(x, y);
        }

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
        public void Save(Stream stream, SKEncodedImageFormat format, int quality = 100)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var data = skBitmap.Encode(format, quality))
            {
                data.SaveTo(stream);
            }
        }

        /// <summary>
        /// Saves the bitmap to the provided MemoryStream in the specified ImageFormat.
        /// </summary>
        /// <param name="s">The MemoryStream to which the bitmap will be written.</param>
        /// <param name="f">The desired ImageFormat (custom enum) for the output.</param>
        public void Save(MemoryStream s, ImageFormat f)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            // Convert our custom ImageFormat enum to SkiaSharp's SKEncodedImageFormat.
            var skEncodedFormat = f.ToSKEncodedImageFormat();

            // Encode the bitmap data with default quality (or you could add an optional parameter for quality).
            using (var data = skBitmap.Encode(skEncodedFormat, 100))
            {
                data.SaveTo(s);
            }
        }

        #endregion

        #region Conversion

        public SKBitmap ToSKBitmap()
        {
            if (skBitmap == null)
                throw new ObjectDisposedException(nameof(Bitmap));
            return skBitmap;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            skBitmap?.Dispose();
            skBitmap = null;
        }

        #endregion

        public override string ToString()
        {
            return $"Bitmap: {Width} x {Height}, Stride: {Stride} bytes";
        }
    }
}
