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
         * ステージの時間
         */
        public int stageTime;

        /**
         * スタートの列
         */
        public TilePosition start;

        /**
         * ゴールの列
         */
        public TilePosition goal;

        /**
         * 侵攻データ
         */
        public InvasionData invasionData = new InvasionData();
    }
}