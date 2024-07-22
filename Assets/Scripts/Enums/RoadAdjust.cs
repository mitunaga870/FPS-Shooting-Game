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
        NoAdjust,

        // 太い道路
        // 4方向
        NoWall,

        // 直線
        TopWall,
        BottomWall,
        RightWall,
        LeftWall,

        // 直線＋ドット
        BottomWallWithRightDot,
        BottomWallWithLeftDot,
        LeftWallWithBottomDot,
        LeftWallWithTopDot,
        TopWallWithLeftDot,
        TopWallWithRightDot,
        RightWallWithTopDot,
        RightWallWithBottomDot,


        // L字外側
        TopLeftHalfOnce,
        BottomLeftHalfOnce,
        TopRightHalfOnce,
        BottomRightHalfOnce,

        // L字内側
        TopLeftDot,
        BottomLeftDot,
        TopRightDot,
        BottomRightDot,

        // 太い道路からの細道
        TopDoubleDot,
        BottomDoubleDot,
        RightDoubleDot,
        LeftDoubleDot,

        // 斜めの点のやつ
        TopLeftAndBottomRightDot,
        TopRightAndBottomLeftDot,

        // ３つのどっと
        ExpectTopRightDot,
        ExpectTopLeftDot,
        ExpectBottomRightDot,
        ExpectBottomLeftDot,
    }
}