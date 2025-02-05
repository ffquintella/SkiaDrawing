using System;
using System.IO;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A simplified class that mimics System.Drawing.Image using SkiaSharp for pixel data.
    /// </summary>
    public class Image : IDisposable
    {
        protected SKBitmap skBitmap;

        protected float horizontalResolution = 96.0f;
        protected float verticalResolution = 96.0f;

        private bool disposed;
        
        public PixelFormat PixelFormat { get; set; }

        #region Constructors

        protected Image()
        {
            // Empty or internal usage.
            PixelFormat = PixelFormat.Format32bppArgb;
        }

        /// <summary>
        /// Constructs an Image from an existing SKBitmap.
        /// </summary>
        public Image(SKBitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));
            skBitmap = bitmap;
        }

        #endregion

        #region Static Creation Methods

        public static Image FromFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            using var fs = File.OpenRead(filename);
            SKBitmap bmp = SKBitmap.Decode(fs);
            if (bmp == null)
                throw new Exception($"Failed to decode image from file: {filename}");
            Image img = new Image(bmp);

            // Could parse metadata for DPI here
            return img;
        }

        public static Image FromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            SKBitmap bmp = SKBitmap.Decode(stream);
            if (bmp == null)
                throw new Exception("Failed to decode image from stream.");
            Image img = new Image(bmp);

            // Could parse metadata for DPI here
            return img;
        }

        #endregion

        #region Properties

        public virtual int Width
        {
            get
            {
                CheckDisposed();
                return skBitmap?.Width ?? 0;
            }
        }

        public virtual int Height
        {
            get
            {
                CheckDisposed();
                return skBitmap?.Height ?? 0;
            }
        }

        public virtual float HorizontalResolution
        {
            get => horizontalResolution;
            set => horizontalResolution = value;
        }

        public virtual float VerticalResolution
        {
            get => verticalResolution;
            set => verticalResolution = value;
        }

        #endregion

        #region Save Methods

        public virtual void Save(string filename, SKEncodedImageFormat format, int quality = 100)
        {
            CheckDisposed();
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            using var data = skBitmap.Encode(format, quality);
            if (data == null)
                throw new Exception("Failed to encode the bitmap.");

            using var fs = File.OpenWrite(filename);
            data.SaveTo(fs);
        }

        public virtual void Save(Stream stream, SKEncodedImageFormat format, int quality = 100)
        {
            CheckDisposed();
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using var data = skBitmap.Encode(format, quality);
            if (data == null)
                throw new Exception("Failed to encode the bitmap.");

            data.SaveTo(stream);
        }

        #endregion

        #region Pixel Format Size

        /// <summary>
        /// Returns the bit depth (bits per pixel) for the specified PixelFormat.
        /// You can expand or modify as needed for additional formats.
        /// </summary>
        public static int GetPixelFormatSize(PixelFormat p)
        {
            switch (p)
            {
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;

                case PixelFormat.Format24bppRgb:
                    return 24;

                case PixelFormat.Format16bppRgb565:
                    return 16;

                case PixelFormat.Format8bppGray:
                    return 8;

                // Add or adjust any additional pixel formats you support:
                // case PixelFormat.Format48bppRgb: return 48; etc.

                default:
                    throw new NotSupportedException($"Unsupported or unknown pixel format {p}.");
            }
        }

        #endregion

        #region Disposal

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                if (skBitmap != null)
                {
                    skBitmap.Dispose();
                    skBitmap = null;
                }
            }
        }

        protected void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(Image));
        }

        #endregion

        #region Utilities

        public virtual SKBitmap ToSKBitmap()
        {
            CheckDisposed();
            return skBitmap;
        }

        public override string ToString()
        {
            if (disposed) return "Image [Disposed]";
            return $"Image [Width={Width}, Height={Height}, DPI={HorizontalResolution}x{VerticalResolution}]";
        }

        #endregion
    }
}
