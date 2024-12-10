using System;
using System.Collections.Generic;
using DataClass;
using JetBrains.Annotations;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;

namespace ScriptableObjects
{
    /**
     * ステージデータのScriptableObject
     * ステージデータを保持するラッパーオブジェクト
     */
    // ReSharper disable InconsistentNaming
    [CreateAssetMenu]
    public class StageObject : ScriptableObject
    {
        [SerializeField]
        private GeneralS2SData generalS2SData;
        
        [SerializeField]
        private LevelData normalLevelData;

        [SerializeField]
        private LevelData eliteLevelData;

        [SerializeField]
        private LevelData bossLevelData;
        
        [SerializeField]
        private EventObject eventObject;


        /**
         * ノーマルステージデータを取得
         * 引数がない場合はランダムで取得
         */
        public StageData getNormalStageData(int stageNum = -1)
        {
            // 現在のスケールのステージデータリストを作成
            var normalStageDataList = normalLevelData.StageDataList[generalS2SData.MapNumber];
            
            if (stageNum == -1)
                // ステージナンバーが未指定の時はランダムで取得
                stageNum = UnityEngine.Random.Range(0, normalStageDataList.Count);

            // ステージタイプをノーマルに変更
            var result = 
                new StageData(
                    normalStageDataList[stageNum]);
            result.StageType = Enums.StageType.Normal;

            return result;
        }

        /**
         * エリートステージデータを取得
         * 引数がない場合はランダムで取得
         */
        public StageData getEliteStageData(int stageNum = -1)
        {
            // 現在のスケールのステージデータリストを作成
            var eliteStageDataList = eliteLevelData.StageDataList[generalS2SData.MapNumber];
            
            // 現在のステージデータリストを作成
            if (stageNum == -1)
                // ステージナンバーが未指定の時はランダムで取得
                stageNum = UnityEngine.Random.Range(0, eliteStageDataList.Count);

            // ステージタイプをエリートに変更
            var result = eliteStageDataList[stageNum];
            result.StageType = Enums.StageType.Elite;
            
            // カスタムをつける
            result.StageCustomData = new StageCustomData();

            return result;
        }

        /**
         * ボスステージデータを取得
         * 引数がない場合はランダムで取得
         */
        public StageData getBossStageData(int stageNum = -1)
        {
            // 現在のスケールのステージデータリストを作成
            var bossStageDataList = bossLevelData.StageDataList[generalS2SData.MapNumber];
            
            if (stageNum == -1)
                // ステージナンバーが未指定の時はランダムで取得
                stageNum = UnityEngine.Random.Range(0, bossStageDataList.Count);

            // ステージタイプをボスに変更
            var result = bossStageDataList[stageNum];
            result.StageType = Enums.StageType.Boss;
            
            // カスタムを当てる
            result.StageCustomData = new StageCustomData();

            return result;
        }

        /**
         * ステージ名からステージデータを取得
         */
        [CanBeNull]
        public StageData GetFromStageName(string stageName)
        {
            var allStageData = new List<StageData>();
            for (var i = 0; i < Environment.TotalMapSize; i++)
            {
                allStageData.AddRange(normalLevelData.StageDataList[i]);
                allStageData.AddRange(eliteLevelData.StageDataList[i]);
                allStageData.AddRange(bossLevelData.StageDataList[i]);
            }
            
            var result = allStageData.Find(stageData => stageData.stageName == stageName);

            return result;
        }

        public RewardData getNormalReward()
        {
            return normalLevelData.RewardDataList[UnityEngine.Random.Range(0, normalLevelData.RewardDataList.Count)];
        }

        public RewardData getEliteReward()
        {
            return eliteLevelData.RewardDataList[UnityEngine.Random.Range(0, eliteLevelData.RewardDataList.Count)];
        }

        public RewardData getBossReward()
        {
            return bossLevelData.RewardDataList[UnityEngine.Random.Range(0, bossLevelData.RewardDataList.Count)];
        }

        public StageData getEventStageData()
        {
            // ノーマルステージデータを取得
            var stageData = getNormalStageData();
            
            // イベント情報を取得
            var eventData = eventObject.GetRandomEventData();
            
            // イベント情報を付与
            stageData.StageCustomData = eventData.StageCustomData;
            
            // ステージ名を改変
            stageData.stageName += "/" + eventData.EventName;
            
            return stageData;
        }
    }
}