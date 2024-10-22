using Enums;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = System.Random;

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

        public MapTile(int row, int column)
        {
            Row = row;
            Column = column;

            // 指定がないときは、通常・エリート・イベントのいずれかにランダムで設定
            // TODO: 今のところ同様の確率だが肩よりつけたいかも
            var random = new Random();
            var randomValue = random.Next(0, 3);

            switch (randomValue)
            {
                case 0:
                    type = MapTileType.Normal;
                    break;
                case 1:
                    type = MapTileType.Elite;
                    break;
                case 2:
                    type = MapTileType.Event;
                    break;
            }
        }

        public MapTile(MapTileType type, int row, int column)
        {
            this.type = type;
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return "(" + Row + "," + Column + ") \n " + type;
        }
    }
}