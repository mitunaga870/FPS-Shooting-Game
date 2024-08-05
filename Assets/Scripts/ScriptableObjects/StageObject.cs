using System.Collections.Generic;
using DataClass;
using UnityEngine;

namespace ScriptableObjects
{
    // ReSharper disable InconsistentNaming
    [CreateAssetMenu]
    public class StageObject : ScriptableObject
    {
        [SerializeField] List<StageData> stageDataList = new List<StageData>();

        /**
         * リロール待機時間を取得する
         */
        public int GetReRollWaitTime(int stageNum)
        {
            // 指定ステージ、レベルのリロール待機時間を取得
            return GetStageData(stageNum).reRollWaitTime;
        }

        /**
         * 迷路の行数を取得する
         */
        public int GetMazeRows(int stageNum)
        {
            // 指定ステージ、レベルの迷路の行数を取得
            return GetStageData(stageNum).mazeRow;
        }

        /**
         * 迷路の列数を取得する
         */
        public int GetMazeColumns(int stageNum)
        {
            // 指定ステージ、レベルの迷路の列数を取得
            return GetStageData(stageNum).mazeColumn;
        }

        /**
         * トラップの設置数を取得する
         */
        public int GetTrapCount(int stageNum)
        {
            // 指定ステージ、レベルのトラップの設置数を取得
            return GetStageData(stageNum).trapCount;
        }

        /**
         * スタート位置を取得する
         */
        public TilePosition GetStartPosition(int stageNum)
        {
            // 指定ステージ、レベルのスタート位置を取得
            return GetStageData(stageNum).start;
        }

        /**
         * ゴール位置を取得する
         */
        public TilePosition GetGoalPosition(int stageNum)
        {
            // 指定ステージ、レベルのゴール位置を取得
            return GetStageData(stageNum).goal;
        }

        public StageData GetStageData(int i)
        {
            return stageDataList[i];
        }
    }
}