using Enums;
using UnityEngine;
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

        public TileData(string tileData)
        {
            var data = tileData.Split("%%");

            Row = int.Parse(data[0]);
            Column = int.Parse(data[1]);
            TileType = (TileTypes)int.Parse(data[2]);
            RoadAdjust = (RoadAdjust)int.Parse(data[3]);
        }

        public override string ToString()
        {
            return $"{Row}%%{Column}%%{(int)TileType}%%{(int)RoadAdjust}";
        }
    }
}