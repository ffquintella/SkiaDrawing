
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A simplified version of System.Drawing.Imaging.ImageFormat, mapped to SkiaSharp's SKEncodedImageFormat.
    /// </summary>
    public enum ImageFormat
    {
        Bmp,
        Gif,
        Ico,
        Jpeg,
        Png,
        Webp
        // You can add others if supported by your version of SkiaSharp, e.g. Heif, Avif, etc.
    }

    /// <summary>
    /// Extension methods to convert between the custom ImageFormat and SkiaSharp's SKEncodedImageFormat.
    /// </summary>
    public static class ImageFormatExtensions
    {
        /// <summary>
        /// Converts the custom ImageFormat enum to the corresponding SKEncodedImageFormat.
        /// </summary>
        /// <param name="format">The custom ImageFormat.</param>
        /// <returns>The corresponding SKEncodedImageFormat.</returns>
        /// <exception cref="System.NotSupportedException">Thrown if the given format is not supported by this mapping.</exception>
        public static SKEncodedImageFormat ToSKEncodedImageFormat(this ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Bmp:
                    return SKEncodedImageFormat.Bmp;
                case ImageFormat.Gif:
                    return SKEncodedImageFormat.Gif;
                case ImageFormat.Ico:
                    return SKEncodedImageFormat.Ico;
                case ImageFormat.Jpeg:
                    return SKEncodedImageFormat.Jpeg;
                case ImageFormat.Png:
                    return SKEncodedImageFormat.Png;
                case ImageFormat.Webp:
                    return SKEncodedImageFormat.Webp;
                default:
                    throw new System.NotSupportedException($"The image format '{format}' is not supported.");
            }
        }
    }
}
