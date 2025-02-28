using Xunit;
using SkiaDrawing;
using System;
//using System.Drawing;

namespace SkiaDrawing.Tests
{
    public class GraphicsTests
    {
        [Fact]
        public void FromImage_WithValidBitmap_ReturnsGraphics()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            Assert.NotNull(graphics);
        }

        [Fact]
        public void FromImage_WithNullBitmap_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Graphics.FromImage(null));
        }

        [Fact]
        public void Clear_WithValidColor_ClearsCanvas()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Red);
            // Assuming a method to verify the canvas is cleared with the specified color
        }

        [Fact]
        public void DrawLine_WithValidParameters_DrawsLine()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Black, 2);
            graphics.DrawLine(pen, 10, 10, 90, 90);
            // Assuming a method to verify the line is drawn
        }

        [Fact]
        public void DrawLine_WithNullPen_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            Assert.Throws<ArgumentNullException>(() => graphics.DrawLine(null, 10, 10, 90, 90));
        }

        [Fact]
        public void DrawRectangle_WithValidParameters_DrawsRectangle()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Black, 2);
            graphics.DrawRectangle(pen, 10, 10, 80, 80);
            // Assuming a method to verify the rectangle is drawn
        }

        [Fact]
        public void DrawRectangle_WithNullPen_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            Assert.Throws<ArgumentNullException>(() => graphics.DrawRectangle(null, 10, 10, 80, 80));
        }

        [Fact]
        public void FillRectangle_WithValidParameters_FillsRectangle()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var brush = new SolidBrush(Color.Blue);
            graphics.FillRectangle(brush, 10, 10, 80, 80);
            // Assuming a method to verify the rectangle is filled
        }

        [Fact]
        public void FillRectangle_WithNullBrush_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            Assert.Throws<ArgumentNullException>(() => graphics.FillRectangle(null, 10, 10, 80, 80));
        }

        [Fact]
        public void DrawEllipse_WithValidParameters_DrawsEllipse()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Black, 2);
            graphics.DrawEllipse(pen, 10, 10, 80, 80);
            // Assuming a method to verify the ellipse is drawn
        }

        [Fact]
        public void DrawEllipse_WithNullPen_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            Assert.Throws<ArgumentNullException>(() => graphics.DrawEllipse(null, 10, 10, 80, 80));
        }

        [Fact]
        public void FillEllipse_WithValidParameters_FillsEllipse()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var brush = new SolidBrush(Color.Blue);
            graphics.FillEllipse(brush, 10, 10, 80, 80);
            // Assuming a method to verify the ellipse is filled
        }

        [Fact]
        public void FillEllipse_WithNullBrush_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            Assert.Throws<ArgumentNullException>(() => graphics.FillEllipse(null, 10, 10, 80, 80));
        }

        [Fact]
        public void DrawImage_WithValidParameters_DrawsImage()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var image = new Bitmap(50, 50);
            graphics.DrawImage(image, 25, 25);
            // Assuming a method to verify the image is drawn
        }

        [Fact]
        public void DrawImage_WithNullImage_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            Assert.Throws<ArgumentNullException>(() => graphics.DrawImage(null, 25, 25));
        }

        [Fact]
        public void DrawString_WithValidParameters_DrawsString()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var font = new Font("Arial", 12);
            var brush = new SolidBrush(Color.Black);
            graphics.DrawString("Hello", font, brush, 10, 10);
            // Assuming a method to verify the string is drawn
        }

        [Fact]
        public void DrawString_WithNullText_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var font = new Font("Arial", 12);
            var brush = new SolidBrush(Color.Black);
            Assert.Throws<ArgumentNullException>(() => graphics.DrawString(null, font, brush, 10, 10));
        }

        [Fact]
        public void DrawString_WithNullFont_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var brush = new SolidBrush(Color.Black);
            Assert.Throws<ArgumentNullException>(() => graphics.DrawString("Hello", null, brush, 10, 10));
        }

        [Fact]
        public void DrawString_WithNullBrush_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var font = new Font("Arial", 12);
            Assert.Throws<ArgumentNullException>(() => graphics.DrawString("Hello", font, null, 10, 10));
        }

        [Fact]
        public void MeasureString_WithValidParameters_ReturnsCorrectSize()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var font = new Font("Arial", 12);
            var size = graphics.MeasureString("Hello", font);
            Assert.True(size.Width > 0);
            Assert.True(size.Height > 0);
        }

        [Fact]
        public void MeasureString_WithNullText_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var font = new Font("Arial", 12);
            Assert.Throws<ArgumentNullException>(() => graphics.MeasureString(null, font));
        }

        [Fact]
        public void MeasureString_WithNullFont_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            Assert.Throws<ArgumentNullException>(() => graphics.MeasureString("Hello", null));
        }

        [Fact]
        public void TranslateTransform_WithValidParameters_TranslatesCanvas()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            graphics.TranslateTransform(10, 10);
            // Assuming a method to verify the canvas is translated
        }

        [Fact]
        public void ScaleTransform_WithValidParameters_ScalesCanvas()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            graphics.ScaleTransform(2, 2);
            // Assuming a method to verify the canvas is scaled
        }

        [Fact]
        public void RotateTransform_WithValidParameters_RotatesCanvas()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            graphics.RotateTransform(45);
            // Assuming a method to verify the canvas is rotated
        }

        [Fact]
        public void ResetTransform_ResetsCanvasTransform()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            graphics.TranslateTransform(10, 10);
            graphics.ResetTransform();
            // Assuming a method to verify the canvas transform is reset
        }

        [Fact]
        public void VisibleClipBounds_ReturnsCorrectBounds()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var bounds = graphics.VisibleClipBounds;
            Assert.Equal(0, bounds.Left);
            Assert.Equal(0, bounds.Top);
            Assert.Equal(100, bounds.Width);
            Assert.Equal(100, bounds.Height);
        }

        [Fact]
        public void Dispose_DisposesCanvas()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            graphics.Dispose();
            Assert.Throws<ObjectDisposedException>(() => graphics.Clear(Color.Red));
        }
        
        [Fact]
        public void DrawImage_WithValidBitmapAndPoint_DrawsImage()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var image = new Bitmap(50, 50);
            var point = new Point(25, 25);
            graphics.DrawImage(image, point);
            // Assuming a method to verify the image is drawn at the correct location
        }

        [Fact]
        public void DrawImage_WithNullGraphics_ThrowsArgumentNullException()
        {
            var image = new Bitmap(50, 50);
            var point = new Point(25, 25);
            Assert.Throws<ArgumentNullException>(() => GraphicsExtensions.DrawImage(null, image, point));
        }

        [Fact]
        public void DrawImage_WithNullBitmap_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var point = new Point(25, 25);
            Assert.Throws<ArgumentNullException>(() => graphics.DrawImage(null, point));
        }

        [Fact]
        public void DrawImage_WithValidBitmapAndPointF_DrawsImage()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var image = new Bitmap(50, 50);
            var pointF = new PointF(25.5f, 25.5f);
            graphics.DrawImage(image, pointF);
            // Assuming a method to verify the image is drawn at the correct location
        }

        [Fact]
        public void DrawImage_WithValidBitmapAndRectangles_DrawsImage()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var image = new Bitmap(50, 50);
            var destRect = new Rectangle(10, 10, 80, 80);
            var srcRect = new Rectangle(0, 0, 50, 50);
            graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            // Assuming a method to verify the image is drawn correctly
        }

        [Fact]
        public void DrawImage_WithNullBitmapInRectangles_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var destRect = new Rectangle(10, 10, 80, 80);
            var srcRect = new Rectangle(0, 0, 50, 50);
            Assert.Throws<ArgumentNullException>(() => graphics.DrawImage(null, destRect, srcRect, GraphicsUnit.Pixel));
        }

        [Fact]
        public void DrawImage_WithValidBitmapAndDestRect_DrawsImage()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var image = new Bitmap(50, 50);
            var destRect = new Rectangle(10, 10, 80, 80);
            graphics.DrawImage(image, destRect);
            // Assuming a method to verify the image is drawn correctly
        }

        [Fact]
        public void DrawImage_WithValidBitmapAndCoordinates_DrawsImage()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var image = new Bitmap(50, 50);
            graphics.DrawImage(image, 10, 10, 80, 80);
            // Assuming a method to verify the image is drawn correctly
        }

        [Fact]
        public void DrawImage_WithValidBitmapAndAttributes_DrawsImage()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var image = new Bitmap(50, 50);
            var destRect = new Rectangle(10, 10, 80, 80);
            var srcRect = new Rectangle(0, 0, 50, 50);
            var attributes = new ImageAttributes();
            graphics.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, attributes);
            // Assuming a method to verify the image is drawn correctly
        }

        [Fact]
        public void DrawImage_WithNullBitmapAndAttributes_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var destRect = new Rectangle(10, 10, 80, 80);
            var srcRect = new Rectangle(0, 0, 50, 50);
            var attributes = new ImageAttributes();
            Assert.Throws<ArgumentNullException>(() => graphics.DrawImage(null, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, attributes));
        }

        [Fact]
        public void FillRectangle_WithValidLinearGradientBrush_FillsRectangle()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var brush = new LinearGradientBrush(new Rectangle(0, 0, 100, 100), Color.Red, Color.Blue, 45);
            var rect = new Rectangle(10, 10, 80, 80);
            graphics.FillRectangle(brush, rect);
            // Assuming a method to verify the rectangle is filled correctly
        }

        [Fact]
        public void FillRectangle_WithNullLinearGradientBrush_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var rect = new Rectangle(10, 10, 80, 80);
            Assert.Throws<ArgumentNullException>(() => graphics.FillRectangle((LinearGradientBrush)null, rect));
        }

        [Fact]
        public void FillRectangle_WithValidSolidBrush_FillsRectangle()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var brush = new SolidBrush(Color.Blue);
            var rect = new Rectangle(10, 10, 80, 80);
            graphics.FillRectangle(brush, rect);
            // Assuming a method to verify the rectangle is filled correctly
        }

        [Fact]
        public void FillRectangle_WithNullSolidBrush_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var rect = new Rectangle(10, 10, 80, 80);
            Assert.Throws<ArgumentNullException>(() => graphics.FillRectangle((SolidBrush)null, rect));
        }
        

        [Fact]
        public void CopyFromScreen_WithInvalidCopyPixelOperation_ThrowsNotImplementedException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var size = new Size(50, 50);
            Assert.Throws<NotImplementedException>(() => graphics.CopyFromScreen(0, 0, 0, 0, size, (CopyPixelOperation)999));
        }

        [Fact]
        public void DrawLines_WithValidPenAndPoints_DrawsLines()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Black, 2);
            var points = new Point[] { new Point(10, 10), new Point(20, 20), new Point(30, 10) };
            graphics.DrawLines(pen, points);
            // Assuming a method to verify the lines are drawn correctly
        }

        [Fact]
        public void DrawLines_WithNullPen_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var points = new Point[] { new Point(10, 10), new Point(20, 20), new Point(30, 10) };
            Assert.Throws<ArgumentNullException>(() => graphics.DrawLines(null, points));
        }

        [Fact]
        public void DrawLines_WithNullPoints_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Black, 2);
            Assert.Throws<ArgumentNullException>(() => graphics.DrawLines(pen, null));
        }


        [Fact]
        public void DrawString_WithNullFormat_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var font = new Font("Arial", 12);
            var brush = new SolidBrush(Color.Black);
            var point = new PointF(10, 10);
            Assert.Throws<ArgumentNullException>(() => graphics.DrawString("Hello", font, brush, point, null));
        }

        [Fact]
        public void DrawRectangle_WithValidPen_DrawsRectangle()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Black, 2);
            var rect = new Rectangle(10, 10, 80, 80);
            graphics.DrawRectangle(pen, rect);
            // Assuming a method to verify the rectangle is drawn correctly
        }
        

        [Fact]
        public void FillRectangle_WithValidSolidBrushAndRectangleF_FillsRectangle()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var brush = new SolidBrush(Color.Blue);
            var rect = new RectangleF(10, 10, 80, 80);
            graphics.FillRectangle(brush, rect);
            // Assuming a method to verify the rectangle is filled correctly
        }

        [Fact]
        public void FillRectangle_WithNullSolidBrushAndRectangleF_ThrowsArgumentNullException()
        {
            var bitmap = new Bitmap(100, 100);
            var graphics = Graphics.FromImage(bitmap);
            var rect = new RectangleF(10, 10, 80, 80);
            Assert.Throws<ArgumentNullException>(() => graphics.FillRectangle((SolidBrush)null, rect));
        }
    }
}