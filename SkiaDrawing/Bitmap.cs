using System;
using System.IO;
using System.Runtime.InteropServices;
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

            FileStream fs = null;
            try
            {
                fs = File.OpenRead(fileName);
                skBitmap = SKBitmap.Decode(fs);
                if (skBitmap == null)
                    throw new Exception("Failed to decode bitmap from file.");
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
                throw new Exception("Failed to scale the source bitmap.");

            skBitmap = newSkBitmap;
        }

        public Bitmap(int width, int height, int stride, SKColorType pixelFormat, IntPtr scan0)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("Width and height must be positive.");
            if (scan0 == IntPtr.Zero)
                throw new ArgumentNullException(nameof(scan0), "scan0 cannot be IntPtr.Zero.");

            SKImageInfo info = new SKImageInfo(width, height, pixelFormat, SKAlphaType.Premul);
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

            return new Bitmap(newSkBitmap);
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
                Scan0 = rectPtr,
                Stride = fullStride,
                Width = r.Width,
                Height = r.Height,
                PixelFormat = f,
                LockMode = m
            };

            return data;
        }

        /// <summary>
        /// Unlocks the specified BitmapData, finalizing any changes.
        /// In this simplified approach, no special copying or resource changes are done.
        /// </summary>
        public void UnlockBits(BitmapData bmData)
        {
            if (bmData == null)
                throw new ArgumentNullException(nameof(bmData));

            // In System.Drawing, UnlockBits might copy data back to the original
            // if the lock was WriteOnly or UserInputBuffer. In this simplified approach,
            // we do not do any additional copying. The SKBitmap memory is still the same.
            // We simply consider the lock concluded.

            // If you need to handle concurrency or partial data, do it here.

            // Nothing else to do in this basic approach.
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

            return $"Bitmap: {Width} x {Height}, Stride: {Stride} bytes";
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
                case PixelFormat.Format8bppGray:
                    return 1;
                case PixelFormat.Format16bppRgb565:
                    return 2;
                default:
                    return 4;
            }
        }
    }
}
