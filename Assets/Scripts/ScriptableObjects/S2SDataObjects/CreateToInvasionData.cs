using System;
using UnityEngine;
using TrapData = DataClass.TrapData;

namespace ScriptableObjects.S2SDataObjects
{
    /**
     * 作成フェーズから侵攻フェーズへの以降に必要なデータ
     */
    [CreateAssetMenu(fileName = "CreateToInvasionData", menuName = "S2SData/CreateToInvasionData")]
    public class CreateToInvasionData : AS2SData, ISerializationCallbackReceiver
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

        public override string ToString()
        {
            var message = $"MazeSize: {Tiles.Length}*{Tiles[0].Length}\n";
            message += $"TrapsCount: {TrapData.Length}:\n";

            return message;
        }

        /** 迷路のタイル情報 */
        public int GetMazeRow()
        {
            return Tiles.Length;
        }

        /** 迷路のタイル情報 */
        public int GetMazeColumn()
        {
            return Tiles[0].Length;
        }

        /** トラップ情報 */
        public int GetTrapCount()
        {
            return TrapData.Length;
        }
    }
}