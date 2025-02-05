namespace SkiaDrawing
{
    /// <summary>
    /// Specifies how text is trimmed when it exceeds the layout rectangle,
    /// mimicking System.Drawing.StringTrimming.
    /// </summary>
    public enum StringTrimming
    {
        None           = 0, // No trimming.
        Character      = 1, // Trim at a character boundary.
        Word          = 2, // Trim at a word boundary.
        EllipsisCharacter = 3, // Show "..." at the character boundary.
        EllipsisWord      = 4, // Show "..." at the word boundary.
        EllipsisPath      = 5  // Show "..." for paths (e.g., file paths).
    }
}