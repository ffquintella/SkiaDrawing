using System;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A simplified version of System.Drawing.Graphics, implemented using SkiaSharp.
    /// Provides basic 2D drawing operations on a Bitmap (SKBitmap).
    /// </summary>
    public class Graphics : IDisposable
    {
        private SKCanvas canvas;
        private bool disposed;

        // The current interpolation mode for image drawing.
        private InterpolationMode interpolationMode = InterpolationMode.Default;

        /// <summary>
        /// Gets or sets the interpolation mode used when drawing images.
        /// </summary>
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

            // We create a new SKCanvas that draws on the internal SKBitmap.
            SKCanvas skCanvas = new SKCanvas(bitmap.ToSKBitmap());
            return new Graphics(skCanvas);
        }

        #region Clear Method

        /// <summary>
        /// Clears the entire drawing surface and fills it with the specified color.
        /// Similar to System.Drawing.Graphics.Clear(Color).
        /// </summary>
        public void Clear(Color c)
        {
            CheckDisposed();
            canvas.Clear(c.ToSKColor());
        }

        #endregion

        #region Drawing Methods

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

        /// <summary>
        /// Draws the specified source bitmap at (x, y), applying the current InterpolationMode.
        /// </summary>
        public void DrawImage(Bitmap image, float x, float y)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            CheckDisposed();

            SKPaint paint = new SKPaint
            {
                FilterQuality = ConvertInterpolationMode(interpolationMode),
                IsAntialias = true
            };

            canvas.DrawBitmap(image.ToSKBitmap(), x, y, paint);
            paint.Dispose();
        }

        /// <summary>
        /// Draws a string at the specified coordinates, with the specified font size and color.
        /// (Simplified approach—no advanced font families, styles, etc.)
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

        #region Internal Helpers

        private SKFilterQuality ConvertInterpolationMode(InterpolationMode mode)
        {
            switch (mode)
            {
                case InterpolationMode.Low:
                case InterpolationMode.NearestNeighbor:
                    return SKFilterQuality.None;

                case InterpolationMode.Bilinear:
                case InterpolationMode.Default:
                    return SKFilterQuality.Low;

                case InterpolationMode.High:
                case InterpolationMode.Bicubic:
                case InterpolationMode.HighQualityBilinear:
                case InterpolationMode.HighQualityBicubic:
                    return SKFilterQuality.High;

                // You could map 'Bilinear' -> Medium, for instance. Skia supports None, Low, Medium, High.
                // Adjust as needed.
            }
            return SKFilterQuality.Low; // default
        }

        #endregion

        #region State / Disposal

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
    }
}
