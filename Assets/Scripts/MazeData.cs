using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MazeData : ScriptableObject
{
    /// <summary>
    /// 迷路の行数
    /// </summary>
    public readonly int MAZE_ROWS = 10;

    /// <summary>
    /// 迷路の列数
    /// </summary>
    public readonly int MAZE_COLUMNS = 10;

    /// <summary>
    /// トラップの設置数
    /// </summary>
    public readonly int TRAP_COUNT = 10;
}