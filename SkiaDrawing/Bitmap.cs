using System;
using System.IO;
using System.Runtime.InteropServices;
using SkiaSharp;

namespace SkiaDrawing
{
    public class Bitmap : Image
    {
        private SKBitmap skBitmap;

        private float horizontalResolution = 96.0f;
        private float verticalResolution   = 96.0f;

        #region Constructors

        public Bitmap(int width, int height)
        {
            skBitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            PixelFormat = PixelFormat.Format32bppArgb;
        }

        public Bitmap(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            FileStream fs = null;
            try
            {
                fs = File.OpenRead(fileName);
                skBitmap = SKBitmap.Decode(fs);
                if (skBitmap == null)
                    throw new Exception("Failed to decode bitmap from file.");
                PixelFormat = PixelFormat.Format32bppArgb;
            }
            finally
            {
                if (fs != null) fs.Dispose();
            }
        }

        public Bitmap(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            skBitmap = SKBitmap.Decode(stream);
            if (skBitmap == null)
                throw new Exception("Failed to decode bitmap from stream.");
            
            PixelFormat = PixelFormat.Format32bppArgb;
        }

        public Bitmap(SKBitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            skBitmap = bitmap;
            PixelFormat = PixelFormat.Format32bppArgb;
        }

        public Bitmap(Bitmap b, Size s)
        {
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            SKImageInfo newInfo = new SKImageInfo(s.Width, s.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            SKBitmap newSkBitmap = new SKBitmap(newInfo);

            bool success = b.ToSKBitmap().ScalePixels(newSkBitmap, SKFilterQuality.High);
            if (!success)
                throw new Exception("Failed to scale the source bitmap to the specified size.");

            skBitmap = newSkBitmap;

            // Copy over resolution from original
            horizontalResolution = b.horizontalResolution;
            verticalResolution   = b.verticalResolution;
            
            PixelFormat = b.PixelFormat;
        }
        
        /// <summary>
        /// Creates a Bitmap of the given width/height in the specified PixelFormat.
        /// In this simplified example, we primarily handle PixelFormat.Format32bppArgb 
        /// by creating a 32-bit RGBA (premultiplied) Skia bitmap.
        /// </summary>
        public Bitmap(int width, int height, PixelFormat format)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("Width and height must be positive.");

            // Example: handle only Format32bppArgb.
            // If you need more formats, add logic or throw for unsupported.
            if (format != PixelFormat.Format32bppArgb)
                throw new NotSupportedException(
                    $"Only Format32bppArgb is supported in this constructor, but got {format}."
                );

            // For Format32bppArgb => Rgba8888
            SKImageInfo info = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            skBitmap = new SKBitmap(info);
            PixelFormat = format;
        }

        /// <summary>
        /// Creates a Bitmap from raw pixel data, using the provided width, height, stride, 
        /// PixelFormat (p), and pointer to the pixel data (scan0).
        /// The raw data is copied into this bitmap’s internal buffer.
        /// </summary>
        public Bitmap(int width, int height, int stride, PixelFormat p, IntPtr scan0)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("Width and height must be positive.");
            if (scan0 == IntPtr.Zero)
                throw new ArgumentNullException(nameof(scan0), "scan0 cannot be IntPtr.Zero.");

            SKColorType skColorType = p.ToSKColorType();
            SKImageInfo info = new SKImageInfo(width, height, skColorType, SKAlphaType.Premul);
            skBitmap = new SKBitmap(info);

            IntPtr destPtr = skBitmap.GetPixels();
            if (destPtr == IntPtr.Zero)
                throw new Exception("Failed to allocate pixels in SKBitmap.");

            unsafe
            {
                byte* srcRow = (byte*)scan0.ToPointer();
                byte* dstRow = (byte*)destPtr.ToPointer();
                int rowBytes = skBitmap.RowBytes;

                for (int y = 0; y < height; y++)
                {
                    int copyBytes = Math.Min(stride, rowBytes);
                    Buffer.MemoryCopy(srcRow, dstRow, rowBytes, copyBytes);
                    srcRow += stride;
                    dstRow += rowBytes;
                }
            }
            
            PixelFormat = p;
        }

        /// <summary>
        /// Creates a new Bitmap from an existing one, scaled to the specified width and height.
        /// </summary>
        public Bitmap(Bitmap b, int width, int height)
        {
            if (b == null)
                throw new ArgumentNullException(nameof(b));
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("Width and height must be positive.");

            // Build a new SKBitmap with the desired dimensions
            SKImageInfo newInfo = new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            SKBitmap newSkBitmap = new SKBitmap(newInfo);

            bool success = b.ToSKBitmap().ScalePixels(newSkBitmap, SKFilterQuality.High);
            if (!success)
                throw new Exception("Failed to scale the source bitmap to the specified size.");

            skBitmap = newSkBitmap;

            // Copy resolution from original
            horizontalResolution = b.horizontalResolution;
            verticalResolution   = b.verticalResolution;
            
            PixelFormat = b.PixelFormat;
        }

        #endregion

        #region Properties

        public new int Width
        {
            get
            {
                if (skBitmap == null)
                    throw new ObjectDisposedException(nameof(Bitmap));
                return skBitmap.Width;
            }
        }

        public new int Height
        {
            get
            {
                if (skBitmap == null)
                    throw new ObjectDisposedException(nameof(Bitmap));
                return skBitmap.Height;
            }
        }

        public float HorizontalResolution
        {
            get => horizontalResolution;
            set => horizontalResolution = value;
        }

        public float VerticalResolution
        {
            get => verticalResolution;
            set => verticalResolution = value;
        }

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

        #region Clone

