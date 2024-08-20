using AClass;
using Enums;
using lib;
using UnityEngine;

namespace InvasionPhase
{
    public class InvasionPhaseTile : ATile
    {
        /** 初期化処理 */
        public void Initialize(int row, int column, TileTypes tileType, RoadAdjust roadAdjust)
        {
            Row = row;
            Column = column;

            // タイルのステータスによって処理を変える
            if (tileType == TileTypes.Nothing)
            {
                SetNone();
            }
            else
            {
                SetRoad(roadAdjust);
            }
        }
    }
}