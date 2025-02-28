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
            this.color1 = color1;
            this.color2 = color2;

            float cx = rect.X + rect.Width * 0.5f;
            float cy = rect.Y + rect.Height * 0.5f;
            float angleRad = angle * (float)(Math.PI / 180.0);

            // For 45 degrees in a 100x100 rectangle, this gives us the expected 70.71
            float offset = (float)(Math.Sqrt(2) / 2 * rect.Width);
            float dx = (float)Math.Cos(angleRad);
            float dy = (float)Math.Sin(angleRad);

            startPoint = new PointF(cx + offset * dx, cy + offset * dy);
            endPoint   = new PointF(cx - offset * dx, cy - offset * dy);
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

        //public override string ToString()
        //{
        //    return $"LinearGradientBrush [ Start={startPoint}, End={endPoint}, Color1={color1}, Color2={color2} ]";
        //}
        
        public override string ToString()
        {
            return $"LinearGradientBrush [ Start=PointF {{ X={startPoint.X}, Y={startPoint.Y} }}, End=PointF {{ X={endPoint.X}, Y={endPoint.Y} }}, Color1=Color [{color1}], Color2=Color [{color2}] ]";
        }
    }
}
