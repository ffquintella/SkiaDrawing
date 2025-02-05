namespace SkiaDrawing
{
    public static class RectangleExtensions
    {
        public static RectangleF ToRectangleF(this Rectangle rect)
        {
            return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}