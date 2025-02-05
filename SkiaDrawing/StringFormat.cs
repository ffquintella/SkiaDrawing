using System;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A class that mimics System.Drawing.StringFormat using SkiaSharp for text layout and alignment.
    /// </summary>
    public class StringFormat : IDisposable
    {
        private bool disposed;

        /// <summary>
        /// Specifies horizontal alignment (Near = left, Center, Far = right).
        /// </summary>
        public StringAlignment Alignment { get; set; } = StringAlignment.Near;

        /// <summary>
        /// Specifies vertical alignment (Near = top, Center, Far = bottom).
        /// </summary>
        public StringAlignment LineAlignment { get; set; } = StringAlignment.Near;

        /// <summary>
        /// Specifies the trimming style used when text is too long.
        /// </summary>
        public StringTrimming Trimming { get; set; } = StringTrimming.None;

        /// <summary>
        /// Specifies whether the text is displayed right-to-left.
        /// </summary>
        public bool DirectionRightToLeft { get; set; } = false;

        /// <summary>
        /// Specifies additional format flags.
        /// </summary>
        public StringFormatFlags FormatFlags { get; set; } = 0;

        /// <summary>
        /// Creates a new default StringFormat.
        /// </summary>
        public StringFormat()
        {
        }

        /// <summary>
        /// Creates a new StringFormat using specific format flags.
        /// </summary>
        /// <param name="flags">The StringFormatFlags to apply.</param>
        public StringFormat(StringFormatFlags flags)
        {
            FormatFlags = flags;

            // Apply right-to-left setting if specified in flags
            if ((flags & StringFormatFlags.DirectionRightToLeft) != 0)
            {
                DirectionRightToLeft = true;
            }
        }

        /// <summary>
        /// Creates a new StringFormat by copying another.
        /// </summary>
        public StringFormat(StringFormat other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            Alignment = other.Alignment;
            LineAlignment = other.LineAlignment;
            Trimming = other.Trimming;
            DirectionRightToLeft = other.DirectionRightToLeft;
            FormatFlags = other.FormatFlags;
        }

        /// <summary>
        /// Predefined default format similar to System.Drawing.StringFormat.GenericDefault.
        /// </summary>
        public static StringFormat GenericDefault => new StringFormat();

        /// <summary>
        /// Predefined typographic format similar to System.Drawing.StringFormat.GenericTypographic.
        /// </summary>
        public static StringFormat GenericTypographic => new StringFormat
        {
            Trimming = StringTrimming.None
        };

        /// <summary>
        /// Applies the string format settings to an SKPaint object for SkiaSharp text rendering.
        /// </summary>
        public void ApplyTo(SKPaint paint, SKRect textBounds)
        {
            if (paint == null)
                throw new ArgumentNullException(nameof(paint));

            // Horizontal alignment
            switch (Alignment)
            {
                case StringAlignment.Center:
                    paint.TextAlign = SKTextAlign.Center;
                    break;
                case StringAlignment.Far:
                    paint.TextAlign = SKTextAlign.Right;
                    break;
                case StringAlignment.Near:
                default:
                    paint.TextAlign = SKTextAlign.Left;
                    break;
            }

            // Right-to-left text direction (affects text positioning)
            if (DirectionRightToLeft)
            {
                paint.TextAlign = SKTextAlign.Right;
            }

            // Trimming (handled by clipping or ellipsis text replacement)
            if (Trimming == StringTrimming.EllipsisCharacter || Trimming == StringTrimming.EllipsisWord)
            {
                paint.LcdRenderText = true; // Improve text readability for ellipsis
            }
        }

        /// <summary>
        /// Cleans up resources.
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
            }
        }

        public override string ToString()
        {
            return $"StringFormat [Alignment={Alignment}, LineAlignment={LineAlignment}, Trimming={Trimming}, DirectionRightToLeft={DirectionRightToLeft}, FormatFlags={FormatFlags}]";
        }
    }
}
