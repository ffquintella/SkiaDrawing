namespace SkiaDrawing
{
    /// <summary>
    /// Mimics System.Drawing.Drawing2D.LinearGradientMode,
    /// specifying the orientation of the gradient.
    /// </summary>
    public enum LinearGradientMode
    {
        /// <summary>
        /// A gradient from left to right.
        /// </summary>
        Horizontal = 0,

        /// <summary>
        /// A gradient from top to bottom.
        /// </summary>
        Vertical = 1,

        /// <summary>
        /// A gradient from upper-left corner to lower-right corner.
        /// </summary>
        ForwardDiagonal = 2,

        /// <summary>
        /// A gradient from upper-right corner to lower-left corner.
        /// </summary>
        BackwardDiagonal = 3
    }
}