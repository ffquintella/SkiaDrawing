using System;

namespace SkiaDrawing
{
    /// <summary>
    /// Specifies flags that control the behavior of locking a bitmap’s bits (similar to System.Drawing.Imaging.ImageLockMode).
    /// </summary>
    [Flags]
    public enum ImageLockMode
    {
        /// <summary>
        /// Indicates that the image is locked for reading only.
        /// </summary>
        ReadOnly = 1,

        /// <summary>
        /// Indicates that the image is locked for writing only.
        /// </summary>
        WriteOnly = 2,

        /// <summary>
        /// Indicates that the image is locked for both reading and writing.
        /// </summary>
        ReadWrite = 3,

        /// <summary>
        /// Indicates that the buffer used for reading or writing pixel data is provided by the user.
        /// </summary>
        UserInputBuffer = 4
    }
}