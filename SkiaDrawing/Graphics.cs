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

        private Graphics(SKCanvas skCanvas)
        {
            canvas = skCanvas ?? throw new ArgumentNullException(nameof(skCanvas));
        }

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
                Color = brush.Color.ToSKColor(),
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
        /// Draws text with a simple color and textSize.
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
        /// Draws text with a Font and Brush, at the given location.
        /// </summary>
        public void DrawString(string text, Font font, Brush brush, float x, float y)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (font == null) throw new ArgumentNullException(nameof(font));
            if (brush == null) throw new ArgumentNullException(nameof(brush));
            CheckDisposed();

            using (SKPaint paint = font.ToSKPaint(96f)) // e.g. 96 DPI
            {
                paint.Color = brush.Color.ToSKColor();
                canvas.DrawText(text, x, y, paint);
            }
        }

        #endregion

        #region MeasureString

        /// <summary>
        /// Measures the width and height of the given string using the specified Font.
        /// Returns a SizeF representing the bounding box (in local drawing units).
        /// </summary>
        public SizeF MeasureString(string numerics, Font f)
        {
            if (numerics == null)
                throw new ArgumentNullException(nameof(numerics));
            if (f == null)
                throw new ArgumentNullException(nameof(f));
            CheckDisposed();

            // Create a paint from the Font. Assume 96 DPI or adjust if needed.
            using (SKPaint paint = f.ToSKPaint(96f))
            {
                // measure the width of the text
                float width = paint.MeasureText(numerics);

                // measure the vertical extent via FontMetrics
                SKFontMetrics metrics = paint.FontMetrics;
                // The full height can be (metrics.Descent - metrics.Ascent). 
                // 'Leading' is extra spacing, typically for text lines.
                float height = metrics.Descent - metrics.Ascent;

                return new SizeF(width, height);
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
