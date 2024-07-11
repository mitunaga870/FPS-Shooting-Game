using DataClass;
using UnityEngine;

namespace ScriptableObjects
{
    // ReSharper disable InconsistentNaming
    [CreateAssetMenu]
    public class StageData : ScriptableObject
    {
        [Tooltip("リロール待機時間")] public int OneOneReRollWaitTime;

        [Tooltip("迷路の行数")] public int OneOneMazeRows;

        [Tooltip("迷路の列数")] public int OneOneMazeColumns;

        [Tooltip("トラップの設置数")] public int OneOneTrapCount;

        [Tooltip("スタートの列")] public int OneOneStartRow;

        [Tooltip("スタートの行")] public int OneOneStartColumn;

        [Tooltip("ゴールの列")] public int OneOneGoalRow;

        [Tooltip("ゴールの行")] public int OneOneGoalColumn;

        private StageData()
        {
            // スタートとゴールがおかしいときはエラーを吐く
            if (OneOneStartRow < 0 || OneOneStartRow > OneOneMazeRows || OneOneStartColumn < 0 ||
                OneOneStartColumn > OneOneMazeColumns)
            {
                throw new System.ArgumentException("StartRow or StartColumn is out of range");
            }

            if (OneOneGoalRow < 0 || OneOneGoalRow > OneOneMazeRows || OneOneGoalColumn < 0 ||
                OneOneGoalColumn > OneOneMazeColumns)
            {
                throw new System.ArgumentException("GoalRow or GoalColumn is out of range");
            }
        }

        /**
         * リロール待機時間を取得する
         */
        public int GetReRollWaitTime(int stage, int level)
        {
            // 指定ステージ、レベルのリロール待機時間を取得
            if (stage == 1 && level == 1)
            {
                return OneOneReRollWaitTime;
            }

            // 指定のデータがないならエラー
            throw new System.ArgumentException("Stage or Level is out of range");
        }

        /**
         * 迷路の行数を取得する
         */
        public int GetMazeRows(int stage, int level)
        {
            // 指定ステージ、レベルの迷路の行数を取得
            if (stage == 1 && level == 1)
            {
                return OneOneMazeRows;
            }

            // 指定のデータがないならエラー
            throw new System.ArgumentException("Stage or Level is out of range");
        }

        /**
         * 迷路の列数を取得する
         */
        public int GetMazeColumns(int stage, int level)
        {
            // 指定ステージ、レベルの迷路の列数を取得
            if (stage == 1 && level == 1)
            {
                return OneOneMazeColumns;
            }

            // 指定のデータがないならエラー
            throw new System.ArgumentException($"Stage or Level is out of range. Stage: {stage}, Level: {level}");
        }

        /**
         * トラップの設置数を取得する
         */
        public int GetTrapCount(int stage, int level)
        {
            // 指定ステージ、レベルのトラップの設置数を取得
            if (stage == 1 && level == 1)
            {
                return OneOneTrapCount;
            }

            // 指定のデータがないならエラー
            throw new System.ArgumentException("Stage or Level is out of range");
        }

        /**
         * スタート位置を取得する
         */
        public TilePosition GetStartPosition(int stage, int level)
        {
            // 指定ステージ、レベルのスタート位置を取得
            if (stage == 1 && level == 1)
            {
                return new TilePosition(OneOneStartRow, OneOneStartColumn);
            }

            // 指定のデータがないならエラー
            throw new System.ArgumentException("Stage or Level is out of range");
        }

        /**
         * ゴール位置を取得する
         */
        public TilePosition GetGoalPosition(int stage, int level)
        {
            // 指定ステージ、レベルのゴール位置を取得
            if (stage == 1 && level == 1)
            {
                return new TilePosition(OneOneGoalRow, OneOneGoalColumn);
            }

            // 指定のデータがないならエラー
            throw new System.ArgumentException("Stage or Level is out of range");
        }
    }
}