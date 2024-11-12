using System;
using Enums;

namespace Map
{
    public class MapTile
    {
        /**
         * マップタイルの種類
         */
        public MapTileType type;

        public int Row { get; private set; }
        public int Column { get; private set; }

        public MapTile(MapTileType type, int row, int column)
        {
            this.type = type;
            Row = row;
            Column = column;
        }

        public MapTile(string saveData)
        {
            var data = saveData.Split(new[] { "$$" }, StringSplitOptions.None);
            Row = int.Parse(data[0]);
            Column = int.Parse(data[1]);
            type = (MapTileType)int.Parse(data[2]);
        }

        /**
         * 保存に主に使う、CSVでラップされるので $$ で区切る
         */
        public override string ToString()
        {
            return Row + "$$" + Column + "$$" + (int)type;
        }

        /**
         * ラベル用に文字列を返す
         */
        public string ToLabelString()
        {
            return type.ToString();
        }
    }
}