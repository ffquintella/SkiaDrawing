namespace SkiaDrawing.Tests;

using Xunit;
using SkiaDrawing;
using System;

public class ColorMatrixTests
{
    [Fact]
    public void ToColorFilter_WithModifiedMatrix_CreatesValidSKColorFilter()
    {
        var colorMatrix = new ColorMatrix();
        colorMatrix[0, 0] = 0.5f;
        var colorFilter = colorMatrix.ToColorFilter();
        Assert.NotNull(colorFilter);
    }

    [Fact]
    public void GetMatrixCopy_ModifyingCopyDoesNotAffectOriginal()
    {
        var colorMatrix = new ColorMatrix();
        var copy = colorMatrix.GetMatrixCopy();
        copy[0, 0] = 0.5f;
        Assert.NotEqual(copy[0, 0], colorMatrix[0, 0]);
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        var colorMatrix = new ColorMatrix();
        Assert.False(colorMatrix.Equals(null));
    }

    [Fact]
    public void Equals_WithDifferentType_ReturnsFalse()
    {
        var colorMatrix = new ColorMatrix();
        Assert.False(colorMatrix.Equals("not a ColorMatrix"));
    }

    [Fact]
    public void GetHashCode_WithModifiedMatrix_ReturnsDifferentHashCode()
    {
        var matrix1 = new ColorMatrix();
        var matrix2 = new ColorMatrix();
        matrix2[0, 0] = 0.5f;
        Assert.NotEqual(matrix1.GetHashCode(), matrix2.GetHashCode());
    }

    [Fact]
    public void ToString_WithModifiedMatrix_ReturnsCorrectFormat()
    {
        var colorMatrix = new ColorMatrix();
        colorMatrix[0, 0] = 0.5f;
        Assert.Equal("ColorMatrix(5x5)", colorMatrix.ToString());
    }
}