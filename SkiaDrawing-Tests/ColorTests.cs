using System;
using Xunit;
using SkiaSharp;
using SkiaDrawing;

namespace SkiaDrawing.Tests
{
    public class ColorTests
    {
        [Fact]
        public void FromArgb_CreatesColorWithCorrectComponents()
        {
            var color = Color.FromArgb(255, 128, 64, 32);
            Assert.Equal(255, color.A);
            Assert.Equal(128, color.R);
            Assert.Equal(64, color.G);
            Assert.Equal(32, color.B);
        }

        [Fact]
        public void FromArgb_ClampsValuesToValidRange()
        {
            var color = Color.FromArgb(300, -10, 500, -20);
            Assert.Equal(255, color.A);
            Assert.Equal(0, color.R);
            Assert.Equal(255, color.G);
            Assert.Equal(0, color.B);
        }

        [Fact]
        public void FromArgb_WithBaseColorPreservesRGB()
        {
            var baseColor = Color.FromArgb(255, 100, 150, 200);
            var color = Color.FromArgb(128, baseColor);
            Assert.Equal(128, color.A);
            Assert.Equal(100, color.R);
            Assert.Equal(150, color.G);
            Assert.Equal(200, color.B);
        }

        [Fact]
        public void FromSKColor_CreatesColorFromSKColor()
        {
            var skColor = new SKColor(10, 20, 30, 40);
            var color = Color.FromSKColor(skColor);
            Assert.Equal(40, color.A);
            Assert.Equal(10, color.R);
            Assert.Equal(20, color.G);
            Assert.Equal(30, color.B);
        }

        [Fact]
        public void IsEmpty_ReturnsTrueForEmptyColor()
        {
            var color = Color.Empty;
            Assert.True(color.IsEmpty);
        }

        [Fact]
        public void IsEmpty_ReturnsFalseForNonEmptyColor()
        {
            var color = Color.FromArgb(255, 0, 0, 0);
            Assert.False(color.IsEmpty);
        }

        [Fact]
        public void Name_ReturnsColorNameIfNamed()
        {
            var color = Color.Black;
            Assert.Equal("Black", color.Name);
        }

        [Fact]
        public void Name_ReturnsHexIfUnnamed()
        {
            var color = Color.FromArgb(255, 0, 0, 0);
            Assert.Equal("FF000000", color.Name);
        }

        [Fact]
        public void ToSKColor_ReturnsUnderlyingSKColor()
        {
            var color = Color.FromArgb(255, 100, 150, 200);
            var skColor = color.ToSKColor();
            Assert.Equal(new SKColor(100, 150, 200, 255), skColor);
        }

        [Fact]
        public void ToArgb_ReturnsCorrectArgbValue()
        {
            var color = Color.FromArgb(255, 100, 150, 200);
            Assert.Equal(0xFF6496C8, (uint)color.ToArgb());
        }

        [Fact]
        public void Equals_ReturnsTrueForEqualColors()
        {
            var color1 = Color.FromArgb(255, 100, 150, 200);
            var color2 = Color.FromArgb(255, 100, 150, 200);
            Assert.True(color1.Equals(color2));
        }

        [Fact]
        public void Equals_ReturnsFalseForDifferentColors()
        {
            var color1 = Color.FromArgb(255, 100, 150, 200);
            var color2 = Color.FromArgb(255, 100, 150, 201);
            Assert.False(color1.Equals(color2));
        }

        [Fact]
        public void GetHashCode_ReturnsSameHashCodeForEqualColors()
        {
            var color1 = Color.FromArgb(255, 100, 150, 200);
            var color2 = Color.FromArgb(255, 100, 150, 200);
            Assert.Equal(color1.GetHashCode(), color2.GetHashCode());
        }

        [Fact]
        public void ToString_ReturnsCorrectStringRepresentation()
        {
            var color = Color.FromArgb(255, 100, 150, 200);
            Assert.Equal("Color [A=255, R=100, G=150, B=200, Name=FF6496C8]", color.ToString());
        }
    }
}