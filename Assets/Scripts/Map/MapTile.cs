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

        public MapTile()
        {
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

        public MapTile(MapTileType type)
        {
            this.type = type;
        }

        public override string ToString()
        {
            return "MapTileType: " + type.ToString();
        }
    }
}