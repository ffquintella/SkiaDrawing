namespace SkiaDrawing.Tests;

public class FontTests
{
       [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        var font = new Font("Arial", 12f, FontStyle.Bold);
        Assert.Equal("Arial", font.FontFamily);
        Assert.Equal(12f, font.Size);
        Assert.Equal(FontStyle.Bold, font.Style);
    }

    [Fact]
    public void Constructor_WithNullFamilyName_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Font(null, 12f));
    }

    [Fact]
    public void Constructor_WithEmptyFamilyName_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Font("", 12f));
    }

    [Fact]
    public void Constructor_WithNegativeSize_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Font("Arial", -1f));
    }

    [Fact]
    public void Bold_ReturnsTrueForBoldFont()
    {
        var font = new Font("Arial", 12f, FontStyle.Bold);
        Assert.True(font.Bold);
    }

    [Fact]
    public void Italic_ReturnsTrueForItalicFont()
    {
        var font = new Font("Arial", 12f, FontStyle.Italic);
        Assert.True(font.Italic);
    }

    [Fact]
    public void Underline_ReturnsTrueForUnderlineFont()
    {
        var font = new Font("Arial", 12f, FontStyle.Underline);
        Assert.True(font.Underline);
    }

    [Fact]
    public void Strikeout_ReturnsTrueForStrikeoutFont()
    {
        var font = new Font("Arial", 12f, FontStyle.Strikeout);
        Assert.True(font.Strikeout);
    }

    [Fact]
    public void ToSKTypeface_ReturnsValidSKTypeface()
    {
        var font = new Font("Arial", 12f);
        var skTypeface = font.ToSKTypeface();
        Assert.NotNull(skTypeface);
    }

    [Fact]
    public void ToSKPaint_ReturnsValidSKPaint()
    {
        var font = new Font("Arial", 12f);
        var skPaint = font.ToSKPaint();
        Assert.NotNull(skPaint);
        Assert.Equal(font.ToSKTypeface(), skPaint.Typeface);
        Assert.Equal(12f * (96f / 72f), skPaint.TextSize);
    }

    [Fact]
    public void Dispose_DisposesSKTypeface()
    {
        var font = new Font("Arial", 12f);
        font.Dispose();
        Assert.Throws<ObjectDisposedException>(() => font.ToSKTypeface());
    }

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        var font = new Font("Arial", 12f, FontStyle.Bold);
        Assert.Equal("Font [Family=Arial, Size=12, Style=Bold]", font.ToString());
    }
}