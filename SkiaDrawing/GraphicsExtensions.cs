using System;
using SkiaSharp;

namespace SkiaDrawing
{
    public static class GraphicsExtensions
    {
        /// <summary>
        /// Draws the specified bitmap at the integer coordinates (p.X, p.Y).
        /// </summary>
        /// <param name="g">The Graphics object on which to draw.</param>
        /// <param name="image">The Bitmap to draw.</param>
        /// <param name="p">The integer-based Point location.</param>
        public static void DrawImage(this Graphics g, Bitmap image, Point p)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            // Call the existing float-based DrawImage, 
            // passing the coordinates from p (converted to float).
            g.DrawImage(image, (float)p.X, (float)p.Y);
        }
        
        /// <summary>
        /// Draws the specified bitmap at the floating-point coordinates (p.X, p.Y).
        /// This extension method calls the existing Graphics.DrawImage(Bitmap, float, float).
        /// </summary>
        /// <param name="g">The Graphics object on which to draw.</param>
        /// <param name="b">The Bitmap to draw.</param>
        /// <param name="p">The floating-point PointF location.</param>
        public static void DrawImage(this Graphics g, Bitmap b, PointF p)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            // Pass the float coordinates p.X and p.Y to the existing draw method.
            g.DrawImage(b, p.X, p.Y);
        }
        
         /// <summary>
        /// Draws the specified portion (srcRect) of the bitmap into the destination rectangle (destRect)
        /// on the drawing surface, interpreting the coordinates according to the specified GraphicsUnit.
        /// </summary>
        /// <param name="g">The Graphics object to draw upon.</param>
        /// <param name="b">The Bitmap source image.</param>
        /// <param name="destRect">The destination rectangle on the drawing surface.</param>
        /// <param name="srcRect">The source rectangle in the bitmap to copy from.</param>
        /// <param name="unit">The GraphicsUnit specifying how to interpret the rectangles.</param>
        public static void DrawImage(this Graphics g, Bitmap b, Rectangle destRect, Rectangle srcRect, GraphicsUnit unit)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            // Convert the srcRect and destRect to SkiaSharp's SKRect, 
            // applying a unit-based conversion if necessary.

            // We'll assume we use the bitmap's HorizontalResolution / VerticalResolution
            // for conversions if the unit is Inch, for instance.
            float dpiX = b.HorizontalResolution; 
            float dpiY = b.VerticalResolution;  

            // Convert rectangles
            var skSrc = ConvertRect(srcRect, unit, dpiX, dpiY);
            var skDest = ConvertRect(destRect, unit, dpiX, dpiY);

            // Now we can call the existing float-based DrawImage(b, x, y) or the 
            // underlying Skia call. We'll create a paint with the current InterpolationMode if needed.
            // We'll do it similarly to "DrawBitmap(..., srcRect, destRect)" approach:
            using (var paint = new SkiaSharp.SKPaint())
            {
                paint.FilterQuality = g.InterpolationMode.ToSKFilterQuality();
                paint.IsAntialias = true; // or as needed
                g.DrawImage(b, skSrc, skDest, paint);
            }
        }

        

        /// <summary>
        /// DrawImage override to draw the portion of 'b' from 'skSrc' to 'skDest' 
        /// using the provided paint. This leverages SkiaSharp's canvas.DrawBitmap API.
        /// </summary>
        private static void DrawImage(this Graphics g, Bitmap b, SkiaSharp.SKRect skSrc, SkiaSharp.SKRect skDest, SkiaSharp.SKPaint paint)
        {
            // We have a float-based drawImage approach in the Graphics class for a single draw call:
            // But let's replicate or call a new method. We'll do direct Skia calls here for demonstration.

            // Retrieve the underlying canvas from 'g' (not publicly accessible in the original, so we do reflection or something).
            // Alternatively, we can create a new method in the Graphics class that draws from SKRect->SKRect.
            // For demonstration, we use reflection or a helper.

            // Reflection approach to get 'canvas' from 'Graphics'
            var canvasField = typeof(Graphics).GetField("canvas", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (canvasField == null)
                throw new Exception("Could not find internal field 'canvas' in Graphics.");
            var canvasObj = canvasField.GetValue(g);
            if (canvasObj == null)
                throw new Exception("The internal 'canvas' is null in Graphics.");
            var canvas = (SkiaSharp.SKCanvas)canvasObj;

            // Now draw the portion
            canvas.DrawBitmap(b.ToSKBitmap(), skSrc, skDest, paint);
        }
        
        /// <summary>
        /// Draws the entire source bitmap into the specified destination rectangle
        /// on the drawing surface, scaling or shrinking as needed.
        /// Similar to System.Drawing.Graphics.DrawImage(Image, Rectangle).
        /// </summary>
        /// <param name="g">The Graphics object on which to draw.</param>
        /// <param name="b">The Bitmap to draw.</param>
        /// <param name="destRect">The destination rectangle where the bitmap is drawn.</param>
        public static void DrawImage(this Graphics g, Bitmap b, Rectangle destRect)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            // Draw the entire bitmap (srcRect = the bitmap's full area),
            // into the specified destRect, using Pixel as the graphics unit.
            Rectangle srcRect = new Rectangle(0, 0, b.Width, b.Height);

            // You can rely on your previously implemented DrawImage(Bitmap, Rectangle, Rectangle, GraphicsUnit) extension:
            g.DrawImage(b, destRect, srcRect, GraphicsUnit.Pixel);
        }
        
        /// <summary>
        /// Draws the entire bitmap 'b' into the specified destination rectangle
        /// at position (x, y) with the specified width and height.
        /// </summary>
        /// <param name="g">The Graphics object on which to draw.</param>
        /// <param name="b">The Bitmap to draw.</param>
        /// <param name="x">The left position of the destination rectangle.</param>
        /// <param name="y">The top position of the destination rectangle.</param>
        /// <param name="width">The width of the destination rectangle.</param>
        /// <param name="height">The height of the destination rectangle.</param>
        public static void DrawImage(this Graphics g, Bitmap b, int x, int y, int width, int height)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            // Create the destination rectangle from the given coordinates
            Rectangle destRect = new Rectangle(x, y, width, height);

            // We can call an existing extension/method that takes (Bitmap, Rectangle)
            // which draws the entire source image into that rectangle.
            g.DrawImage(b, destRect);
        }
        
        /// <summary>
        /// Draws the specified portion (srcX, srcY, srcWidth, srcHeight) of bitmap 'b'
        /// into the destination rectangle 'destRect' on the drawing surface,
        /// interpreting coordinates via the specified GraphicsUnit.
        /// Optionally applies any color transforms / gamma / etc. from the ImageAttributes.
        /// </summary>
        public static void DrawImage(
            this Graphics g,
            Bitmap b,
            Rectangle destRect,
            int srcX,
            int srcY,
            int srcWidth,
            int srcHeight,
            GraphicsUnit unit,
            ImageAttributes attributes)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            // Convert the source rectangle
            var srcRect = new Rectangle(srcX, srcY, srcWidth, srcHeight);

            // We'll interpret the destRect, srcRect using 'unit' if needed:
            // Get the bitmap's DPI for conversions:
            float dpiX = b.HorizontalResolution;
            float dpiY = b.VerticalResolution;

            // Convert rectangles to Skia's SKRect (float-based)
            var skSrc = ConvertRect(srcRect, unit, dpiX, dpiY);
            var skDest = ConvertRect(destRect, unit, dpiX, dpiY);

            // Create a paint that includes filtering (based on g.InterpolationMode) and
            // possibly a color filter from attributes.
            using (var paint = new SkiaSharp.SKPaint())
            {
                // If you have a method like g.InterpolationMode.ToSKFilterQuality(), call it here:
                paint.FilterQuality = g.InterpolationMode.ToSKFilterQuality(); 
                paint.IsAntialias = true;

                // If attributes != null, retrieve color filter
                if (attributes != null)
                {
                    var cf = attributes.GetSKColorFilter();
                    if (cf != null)
                        paint.ColorFilter = cf;
                }

                // Now we draw the portion of the bitmap
                var canvas = GetCanvas(g);
                canvas.DrawBitmap(b.ToSKBitmap(), skSrc, skDest, paint);
            }
        }

        /// <summary>
        /// Converts a Rectangle from the specified GraphicsUnit into Skia's SKRect (float-based).
        /// If unit=Pixel, we use direct integer coords. If unit=Inch, we multiply by dpi, etc.
        /// For demonstration, we handle Pixel, Inch, Millimeter, Point, etc.
        /// </summary>
        private static SkiaSharp.SKRect ConvertRect(Rectangle r, GraphicsUnit unit, float dpiX, float dpiY)
        {
            float x = r.X;
            float y = r.Y;
            float w = r.Width;
            float h = r.Height;

            switch (unit)
            {
                case GraphicsUnit.Pixel:
                case GraphicsUnit.Display:
                    // Use integer coords as-is
                    break;

                case GraphicsUnit.Inch:
                    // 1 inch => multiply by DPI
                    x *= dpiX;
                    y *= dpiY;
                    w *= dpiX;
                    h *= dpiY;
                    break;

                case GraphicsUnit.Millimeter:
                    // 1 mm = 1/25.4 inch => multiply by (dpi / 25.4)
                    float scaleX_mm = dpiX / 25.4f;
                    float scaleY_mm = dpiY / 25.4f;
                    x *= scaleX_mm; 
                    y *= scaleY_mm;
                    w *= scaleX_mm; 
                    h *= scaleY_mm;
                    break;

                case GraphicsUnit.Point:
                    // 1 point = 1/72 inch => multiply by (dpi / 72)
                    float scaleX_pt = dpiX / 72f;
                    float scaleY_pt = dpiY / 72f;
                    x *= scaleX_pt;
                    y *= scaleY_pt;
                    w *= scaleX_pt;
                    h *= scaleY_pt;
                    break;

                case GraphicsUnit.Document:
                    // 1 doc unit = 1/300 inch => multiply by (dpi / 300)
                    float scaleX_doc = dpiX / 300f;
                    float scaleY_doc = dpiY / 300f;
                    x *= scaleX_doc;
                    y *= scaleY_doc;
                    w *= scaleX_doc;
                    h *= scaleY_doc;
                    break;

                // You could add more or map Display differently, etc.
                default:
                    // Fall back to Pixel
                    break;
            }

            return new SkiaSharp.SKRect(x, y, x + w, y + h);
        }

        /// <summary>
        /// Retrieves the internal SKCanvas from the Graphics object via reflection,
        /// or throws if not found.
        /// </summary>
        private static SkiaSharp.SKCanvas GetCanvas(Graphics g)
        {
            var canvasField = typeof(Graphics).GetField("canvas", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (canvasField == null)
                throw new Exception("Could not find internal field 'canvas' in Graphics.");
            var canvasObj = canvasField.GetValue(g);
            if (canvasObj == null)
                throw new Exception("The internal 'canvas' is null in Graphics.");
            return (SkiaSharp.SKCanvas)canvasObj;
        }
        
        /// <summary>
        /// Fills the specified rectangle on the drawing surface with a linear gradient,
        /// using the given LinearGradientBrush.
        /// </summary>
        /// <param name="g">The Graphics object on which to draw.</param>
        /// <param name="b">The LinearGradientBrush defining the gradient fill.</param>
        /// <param name="r">An integer-based Rectangle specifying the area to fill.</param>
        public static void FillRectangle(this Graphics g, LinearGradientBrush b, Rectangle r)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (b == null)
                throw new ArgumentNullException(nameof(b));

            // Obtain the underlying SKCanvas from the Graphics object via reflection.
            var canvas = GetCanvas(g);

            // Create an SKPaint from the brush.
            // We'll dispose it immediately after use (no 'using' statements, per your style).
            var paint = b.ToSKPaint();

            // Draw the rectangle
            canvas.DrawRect(r.X, r.Y, r.Width, r.Height, paint);

            // Dispose the paint
            paint.Dispose();
        }
        
        
        /// <summary>
        /// Attempts to copy pixels from the screen (at source coords x,y) 
        /// to the current Graphics surface (at 0,0) with size 'size',
        /// using the specified CopyPixelOperation (which is SourceCopy here).
        /// 
        /// Note: This method is a stub demonstrating signature only, 
        /// as SkiaSharp does not provide cross-platform screen capture.
        /// Real usage would require platform-specific code (e.g., Windows GDI BitBlt).
        /// </summary>
        public static void CopyFromScreen(
            this Graphics g,
            int x,
            int y,
            int zero1,
            int zero2,
            Size size,
            CopyPixelOperation copyPixelOp // typically SourceCopy
        )
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));

            if (copyPixelOp != CopyPixelOperation.SourceCopy)
            {
                // For demonstration, we only support SourceCopy in this stub.
                throw new NotImplementedException("Only SourceCopy is supported in this example.");
            }

            // In real code, you might do a platform-specific approach:
            // 
            //  1) On Windows: 
            //     - P/Invoke BitBlt from the screen DC to an offscreen HDC, 
            //       then create an SKBitmap from that HDC, 
            //       then draw to the Graphics's SKCanvas.
            //  2) On macOS: 
            //     - Use CGDisplay, capture a CGImage, convert to Skia, etc.
            //  3) On Linux or other OS: 
            //     - There's no universal approach. Possibly X11 calls, etc.
            // 
            // For demonstration, we throw:
            throw new NotImplementedException(
                "CopyFromScreen is not implemented cross-platform. " + 
                "Use platform-specific APIs to capture the screen."
            );
        }
        
        /// <summary>
        /// Draws a series of connected lines (Point[i] to Point[i+1]) 
        /// using the specified Pen, mimicking System.Drawing.Graphics.DrawLines.
        /// </summary>
        /// <param name="g">The Graphics on which to draw.</param>
        /// <param name="p">The Pen used to draw the lines.</param>
        /// <param name="points">An array of Point structs defining line endpoints.</param>
        public static void DrawLines(this Graphics g, Pen p, Point[] points)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (p == null)
                throw new ArgumentNullException(nameof(p));
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (points.Length < 2)
                return; // Nothing to draw if fewer than 2 points.

            // Loop over each consecutive pair of points.
            for (int i = 0; i < points.Length - 1; i++)
            {
                float x1 = points[i].X;
                float y1 = points[i].Y;
                float x2 = points[i + 1].X;
                float y2 = points[i + 1].Y;

                g.DrawLine(p, x1, y1, x2, y2);
            }
        }
        
        /// <summary>
        /// Draws a string at a given location using a Font, SolidBrush, and StringFormat.
        /// </summary>
        /// <param name="g">The Graphics object to draw on.</param>
        /// <param name="text">The text to draw.</param>
        /// <param name="font">The Font to use for rendering the text.</param>
        /// <param name="brush">The SolidBrush to determine text color.</param>
        /// <param name="point">The location (x, y) to draw the text.</param>
        /// <param name="format">The StringFormat specifying alignment, direction, etc.</param>
        public static void DrawString(this Graphics g, string text, Font font, SolidBrush brush, PointF point, StringFormat format)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            if (font == null)
                throw new ArgumentNullException(nameof(font));
            if (brush == null)
                throw new ArgumentNullException(nameof(brush));
            if (format == null)
                throw new ArgumentNullException(nameof(format));

            g.CheckDisposed();

            using (SKPaint paint = font.ToSKPaint(96f)) // Assume 96 DPI scaling
            {
                // Apply the brush color
                paint.Color = brush.Color.ToSKColor();

                // Measure text bounds
                SKRect textBounds = new SKRect();
                paint.MeasureText(text, ref textBounds);

                float x = point.X;
                float y = point.Y;

                // Apply StringFormat settings
                format.ApplyTo(paint, textBounds);

                // Adjust vertical alignment
                switch (format.LineAlignment)
                {
                    case StringAlignment.Center:
                        y -= textBounds.MidY;
                        break;
                    case StringAlignment.Far:
                        y -= textBounds.Bottom;
                        break;
                }

                // Adjust horizontal alignment
                switch (format.Alignment)
                {
                    case StringAlignment.Center:
                        x -= textBounds.MidX;
                        break;
                    case StringAlignment.Far:
                        x -= textBounds.Right;
                        break;
                }

                // Draw the text
                g.ToSKCanvas().DrawText(text, x, y, paint);
            }
        }

        /// <summary>
        /// Helper to get the underlying SKCanvas from a Graphics object.
        /// </summary>
        private static SKCanvas ToSKCanvas(this Graphics g)
        {
            return (SKCanvas)g.GetType()
                              .GetField("canvas", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                              ?.GetValue(g);
        }

        /// <summary>
        /// Ensures the Graphics object is not disposed.
        /// </summary>
        private static void CheckDisposed(this Graphics g)
        {
            var disposedField = g.GetType().GetField("disposed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (disposedField != null && (bool)disposedField.GetValue(g))
                throw new ObjectDisposedException(nameof(Graphics));
        }
        
    }
}








