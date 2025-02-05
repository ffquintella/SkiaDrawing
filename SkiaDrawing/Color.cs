using SkiaSharp;

namespace SkiaDrawing
{
    /// <summary>
    /// A color struct that mimics System.Drawing.Color, backed by a SkiaSharp SKColor.
    /// Includes static named colors, ARGB creation, and a method to convert to SKColor.
    /// </summary>
    public struct Color
    {
        private readonly SKColor _color;
        private readonly string _name;

        #region Private Constructor

        /// <summary>
        /// Private constructor wrapping a Skia color plus an optional name.
        /// </summary>
        private Color(SKColor color, string name = null)
        {
            _color = color;
            _name = name;
        }

        #endregion

        #region ARGB / Creation Methods

        /// <summary>
        /// Creates a Color from the specified ARGB components.
        /// </summary>
        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            // Clamp values to [0..255].
            if (alpha < 0) alpha = 0; if (alpha > 255) alpha = 255;
            if (red < 0)   red   = 0; if (red   > 255) red   = 255;
            if (green < 0) green = 0; if (green > 255) green = 255;
            if (blue < 0)  blue  = 0; if (blue  > 255) blue  = 255;

            var sk = new SKColor((byte)red, (byte)green, (byte)blue, (byte)alpha);
            return new Color(sk);
        }

        /// <summary>
        /// Creates a fully opaque Color from the specified R, G, B components (alpha = 255).
        /// </summary>
        public static Color FromArgb(byte red, byte green, byte blue)
        {
            return FromArgb(255, red, green, blue);
        }
        
        /// <summary>
        /// Creates a new Color with the specified alpha but preserves the base color's RGB values.
        /// </summary>
        /// <param name="alpha">The new alpha (transparency) value.</param>
        /// <param name="baseColor">The base color whose RGB values will be retained.</param>
        /// <returns>A new Color with the modified alpha value.</returns>
        public static Color FromArgb(byte alpha, Color baseColor)
        {
            return new Color(new SKColor(baseColor.R, baseColor.G, baseColor.B, alpha), baseColor._name);
        }

        /// <summary>
        /// Creates a Color from a SkiaSharp SKColor.
        /// </summary>
        public static Color FromSKColor(SKColor skColor)
        {
            return new Color(skColor);
        }

        #endregion

        #region Named Helper

        /// <summary>
        /// A helper that creates a named color with alpha=255.
        /// </summary>
        private static Color Named(byte r, byte g, byte b, string name)
        {
            SKColor sk = new SKColor(r, g, b, 255);
            return new Color(sk, name);
        }

        #endregion

        #region Static Named Colors

        // A small set of commonly used named colors. Extend as needed.

