namespace SkiaDrawing
{
    /// <summary>
    /// A simplified version of System.Drawing.Imaging.PixelFormat,
    /// mapped to SkiaSharp's SKColorType where possible.
    /// </summary>
    public enum PixelFormat
    {
        Undefined          = 0,
        Format16bppRgb565  = 0x20000, // Example hex values to be unique (as in .NET)
        Format24bppRgb     = 0x21808,
        Format32bppRgb     = 0x22009,
        Format32bppArgb    = 0x26200,
        Format32bppPArgb   = 0x26201,
        Format8bppGray     = 0x20308, // not a GDI+ standard but let's include a gray8 for demonstration
        Format8bppIndexed  = 0x20401,
        // Add others as needed (e.g. Format16bppArgb1555, Format48bppRgb, Format64bppArgb, etc.).
    }
}
