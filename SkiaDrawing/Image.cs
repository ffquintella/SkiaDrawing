using System;
using System.IO;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A simplified class that mimics System.Drawing.Image using SkiaSharp for pixel data.
    /// In System.Drawing, Image is an abstract base class; here it's a concrete class for demonstration.
    /// </summary>
    public class Image : IDisposable
    {
        /// <summary>
        /// The underlying SkiaSharp bitmap holding the image data.
        /// </summary>
        protected SKBitmap skBitmap;

        /// <summary>
        /// Internal fields for DPI.
        /// By default, we assume 96 if not otherwise set or read from metadata.
        /// </summary>
        protected float horizontalResolution = 96.0f;
        protected float verticalResolution = 96.0f;

        private bool disposed;

        #region Constructors

        /// <summary>
        /// Protected constructor for derived classes or internal usage.
        /// </summary>
        protected Image()
        {
            // You could leave it empty or handle default allocation. 
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

        /// <summary>
        /// Creates an Image from a file, decoding the image via SkiaSharp.
        /// </summary>
        public static Image FromFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename));

            using var fs = File.OpenRead(filename);
            SKBitmap bmp = SKBitmap.Decode(fs);
            if (bmp == null)
                throw new Exception($"Failed to decode image from file: {filename}");
            Image img = new Image(bmp);

            // If you want to read metadata to set DPI, do so here (not shown).
            return img;
        }

        /// <summary>
        /// Creates an Image from a stream, decoding via SkiaSharp.
        /// </summary>
        public static Image FromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            SKBitmap bmp = SKBitmap.Decode(stream);
            if (bmp == null)
                throw new Exception("Failed to decode image from stream.");
            Image img = new Image(bmp);
            return img;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the width of this image in pixels.
        /// </summary>
        public virtual int Width
        {
            get
            {
                CheckDisposed();
                return skBitmap?.Width ?? 0;
            }
        }

        /// <summary>
        /// Gets the height of this image in pixels.
        /// </summary>
        public virtual int Height
        {
            get
            {
                CheckDisposed();
                return skBitmap?.Height ?? 0;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal resolution (DPI) of this image.
        /// </summary>
        public virtual float HorizontalResolution
        {
            get => horizontalResolution;
            set => horizontalResolution = value;
        }

        /// <summary>
        /// Gets or sets the vertical resolution (DPI) of this image.
        /// </summary>
        public virtual float VerticalResolution
        {
            get => verticalResolution;
            set => verticalResolution = value;
        }

        #endregion

        #region Save Methods

        /// <summary>
        /// Saves the image to the specified file path in the given SkiaSharp format (PNG, JPEG, etc.).
        /// </summary>
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

        /// <summary>
        /// Saves the image to a stream in the given SkiaSharp format.
        /// </summary>
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

        #region Dispose

        /// <summary>
        /// Disposes the image and its underlying resources.
        /// </summary>
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

        #region Example Additional Methods

        /// <summary>
        /// Returns the underlying SkiaSharp SKBitmap (if needed for advanced usage).
        /// </summary>
        public virtual SKBitmap ToSKBitmap()
        {
            CheckDisposed();
            return skBitmap;
        }

        /// <summary>
        /// A simple string representation.
        /// </summary>
        public override string ToString()
        {
            if (disposed) return "Image [Disposed]";
            return $"Image [Width={Width}, Height={Height}, DPI={HorizontalResolution}x{VerticalResolution}]";
        }

        #endregion
    }
}
