using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    // ReSharper disable InconsistentNaming
    [CreateAssetMenu]
    public class MazeData : ScriptableObject
    {
        [Tooltip("リロール待機時間")] public int ReRollWaitTime;

        [Tooltip("迷路の行数")] public int MazeRows;

        [Tooltip("迷路の列数")] public int MazeColumns;

        [Tooltip("トラップの設置数")] public int TrapCount;
    }
}