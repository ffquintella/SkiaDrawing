# SkiaDrawing

This is an implementation of the methods of System.Drawing in SkiaSharp. It is a work in progress and is not yet complete. The goal is to provide a drop-in replacement for System.Drawing that uses SkiaSharp for rendering.


## Usage

To use this library, you need to add a reference to the SkiaDrawing project in your project. Then, you can use the SkiaDrawing namespace in your code.

It is meant to be a drop-in replacement for System.Drawing, so you can use it in the same way. For example, you can create a new bitmap like this:

```csharp

using SkiaDrawing;

Bitmap bitmap = new Bitmap(100, 100);

```

You can then use the Graphics class to draw on the bitmap:

```csharp

using (Graphics g = Graphics.FromImage(bitmap))
{
    g.Clear(Color.White);
    g.DrawLine(Pens.Black, 0, 0, 100, 100);
}

```

You can also use the Graphics class to draw on a control:

```csharp

using (Graphics g = e.Graphics)
{
    g.Clear(Color.White);
    g.DrawLine(Pens.Black, 0, 0, 100, 100);
}

```

## Supported Features

The following features are currently supported:

- Bitmap
- Color
- Graphics
- Pen
- Brush
- Font
- StringFormat
- Rectangle
- Point
- Size
- Image
- ImageFormat

The following features are not yet supported:

- Region
- Path
- Matrix

## No intended features
- ScreenCapture