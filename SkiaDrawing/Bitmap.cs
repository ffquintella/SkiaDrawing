using System;
using System.IO;
using System.Runtime.InteropServices;
using SkiaSharp;

namespace SkiaDrawing
{
    public class Bitmap : IDisposable
    {
        private SKBitmap skBitmap;

        // Internal resolution fields.
        private float horizontalResolution = 96.0f;
        private float verticalResolution   = 96.0f;

        #region Constructors

        public Bitmap(int width, int height)
        {
            skBitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
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
                
                // Here you could parse metadata to set horizontalResolution and verticalResolution if desired
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }
        }

        public Bitmap(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            skBitmap = SKBitmap.Decode(stream);
            if (skBitmap == null)
                throw new Exception("Failed to decode bitmap from stream.");
            
            // Could parse stream metadata for DPI here if needed
        }

        public Bitmap(SKBitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            skBitmap = bitmap;
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

            // Copy resolution from the original
            horizontalResolution = b.horizontalResolution;
            verticalResolution   = b.verticalResolution;
        }

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
        }

        #endregion

        #region Properties

        public int Width
        {
            get
            {
                if (skBitmap == null)
                    throw new ObjectDisposedException(nameof(Bitmap));
                return skBitmap.Width;
            }
        }

        public int Height
        {
            get
            {
                if (skBitmap == null)
                    throw new ObjectDisposedException(nameof(Bitmap));
                return skBitmap.Height;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal resolution, in dots per inch (DPI).
        /// Defaults to 96 if not otherwise specified.
        /// </summary>
        public float HorizontalResolution
        {
            get => horizontalResolution;
            set => horizontalResolution = value;
        }

        /// <summary>
        /// Gets or sets the vertical resolution, in dots per inch (DPI).
        /// Defaults to 96 if not otherwise specified.
        /// </summary>
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

        #region Clone (Crop + Convert)

        /// <summary>
        /// Clones the specified rectangle from this bitmap into a new Bitmap,
        /// using the specified PixelFormat <paramref name="f"/>.
        /// </summary>
        public Bitmap Clone(Rectangle r, PixelFormat f)
        {
            if (skBitmap == null)
                throw new ObjectDisposedException(nameof(Bitmap));

            if (r.X < 0 || r.Y < 0 || r.Width < 0 || r.Height < 0 ||
                r.X + r.Width > Width || r.Y + r.Height > Height)
            {
                throw new ArgumentException("The specified rectangle is out of the bitmap bounds.");
            }

            SKColorType colorType = f.ToSKColorType();

            SKImageInfo newInfo = new SKImageInfo(r.Width, r.Height, colorType, SKAlphaType.Premul);
            SKBitmap newSkBitmap = new SKBitmap(newInfo);

            SKCanvas canvas = new SKCanvas(newSkBitmap);
            SKRect srcRect = new SKRect(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
            SKRect dstRect = new SKRect(0, 0, r.Width, r.Height);

            canvas.DrawBitmap(skBitmap, srcRect, dstRect);
            canvas.Dispose();

            // Create the new Bitmap
            Bitmap newBmp = new Bitmap(newSkBitmap)
            {
                HorizontalResolution = this.HorizontalResolution,
                VerticalResolution = this.VerticalResolution
            };

            return newBmp;
        }

        #endregion

        #region LockBits / UnlockBits

        /// <summary>
        /// Locks the specified rectangular portion of this Bitmap into system memory.
        /// </summary>
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

            BitmapData data = new BitmapData
            {
                Scan0       = rectPtr,
                Stride      = fullStride,
                Width       = r.Width,
                Height      = r.Height,
                PixelFormat = f,
                LockMode    = m
            };

            return data;
        }

        public void UnlockBits(BitmapData bmData)
        {
            if (bmData == null)
                throw new ArgumentNullException(nameof(bmData));
            // No additional copying done here
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

        /// <summary>
        /// Sets the horizontal and vertical resolution (in DPI).
        /// This method mimics System.Drawing.Bitmap.SetResolution(float xDpi, float yDpi).
        /// </summary>
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
