using System;
using System.IO;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A simple Icon class that mimics some of the functionality of System.Drawing.Icon
    /// but is implemented using SkiaSharp.
    /// </summary>
    public class Icon : IDisposable
    {
        private SKImage image;

        /// <summary>
        /// Initializes a new instance of the Icon class from an SKImage.
        /// </summary>
        /// <param name="image">The SKImage to wrap as an icon.</param>
        public Icon(SKImage image)
        {
            this.image = image ?? throw new ArgumentNullException(nameof(image));
        }

        /// <summary>
        /// Gets the width of the icon.
        /// </summary>
        public int Width => image?.Width ?? 0;

        /// <summary>
        /// Gets the height of the icon.
        /// </summary>
        public int Height => image?.Height ?? 0;

        /// <summary>
        /// Loads an icon from a file.
        /// </summary>
        /// <param name="fileName">The file path of the icon.</param>
        /// <returns>An Icon instance.</returns>
        public static Icon FromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            using (var data = SKData.Create(fileName))
            {
                SKImage img = SKImage.FromEncodedData(data);
                if (img == null)
                    throw new Exception("Unable to decode the icon from file.");
                return new Icon(img);
            }
        }

        /// <summary>
        /// Loads an icon from a stream.
        /// </summary>
        /// <param name="stream">A stream containing the encoded icon data.</param>
        /// <returns>An Icon instance.</returns>
        public static Icon FromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            // SKData.Create(Stream) does not dispose the stream.
            using (var data = SKData.Create(stream))
            {
                SKImage img = SKImage.FromEncodedData(data);
                if (img == null)
                    throw new Exception("Unable to decode the icon from the stream.");
                return new Icon(img);
            }
        }

        /// <summary>
        /// Converts the icon to an SKBitmap.
        /// </summary>
        /// <returns>An SKBitmap representing the icon.</returns>
        public SKBitmap ToSKBitmap()
        {
            if (image == null)
                throw new ObjectDisposedException(nameof(Icon));
            return SKBitmap.FromImage(image);
        }

        /// <summary>
        /// Saves the icon to the specified stream in the given image format.
        /// </summary>
        /// <param name="stream">The output stream.</param>
        /// <param name="format">The desired image format.</param>
        /// <param name="quality">The quality (0-100) for encoding.</param>
        public void Save(Stream stream, SKEncodedImageFormat format, int quality = 100)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (image == null)
                throw new ObjectDisposedException(nameof(Icon));

            using (var data = image.Encode(format, quality))
            {
                data.SaveTo(stream);
            }
        }

        /// <summary>
        /// Releases all resources used by the Icon.
        /// </summary>
        public void Dispose()
        {
            image?.Dispose();
            image = null;
        }

        /// <summary>
        /// Returns a string that represents the current Icon.
        /// </summary>
        public override string ToString()
        {
            return $"Icon: {Width} x {Height}";
        }
    }
}