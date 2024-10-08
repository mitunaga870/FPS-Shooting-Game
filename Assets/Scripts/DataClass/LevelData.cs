using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataClass
{
    /**
     * ノーマル・エリート・ボス等の大枠データ
     * それに属するステージデータと報酬データを持つ
     */
    [Serializable]
    public class LevelData
    {
        // ReSharper disable once InconsistentNaming
        public List<StageData> StageDataList = new List<StageData>();
        
        public List<RewardData> RewardDataList = new List<RewardData>();
    }
}