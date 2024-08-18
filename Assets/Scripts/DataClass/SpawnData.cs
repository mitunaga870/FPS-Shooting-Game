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
    }
}