        public static Color Transparent => new Color(new SKColor(255, 255, 255, 0), "Transparent");
        public static Color Black       => Named(0x00, 0x00, 0x00, "Black");
        public static Color Lime        => Named(0x00, 0xFF, 0x00, "Lime");
        public static Color Blue        => Named(0x00, 0x00, 0xFF, "Blue");
        public static Color Gray        => Named(0x80, 0x80, 0x80, "Gray");
        public static Color AliceBlue => Named(0xF0, 0xF8, 0xFF, "AliceBlue");
        public static Color AntiqueWhite => Named(0xFA, 0xEB, 0xD7, "AntiqueWhite");
        public static Color Aqua => Named(0x00, 0xFF, 0xFF, "Aqua");
        public static Color Aquamarine => Named(0x7F, 0xFF, 0xD4, "Aquamarine");
        public static Color Azure => Named(0xF0, 0xFF, 0xFF, "Azure");
        public static Color Beige => Named(0xF5, 0xF5, 0xDC, "Beige");
        public static Color Bisque => Named(0xFF, 0xE4, 0xC4, "Bisque");
        public static Color BlanchedAlmond => Named(0xFF, 0xEB, 0xCD, "BlanchedAlmond");
        public static Color BlueViolet => Named(0x8A, 0x2B, 0xE2, "BlueViolet");
        public static Color Brown => Named(0xA5, 0x2A, 0x2A, "Brown");
        public static Color BurlyWood => Named(0xDE, 0xB8, 0x87, "BurlyWood");
        public static Color CadetBlue => Named(0x5F, 0x9E, 0xA0, "CadetBlue");
        public static Color Chartreuse => Named(0x7F, 0xFF, 0x00, "Chartreuse");
        public static Color Chocolate => Named(0xD2, 0x69, 0x1E, "Chocolate");
        public static Color Coral => Named(0xFF, 0x7F, 0x50, "Coral");
        public static Color CornflowerBlue => Named(0x64, 0x95, 0xED, "CornflowerBlue");
        public static Color Cornsilk => Named(0xFF, 0xF8, 0xDC, "Cornsilk");
        public static Color Crimson => Named(0xDC, 0x14, 0x3C, "Crimson");
        public static Color Cyan => Named(0x00, 0xFF, 0xFF, "Cyan");
        public static Color DarkBlue => Named(0x00, 0x00, 0x8B, "DarkBlue");
        public static Color DarkCyan => Named(0x00, 0x8B, 0x8B, "DarkCyan");
        public static Color DarkGoldenrod => Named(0xB8, 0x86, 0x0B, "DarkGoldenrod");
        public static Color DarkGray => Named(0xA9, 0xA9, 0xA9, "DarkGray");
        public static Color DarkGreen => Named(0x00, 0x64, 0x00, "DarkGreen");
        public static Color DarkKhaki => Named(0xBD, 0xB7, 0x6B, "DarkKhaki");
        public static Color DarkMagenta => Named(0x8B, 0x00, 0x8B, "DarkMagenta");
        public static Color DarkOliveGreen => Named(0x55, 0x6B, 0x2F, "DarkOliveGreen");
        public static Color DarkOrange => Named(0xFF, 0x8C, 0x00, "DarkOrange");
        public static Color DarkOrchid => Named(0x99, 0x32, 0xCC, "DarkOrchid");
        public static Color DarkRed => Named(0x8B, 0x00, 0x00, "DarkRed");
        public static Color DarkSalmon => Named(0xE9, 0x96, 0x7A, "DarkSalmon");
        public static Color DarkSeaGreen => Named(0x8F, 0xBC, 0x8F, "DarkSeaGreen");
        public static Color DarkSlateBlue => Named(0x48, 0x3D, 0x8B, "DarkSlateBlue");
        public static Color DarkSlateGray => Named(0x2F, 0x4F, 0x4F, "DarkSlateGray");
        public static Color DarkTurquoise => Named(0x00, 0xCE, 0xD1, "DarkTurquoise");
        public static Color DarkViolet => Named(0x94, 0x00, 0xD3, "DarkViolet");
        public static Color DeepPink => Named(0xFF, 0x14, 0x93, "DeepPink");
        public static Color DeepSkyBlue => Named(0x00, 0xBF, 0xFF, "DeepSkyBlue");
        public static Color DimGray => Named(0x69, 0x69, 0x69, "DimGray");
        public static Color DodgerBlue => Named(0x1E, 0x90, 0xFF, "DodgerBlue");
        public static Color Firebrick => Named(0xB2, 0x22, 0x22, "Firebrick");
        public static Color FloralWhite => Named(0xFF, 0xFA, 0xF0, "FloralWhite");
        public static Color ForestGreen => Named(0x22, 0x8B, 0x22, "ForestGreen");
        public static Color Fuchsia => Named(0xFF, 0x00, 0xFF, "Fuchsia");
        public static Color Gainsboro => Named(0xDC, 0xDC, 0xDC, "Gainsboro");
        public static Color GhostWhite => Named(0xF8, 0xF8, 0xFF, "GhostWhite");
        public static Color Gold => Named(0xFF, 0xD7, 0x00, "Gold");
        public static Color Goldenrod => Named(0xDA, 0xA5, 0x20, "Goldenrod");
        public static Color Green => Named(0x00, 0x80, 0x00, "Green");
        public static Color GreenYellow => Named(0xAD, 0xFF, 0x2F, "GreenYellow");
        public static Color Honeydew => Named(0xF0, 0xFF, 0xF0, "Honeydew");
        public static Color HotPink => Named(0xFF, 0x69, 0xB4, "HotPink");
        public static Color IndianRed => Named(0xCD, 0x5C, 0x5C, "IndianRed");
        public static Color Indigo => Named(0x4B, 0x00, 0x82, "Indigo");
        public static Color Ivory => Named(0xFF, 0xFF, 0xF0, "Ivory");
        public static Color Khaki => Named(0xF0, 0xE6, 0x8C, "Khaki");
        public static Color Lavender => Named(0xE6, 0xE6, 0xFA, "Lavender");
        public static Color LavenderBlush => Named(0xFF, 0xF0, 0xF5, "LavenderBlush");
        public static Color LawnGreen => Named(0x7C, 0xFC, 0x00, "LawnGreen");
        public static Color LemonChiffon => Named(0xFF, 0xFA, 0xCD, "LemonChiffon");
        public static Color LightBlue => Named(0xAD, 0xD8, 0xE6, "LightBlue");
        public static Color LightCoral => Named(0xF0, 0x80, 0x80, "LightCoral");
        public static Color LightCyan => Named(0xE0, 0xFF, 0xFF, "LightCyan");
        public static Color LightGoldenrodYellow => Named(0xFA, 0xFA, 0xD2, "LightGoldenrodYellow");
        public static Color LightGray => Named(0xD3, 0xD3, 0xD3, "LightGray");
        public static Color LightGreen => Named(0x90, 0xEE, 0x90, "LightGreen");
        public static Color LightPink => Named(0xFF, 0xB6, 0xC1, "LightPink");
        public static Color LightSalmon => Named(0xFF, 0xA0, 0x7A, "LightSalmon");
        public static Color LightSeaGreen => Named(0x20, 0xB2, 0xAA, "LightSeaGreen");
        public static Color LightSkyBlue => Named(0x87, 0xCE, 0xFA, "LightSkyBlue");
        public static Color LightSlateGray => Named(0x77, 0x88, 0x99, "LightSlateGray");
        public static Color LightSteelBlue => Named(0xB0, 0xC4, 0xDE, "LightSteelBlue");
        public static Color LightYellow => Named(0xFF, 0xFF, 0xE0, "LightYellow");
        public static Color LimeGreen => Named(0x32, 0xCD, 0x32, "LimeGreen");
        public static Color Linen => Named(0xFA, 0xF0, 0xE6, "Linen");
        public static Color Magenta => Named(0xFF, 0x00, 0xFF, "Magenta");
        public static Color Maroon => Named(0x80, 0x00, 0x00, "Maroon");
        public static Color MediumAquamarine => Named(0x66, 0xCD, 0xAA, "MediumAquamarine");
        public static Color MediumBlue => Named(0x00, 0x00, 0xCD, "MediumBlue");
        public static Color MediumOrchid => Named(0xBA, 0x55, 0xD3, "MediumOrchid");
        public static Color MediumPurple => Named(0x93, 0x70, 0xDB, "MediumPurple");
        public static Color MediumSeaGreen => Named(0x3C, 0xB3, 0x71, "MediumSeaGreen");
        public static Color MediumSlateBlue => Named(0x7B, 0x68, 0xEE, "MediumSlateBlue");
        public static Color MediumSpringGreen => Named(0x00, 0xFA, 0x9A, "MediumSpringGreen");
        public static Color MediumTurquoise => Named(0x48, 0xD1, 0xCC, "MediumTurquoise");
        public static Color MediumVioletRed => Named(0xC7, 0x15, 0x85, "MediumVioletRed");
        public static Color MidnightBlue => Named(0x19, 0x19, 0x70, "MidnightBlue");
        public static Color MintCream => Named(0xF5, 0xFF, 0xFA, "MintCream");
        public static Color MistyRose => Named(0xFF, 0xE4, 0xE1, "MistyRose");
        public static Color Moccasin => Named(0xFF, 0xE4, 0xB5, "Moccasin");
        public static Color NavajoWhite => Named(0xFF, 0xDE, 0xAD, "NavajoWhite");
        public static Color Navy => Named(0x00, 0x00, 0x80, "Navy");
        public static Color OldLace => Named(0xFD, 0xF5, 0xE6, "OldLace");
        public static Color Olive => Named(0x80, 0x80, 0x00, "Olive");
        public static Color OliveDrab => Named(0x6B, 0x8E, 0x23, "OliveDrab");
        public static Color Orange => Named(0xFF, 0xA5, 0x00, "Orange");
        public static Color OrangeRed => Named(0xFF, 0x45, 0x00, "OrangeRed");
        public static Color Orchid => Named(0xDA, 0x70, 0xD6, "Orchid");
        public static Color PaleGoldenrod => Named(0xEE, 0xE8, 0xAA, "PaleGoldenrod");
        public static Color PaleGreen => Named(0x98, 0xFB, 0x98, "PaleGreen");
        public static Color PaleTurquoise => Named(0xAF, 0xEE, 0xEE, "PaleTurquoise");
        public static Color PaleVioletRed => Named(0xDB, 0x70, 0x93, "PaleVioletRed");
        public static Color PapayaWhip => Named(0xFF, 0xEF, 0xD5, "PapayaWhip");
        public static Color PeachPuff => Named(0xFF, 0xDA, 0xB9, "PeachPuff");
        public static Color Peru => Named(0xCD, 0x85, 0x3F, "Peru");
        public static Color Pink => Named(0xFF, 0xC0, 0xCB, "Pink");
        public static Color Plum => Named(0xDD, 0xA0, 0xDD, "Plum");
        public static Color PowderBlue => Named(0xB0, 0xE0, 0xE6, "PowderBlue");
        public static Color Purple => Named(0x80, 0x00, 0x80, "Purple");
        public static Color RebeccaPurple => Named(0x66, 0x33, 0x99, "RebeccaPurple");
        public static Color Red => Named(0xFF, 0x00, 0x00, "Red");
        public static Color RosyBrown => Named(0xBC, 0x8F, 0x8F, "RosyBrown");
        public static Color RoyalBlue => Named(0x41, 0x69, 0xE1, "RoyalBlue");
        public static Color SaddleBrown => Named(0x8B, 0x45, 0x13, "SaddleBrown");
        public static Color Salmon => Named(0xFA, 0x80, 0x72, "Salmon");
        public static Color SandyBrown => Named(0xF4, 0xA4, 0x60, "SandyBrown");
        public static Color SeaGreen => Named(0x2E, 0x8B, 0x57, "SeaGreen");
        public static Color SeaShell => Named(0xFF, 0xF5, 0xEE, "SeaShell");
        public static Color Sienna => Named(0xA0, 0x52, 0x2D, "Sienna");
        public static Color Silver => Named(0xC0, 0xC0, 0xC0, "Silver");
        public static Color SkyBlue => Named(0x87, 0xCE, 0xEB, "SkyBlue");
        public static Color SlateBlue => Named(0x6A, 0x5A, 0xCD, "SlateBlue");
        public static Color SlateGray => Named(0x70, 0x80, 0x90, "SlateGray");
        public static Color Snow => Named(0xFF, 0xFA, 0xFA, "Snow");
        public static Color SpringGreen => Named(0x00, 0xFF, 0x7F, "SpringGreen");
        public static Color SteelBlue => Named(0x46, 0x82, 0xB4, "SteelBlue");
        public static Color Tan => Named(0xD2, 0xB4, 0x8C, "Tan");
        public static Color Teal => Named(0x00, 0x80, 0x80, "Teal");
        public static Color Thistle => Named(0xD8, 0xBF, 0xD8, "Thistle");
        public static Color Tomato => Named(0xFF, 0x63, 0x47, "Tomato");
        public static Color Turquoise => Named(0x40, 0xE0, 0xD0, "Turquoise");
        public static Color Violet => Named(0xEE, 0x82, 0xEE, "Violet");
        public static Color Wheat => Named(0xF5, 0xDE, 0xB3, "Wheat");
        public static Color White => Named(0xFF, 0xFF, 0xFF, "White");
        public static Color WhiteSmoke => Named(0xF5, 0xF5, 0xF5, "WhiteSmoke");
        public static Color Yellow => Named(0xFF, 0xFF, 0x00, "Yellow");
        public static Color YellowGreen => Named(0x9A, 0xCD, 0x32, "YellowGreen");
        
        
        
