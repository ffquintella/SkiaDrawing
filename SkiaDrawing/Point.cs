
namespace SkiaDrawing
{
    public struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point Empty => new Point(0, 0);
        
        // Example overrides, operators, etc.
        public override string ToString() => $"{{X={X},Y={Y}}}";
    }
}
