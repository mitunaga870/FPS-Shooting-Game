using System;
using UnityEngine;

namespace ScriptableObjects.S2SDataObjects
{
    /**
     * 作成フェーズから侵攻フェーズへの以降に必要なデータ
     */
    [CreateAssetMenu(fileName = "CreateToInvasionData", menuName = "S2SData/CreateToInvasionData")]
    public class CreateToInvasionData : AS2SData, ISerializationCallbackReceiver
    {
        [NonSerialized] public DataClass.TileData[][] TileData;
        [NonSerialized] public DataClass.TrapData[] TrapData;

        public void OnBeforeSerialize()
        {
            TileData = Array.Empty<DataClass.TileData[]>();
            TrapData = Array.Empty<DataClass.TrapData>();
        }

        public void OnAfterDeserialize()
        {
        }

        public override string ToString()
        {
            var message = $"MazeSize: {TileData.Length}*{TileData[0].Length}\n";
            message += $"TrapsCount: {TrapData.Length}:\n";

            return message;
        }

        /** 迷路のタイル情報 */
        public int GetMazeRow()
        {
            return TileData.Length;
        }

        /** 迷路のタイル情報 */
        public int GetMazeColumn()
        {
            return TileData[0].Length;
        }

        /** トラップ情報 */
        public int GetTrapCount()
        {
            return TrapData.Length;
        }
    }
}