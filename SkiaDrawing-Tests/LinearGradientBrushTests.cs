namespace SkiaDrawing.Tests;

public class LinearGradientBrushTests
{
    [Fact]
    public void Constructor_WithValidParameters_InitializesCorrectly()
    {
        var rect = new Rectangle(0, 0, 100, 100);
        var brush = new LinearGradientBrush(rect, Color.Red, Color.Blue, 45);
        Assert.Equal(new PointF(100, 100), brush.StartPoint);
        Assert.Equal(new PointF(0, 0), brush.EndPoint);
        Assert.Equal(Color.Red, brush.Color1);
        Assert.Equal(Color.Blue, brush.Color2);
    }

    [Fact]
    public void Constructor_WithZeroWidthRectangle_InitializesCorrectly()
    {
        var rect = new Rectangle(0, 0, 0, 100);
        var brush = new LinearGradientBrush(rect, Color.Red, Color.Blue, 45);
        Assert.Equal(new PointF(0, 50), brush.StartPoint);
        Assert.Equal(new PointF(0, 50), brush.EndPoint);
    }

    [Fact]
    public void Constructor_WithZeroHeightRectangle_InitializesCorrectly()
    {
        var rect = new Rectangle(0, 0, 100, 0);
        var brush = new LinearGradientBrush(rect, Color.Red, Color.Blue, 45);
        Assert.Equal(new PointF(100, 50), brush.StartPoint);
        Assert.Equal(new PointF(0, -50), brush.EndPoint);
    }

    [Fact]
    public void ToSKPaint_ReturnsValidSKPaint()
    {
        var rect = new Rectangle(0, 0, 100, 100);
        var brush = new LinearGradientBrush(rect, Color.Red, Color.Blue, 45);
        var paint = brush.ToSKPaint();
        Assert.NotNull(paint);
        Assert.NotNull(paint.Shader);
    }

    [Fact]
    public void Dispose_DisposesBrush()
    {
        var rect = new Rectangle(0, 0, 100, 100);
        var brush = new LinearGradientBrush(rect, Color.Red, Color.Blue, 45);
        brush.Dispose();
        Assert.Throws<ObjectDisposedException>(() => brush.ToSKPaint());
    }

    [Fact]
    public void ToString_ReturnsCorrectFormat()
    {
        var rect = new Rectangle(0, 0, 100, 100);
        var brush = new LinearGradientBrush(rect, Color.Red, Color.Blue, 45);
        Assert.Equal("LinearGradientBrush [ Start=PointF { X=100, Y=100 }, End=PointF { X=0, Y=0 }, Color1=Color [Color [A=255, R=255, G=0, B=0, Name=Red]], Color2=Color [Color [A=255, R=0, G=0, B=255, Name=Blue]] ]", brush.ToString());
    }
}