        public static Color Empty      =>  new Color(new SKColor(0, 0, 0, 0), "Empty");

        #endregion

        #region ARGB Properties

        /// <summary>
        /// Gets the alpha component of this color.
        /// </summary>
        public byte A => _color.Alpha;

        /// <summary>
        /// Gets the red component of this color.
        /// </summary>
        public byte R => _color.Red;

        /// <summary>
        /// Gets the green component of this color.
        /// </summary>
        public byte G => _color.Green;

        /// <summary>
        /// Gets the blue component of this color.
        /// </summary>
        public byte B => _color.Blue;

        /// <summary>
        /// Indicates whether this Color is empty (i.e., alpha=0 and RGB=0).
        /// </summary>
        public bool IsEmpty => _color.Equals(default(SKColor));

        /// <summary>
        /// If this color was created as a named color, returns that name;
        /// otherwise returns the ARGB hex code.
        /// </summary>
        public string Name
        {
            get
            {
                if (!string.IsNullOrEmpty(_name))
                    return _name;
                
                // Return ARGB as hex if unnamed
                int argb = (A << 24) | (R << 16) | (G << 8) | B;
                return argb.ToString("X8");
            }
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Returns the underlying SkiaSharp SKColor for advanced usage.
        /// </summary>
        public SKColor ToSKColor()
        {
            return _color;
        }

        /// <summary>
        /// Returns the ARGB value as a 32-bit integer (0xAARRGGBB).
        /// </summary>
        public int ToArgb()
        {
            return (A << 24) | (R << 16) | (G << 8) | B;
        }

        #endregion

        #region Equality / Overrides

        public override bool Equals(object obj)
        {
            if (obj is Color c)
                return _color.Equals(c._color);
            return false;
        }

        public override int GetHashCode()
        {
            // Combine the SKColor hash with the optional name's hash
            return _color.GetHashCode() ^ (_name?.GetHashCode() ?? 0);
        }

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"Color [A={A}, R={R}, G={G}, B={B}, Name={Name}]";
        }

        #endregion
    }
}
