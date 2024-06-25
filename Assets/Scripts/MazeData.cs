using UnityEngine;

[CreateAssetMenu]
public class MazeData : ScriptableObject
{
    // ReSharper disable once InconsistentNaming
    [Tooltip("迷路の行数")] public int MazeRows;

    // ReSharper disable once InconsistentNaming
    [Tooltip("迷路の列数")] public int MazeColumns;

    // ReSharper disable once InconsistentNaming
    [Tooltip("トラップの設置数")] public int TrapCount;
}