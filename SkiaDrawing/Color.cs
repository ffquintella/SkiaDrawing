using System;
using System.Collections.Generic;
using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A struct that mimics System.Drawing.Color, backed by a SkiaSharp SKColor.
    /// Provides ARGB channel properties, named color support, and common static methods.
    /// </summary>
    public struct Color : IEquatable<Color>
    {
        // The underlying Skia color that stores the ARGB data.
        private readonly SKColor _color;

        // If this color is known/named (e.g., "Red", "Green"), we store its name here.
        // Otherwise, it's null or empty.
        private readonly string _name;

        // A static lookup of known color names -> ARGB values (via SKColor).
        // You can expand this dictionary with additional known colors as desired.
        private static readonly Dictionary<string, SKColor> s_knownColors = new Dictionary<string, SKColor>(StringComparer.OrdinalIgnoreCase)
        {
            { "Transparent", new SKColor(0x00, 0x00, 0x00, 0x00) },
            { "Black",       new SKColor(0xFF, 0x00, 0x00, 0x00) },
            { "White",       new SKColor(0xFF, 0xFF, 0xFF, 0xFF) },
            { "Red",         new SKColor(0xFF, 0xFF, 0x00, 0x00) },
            { "Green",       new SKColor(0xFF, 0x00, 0x80, 0x00) },
            { "Blue",        new SKColor(0xFF, 0x00, 0x00, 0xFF) },
            { "Yellow",      new SKColor(0xFF, 0xFF, 0xFF, 0x00) },
            { "Gray",        new SKColor(0xFF, 0x80, 0x80, 0x80) },
            { "Silver",      new SKColor(0xFF, 0xC0, 0xC0, 0xC0) }
        };

        #region Constructors

        /// <summary>
        /// A private constructor used for known/named colors or direct SKColor usage.
        /// </summary>
        /// <param name="color">The underlying Skia color.</param>
        /// <param name="name">An optional name if this color is known.</param>
        private Color(SKColor color, string name = null)
        {
            _color = color;
            _name = name;
        }

        #endregion

        #region Static Predefined Colors

        public static Color Empty => new Color(default(SKColor), null);

        public static Color Transparent => FromKnownColor("Transparent");
        public static Color Black       => FromKnownColor("Black");
        public static Color White       => FromKnownColor("White");
        public static Color Red         => FromKnownColor("Red");
        public static Color Green       => FromKnownColor("Green");
        public static Color Blue        => FromKnownColor("Blue");
        public static Color Yellow      => FromKnownColor("Yellow");
        public static Color Gray        => FromKnownColor("Gray");
        public static Color Silver      => FromKnownColor("Silver");

        #endregion

        #region Properties

        /// <summary>
        /// Gets the alpha component value of this Color.
        /// </summary>
        public byte A => _color.Alpha;

        /// <summary>
        /// Gets the red component value of this Color.
        /// </summary>
        public byte R => _color.Red;

        /// <summary>
        /// Gets the green component value of this Color.
        /// </summary>
        public byte G => _color.Green;

        /// <summary>
        /// Gets the blue component value of this Color.
        /// </summary>
        public byte B => _color.Blue;

        /// <summary>
        /// Gets a value indicating whether this Color is the empty color.
        /// </summary>
        public bool IsEmpty => _color.Equals(default(SKColor));

        /// <summary>
        /// Gets a value indicating whether this Color is a named color.
        /// </summary>
        public bool IsNamedColor => !string.IsNullOrEmpty(_name);

        /// <summary>
        /// Gets a value indicating whether this Color is defined as a known color (one from our dictionary).
        /// </summary>
        public bool IsKnownColor
        {
            get
            {
                // If _name is in our dictionary, we consider it known
                return !string.IsNullOrEmpty(_name) && s_knownColors.ContainsKey(_name);
            }
        }

        /// <summary>
        /// Gets the name of this Color. If it is a known color, returns the known color name.
        /// Otherwise, returns an ARGB string like "ff112233".
        /// </summary>
        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_name))
                    return _name;

                // If there's no known name, we return the ARGB as a hex string
                return ToArgb().ToString("X8");
            }
        }

        #endregion

        #region Static Creation Methods

        /// <summary>
        /// Creates a Color from an ARGB integer.
        /// </summary>
        public static Color FromArgb(int argb)
        {
            byte a = (byte)((argb >> 24) & 0xFF);
            byte r = (byte)((argb >> 16) & 0xFF);
            byte g = (byte)((argb >> 8) & 0xFF);
            byte b = (byte)(argb & 0xFF);

            SKColor skColor = new SKColor(r, g, b, a);
            return new Color(skColor, null);
        }

        /// <summary>
        /// Creates a Color from the specified alpha and an existing Color.
        /// </summary>
        public static Color FromArgb(int alpha, Color baseColor)
        {
            return FromArgb(alpha, baseColor.R, baseColor.G, baseColor.B);
        }

        /// <summary>
        /// Creates a Color from ARGB channels.
        /// </summary>
        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            if ((alpha < 0) || (alpha > 255) ||
                (red < 0)   || (red > 255)   ||
                (green < 0) || (green > 255) ||
                (blue < 0)  || (blue > 255))
            {
                throw new ArgumentOutOfRangeException("ARGB values must be in [0..255].");
            }

            SKColor skColor = new SKColor((byte)red, (byte)green, (byte)blue, (byte)alpha);
            return new Color(skColor, null);
        }

        /// <summary>
        /// Creates a Color from the specified named color (e.g. "Red").
        /// </summary>
        /// <exception cref="ArgumentException">If the name is not in the known-colors dictionary.</exception>
        public static Color FromKnownColor(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (s_knownColors.TryGetValue(name, out SKColor known))
            {
                return new Color(known, name);
            }
            throw new ArgumentException($"Unknown color name '{name}'.", nameof(name));
        }

        /// <summary>
        /// Creates a Color from the specified color name or ARGB hex string.
        /// If 'name' is recognized as a known color, returns that; otherwise tries to parse as ARGB.
        /// </summary>
        public static Color FromName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            // If it's in our known dictionary
            if (s_knownColors.TryGetValue(name, out SKColor known))
            {
                return new Color(known, name);
            }

            // Otherwise, try parse as an ARGB hex, e.g. "FF112233"
            if (int.TryParse(name, System.Globalization.NumberStyles.HexNumber, null, out int argb))
            {
                return FromArgb(argb);
            }

            // Fallback: error
            throw new ArgumentException($"Unknown color name or invalid ARGB hex '{name}'.", nameof(name));
        }

        /// <summary>
        /// Wraps a SkiaSharp SKColor into a new Color.
        /// </summary>
        public static Color FromSKColor(SKColor skColor)
        {
            return new Color(skColor, null);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the ARGB value for this Color as a 32-bit integer: 0xAARRGGBB
        /// </summary>
        public int ToArgb()
        {
            // Format: [A(8 bits), R(8 bits), G(8 bits), B(8 bits)]
            return (A << 24) | (R << 16) | (G << 8) | (B);
        }

        /// <summary>
        /// Converts this Color to the underlying SkiaSharp SKColor.
        /// </summary>
        public SKColor ToSKColor() => _color;

        /// <summary>
        /// Determines whether this color is equal to another color.
        /// </summary>
        public bool Equals(Color other)
        {
            // Compare the SKColor and the name.
            return _color.Equals(other._color) &&
                   string.Equals(_name, other._name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return (obj is Color c) && Equals(c);
        }

        public override int GetHashCode()
        {
            // Combine the ARGB and the name if present
            int argb = ToArgb();
            // Use a simple hash approach
            unchecked
            {
                int hash = 17;
                hash = (hash * 31) + argb;
                if (!string.IsNullOrEmpty(_name))
                    hash = (hash * 31) + _name.ToLowerInvariant().GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            // System.Drawing.Color often outputs in the form: "Color [Name=xxx]"
            // If we have a known name, use that, else show ARGB
            if (IsEmpty)
                return "Color [ Empty ]";

            if (IsNamedColor)
                return $"Color [ {Name} ]";

            // If not named, show ARGB
            return $"Color [ A={A}, R={R}, G={G}, B={B} ]";
        }

        #endregion

        #region Operators

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
