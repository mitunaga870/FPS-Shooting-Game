using System;
using UnityEngine;

namespace ScriptableObjects.S2SDataObjects
{
    /**
     * 基本的なフェーズ間でのデータ受け渡しを行うクラス
     */
    [CreateAssetMenu(fileName = "GeneralS2SData", menuName = "S2SData/GeneralS2SData")]
    public class GeneralS2SData : AS2SData
    {
        [NonSerialized] public int Stage = 1;
        [NonSerialized] public int Level = 1;
        public static int PlayerHp { get; set; }

        public override string ToString()
        {
            return $"Stage: {Stage}, Level: {Level}";
        }

        public override void OnBeforeSerialize()
        {
            Stage = PlayerPrefs.GetInt("Stage", 1);
            Level = PlayerPrefs.GetInt("Level", 1);

            PlayerHp = PlayerPrefs.GetInt("PlayerHp", 10);
        }

        public override void OnAfterDeserialize()
        {
        }
    }
}