        /// <summary>
        /// Creates an exact copy of this entire bitmap.
        /// </summary>
        public Bitmap Clone()
        {
            if (skBitmap == null)
                throw new ObjectDisposedException(nameof(Bitmap));

            var info = new SKImageInfo(skBitmap.Width, skBitmap.Height, skBitmap.ColorType, skBitmap.AlphaType);
            SKBitmap newSkBitmap = new SKBitmap(info);

            using (var canvas = new SKCanvas(newSkBitmap))
            {
                canvas.DrawBitmap(skBitmap, 0, 0);
            }

            Bitmap newBmp = new Bitmap(newSkBitmap)
            {
                HorizontalResolution = this.horizontalResolution,
                VerticalResolution   = this.verticalResolution,
                PixelFormat = this.PixelFormat
            };

            return newBmp;
        }

        /// <summary>
        /// Clones a portion of this bitmap into a new one, possibly converting pixel format.
        /// </summary>
        public Bitmap Clone(Rectangle r, PixelFormat f)
        {
            if (skBitmap == null)
                throw new ObjectDisposedException(nameof(Bitmap));

            if (r.X < 0 || r.Y < 0 || r.Width < 0 || r.Height < 0 ||
                r.X + r.Width > Width || r.Y + r.Height > Height)
            {
                throw new ArgumentException("The specified rectangle is out of bounds.");
            }

            SKColorType colorType = f.ToSKColorType();
            SKImageInfo newInfo = new SKImageInfo(r.Width, r.Height, colorType, SKAlphaType.Premul);
            SKBitmap newSkBitmap = new SKBitmap(newInfo);

            using (SKCanvas canvas = new SKCanvas(newSkBitmap))
            {
                var srcRect = new SKRect(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
                var dstRect = new SKRect(0, 0, r.Width, r.Height);
                canvas.DrawBitmap(skBitmap, srcRect, dstRect);
            }

            var newBmp = new Bitmap(newSkBitmap)
            {
                HorizontalResolution = this.horizontalResolution,
                VerticalResolution   = this.verticalResolution,
                PixelFormat = f
            };
            return newBmp;
        }

        #endregion

        #region LockBits / UnlockBits

        public BitmapData LockBits(Rectangle r, ImageLockMode m, PixelFormat f)
        {
            if (skBitmap == null)
                throw new ObjectDisposedException(nameof(Bitmap));

            if (r.X < 0 || r.Y < 0 || r.Width < 0 || r.Height < 0 ||
                r.X + r.Width > Width || r.Y + r.Height > Height)
            {
                throw new ArgumentException("The specified rectangle is out of the bitmap bounds.");
            }

            IntPtr basePtr = skBitmap.GetPixels();
            if (basePtr == IntPtr.Zero)
                throw new Exception("Failed to get bitmap pixel pointer.");

            int fullStride = skBitmap.RowBytes;
            int bytesPerPixel = EstimateBytesPerPixel(f);

            int rowOffset = r.Y * fullStride;
            int colOffset = r.X * bytesPerPixel;
            IntPtr rectPtr = IntPtr.Add(basePtr, rowOffset + colOffset);

            return new BitmapData
            {
                Scan0       = rectPtr,
                Stride      = fullStride,
                Width       = r.Width,
                Height      = r.Height,
                PixelFormat = f,
                LockMode    = m
            };
        }

        public void UnlockBits(BitmapData bmData)
        {
            if (bmData == null)
                throw new ArgumentNullException(nameof(bmData));
            // No copy-back in this simplified approach
        }

        #endregion

        #region Save Methods

        public void Save(string fileName, SKEncodedImageFormat format, int quality = 100)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            SKData data = skBitmap.Encode(format, quality);
            if (data == null)
                throw new Exception("Failed to encode bitmap.");

            FileStream fs = null;
            try
            {
                fs = File.OpenWrite(fileName);
                data.SaveTo(fs);
            }
            finally
            {
                if (fs != null) fs.Dispose();
                data.Dispose();
            }
        }

        public void Save(Stream stream, SKEncodedImageFormat format, int quality = 100)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            SKData data = skBitmap.Encode(format, quality);
            if (data == null)
                throw new Exception("Failed to encode bitmap.");

            data.SaveTo(stream);
            data.Dispose();
        }

        public void Save(MemoryStream s, ImageFormat f)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            var skEncodedFormat = f.ToSKEncodedImageFormat();
            SKData data = skBitmap.Encode(skEncodedFormat, 100);
            if (data == null)
                throw new Exception("Failed to encode bitmap.");

            data.SaveTo(s);
            data.Dispose();
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

        #region DPI Helpers

        public void SetResolution(float horizontal, float vertical)
        {
            if (horizontal <= 0 || vertical <= 0)
                throw new ArgumentOutOfRangeException("Resolution must be positive.");
            horizontalResolution = horizontal;
            verticalResolution   = vertical;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (skBitmap != null)
            {
                skBitmap.Dispose();
                skBitmap = null;
            }
        }

        #endregion

        public override string ToString()
        {
            if (skBitmap == null)
                return "Bitmap: Disposed";

            return $"Bitmap: {Width} x {Height}, Stride: {Stride} bytes, DPI: {horizontalResolution}x{verticalResolution}";
        }

        /// <summary>
        /// Estimate bytes/pixel for the chosen pixel format.
        /// </summary>
        private int EstimateBytesPerPixel(PixelFormat fmt)
        {
            switch (fmt)
            {
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 4;
                case PixelFormat.Format24bppRgb:
                    return 3;
                case PixelFormat.Format16bppRgb565:
                    return 2;
                case PixelFormat.Format8bppGray:
                    return 1;
                default:
                    return 4;
            }
        }
    }
}
