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
            // The SKCanvas should NOT dispose the SKBitmap itself, so we can keep using the Bitmap afterwards.
            SKCanvas skCanvas = new SKCanvas(bitmap.ToSKBitmap());
            return new Graphics(skCanvas);
        }

        #region Drawing Methods

        /// <summary>
        /// Draws a line between two points using the specified Pen.
        /// </summary>
        public void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            if (pen == null) throw new ArgumentNullException(nameof(pen));
            CheckDisposed();

            // Create a paint for stroke
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

        /// <summary>
        /// Draws the outline of a rectangle using the specified Pen.
        /// </summary>
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

        /// <summary>
        /// Fills the interior of a rectangle using the specified Brush.
        /// </summary>
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

        /// <summary>
        /// Draws an ellipse defined by a bounding rectangle using the specified Pen.
        /// </summary>
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

        /// <summary>
        /// Fills an ellipse (circle/oval) defined by a bounding rectangle using the specified Brush.
        /// </summary>
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
        /// Draws the specified source bitmap at (x, y).
        /// </summary>
        public void DrawImage(Bitmap image, float x, float y)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            CheckDisposed();

            // We can just draw the SKBitmap of the given image at the specified coords
            canvas.DrawBitmap(image.ToSKBitmap(), x, y);
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
                // We could add more advanced font settings here
            };

            canvas.DrawText(text, x, y, paint);
            paint.Dispose();
        }

        #endregion

        #region Transforms

        /// <summary>
        /// Translates the origin of the canvas by the specified amounts in x and y.
        /// </summary>
        public void TranslateTransform(float dx, float dy)
        {
            CheckDisposed();
            canvas.Translate(dx, dy);
        }

        /// <summary>
        /// Scales the canvas by the specified factors in x and y.
        /// </summary>
        public void ScaleTransform(float sx, float sy)
        {
            CheckDisposed();
            canvas.Scale(sx, sy);
        }

        /// <summary>
        /// Rotates the canvas by 'degrees' about the current origin.
        /// </summary>
        public void RotateTransform(float degrees)
        {
            CheckDisposed();
            canvas.RotateDegrees(degrees);
        }

        /// <summary>
        /// Resets any transformations to the identity matrix.
        /// </summary>
        public void ResetTransform()
        {
            CheckDisposed();
            canvas.ResetMatrix();
        }

        #endregion

        #region State / Disposal

        private void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(Graphics));
        }

        /// <summary>
        /// Disposes of the underlying resources. 
        /// Closes the SKCanvas if needed.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                // Typically, an SKCanvas constructed from an SKBitmap doesn't need explicit disposal
                // for the SKBitmap itself. But we do dispose the SKCanvas to free any native references.
                canvas.Dispose();
                canvas = null;
                disposed = true;
            }
        }

        #endregion
    }
}
