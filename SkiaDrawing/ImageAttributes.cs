using System;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// Mimics System.Drawing.Imaging.ImageAttributes in a simplified manner,
    /// storing color transforms (ColorMatrix), gamma, color key, wrap mode, etc.
    /// </summary>
    public class ImageAttributes : IDisposable
    {
        private ColorMatrix colorMatrix;
        private bool hasColorMatrix;

        private float gamma = 1.0f;
        private bool hasGamma;

        private Color colorKeyLow;
        private Color colorKeyHigh;
        private bool hasColorKey;

        private WrapMode wrapMode = WrapMode.Clamp;  // if you define a WrapMode enum
        private Color wrapModeColor = Color.Transparent;
        private bool wrapModeClamp = false;

        private bool disposed;

        #region ColorMatrix

        /// <summary>
        /// Sets a color matrix that defines a 5x5 ARGB transformation.
        /// </summary>
        public void SetColorMatrix(ColorMatrix matrix)
        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));
            colorMatrix = matrix;
            hasColorMatrix = true;
        }

        /// <summary>
        /// Clears the color matrix so that no color-matrix transform is applied.
        /// </summary>
        public void ClearColorMatrix()
        {
            colorMatrix = null;
            hasColorMatrix = false;
        }

        #endregion

        #region Gamma

        /// <summary>
        /// Sets the gamma value for this ImageAttributes (1.0 = no change).
        /// </summary>
        /// <param name="gamma">Gamma correction value (typically in [0.1..5.0]).</param>
        public void SetGamma(float gamma)
        {
            if (gamma <= 0f)
                throw new ArgumentOutOfRangeException(nameof(gamma), "Gamma must be positive.");
            this.gamma = gamma;
            this.hasGamma = true;
        }

        /// <summary>
        /// Clears the gamma setting so no gamma correction is applied.
        /// </summary>
        public void ClearGamma()
        {
            gamma = 1.0f;
            hasGamma = false;
        }

        #endregion

        #region ColorKey

        /// <summary>
        /// Sets a color key range for transparency or special handling.
        /// In GDI+, this is used to specify a color range turned transparent.
        /// </summary>
        /// <param name="lowColor">Lower bound of color key range.</param>
        /// <param name="highColor">Upper bound of color key range.</param>
        public void SetColorKey(Color lowColor, Color highColor)
        {
            colorKeyLow = lowColor;
            colorKeyHigh = highColor;
            hasColorKey = true;
        }

        /// <summary>
        /// Clears the color key setting.
        /// </summary>
        public void ClearColorKey()
        {
            colorKeyLow = Color.Empty;
            colorKeyHigh = Color.Empty;
            hasColorKey = false;
        }

        #endregion

        #region WrapMode

        /// <summary>
        /// Sets the wrap mode for texture fills or image sampling edges.
        /// E.g., Clamp, Tile, etc. Also a border color if needed.
        /// </summary>
        public void SetWrapMode(WrapMode mode, Color color, bool clamp)
        {
            wrapMode = mode;
            wrapModeColor = color;
            wrapModeClamp = clamp;
        }

        #endregion

        #region Create Color Filter (Simplified)

        /// <summary>
        /// Creates or returns an SKColorFilter that merges the current 
        /// color matrix and possibly other adjustments (e.g. gamma).
        /// In a real implementation, you'd combine gamma, color keys, etc. 
        /// Here, we just demonstrate color matrix usage.
        /// </summary>
        /// <returns>An SKColorFilter, or null if no filter is needed.</returns>
        public SKColorFilter GetSKColorFilter()
        {
            // 1) If we have a color matrix, create that filter
            if (hasColorMatrix)
            {
                // Convert to Skia's color filter
                return colorMatrix.ToColorFilter();
            }

            // 2) If we had gamma or color key, we'd do more advanced logic. 
            //    For demonstration, we skip it.

            return null;
        }

        #endregion

        #region Disposal

        public void Dispose()
        {
            if (!disposed)
            {
                // If you had any native resources, free them here. 
                // Typically, color matrix, gamma, etc., are just data.
                disposed = true;
            }
        }

        #endregion

        #region Additional System.Drawing-like Methods

        // System.Drawing.ImageAttributes also has overloads with:
        //  - (ColorMatrix matrix, ColorMatrixFlag, ColorAdjustType)
        //  - (float gamma, ColorAdjustType)
        //  - (Color low, Color high, ColorAdjustType)
        // etc. For simplicity, we skip them or default to 'ColorAdjustType.Default'.
        // You could define them similarly and store separate transforms per category.

        // Also, System.Drawing has .SetThreshold, .SetNoOp, .SetOutputChannel, etc.

        #endregion
    }
}
