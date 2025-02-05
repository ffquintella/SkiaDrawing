namespace SkiaDrawing
{
    /// <summary>
    /// A version of CopyPixelOperation that does not use the GDI (Windows raster op) values.
    /// Instead, each enum member has a simple integer code. 
    /// The names match those in System.Drawing.CopyPixelOperation, but the integral values here
    /// are arbitrary placeholders (0,1,2,...) rather than the actual GDI ROP codes.
    /// </summary>
    public enum CopyPixelOperation
    {
        /// <summary>
        /// Copies the source area directly to the destination.
        /// </summary>
        SourceCopy = 0,

        /// <summary>
        /// Combines the colors of the source and destination using OR.
        /// </summary>
        SourcePaint = 1,

        /// <summary>
        /// Combines the colors of the source and the destination using XOR.
        /// </summary>
        SourceInvert = 2,

        /// <summary>
        /// Combines the inverted source with the destination using AND.
        /// </summary>
        SourceErase = 3,

        /// <summary>
        /// Merges the colors of the source with the brush pattern using AND.
        /// </summary>
        MergeCopy = 4,

        /// <summary>
        /// Merges the colors of the inverted source with the destination using OR.
        /// </summary>
        MergePaint = 5,

        /// <summary>
        /// Copies the inverted source to the destination.
        /// </summary>
        NotSourceCopy = 6,

        /// <summary>
        /// Combines the colors of the source and the brush pattern 
        /// using NOT then AND.
        /// </summary>
        NotSourceErase = 7,

        /// <summary>
        /// Copies the brush to the destination.
        /// </summary>
        PatCopy = 8,

        /// <summary>
        /// Combines the colors of the brush with the destination using XOR.
        /// </summary>
        PatInvert = 9,

        /// <summary>
        /// Combines the brush with the inverted source using OR.
        /// </summary>
        PatPaint = 10,

        /// <summary>
        /// Inverts the destination area.
        /// </summary>
        DestInvert = 11,

        /// <summary>
        /// Fills the destination area with black.
        /// </summary>
        Blackness = 12,

        /// <summary>
        /// Fills the destination area with white.
        /// </summary>
        Whiteness = 13,

        /// <summary>
        /// Used for capturing layered windows during bit-block transfers.
        /// </summary>
        CaptureBlt = 14,

        /// <summary>
        /// No mirror on the bitmap operation.
        /// </summary>
        NoMirrorBitmap = 15
    }
}
