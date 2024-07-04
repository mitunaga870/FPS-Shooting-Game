namespace Enums
{
    public enum RoadAdjust
    {
        TopDeadEnd,
        LeftDeadEnd,
        RightDeadEnd,
        BottomDeadEnd,
        LeftRight,
        TopBottom,
        LeftTop,
        LeftBottom,
        TopRight,
        TopLeft,
        RightBottom,
        RightTop,
        BottomRight,
        BottomLeft,
        LeftTopBottom,
        TopRightLeft,
        RightBottomTop,
        BottomLeftRight,
        Cross,
        None,

        // 太い道路
        // 4方向
        NoWall,

        // 直線
        FatTopBottomLeft,
        FatTopBottomRight,
        FatLeftRightTop,
        FatLeftRightBottom,

        // L字外側
        FatTopLeftOuter,
        FatBottomLeftOuter,
        FatTopRightOuter,
        FatBottomRightOuter,

        // L字内側
        FatTopLeftInner,
        FatBottomLeftInner,
        FatTopRightInner,
        FatBottomRightInner,
    }
}