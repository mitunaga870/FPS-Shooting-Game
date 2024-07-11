using System;
using System.Collections.Generic;
using DataClass;
using Enums;
using JetBrains.Annotations;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;

public abstract class AMazeController : MonoBehaviour
{
    /** 各迷路の行列数等の情報格納するスクリプタブルオブジェクト */
    [Header("迷路データ")] [SerializeField] private StageData stageData;

    /** 汎用情報 */
    [SerializeField] protected GeneralS2SData GeneralS2SData;

    /** 迷路タイル配列 */
    protected ATile[][] Maze { get; set; }


    /**
     * 全体を同期する
     */
    protected abstract void Sync();

    protected int Level => GeneralS2SData.Level;
    protected int Stage => GeneralS2SData.Stage;
    protected int MazeRows => stageData.GetMazeRows(Stage, Level);
    protected int MazeColumns => stageData.GetMazeColumns(Stage, Level);
    protected TilePosition StartPosition => stageData.GetStartPosition(Stage, Level);
    protected TilePosition GoalPosition => stageData.GetGoalPosition(Stage, Level);
    protected int ReRollWaitTime => stageData.GetReRollWaitTime(Stage, Level);
    protected int TrapCount => stageData.GetTrapCount(Stage, Level);

    /**
     * ベースの迷路配列と指定配列を同期する
     */
    protected void SyncMazeData(ATile[][] maze)
    {
        Maze = new ATile[maze.Length][];
        for (var i = 0; i < maze.Length; i++)
        {
            Maze[i] = new ATile[maze[i].Length];
            for (var j = 0; j < maze[i].Length; j++)
            {
                Maze[i][j] = maze[i][j];
            }
        }
    }

    /**
     * 現在の迷路のスタートからゴールまでの最短経路を出す
     */
    [CanBeNull]
    public Path GetShortestS2GPath()
    {
        if (StartPosition == null || GoalPosition == null)
            throw new ArgumentNullException();

        return GetShortestPath(StartPosition, GoalPosition);
    }

    /**
     * 現在の迷路での指定地点間の最短経路を出す
     * ないときはnull
     */
    [CanBeNull]
    public Path GetShortestPath(TilePosition start, TilePosition destination)
    {
        Sync();

        // 検索中のパスを保持するリスト
        var pathList = new List<Path>();
        if (pathList == null) throw new ArgumentNullException(nameof(pathList));

        // スタートを登録
        pathList.Add(new Path(start));

        while (true)
        {
            // 次用のパスを保持するリスト
            var nextPathList = new List<Path>();
            if (nextPathList == null) throw new ArgumentNullException(nameof(nextPathList));

            // すべてのパスに対して次のステップを取得
            foreach (var path in pathList)
            {
                // 次のステップのパスを取得
                var paths = GetNextStepPaths(path);

                // 次のステップのパスがない場合別を当たる
                if (paths == null) continue;

                // ゴールにたどりついた者があればそれを返す
                foreach (var newPath in paths)
                {
                    if (newPath.GetLast().Equals(destination))
                    {
                        return newPath;
                    }
                }

                // 次のステップのパスをリストに追加
                nextPathList.AddRange(paths);
            }

            // 次のステップのパスがない場合はnullを返す
            if (nextPathList.Count == 0) return null;

            // 現在入手したパスを次の検索用に更新
            pathList = nextPathList;
        }
    }

    /**
     * 隣接した道を取得する
     */
    [CanBeNull]
    private Path[] GetNextStepPaths(Path path)
    {
        var tilePosition = path.GetLast();

        var nextTile = new TilePosition[4];
        var index = 0;

        // 隣接が道かつ、パスに含まれていない場合のみ追加
        // 上
        if (tilePosition.Row - 1 >= 0 && Maze[tilePosition.Row - 1][tilePosition.Col].TileType == TileTypes.Road &&
            !path.Contains(tilePosition.Row - 1, tilePosition.Col))
        {
            nextTile[index] = new TilePosition(tilePosition.Row - 1, tilePosition.Col);
            index++;
        }

        // 下
        if (tilePosition.Row + 1 < MazeRows &&
            Maze[tilePosition.Row + 1][tilePosition.Col].TileType == TileTypes.Road &&
            !path.Contains(tilePosition.Row + 1, tilePosition.Col))
        {
            nextTile[index] = new TilePosition(tilePosition.Row + 1, tilePosition.Col);
            index++;
        }

        // 左
        if (tilePosition.Col - 1 >= 0 && Maze[tilePosition.Row][tilePosition.Col - 1].TileType == TileTypes.Road &&
            !path.Contains(tilePosition.Row, tilePosition.Col - 1))
        {
            nextTile[index] = new TilePosition(tilePosition.Row, tilePosition.Col - 1);
            index++;
        }

        // 右
        if (tilePosition.Col + 1 < MazeColumns &&
            Maze[tilePosition.Row][tilePosition.Col + 1].TileType == TileTypes.Road &&
            !path.Contains(tilePosition.Row, tilePosition.Col + 1))
        {
            nextTile[index] = new TilePosition(tilePosition.Row, tilePosition.Col + 1);
            index++;
        }

        // 隣接する道がない場合はnullを返す
        if (index == 0) return null;

        var result = new Path[index];
        for (var i = 0; i < index; i++)
        {
            result[i] = path.Add(nextTile[i]);
        }

        return result;
    }
}