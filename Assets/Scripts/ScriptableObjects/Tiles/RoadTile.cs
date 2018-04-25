// Made by Sipi Raussi 25.4.2018
public class RoadTile : Tile
{
    public enum RoadType
    {
        Empty,
        HorizontalBottom,
        HorizontalBottomLeft,
        HorizontalTop,
        HorizontalTopRight,
        InnerTopLeft,
        OuterBottomLeft,
        OuterBottomRight,
        OuterTopLeft,
        OuterTopRight,
        VerticalBottomLeft,
        VerticalBottomRight,
        VerticalLeft,
        VerticalRight,
        VerticalTopRight
    }

    public RoadType RT = RoadType.Empty;
}
