using System;
using System.Collections.Generic;
using DataClass;
using JetBrains.Annotations;
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
            if (stageNum == -1)
                // ステージナンバーが未指定の時はランダムで取得
                stageNum = UnityEngine.Random.Range(0, normalLevelData.StageDataList.Count);

            // ステージタイプをノーマルに変更
            var result = new StageData(normalLevelData.StageDataList[stageNum]);
            result.StageType = Enums.StageType.Normal;

            return result;
        }

        /**
         * エリートステージデータを取得
         * 引数がない場合はランダムで取得
         */
        public StageData getEliteStageData(int stageNum = -1)
        {
            if (stageNum == -1)
                // ステージナンバーが未指定の時はランダムで取得
                stageNum = UnityEngine.Random.Range(0, eliteLevelData.StageDataList.Count);

            // ステージタイプをエリートに変更
            var result = eliteLevelData.StageDataList[stageNum];
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
            if (stageNum == -1)
                // ステージナンバーが未指定の時はランダムで取得
                stageNum = UnityEngine.Random.Range(0, bossLevelData.StageDataList.Count);

            // ステージタイプをボスに変更
            var result = bossLevelData.StageDataList[stageNum];
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
            allStageData.AddRange(normalLevelData.StageDataList);
            allStageData.AddRange(eliteLevelData.StageDataList);
            allStageData.AddRange(bossLevelData.StageDataList);

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