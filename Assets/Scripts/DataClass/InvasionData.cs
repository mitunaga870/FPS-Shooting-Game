using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataClass
{
    [Serializable]
    public class InvasionData
    {
        [SerializeField] List<SpawnData> spawnDataList = new List<SpawnData>();
    }
}