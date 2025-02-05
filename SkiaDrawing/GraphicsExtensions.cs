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
    }
}