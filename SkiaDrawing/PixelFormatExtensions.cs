using System;
using SkiaSharp;

namespace SkiaDrawing
{
    public static class PixelFormatExtensions
    {
        /// <summary>
        /// Converts our custom PixelFormat to SkiaSharp's SKColorType, if possible.
        /// Throws NotSupportedException if there is no direct mapping.
        /// </summary>
        public static SKColorType ToSKColorType(this PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Format16bppRgb565:
                    return SKColorType.Rgb565;

                case PixelFormat.Format24bppRgb:
                    // Skia doesn't have a strict "24-bit RGB" type.
                    // Typically we would treat this as 32bpp with alpha = opaque.
                    return SKColorType.Rgba8888;

                case PixelFormat.Format32bppRgb:
                    // GDI+ "32bppRgb" is actually 0x22009, alpha is unused but present.
                    // We'll map to RGBA8888. The alpha bytes will be 0xFF or unused.
                    return SKColorType.Rgba8888;

                case PixelFormat.Format32bppArgb:
                    return SKColorType.Rgba8888;

                case PixelFormat.Format32bppPArgb:
                    // Skia doesn't differentiate "premultiplied" color type separately
                    // from non-premultiplied in SKColorType. We still use Rgba8888.
                    return SKColorType.Rgba8888;

                case PixelFormat.Format8bppGray:
                    return SKColorType.Gray8;
                
                case PixelFormat.Format8bppIndexed:
                    return SKColorType.Gray8;

                // Potential expansions:
                // case PixelFormat.Format64bppArgb:
                //     return SKColorType.Rgba16161616; // (only if your Skia supports it)

                case PixelFormat.Undefined:
                default:
                    throw new NotSupportedException(
                        $"The pixel format '{format}' is not supported or not mapped to a SKColorType.");
            }
        }
    }
}
