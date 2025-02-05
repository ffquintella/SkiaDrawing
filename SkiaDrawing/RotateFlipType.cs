namespace SkiaDrawing
{
    /// <summary>
    /// Specifies how an image is rotated and flipped.
    /// Mimics System.Drawing.RotateFlipType.
    /// </summary>
    public enum RotateFlipType
    {
        RotateNoneFlipNone = 0,
        Rotate90FlipNone   = 1,
        Rotate180FlipNone  = 2,
        Rotate270FlipNone  = 3,

        RotateNoneFlipX    = 4,
        Rotate90FlipX      = 5,
        Rotate180FlipX     = 6,
        Rotate270FlipX     = 7,

        RotateNoneFlipY    = 8,
        Rotate90FlipY      = 9,
        Rotate180FlipY     = 10,
        Rotate270FlipY     = 11,

        RotateNoneFlipXY   = 12,
        Rotate90FlipXY     = 13,
        Rotate180FlipXY    = 14,
        Rotate270FlipXY    = 15
    }
}