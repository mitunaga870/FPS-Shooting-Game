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

        public override void OnBeforeSerialize()
        {
            PlayerHp = PlayerPrefs.GetInt("PlayerHp", 10);
            MapNumber = PlayerPrefs.GetInt("MapNumber", 0);
            CurrentMapRow = PlayerPrefs.GetInt("CurrentMapRow", 0);
            CurrentMapColumn = PlayerPrefs.GetInt("CurrentMapColumn", 0);
            Maps = null;
        }

        public override void OnAfterDeserialize()
        {
        }
    }
}