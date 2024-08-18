using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace DataClass
{
    [Serializable]
    public class InvasionData
    {
        [SerializeField] List<SpawnData> spawnDataList = new List<SpawnData>();
        
        /**
         * 指定時刻の侵攻データを取得する
         */
        [CanBeNull]
        public SpawnData GetSpawnData(int time)
        {
            foreach (var spawnData in spawnDataList)
            {
                if (spawnData.spawnTime == time)
                {
                    return spawnData;
                }
            }

            return null;
        }

        /**
         * 最後の侵攻時間を取得する
         */
        public int GetLastSpawnTime()
        {
            if (spawnDataList.Count == 0)
            {
                return 0;
            }

            // 最後の侵攻時間を取得する
            var lastTime = -1;
            foreach (var spawnData in spawnDataList)
            {
                if (spawnData.spawnTime > lastTime || lastTime == -1)
                {
                    lastTime = spawnData.spawnTime;
                }
            }

            return lastTime;
        }
    }
}