using System;
using System.Diagnostics.CodeAnalysis;
using Map;
using UnityEngine;

namespace ScriptableObjects.S2SDataObjects
{
    /**
     * 基本的なフェーズ間でのデータ受け渡しを行うクラス
     */
    [CreateAssetMenu(fileName = "GeneralS2SData", menuName = "S2SData/GeneralS2SData")]
    public class GeneralS2SData : AS2SData
    {
        [NonSerialized]
        private int mapNumber = -1;

        [NonSerialized]
        private int currentMapRow = -1;

        [NonSerialized]
        private int currentMapColumn = -1;

        [NonSerialized]
        private int playerHp = 10;

        [NonSerialized]
        public int MapNumber;

        [NonSerialized]
        public int CurrentMapRow;

        [NonSerialized]
        public int CurrentMapColumn;

        [NonSerialized]
        public int PlayerHp;

        [NonSerialized]
        [AllowNull]
        public MapWrapper[] Maps;

        public override string ToString()
        {
            return
                $"MapNumber: {MapNumber}, CurrentMapRow: {CurrentMapRow}, CurrentMapColumn: {CurrentMapColumn}, PlayerHp: {PlayerHp}";
        }

        public override void OnAfterDeserialize()
        {
            PlayerHp = playerHp;
            MapNumber = mapNumber;
            CurrentMapRow = currentMapRow;
            CurrentMapColumn = currentMapColumn;
            Maps = null;
        }

        public override void OnBeforeSerialize()
        {
        }
    }
}