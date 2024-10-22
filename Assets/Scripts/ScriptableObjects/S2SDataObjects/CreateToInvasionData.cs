using System;
using DataClass;
using UnityEngine;

namespace ScriptableObjects.S2SDataObjects
{
    /**
     * 作成フェーズから侵攻フェーズへの以降に必要なデータ
     */
    [CreateAssetMenu(fileName = "CreateToInvasionData", menuName = "S2SData/CreateToInvasionData")]
    public class CreateToInvasionData : AS2SData
    {
        [NonSerialized]
        public bool IsInvasion;

        [NonSerialized]
        public TileData[][] TileData;

        [NonSerialized]
        public TrapData[] TrapData;

        [NonSerialized]
        public Vector3 MazeOrigin;

        [NonSerialized]
        public StageData StageData;


        public override void OnBeforeSerialize()
        {
            TileData = Array.Empty<TileData[]>();
            TrapData = Array.Empty<TrapData>();
            MazeOrigin = Vector3.zero;
            IsInvasion = false;
            StageData = null;
        }

        public override void OnAfterDeserialize()
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