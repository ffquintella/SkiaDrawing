namespace SkiaDrawing
{
    /// <summary>
    /// Defines text alignment in a layout box, mimicking System.Drawing.StringAlignment.
    /// </summary>
    public enum StringAlignment
    {
        Near   = 0, // Align text to the left (or top for vertical).
        Center = 1, // Align text to the center.
        Far    = 2  // Align text to the right (or bottom for vertical).
    }
}