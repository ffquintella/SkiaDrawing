namespace SkiaDrawing
{
    /// <summary>
    /// Specifies the unit of measure used for drawing operations.
    /// Mirrors the System.Drawing.GraphicsUnit enum.
    /// </summary>
    public enum GraphicsUnit
    {
        /// <summary>
        /// Specifies the world coordinate system unit (often custom).
        /// </summary>
        World = 0,

        /// <summary>
        /// Specifies the unit of the display device (for example, pixels on a screen).
        /// </summary>
        Display = 1,

        /// <summary>
        /// Specifies a device pixel as the unit of measure.
        /// </summary>
        Pixel = 2,

        /// <summary>
        /// Specifies a printer's point (1/72 inch) as the unit of measure.
        /// </summary>
        Point = 3,

        /// <summary>
        /// Specifies 1 inch as the unit of measure.
        /// </summary>
        Inch = 4,

        /// <summary>
        /// Specifies the document unit (1/300 inch) as the unit of measure.
        /// </summary>
        Document = 5,

        /// <summary>
        /// Specifies a millimeter as the unit of measure.
        /// </summary>
        Millimeter = 6
    }
}