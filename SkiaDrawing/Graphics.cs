using System;
using SkiaSharp;

namespace SkiaDrawing
{
    public class Graphics : IDisposable
    {
        private SKCanvas canvas;
        private bool disposed;

        // Example InterpolationMode property and other members...
        private InterpolationMode interpolationMode = InterpolationMode.Default;
        public InterpolationMode InterpolationMode
        {
            get => interpolationMode;
            set => interpolationMode = value;
        }

        /// <summary>
        /// Private constructor. Use FromImage(Bitmap) to create a Graphics for a given Bitmap.
        /// </summary>
        private Graphics(SKCanvas skCanvas)
        {
            canvas = skCanvas ?? throw new ArgumentNullException(nameof(skCanvas));
        }

        /// <summary>
        /// Creates a new Graphics object for drawing on the specified Bitmap.
        /// Similar to System.Drawing.Graphics.FromImage(Image).
        /// </summary>
        public static Graphics FromImage(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            SKCanvas skCanvas = new SKCanvas(bitmap.ToSKBitmap());
            return new Graphics(skCanvas);
        }

        #region Clear and Drawing Methods

        public void Clear(Color c)
        {
            CheckDisposed();
            canvas.Clear(c.ToSKColor());
        }

        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            if (pen == null) throw new ArgumentNullException(nameof(pen));
            CheckDisposed();

            SKPaint paint = new SKPaint
            {
                Color = pen.Color.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = pen.Width,
                IsAntialias = true
            };

            canvas.DrawLine(x1, y1, x2, y2, paint);
            paint.Dispose();
        }

        public void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            if (pen == null) throw new ArgumentNullException(nameof(pen));
            CheckDisposed();

            SKPaint paint = new SKPaint
            {
                Color = pen.Color.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = pen.Width,
                IsAntialias = true
            };

            canvas.DrawRect(x, y, width, height, paint);
            paint.Dispose();
        }

        public void FillRectangle(Brush brush, float x, float y, float width, float height)
        {
            if (brush == null) throw new ArgumentNullException(nameof(brush));
            CheckDisposed();

            SKPaint paint = new SKPaint
            {
                Color = brush.Color.ToSKColor(), // Brush must have a Color property
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            canvas.DrawRect(x, y, width, height, paint);
            paint.Dispose();
        }

        public void DrawEllipse(Pen pen, float x, float y, float width, float height)
        {
            if (pen == null) throw new ArgumentNullException(nameof(pen));
            CheckDisposed();

            SKPaint paint = new SKPaint
            {
                Color = pen.Color.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeWidth = pen.Width,
                IsAntialias = true
            };

            float cx = x + width / 2f;
            float cy = y + height / 2f;
            float rx = width / 2f;
            float ry = height / 2f;
            canvas.DrawOval(cx, cy, rx, ry, paint);
            paint.Dispose();
        }

        public void FillEllipse(Brush brush, float x, float y, float width, float height)
        {
            if (brush == null) throw new ArgumentNullException(nameof(brush));
            CheckDisposed();

            SKPaint paint = new SKPaint
            {
                Color = brush.Color.ToSKColor(),
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            };

            float cx = x + width / 2f;
            float cy = y + height / 2f;
            float rx = width / 2f;
            float ry = height / 2f;
            canvas.DrawOval(cx, cy, rx, ry, paint);
            paint.Dispose();
        }

        public void DrawImage(Bitmap image, float x, float y)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            CheckDisposed();

            SKPaint paint = new SKPaint
            {
                FilterQuality = InterpolationMode.ToSKFilterQuality(),
                IsAntialias = true
            };

            canvas.DrawBitmap(image.ToSKBitmap(), x, y, paint);
            paint.Dispose();
        }

        /// <summary>
        /// Draws a text string at the specified location using a simple color
        /// and textSize. (Existing version in your code.)
        /// </summary>
        public void DrawString(string text, float x, float y, Color color, float textSize = 16)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            CheckDisposed();

            SKPaint paint = new SKPaint
            {
                Color = color.ToSKColor(),
                TextSize = textSize,
                IsAntialias = true
            };

            canvas.DrawText(text, x, y, paint);
            paint.Dispose();
        }

        /// <summary>
        /// NEW OVERLOAD:
        /// Draws a text string at the specified location using a Font and a Brush,
        /// mirroring System.Drawing.Graphics.DrawString(string, Font, Brush, float, float).
        /// </summary>
        public void DrawString(string text, Font font, Brush brush, float x, float y)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (font == null) throw new ArgumentNullException(nameof(font));
            if (brush == null) throw new ArgumentNullException(nameof(brush));
            CheckDisposed();

            // Convert our Font to an SKPaint
            // If you want to pass a specific DPI, do so (example: 96f).
            using (SKPaint paint = font.ToSKPaint(96f))
            {
                // If the brush is a SolidBrush, set paint.Color to that brush color.
                // If the brush is a gradient or something else, you may need additional logic.
                paint.Color = brush.Color.ToSKColor();

                // If you want to handle underline or strikeout:
                // if (font.Underline) { ... } etc. (In Skia, you'd typically do a separate line.)

                // Draw the text
                canvas.DrawText(text, x, y, paint);
            }
        }

        #endregion

        #region Transforms

        public void TranslateTransform(float dx, float dy)
        {
            CheckDisposed();
            canvas.Translate(dx, dy);
        }

        public void ScaleTransform(float sx, float sy)
        {
            CheckDisposed();
            canvas.Scale(sx, sy);
        }

        public void RotateTransform(float degrees)
        {
            CheckDisposed();
            canvas.RotateDegrees(degrees);
        }

        public void ResetTransform()
        {
            CheckDisposed();
            canvas.ResetMatrix();
        }

        #endregion

        #region VisibleClipBounds

        /// <summary>
        /// Gets the visible clipping bounds of the Graphics object as a RectangleF.
        /// </summary>
        public RectangleF VisibleClipBounds
        {
            get
            {
                CheckDisposed();
                SKRect skRect = canvas.DeviceClipBounds;
                return new RectangleF(skRect.Left, skRect.Top, skRect.Width, skRect.Height);
            }
        }

        #endregion

        #region Disposal

        private void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(Graphics));
        }

        public void Dispose()
        {
            if (!disposed)
            {
                canvas.Dispose();
                canvas = null;
                disposed = true;
            }
        }

        #endregion

        public override string ToString()
        {
            return $"Graphics [VisibleClipBounds={VisibleClipBounds}]";
        }
    }
}
