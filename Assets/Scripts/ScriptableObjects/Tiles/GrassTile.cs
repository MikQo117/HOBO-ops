public class GrassTile : Tile
{
    public enum GrassType
    {
        BottomLeft,
        BottomRight,
        CornerBottomLeft,
        CornerBottomRight,
        CornerTopLeft,
        CornerTopRight,
        Empty,
        HorizontalBottom,
        HorizontalTop,
        TopLeft,
        TopRight,
        VerticalLeft,
        VerticalRight
    }

    public GrassType GT;
}