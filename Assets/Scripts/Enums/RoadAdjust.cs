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
        TopBottomLeftHalfRoad,
        TopBottomRightHalfRoad,
        LeftRightTopHalfRoad,
        LeftRightBottomHalfRoad,

        // L字外側
        TopLeftHalfOnce,
        BottomLeftHalfOnce,
        TopRightHalfOnce,
        BottomRightHalfOnce,

        // L字内側
        TopLeftInner,
        BottomLeftInner,
        TopRightInner,
        BottomRightInner,
    }
}