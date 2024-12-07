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
        public List<StageData> FirstMapStageDataList = new List<StageData>();
        // ReSharper disable once InconsistentNaming
        public List<StageData> SecondMapStageDataList = new List<StageData>();
        // ReSharper disable once InconsistentNaming
        public List<StageData> ThirdMapStageDataList = new List<StageData>();
        // ReSharper disable once InconsistentNaming
        public List<StageData> FourthMapStageDataList = new List<StageData>();
        
        public List<List<StageData>> StageDataList => new List<List<StageData>>
        {
            FirstMapStageDataList,
            SecondMapStageDataList,
            ThirdMapStageDataList,
            FourthMapStageDataList
        };
        
        [Header("報酬データ")]
        public List<RewardData> RewardDataList = new List<RewardData>();
    }
}