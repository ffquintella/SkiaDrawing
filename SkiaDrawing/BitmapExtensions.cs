using System;
using SkiaSharp;

namespace SkiaDrawing
{
    public static class BitmapExtensions
    {
        public static void RotateFlip(this Bitmap bmp, RotateFlipType rft)
        {
            if (bmp == null)
                throw new ArgumentNullException(nameof(bmp));

            // Extract rotation (0..3) from the lower 2 bits
            // and flip (0..3) from the upper bits
            int rotateIndex = (int)rft % 4;  // 0=none, 1=90, 2=180, 3=270
            int flipIndex = (int)rft / 4;    // 0=none, 1=flipX, 2=flipY, 3=flipXY

            // Original width/height
            int oldW = bmp.Width;
            int oldH = bmp.Height;

            if (oldW <= 0 || oldH <= 0)
                return; // Nothing to do

            // Determine final width/height after rotation
            int newW = oldW;
            int newH = oldH;

            // If rotating 90 or 270, we swap width/height
            if (rotateIndex == 1 || rotateIndex == 3)
            {
                newW = oldH;
                newH = oldW;
            }

            // Create a new SKBitmap with the final dimensions, same color type as the original
            var oldSKB = bmp.ToSKBitmap();
            var info = new SKImageInfo(newW, newH, oldSKB.ColorType, oldSKB.AlphaType);
            var newSKB = new SKBitmap(info);

            // We'll draw the old bitmap into the new one, applying transformations
            SKCanvas canvas = new SKCanvas(newSKB);

            // Apply transformations
            // 1) Rotation
            switch (rotateIndex)
            {
                case 0: // none
                    break;
                case 1: // 90
                    canvas.Translate(newW, 0); // shift right
                    canvas.RotateDegrees(90);
                    break;
                case 2: // 180
                    canvas.Translate(newW, newH);
                    canvas.RotateDegrees(180);
                    break;
                case 3: // 270
                    canvas.Translate(0, newH);
                    canvas.RotateDegrees(270);
                    break;
            }

            // 2) Flip
            switch (flipIndex)
            {
                case 0: // none
                    break;
                case 1: // flip X (horizontal)
                    canvas.Scale(-1, 1);
                    canvas.Translate(-newW, 0);
                    break;
                case 2: // flip Y (vertical)
                    canvas.Scale(1, -1);
                    canvas.Translate(0, -newH);
                    break;
                case 3: // flip XY
                    canvas.Scale(-1, -1);
                    canvas.Translate(-newW, -newH);
                    break;
            }

            // Draw the old bitmap at (0,0) after the transformations
            canvas.DrawBitmap(oldSKB, 0, 0);
            canvas.Dispose();

            // Replace the old SKBitmap in 'bmp' with newSKB
            // Dispose the old one if desired
            oldSKB.Dispose();

            // We'll set the new SKBitmap in the 'bmp' object.
            // For that, we might need a setter or we can do it by re-constructing 'bmp'.
            // Let's do it by reflection or direct approach. 
            // If 'bmp' has an internal field 'skBitmap', we can do:
            bmp.Dispose(); // Dispose old references
            // Now re-construct 'bmp' from the new SKBitmap
            bmp.ReplaceInternalBitmap(newSKB);
        }

        /// <summary>
        /// Helper method to replace the internal SKBitmap in the existing Bitmap with a new one.
        /// We'll add an internal method in the Bitmap to do so. 
        /// 
        /// If you don't want to do this, you can make the field 'skBitmap' internal 
        /// or create a special constructor. 
        /// For demonstration, let's assume we can do something like this:
        /// </summary>
        private static void ReplaceInternalBitmap(this Bitmap bmp, SKBitmap newSKB)
        {
            // We'll reassign the internal field. We rely on a hypothetical 'SetSKBitmap' method 
            // or constructor that sets 'skBitmap'. 
            // We'll do a reflection-based approach for demonstration:
            var field = typeof(Bitmap).GetField("skBitmap", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field == null)
                throw new Exception("Could not find internal field 'skBitmap'.");
            field.SetValue(bmp, newSKB);
        }
    }
}
