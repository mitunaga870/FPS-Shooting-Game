using Enums;
using UnityEngine.UIElements;

namespace DataClass
{
    public class TileData
    {
        public readonly int Column;
        public readonly int Row;
        public readonly TileTypes TileType;
        public readonly RoadAdjust RoadAdjust;

        public TileData(int row, int column, TileTypes tileType, RoadAdjust roadAdjust)
        {
            Row = row;
            Column = column;
            TileType = tileType;
            RoadAdjust = roadAdjust;
        }
    }
}