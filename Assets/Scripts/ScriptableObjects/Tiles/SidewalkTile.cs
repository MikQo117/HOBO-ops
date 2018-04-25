public class SidewalkTile : Tile
{
    public enum SidewalkType
    {
        Horizontal,
        InnerBottomLeft,
        InnerBottomRight,
        InnerTopLeft,
        InnerTopRight,
        OuterBottomLeft,
        OuterBottomRight,
        OuterTopLeft,
        Vertical
    }

    public SidewalkType ST;
}
