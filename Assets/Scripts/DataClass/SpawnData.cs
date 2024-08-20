using System;
using AClass;

namespace DataClass
{
    [Serializable]
    public class SpawnData
    {
        public int spawnTime;
        public int spawnCount;
        public AEnemy enemy;

        public SpawnData(SpawnData spawnData)
        {
            spawnTime = spawnData.spawnTime;
            spawnCount = spawnData.spawnCount;
            enemy = spawnData.enemy;
        }
    }
}