using System.Collections.Generic;
using DataClass;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class MapObject : ScriptableObject
    {
        [SerializeField]
        public MapData FirstStage;

        [SerializeField]
        public MapData SecondStage;

        [SerializeField]
        public MapData ThirdStage;

        [SerializeField]
        public MapData FourthStage;
    }
}