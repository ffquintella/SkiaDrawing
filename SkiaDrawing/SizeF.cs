namespace SkiaDrawing;


/// <summary>
/// A structure to represent width/height in floating point, 
/// mimicking System.Drawing.SizeF.
/// </summary>
public struct SizeF
{
    public float Width;
    public float Height;

    public SizeF(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public bool IsEmpty => (Width == 0 && Height == 0);
}