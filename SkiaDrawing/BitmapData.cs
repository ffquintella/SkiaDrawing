namespace SkiaDrawing
{
    /// <summary>
    /// A simple class that mimics System.Drawing.Imaging.BitmapData for locked bitmap regions.
    /// </summary>
    public class BitmapData
    {
        /// <summary>
        /// Pointer to the first scan line of the locked data.
        /// </summary>
        public System.IntPtr Scan0 { get; internal set; }

        /// <summary>
        /// Stride (bytes per row) of the locked data region.
        /// </summary>
        public int Stride { get; internal set; }

        /// <summary>
        /// Width of the locked region (in pixels).
        /// </summary>
        public int Width { get; internal set; }

        /// <summary>
        /// Height of the locked region (in pixels).
        /// </summary>
        public int Height { get; internal set; }

        /// <summary>
        /// PixelFormat of the locked data.
        /// </summary>
        public PixelFormat PixelFormat { get; internal set; }

        /// <summary>
        /// The ImageLockMode used when locking the bits (read-only, write-only, etc.).
        /// </summary>
        public ImageLockMode LockMode { get; internal set; }
    }
}