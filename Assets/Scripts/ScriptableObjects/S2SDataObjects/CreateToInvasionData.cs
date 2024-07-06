using System;
using UnityEngine;
using TrapData = DataClass.TrapData;

namespace ScriptableObjects.S2SDataObjects
{
    /**
     * 作成フェーズから侵攻フェーズへの以降に必要なデータ
     */
    [CreateAssetMenu(fileName = "CreateToInvasionData", menuName = "S2SData/CreateToInvasionData")]
    public class CreateToInvasionData : ScriptableObject, ISerializationCallbackReceiver
    {
        [NonSerialized] public Tile[][] Tiles;
        [NonSerialized] public DataClass.TrapData[] TrapData;

        public void OnBeforeSerialize()
        {
            Tiles = Array.Empty<Tile[]>();
            TrapData = Array.Empty<DataClass.TrapData>();
        }

        public void OnAfterDeserialize()
        {
        }
    }
}