using SkiaSharp;

namespace SkiaDrawing.Tests;

public class ImageTests
{
    [Fact]
public void FromFile_WithValidFilename_ReturnsImage()
{
    var image = Image.FromFile("testImages/white.png");
    Assert.NotNull(image);
    Assert.Equal(10, image.Width);
    Assert.Equal(10, image.Height);
}

[Fact]
public void FromFile_WithNullFilename_ThrowsArgumentNullException()
{
    Assert.Throws<ArgumentNullException>(() => Image.FromFile(null));
}

[Fact]
public void FromFile_WithEmptyFilename_ThrowsArgumentNullException()
{
    Assert.Throws<ArgumentNullException>(() => Image.FromFile(""));
}

[Fact]
public void FromStream_WithValidStream_ReturnsImage()
{
    using var stream = File.OpenRead("testImages/white.png");
    var image = Image.FromStream(stream);
    Assert.NotNull(image);
    Assert.Equal(10, image.Width);
    Assert.Equal(10, image.Height);
}

[Fact]
public void FromStream_WithNullStream_ThrowsArgumentNullException()
{
    Assert.Throws<ArgumentNullException>(() => Image.FromStream(null));
}

[Fact]
public void Save_WithValidParameters_SavesImage()
{
    var image = Image.FromFile("testImages/white.png");
    image.Save("output_image_path.png", SKEncodedImageFormat.Png);
    Assert.True(File.Exists("output_image_path.png"));
}

[Fact]
public void Save_WithNullFilename_ThrowsArgumentNullException()
{
    var image = Image.FromFile("testImages/white.png");
    Assert.Throws<ArgumentNullException>(() => image.Save("", SKEncodedImageFormat.Png));
}

[Fact]
public void Save_WithNullStream_ThrowsArgumentNullException()
{
    var image = Image.FromFile("testImages/white.png");
    Assert.Throws<ArgumentNullException>(() => image.Save((Stream)null, SKEncodedImageFormat.Png));
}

[Fact]
public void GetPixelFormatSize_WithValidPixelFormat_ReturnsCorrectSize()
{
    int size = Image.GetPixelFormatSize(PixelFormat.Format32bppArgb);
    Assert.Equal(32, size);
}

[Fact]
public void GetPixelFormatSize_WithUnsupportedPixelFormat_ThrowsNotSupportedException()
{
    Assert.Throws<NotSupportedException>(() => Image.GetPixelFormatSize((PixelFormat)999));
}

[Fact]
public void Dispose_DisposesImage()
{
    var image = Image.FromFile("testImages/white.png");
    image.Dispose();
    Assert.Throws<ObjectDisposedException>(() => image.Width);
}

[Fact]
public void ToString_WithValidImage_ReturnsCorrectFormat()
{
    var image = Image.FromFile("testImages/white.png");
    string result = image.ToString();
    Assert.Equal("Image [Width=10, Height=10, DPI=96x96]", result);
}

[Fact]
public void ToString_WithDisposedImage_ReturnsDisposedMessage()
{
    var image = Image.FromFile("testImages/white.png");
    image.Dispose();
    string result = image.ToString();
    Assert.Equal("Image [Disposed]", result);
}
}