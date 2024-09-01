using System;
using AClass;
using Enums;
using lib;
using Unity.VisualScripting;
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
            switch (tileType)
            {
                case TileTypes.Nothing:
                    SetNone();
                    break;
                case TileTypes.Road:
                    SetRoad(roadAdjust);
                    break;
                default:
                    throw new Exception("未対応のタイルタイプです" + tileType);
            }
        }
    }
}