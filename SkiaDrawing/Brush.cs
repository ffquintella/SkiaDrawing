namespace SkiaDrawing
{
    /// <summary>
    /// A minimal stand-in for System.Drawing.Brush, used for filling shapes.
    /// </summary>
    public class Brush
    {
        public Color Color { get; set; }

        public Brush(Color color)
        {
            Color = color;
        }
    }
}