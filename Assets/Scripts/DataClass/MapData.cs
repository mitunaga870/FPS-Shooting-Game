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

        public int Row => row;
        public int Column => column;
    }
}