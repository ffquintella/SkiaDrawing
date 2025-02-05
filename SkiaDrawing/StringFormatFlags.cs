using System;

namespace SkiaDrawing
{
    /// <summary>
    /// Specifies text layout information and display options,
    /// mimicking System.Drawing.StringFormatFlags.
    /// </summary>
    [Flags]
    public enum StringFormatFlags
    {
        /// <summary>
        /// No special flags.
        /// </summary>
        NoClip = 0,

        /// <summary>
        /// The text is displayed from right to left.
        /// </summary>
        DirectionRightToLeft = 1,

        /// <summary>
        /// The text is displayed vertically.
        /// </summary>
        DirectionVertical = 2,

        /// <summary>
        /// Parts of characters are allowed outside the layout rectangle.
        /// </summary>
        FitBlackBox = 4,

        /// <summary>
        /// Control characters such as newline and tab are displayed visually instead of interpreted.
        /// </summary>
        DisplayFormatControl = 8,

        /// <summary>
        /// The text is wrapped within the layout rectangle.
        /// </summary>
        NoWrap = 16,

        /// <summary>
        /// The text is laid out from bottom to top.
        /// </summary>
        NoFontFallback = 32,

        /// <summary>
        /// The text is displayed vertically only if the font supports it.
        /// </summary>
        MeasureTrailingSpaces = 64,

        /// <summary>
        /// The text is drawn without antialiasing.
        /// </summary>
        NoFitBlackBox = 128
    }
}