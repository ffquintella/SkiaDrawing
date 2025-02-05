using System;

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
        /// Converts a Rectangle from the specified GraphicsUnit into Skia's SKRect (float-based).
        /// If unit=Pixel, we use direct integer coords. If unit=Inch, we multiply by dpi, etc.
        /// For demonstration, we handle Pixel and Inch; other units can be added similarly.
        /// </summary>
        private static SkiaSharp.SKRect ConvertRect(Rectangle r, GraphicsUnit unit, float dpiX, float dpiY)
        {
            // By default, interpret as pixel (no scaling).
            float x = r.X;
            float y = r.Y;
            float w = r.Width;
            float h = r.Height;

            switch (unit)
            {
                case GraphicsUnit.Pixel:
                case GraphicsUnit.Display:
                    // direct usage of integer coordinates => no scaling
                    break;

                case GraphicsUnit.Inch:
                    // 1 inch => multiply by DPI
                    x *= dpiX;
                    y *= dpiY;
                    w *= dpiX;
                    h *= dpiY;
                    break;

                // For demonstration, we show how you might handle another unit
                case GraphicsUnit.Millimeter:
                    // 1 mm = (1/25.4) inch => multiply by (dpi / 25.4)
                    float scaleX_mm = dpiX / 25.4f;
                    float scaleY_mm = dpiY / 25.4f;
                    x *= scaleX_mm; 
                    y *= scaleY_mm;
                    w *= scaleX_mm; 
                    h *= scaleY_mm;
                    break;

                case GraphicsUnit.Point:
                    // 1 point = 1/72 inch => multiply by (dpi / 72)
                    float scaleX_pt = dpiX / 72.0f;
                    float scaleY_pt = dpiY / 72.0f;
                    x *= scaleX_pt; 
                    y *= scaleY_pt;
                    w *= scaleX_pt; 
                    h *= scaleY_pt;
                    break;

                // Add Document (1/300 inch), etc., as needed.
                // Just default to pixel if not recognized
                default:
                    break;
            }

            return new SkiaSharp.SKRect(x, y, x + w, y + h);
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
    }
}


