using System;
using System.Collections.Generic;
using DataClass;
using Enums;
using JetBrains.Annotations;
using UnityEngine;

public abstract class AMazeController : MonoBehaviour
{
    /** 迷路のデータ */
    protected ATile[][] Maze { get; set; }

    /**
     * 全体を同期する
     */
    protected abstract void Sync();

    public int MazeRows => Maze.Length;
    public int MazeColumns => Maze[0].Length;

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
     * 現在の迷路の最短経路を取得する
     */
    [CanBeNull]
    public Path GetShortestPath()
    {
        Sync();

        // とりあえずゴール指定
        var start = new TilePosition(0, 0);
        var goal = new TilePosition(MazeRows - 1, MazeColumns - 1);

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
                    if (newPath.GetLast().Equals(goal))
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