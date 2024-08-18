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
         * 取得したデータはリストから削除される
         */
        [CanBeNull]
        public SpawnData GetSpawnData(int time)
        {
            foreach (var spawnData in spawnDataList)
            {
                if (spawnData.spawnTime == time)
                {
                    var result = new SpawnData(spawnData);

                    return result;
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

        /**
         * 敵の総数を取得する
         */
        public int GetEnemyCount()
        {
            var count = 0;
            foreach (var spawnData in spawnDataList)
            {
                count += spawnData.spawnCount;
            }

            return count;
        }
    }
}