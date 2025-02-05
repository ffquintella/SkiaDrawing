namespace SkiaDrawing
{
    /// <summary>
    /// Mimics System.Drawing.Drawing2D.InterpolationMode in a simplified manner,
    /// allowing you to specify how images are interpolated (scaled, drawn) by Graphics.
    /// </summary>
    public enum InterpolationMode
    {
        /// <summary>
        /// Default interpolation mode (usually medium quality).
        /// </summary>
        Default,

        /// <summary>
        /// Low quality, often nearest-neighbor or similar.
        /// </summary>
        Low,

        /// <summary>
        /// High quality, often bicubic or similar.
        /// </summary>
        High,

        /// <summary>
        /// Bilinear interpolation.
        /// </summary>
        Bilinear,

        /// <summary>
        /// Bicubic interpolation.
        /// </summary>
        Bicubic,

        /// <summary>
        /// Nearest-neighbor interpolation (no smoothing).
        /// </summary>
        NearestNeighbor,

        /// <summary>
        /// High-quality bilinear.
        /// </summary>
        HighQualityBilinear,

        /// <summary>
        /// High-quality bicubic.
        /// </summary>
        HighQualityBicubic
    }
}