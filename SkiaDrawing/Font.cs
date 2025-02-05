using System;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A simplified class that mimics System.Drawing.Font
    /// using SkiaSharp to store typeface and style information.
    /// </summary>
    public class Font : IDisposable
    {
        private bool disposed;

        private SKTypeface skTypeface;
        private float size;  // in "points" or user-chosen units
        private FontStyle style;
        private string familyName;

        /// <summary>
        /// Initializes a new Font with the specified family name, size, and style.
        /// </summary>
        /// <param name="familyName">The font family name (e.g. "Arial").</param>
        /// <param name="emSize">The font size in points (or float units).</param>
        /// <param name="style">The font style (regular, bold, italic, etc.).</param>
        public Font(string familyName, float emSize, FontStyle style = FontStyle.Regular)
        {
            if (string.IsNullOrEmpty(familyName))
                throw new ArgumentNullException(nameof(familyName));
            if (emSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(emSize), "Font size must be positive.");

            this.familyName = familyName;
            this.size = emSize;
            this.style = style;

            // Convert our FontStyle to a SkiaSharp SKFontStyle (weight, width, slant).
            var skStyle = CreateSkFontStyle(style);

            // Create the SKTypeface from the family name with the desired style.
            // If the font isn't found, Skia will fallback to a default typeface.
            skTypeface = SKTypeface.FromFamilyName(familyName, skStyle);
            if (skTypeface == null)
            {
                // As a fallback, we might do:
                skTypeface = SKTypeface.Default;
            }
        }

        /// <summary>
        /// The name of the font family (e.g. "Arial", "Times New Roman").
        /// </summary>
        public string FontFamily
        {
            get
            {
                CheckDisposed();
                return familyName;
            }
        }

        /// <summary>
        /// The em-size (in points or user-chosen float) of this font.
        /// </summary>
        public float Size
        {
            get
            {
                CheckDisposed();
                return size;
            }
        }

        /// <summary>
        /// The style (bold, italic, etc.) of this font.
        /// </summary>
        public FontStyle Style
        {
            get
            {
                CheckDisposed();
                return style;
            }
        }

        /// <summary>
        /// Returns true if this font is bold.
        /// </summary>
        public bool Bold => (style & FontStyle.Bold) != 0;

        /// <summary>
        /// Returns true if this font is italic.
        /// </summary>
        public bool Italic => (style & FontStyle.Italic) != 0;

        /// <summary>
        /// Returns true if this font is underlined.
        /// (In System.Drawing, you'd typically handle the underline in text drawing.)
        /// </summary>
        public bool Underline => (style & FontStyle.Underline) != 0;

        /// <summary>
        /// Returns true if this font is strikeout.
        /// (Similarly, handle the strike line in text drawing if needed.)
        /// </summary>
        public bool Strikeout => (style & FontStyle.Strikeout) != 0;

        #region SkiaSharp Interop

        /// <summary>
        /// Creates or returns the underlying SKTypeface for advanced usage.
        /// </summary>
        public SKTypeface ToSKTypeface()
        {
            CheckDisposed();
            return skTypeface;
        }

        /// <summary>
        /// Creates an SKPaint with this font’s typeface and size (in pixels).
        /// By default, 1 point = 1.3333... device pixels at 96 DPI,
        /// so you might multiply size by (DPI / 72) if you want approximate point scaling.
        /// In a simplified approach, you can treat size as direct pixels or use a conversion.
        /// </summary>
        /// <param name="dpi">The desired DPI for converting points to device pixels. If not needed, pass 96 or 72, etc.</param>
        /// <returns>A new SKPaint object using this font’s typeface and size in pixels.</returns>
        public SKPaint ToSKPaint(float dpi = 96f)
        {
            CheckDisposed();

            // Convert the point size to approximate device pixels.
            // By default, 1 point = 1/72 inch, so at 96 DPI => * (96 / 72) = 1.3333
            float pixelSize = size * (dpi / 72f);

            SKPaint paint = new SKPaint
            {
                Typeface   = skTypeface,
                TextSize   = pixelSize,
                IsAntialias = true
            };
            // If you want to check style for underline/strike, do it in drawing code or here.

            return paint;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                skTypeface?.Dispose();
                skTypeface = null;
            }
        }

        private void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(Font));
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Converts our FontStyle flags into an SKFontStyle with weight, width, slant.
        /// Bold => Bold weight, Italic => Italic slant.
        /// </summary>
        private SKFontStyle CreateSkFontStyle(FontStyle style)
        {
            // Handle bold
            var weight = (style & FontStyle.Bold) != 0
                ? SKFontStyleWeight.Bold
                : SKFontStyleWeight.Normal;

            // We ignore width for simplicity, use Normal
            var width = SKFontStyleWidth.Normal;

            // Handle italic
            var slant = (style & FontStyle.Italic) != 0
                ? SKFontStyleSlant.Italic
                : SKFontStyleSlant.Upright;

            return new SKFontStyle(weight, width, slant);
        }

        public override string ToString()
        {
            return $"Font [Family={familyName}, Size={size}, Style={style}]";
        }

        #endregion
    }
}
