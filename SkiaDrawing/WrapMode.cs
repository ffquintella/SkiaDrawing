namespace SkiaDrawing
{
    /// <summary>
    /// Specifies how textures or images are tiled or clamped 
    /// when they are smaller or larger than the destination area.
    /// Mimics System.Drawing.Drawing2D.WrapMode.
    /// </summary>
    public enum WrapMode
    {
        /// <summary>
        /// Tiles the image infinitely in all directions.
        /// </summary>
        Tile = 0,

        /// <summary>
        /// Clamps the image to the edges, no repetition.
        /// </summary>
        Clamp = 1,

        /// <summary>
        /// Mirrors the image horizontally and vertically, repeating.
        /// </summary>
        TileFlipXY = 2,

        /// <summary>
        /// Mirrors the image horizontally, repeating.
        /// </summary>
        TileFlipX = 3,

        /// <summary>
        /// Mirrors the image vertically, repeating.
        /// </summary>
        TileFlipY = 4
    }
}