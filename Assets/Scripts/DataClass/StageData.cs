using System;
using Enums;
using ScriptableObjects;

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
        public InvasionData invasionData;
        
        /**
         * 敵のHP
         */
        public int enemyHp;

        /**
         * ステージデータの識別名
         * セーブロードでの情報取得に利用
         */
        public string stageName;
        
        // ================= イベントやマップ侵攻によるスケール情報 =================
        [NonSerialized]
        public StageCustomData StageCustomData = new();

        /**
         * ノーマル・エリート・ボスのどれか
         */
        public StageType StageType
        {
            set
            {
                // 同じステージタイプに変更しようとしている場合は変更しない
                if (_stageType == value)
                    return;

                // ステージタイプが未定義の場合のみ変更可能
                if (_stageType != StageType.Undefined)
                    throw new Exception("ステージタイプは変更できません");

                _stageType = value;
            }
            get => _stageType;
        }

        [NonSerialized]
        private StageType _stageType = StageType.Undefined;

        public StageData(StageData stageData)
        {
            mazeRow = stageData.mazeRow;
            mazeColumn = stageData.mazeColumn;
            trapCount = stageData.trapCount;
            reRollWaitTime = stageData.reRollWaitTime;
            stageTime = stageData.stageTime;
            start = stageData.start;
            goal = stageData.goal;
            invasionData = stageData.invasionData;
            StageType = stageData.StageType;
            stageName = stageData.stageName;
        }

        public RewardData GetReward(StageObject stageObject)
        {
            switch (_stageType)
            {
                case StageType.Normal:
                    return stageObject.getNormalReward();
                case StageType.Elite:
                    return stageObject.getEliteReward();
                case StageType.Boss:
                    return stageObject.getBossReward();
                default:
                    throw new Exception("ステージタイプが未定義です");
            }
        }
    }
}