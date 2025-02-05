using SkiaSharp;

namespace SkiaDrawing
{
    public static class InterpolationModeExtensions
    {
        /// <summary>
        /// Converts an InterpolationMode to a SkiaSharp SKFilterQuality.
        /// </summary>
        /// <param name="mode">The InterpolationMode to convert.</param>
        /// <returns>A corresponding SKFilterQuality.</returns>
        public static SKFilterQuality ToSKFilterQuality(this InterpolationMode mode)
        {
            switch (mode)
            {
                case InterpolationMode.Low:
                case InterpolationMode.NearestNeighbor:
                    // Possibly no smoothing
                    return SKFilterQuality.None;

                case InterpolationMode.Bilinear:
                case InterpolationMode.Default:
                    // Basic smoothing
                    return SKFilterQuality.Low;

                case InterpolationMode.Bicubic:
                    // You can choose Medium or High based on preference
                    return SKFilterQuality.Medium;

                case InterpolationMode.High:
                case InterpolationMode.HighQualityBilinear:
                case InterpolationMode.HighQualityBicubic:
                    // Highest quality smoothing
                    return SKFilterQuality.High;

                // If not recognized, default to Low
                default:
                    return SKFilterQuality.Low;
            }
        }
    }
}