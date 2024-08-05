using System;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = System.Diagnostics.Debug;

namespace DataClass
{
    [Serializable]
    public class StageData
    {
        /**
         * 迷路の行数
         */
        public int mazeRow;

        /**
         * 迷路の列数
         */
        public int mazeColumn;

        /**
         * トラップの設置数
         */
        public int trapCount;

        /**
         * リロール待機時間
         */
        public int reRollWaitTime;

        /**
         * スタートの列
         */
        public TilePosition start = new TilePosition(0, 0);

        /**
         * ゴールの列
         */
        public TilePosition goal = new TilePosition(0, 0);

        /**
         * 侵攻データ
         */
        public InvasionData invasionData;

        public StageData()
        {
            // スタートとゴールがおかしいときはエラーを吐く
            if (start.Row < 0 || start.Row > mazeRow || start.Col < 0 ||
                start.Col > mazeColumn)
            {
                throw new System.ArgumentException("StartRow or StartColumn is out of range");
            }

            if (goal.Row < 0 || goal.Row > mazeRow || goal.Col < 0 ||
                goal.Col > mazeColumn)
            {
                throw new System.ArgumentException("GoalRow or GoalColumn is out of range");
            }
        }
    }
}