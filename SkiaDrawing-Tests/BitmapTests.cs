using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Xunit;
using SkiaSharp;
using SkiaDrawing;

namespace SkiaDrawing.Tests
{
    public class BitmapTests
    {
        string pxlPng = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==";

        [Fact]
        public void Constructor_WithWidthAndHeight_CreatesBitmap()
        {
            var bitmap = new Bitmap(100, 200);
            Assert.Equal(100, bitmap.Width);
            Assert.Equal(200, bitmap.Height);
        }

        [Fact]
        public void Constructor_WithFileName_CreatesBitmap()
        {
            
            var pixelPngData = Convert.FromBase64String(pxlPng);
            
            string assemblyRunDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fileName = Path.Combine(assemblyRunDir, "test.png");
            File.WriteAllBytes(fileName, pixelPngData);
            var bitmap = new Bitmap(fileName);
            Assert.NotNull(bitmap);
            File.Delete(fileName);
        }

        [Fact]
        public void Constructor_WithStream_CreatesBitmap()
        {
            var pixelPngData = Convert.FromBase64String(pxlPng);
            
            using (var stream = new MemoryStream(pixelPngData))
            {
                var bitmap = new Bitmap(stream);
                Assert.NotNull(bitmap);
            }
        }

        [Fact]
        public void Constructor_WithSKBitmap_CreatesBitmap()
        {
            var skBitmap = new SKBitmap(100, 200);
            var bitmap = new Bitmap(skBitmap);
            Assert.Equal(100, bitmap.Width);
            Assert.Equal(200, bitmap.Height);
        }

        [Fact]
        public void Constructor_WithBitmapAndSize_CreatesScaledBitmap()
        {
            var originalBitmap = new Bitmap(100, 200);
            var newSize = new Size(50, 100);
            var scaledBitmap = new Bitmap(originalBitmap, newSize);
            Assert.Equal(50, scaledBitmap.Width);
            Assert.Equal(100, scaledBitmap.Height);
        }

        [Fact]
        public void Constructor_WithWidthHeightAndPixelFormat_CreatesBitmap()
        {
            var bitmap = new Bitmap(100, 200, PixelFormat.Format32bppArgb);
            Assert.Equal(100, bitmap.Width);
            Assert.Equal(200, bitmap.Height);
        }

        [Fact]
        public void Constructor_WithRawPixelData_CreatesBitmap()
        {
            var width = 100;
            var height = 200;
            var stride = width * 4;
            var pixelData = new byte[width * height * 4];
            var handle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
            var bitmap = new Bitmap(width, height, stride, PixelFormat.Format32bppArgb, handle.AddrOfPinnedObject());
            Assert.Equal(100, bitmap.Width);
            Assert.Equal(200, bitmap.Height);
            handle.Free();
        }

        [Fact]
        public void GetPixel_ReturnsCorrectColor()
        {
            var bitmap = new Bitmap(100, 200);
            var color = new SKColor(255, 0, 0, 255);
            bitmap.SetPixel(10, 10, color);
            var retrievedColor = bitmap.GetPixel(10, 10);
            Assert.Equal(color, retrievedColor);
        }

        [Fact]
        public void SetPixel_SetsCorrectColor()
        {
            var bitmap = new Bitmap(100, 200);
            var color = new SKColor(255, 0, 0, 255);
            bitmap.SetPixel(10, 10, color);
            var retrievedColor = bitmap.GetPixel(10, 10);
            Assert.Equal(color, retrievedColor);
        }

        [Fact]
        public void Clone_CreatesExactCopy()
        {
            var bitmap = new Bitmap(100, 200);
            var clone = bitmap.Clone();
            Assert.Equal(bitmap.Width, clone.Width);
            Assert.Equal(bitmap.Height, clone.Height);
        }

        [Fact]
        public void Clone_WithRectangleAndPixelFormat_CreatesPartialCopy()
        {
            var bitmap = new Bitmap(100, 200);
            var rect = new Rectangle(10, 10, 50, 50);
            var clone = bitmap.Clone(rect, PixelFormat.Format32bppArgb);
            Assert.Equal(50, clone.Width);
            Assert.Equal(50, clone.Height);
        }

        [Fact]
        public void Save_WithFileName_SavesBitmap()
        {
            var bitmap = new Bitmap(100, 200);
            var fileName = "test.png";
            bitmap.Save(fileName);
            Assert.True(File.Exists(fileName));
            File.Delete(fileName);
        }

        [Fact]
        public void Save_WithStream_SavesBitmap()
        {
            var bitmap = new Bitmap(100, 200);
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, SKEncodedImageFormat.Png);
                Assert.True(stream.Length > 0);
            }
        }

        [Fact]
        public void SetResolution_ChangesResolution()
        {
            var bitmap = new Bitmap(100, 200);
            bitmap.SetResolution(300, 300);
            Assert.Equal(300, bitmap.HorizontalResolution);
            Assert.Equal(300, bitmap.VerticalResolution);
        }

        [Fact]
        public void Dispose_ReleasesResources()
        {
            var bitmap = new Bitmap(100, 200);
            bitmap.Dispose();
            Assert.Throws<ObjectDisposedException>(() => { var width = bitmap.Width; });
        }
    }
}