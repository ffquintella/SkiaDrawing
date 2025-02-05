namespace SkiaDrawing
{
    /// <summary>
    /// A float-based rectangle structure,
    /// mimicking System.Drawing.RectangleF.
    /// </summary>
    public struct RectangleF
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float Left   => X;
        public float Top    => Y;
        public float Right  => X + Width;
        public float Bottom => Y + Height;
    }
}