namespace SkiaDrawing
{
    /// <summary>
    /// A minimal stand-in for System.Drawing.Pen, used for drawing lines, rectangles, etc.
    /// </summary>
    public class Pen: IDisposable
    {
        public Color Color { get; set; }
        public float Width { get; set; }

        // You could add more properties: DashStyle, LineJoin, etc.

        public Pen(Color color, float width = 1.0f)
        {
            Color = color;
            Width = width;
        }

        public void Dispose()
        {
            // No unmanaged resources to release.
        }
    }
}