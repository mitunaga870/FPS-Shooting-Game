using System;
using UnityEngine;

namespace DataClass
{
    [Serializable]
    public class MapData
    {
        [SerializeField]
        private int row = 0;

        [SerializeField]
        private int column = 0;

        [SerializeField]
        private int eliteCount = 0;

        [SerializeField]
        private int eventCount = 0;

        [SerializeField]
        private int shopCount = 0;

        public int Row => row;
        public int Column => column;
        public int EliteCount => eliteCount;
        public int EventCount => eventCount;
        public int ShopCount => shopCount;
    }
}