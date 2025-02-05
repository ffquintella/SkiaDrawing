namespace SkiaDrawing
{
    /// <summary>
    /// A simple enum to mimic System.Drawing.FontStyle,
    /// allowing flags like Bold, Italic, Underline, Strikeout.
    /// </summary>
    [System.Flags]
    public enum FontStyle
    {
        Regular   = 0,
        Bold      = 1,
        Italic    = 2,
        Underline = 4,
        Strikeout = 8
    }
}