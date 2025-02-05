using System;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A brush that paints an area with a linear gradient between two points,
    /// mimicking System.Drawing.Drawing2D.LinearGradientBrush using SkiaSharp.
    /// </summary>
    public sealed class LinearGradientBrush : Brush, IDisposable
    {
        private bool disposed;

        private PointF startPoint;
        private PointF endPoint;
        private Color color1;
        private Color color2;

        // Other constructors omitted for brevity. 
        // (Include them if you wish to have the full class from earlier.)

        /// <summary>
        /// Creates a LinearGradientBrush based on a rectangle, two colors, and an angle (in degrees).
        /// This approximates System.Drawing.Drawing2D.LinearGradientBrush(Rectangle, Color, Color, float).
        /// </summary>
        /// <param name="rect">The bounding rectangle for the gradient.</param>
        /// <param name="color1">The first gradient color.</param>
        /// <param name="color2">The second gradient color.</param>
        /// <param name="angle">Angle in degrees, clockwise from the x-axis.</param>
        public LinearGradientBrush(Rectangle rect, Color color1, Color color2, float angle)
            : base()  // Call the base Brush constructor
        {
            // Store the colors
            this.color1 = color1;
            this.color2 = color2;

            // Compute the center of the rectangle
            float cx = rect.X + rect.Width  * 0.5f;
            float cy = rect.Y + rect.Height * 0.5f;

            // Convert angle to radians, measured clockwise from x-axis => 
            // we can define an anti-clockwise standard but let's just do clockwise anyway.
            float angleRad = angle * (float)(Math.PI / 180.0);

            // Compute half the diagonal for the radius
            float halfDiag = 0.5f * (float)Math.Sqrt(rect.Width  * rect.Width +
                                                     rect.Height * rect.Height);

            // We define the direction vector for the angle
            // In standard math, an angle 0 => (cos(0)=1, sin(0)=0), i.e. along +X axis
            // If it's clockwise, we might do sin negative, but let's keep it simple:
            float dx = (float)(Math.Cos(angleRad));
            float dy = (float)(Math.Sin(angleRad));

            // Start = center + radius*(dx, dy)
            // End   = center - radius*(dx, dy)
            startPoint = new PointF(cx + halfDiag * dx, cy + halfDiag * dy);
            endPoint   = new PointF(cx - halfDiag * dx, cy - halfDiag * dy);
        }

        #region Properties

        public PointF StartPoint
        {
            get => startPoint;
            set => startPoint = value;
        }

        public PointF EndPoint
        {
            get => endPoint;
            set => endPoint = value;
        }

        public Color Color1
        {
            get => color1;
            set => color1 = value;
        }

        public Color Color2
        {
            get => color2;
            set => color2 = value;
        }

        #endregion

        /// <summary>
        /// Creates or returns an SKPaint for filling with a linear gradient 
        /// from StartPoint to EndPoint between Color1 and Color2.
        /// </summary>
        public SKPaint ToSKPaint()
        {
            CheckDisposed();

            SKPoint skStart = startPoint.ToSKPoint();
            SKPoint skEnd   = endPoint.ToSKPoint();

            SKColor[] colors = new SKColor[] { color1.ToSKColor(), color2.ToSKColor() };
            float[] colorPositions = new float[] { 0.0f, 1.0f };

            SKShader shader = SKShader.CreateLinearGradient(
                skStart,
                skEnd,
                colors,
                colorPositions,
                SKShaderTileMode.Clamp 
            );

            SKPaint paint = new SKPaint
            {
                Shader = shader,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            return paint;
        }

        #region Dispose

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
            }
        }

        private void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(LinearGradientBrush));
        }

        #endregion

        public override string ToString()
        {
            return $"LinearGradientBrush [ Start={startPoint}, End={endPoint}, Color1={color1}, Color2={color2} ]";
        }
    }
}
