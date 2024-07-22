using System;
using System.Collections.Generic;
using System.Linq;
using AClass;
using CreatePhase.UI;
using Enums;
using ScriptableObjects.S2SDataObjects;
using Traps;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using TrapData = DataClass.TrapData;
using TileData = DataClass.TileData;


namespace CreatePhase
{
    /**
     * 制作フェーズでの迷路の生成を管理するクラス
     */
    public class MazeCreationController : AMazeController
    {
        /** タイルのプレハブ */
        [FormerlySerializedAs("tile")] [SerializeField]
        private CreatePhaseTile createPhaseTile;

        /** リロールボタン */
        [SerializeField] private ReRollButton reRollButton;

        /** シーン間のデータ共有用オブジェクト */
        [SerializeField] private CreateToInvasionData createToInvasionData;

        /** 迷路の原点 */
        private Vector3 _mazeOrigin;


        /** 迷路のデータ */
        private new CreatePhaseTile[][] Maze { get; set; }


        /** 道路編集中フラグのゲッター */
        public bool IsEditingRoad { get; private set; }

        /** 道路の編集開始列 */
        private int? _startEditCol;

        /** 道路の編集開始行 */
        private int? _startEditRow;

        /** 道路編集のターゲット状態のゲッター */
        public TileTypes? EditingTargetTileType { get; private set; }

        /** プレビュー中のタイル */
        private List<Dictionary<string, int>> _previewAddresses;

        /** 最後に縦からつないだ時のフラグ */
        private bool _lastEditVertical;

        /** 道制作モードの一筆書きモードのフラグのゲッター */
        public bool IsOneStrokeMode { get; private set; }

        /** 設置したトラップ情報 */
        private TrapData[] TrapData { get; set; }

        // Start is called before the first frame update
        private void Start()
        {
            // 初期値設定
            IsEditingRoad = false;
            _mazeOrigin = new Vector3(0, 0, 0);
            _previewAddresses = new List<Dictionary<string, int>>();
            _lastEditVertical = false;

            // 迷路の生成
            CreateMaze();

            // リロールボタンの表示
            reRollButton.Show(ReRollWaitTime);

            reRollButton.AddClickEvent(() =>
            {
                // 迷路を消す
                ResetMaze();
                // 迷路の再生成
                CreateMaze();
                // リロールボタンを消す
                reRollButton.Hide();
            });

            // ユーザーに設定される可能性あり
            IsOneStrokeMode = false;
        }

        /**
     * 迷路の生成
     * すべてのタイルを生成し、初期化する
     */
        private void CreateMaze()
        {
            // 原点を設定
            _mazeOrigin = new Vector3(-(MazeColumns - 1) / 2.0f, 0,
                -(MazeRows - 1) / 2.0f);
            // すべてのタイルを生成し、初期化する
            // 行の初期化
            Maze = new CreatePhaseTile[MazeRows][];
            createToInvasionData.TileData = new TileData[MazeRows][];
            for (var row = 0; row < MazeRows; row++)
            {
                // 列の初期化
                Maze[row] = new CreatePhaseTile[MazeColumns];
                createToInvasionData.TileData[row] = new TileData[MazeColumns];
                for (var column = 0; column < MazeColumns; column++)
                {
                    // タイルの位置と回転を設定
                    var tilePosition = new Vector3(column, 0, row) * Environment.TileSize + _mazeOrigin;
                    var tileRotation = Quaternion.Euler(-90, 0, 0);
                    // タイルを生成し、初期化する
                    var newTile = Instantiate(createPhaseTile, tilePosition, tileRotation);
                    newTile.Initialize(this, row, column);

                    // タイルを迷路に追加する
                    Maze[row][column] = newTile;
                }
            }

            // ============== トラップ設置 ================
            // トラップ配列を初期化
            TrapData = new TrapData[TrapCount];
            createToInvasionData.TrapData = new TrapData[TrapCount];
            // トラップの設置数分乱数をもとに場所を決定
            for (var i = 0; i < TrapCount; i++)
            {
                ATrap trap = null;

                var loopCount = 0;

                // nullの場合は設置できてないので再度設置
                while (trap == null)
                {
                    // 乱数をもとに場所を決定
                    var row = Random.Range(0, MazeRows);
                    var column = Random.Range(0, MazeColumns);

                    // トラップを設置
                    trap = Maze[row][column].SetTrap();

                    // トラップ情報を格納
                    TrapData[i] = new TrapData(row, column, trap);

                    // 設置できるものがない等で無限ループになる場合があるので、10回で終了
                    if (loopCount++ > 10) break;
                }
            }

            // スタートとゴールを設置
            Maze[StartPosition.Row][StartPosition.Col].SetStart();
            Maze[GoalPosition.Row][GoalPosition.Col].SetGoal();
        }

        /**
     * 迷路をリセットする
     */
        private void ResetMaze()
        {
            // すべてのタイルをリセットする
            for (var row = 0;
                 row < MazeRows;
                 row++)
            {
                for (var column = 0;
                     column < MazeColumns;
                     column++)
                {
                    Maze[row][column].ResetTile();
                }
            }

            // トラップ情報を削除
            foreach (var trapData in TrapData)
            {
                trapData.Dispose();
            }
        }

        // パブリック関数
        /**
     * 道制作モードの開始処理
     */
        public void StartRoadEdit(int startCol, int startRow, TileTypes targetTypes)
        {
            // 道制作モードの開始処理
            _startEditCol = startCol;
            _startEditRow = startRow;
            IsEditingRoad = true;
            EditingTargetTileType = targetTypes;

            // プレビュー配列初期化
            _previewAddresses = new List<Dictionary<string, int>>();

            // プレビュー中のタイルに追加
            PreviewRoadEdit(startCol, startRow);
        }

        /**
     * 道制作モードの終了処理
     */
        public void EndRoadEdit()
        {
            // 変数確認
            if (_startEditRow == null || _startEditCol == null || EditingTargetTileType == null) return;

            // 既存の道を削除
            var roadAddresses = GetRoadAddresses();
            foreach (var address in roadAddresses)
            {
                Maze[address["row"]][address["col"]].SetNone();
            }

            // プレビューを下げる
            foreach (var address in _previewAddresses)
            {
                Maze[address["row"]][address["col"]].ResetPreview();
            }

            var newRoadAddresses = new List<Dictionary<string, int>>();

            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (this.EditingTargetTileType)
            {
                // 道を設置のとき
                case TileTypes.Road:
                {
                    // 既存の道にプレビュー中の道を追加
                    newRoadAddresses.AddRange(roadAddresses);

                    newRoadAddresses.AddRange(_previewAddresses);

                    break;
                }
                // 道を削除のとき
                case TileTypes.Nothing:
                {
                    // 既存の道からプレビュー中の道を削除
                    // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
                    foreach (var address in roadAddresses)
                    {
                        if (_previewAddresses.Exists(previewAddress =>
                                previewAddress["col"] == address["col"] && previewAddress["row"] == address["row"]))
                        {
                            continue;
                        }

                        newRoadAddresses.Add(address);
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Debug.Log(newRoadAddresses.Count);

            // 道を設置
            foreach (var address in newRoadAddresses)
            {
                if (address == null || !address.ContainsKey("row") || !address.ContainsKey("col")) continue;

                // つながり肩を取得
                var roadAdjust = GetRoadAdjust(address["col"], address["row"], newRoadAddresses);

                // タイルの種類を変更
                Maze[address["row"]][address["col"]].SetRoad(roadAdjust);
            }

            _previewAddresses.Clear();

            // 道制作モードの終了処理
            IsEditingRoad = false;

            // 道制作モードの開始列と行をリセット
            _startEditCol = null;
            _startEditRow = null;
            EditingTargetTileType = null;
        }


        /**
     * 道制作モードのプレビュー
     */
        public void PreviewRoadEdit(int endCol, int endRow)
        {
            // 変数確認
            if (_startEditRow == null || _startEditCol == null || EditingTargetTileType == null) return;

            // 最初の列と行を取得
            var startCol = (int)_startEditCol;
            var startRow = (int)_startEditRow;

            // 最後に編集した列と行を取得 カウントが0の時は最初なので開始地点を設定
            var lastCol = _previewAddresses.Count == 0 ? startCol : _previewAddresses[^1]["col"];
            var lastRow = _previewAddresses.Count == 0 ? startRow : _previewAddresses[^1]["row"];

            // 横からつないだか縦からつないだか判定
            if (lastCol == endCol)
            {
                _lastEditVertical = false;
            }
            else if (lastRow == endRow)
            {
                _lastEditVertical = true;
            }

            // プレビュー中のタイルを削除
            foreach (var address in _previewAddresses)
            {
                if (address == null || !address.ContainsKey("row") || !address.ContainsKey("col")) continue;

                Maze[address["row"]][address["col"]].ResetPreview();
            }

            _previewAddresses.Clear();

            // 行を左右どちらにずらすか
            int diffCol;
            if (startCol < endCol)
            {
                diffCol = 1;
            }
            else
            {
                diffCol = -1;
            }

            // 列を上下どちらにずらすか
            int diffRow;
            if (startRow < endRow)
            {
                diffRow = 1;
            }
            else
            {
                diffRow = -1;
            }

            // カレントの列と行を設定
            var currentCol = startCol;
            var currentRow = startRow;

            if (_lastEditVertical)
            {
                // 行を合わせる
                for (var i = 0; i < Mathf.Abs(endRow - startRow); i++)
                {
                    // タイルの種類を変更
                    Maze[currentRow][currentCol].SetPreview();
                    // プレビュー用のタイルを追加
                    _previewAddresses.Add(new Dictionary<string, int> { ["col"] = currentCol, ["row"] = currentRow });
                    // インクリメント
                    currentRow += diffRow;
                }

                // 列を先に合わせる
                for (var i = 0; i < Mathf.Abs(endCol - startCol) + 1; i++)
                {
                    // タイルの種類を変更
                    Maze[currentRow][currentCol].SetPreview();
                    // プレビュー用のタイルを追加
                    _previewAddresses.Add(new Dictionary<string, int> { ["col"] = currentCol, ["row"] = currentRow });
                    // インクリメント
                    currentCol += diffCol;
                }
            }
            else
            {
                // 列を先に合わせる
                for (var i = 0; i < Mathf.Abs(endCol - startCol); i++)
                {
                    // タイルの種類を変更
                    Maze[currentRow][currentCol].SetPreview();
                    // プレビュー用のタイルを追加
                    _previewAddresses.Add(new Dictionary<string, int> { ["col"] = currentCol, ["row"] = currentRow });
                    // インクリメント
                    currentCol += diffCol;
                }

                // 行を合わせる
                for (var i = 0; i < Mathf.Abs(endRow - startRow) + 1; i++)
                {
                    // タイルの種類を変更
                    Maze[currentRow][currentCol].SetPreview();
                    // プレビュー用のタイルを追加
                    _previewAddresses.Add(new Dictionary<string, int> { ["col"] = currentCol, ["row"] = currentRow });
                    // インクリメント
                    currentRow += diffRow;
                }
            }
        }

        /**
     * 一筆書きモードのプレビュー
     */
        public void PreviewOneStrokeMode(int col, int row)
        {
            // 変数確認
            if (_startEditRow == null || _startEditCol == null || EditingTargetTileType == null) return;

            // 斜めにつなごうとしたときに中継地点を設定
            var lastCol = _previewAddresses[^1]["col"];
            var lastRow = _previewAddresses[^1]["row"];
            if (lastCol != col && lastRow != row)
            {
                // 中継地点を設定
                Maze[lastRow][col].SetPreview();
                _previewAddresses.Add(new Dictionary<string, int> { ["col"] = col, ["row"] = lastRow });
            }

            // プレビュー中のタイルに追加
            Maze[row][col].SetPreview();
            _previewAddresses.Add(new Dictionary<string, int> { ["col"] = col, ["row"] = row });
        }

        // ==================== プライベート関数 ====================

        /**
     * タイルのつながり肩を取得
     */
        private RoadAdjust GetRoadAdjust(int col, int row, List<Dictionary<string, int>> connectedTileAddress)
        {
            // 上下左右のタイルがつながっているか
            var bottom = row - 1 >= 0 &&
                         connectedTileAddress.Exists(address => address["col"] == col && address["row"] == row - 1);
            var left = col - 1 >= 0 &&
                       connectedTileAddress.Exists(address => address["col"] == col - 1 && address["row"] == row);
            var right = col + 1 < MazeColumns &&
                        connectedTileAddress.Exists(address => address["col"] == col + 1 && address["row"] == row);
            var top = row + 1 < MazeRows &&
                      connectedTileAddress.Exists(address => address["col"] == col && address["row"] == row + 1);

            // 十字
            if (top && left && right && bottom)
            {
                return RoadAdjust.Cross;
            }
            // T字
            else if (top && left && right)
            {
                return RoadAdjust.TopRightLeft;
            }
            else if (top && left && bottom)
            {
                return RoadAdjust.LeftTopBottom;
            }
            else if (top && right && bottom)
            {
                return RoadAdjust.RightBottomTop;
            }
            else if (left && right && bottom)
            {
                return RoadAdjust.BottomLeftRight;
            }
            // L字
            else if (top && left)
            {
                return RoadAdjust.TopLeft;
            }
            else if (top && right)
            {
                return RoadAdjust.TopRight;
            }
            else if (left && bottom)
            {
                return RoadAdjust.BottomLeft;
            }
            else if (right && bottom)
            {
                return RoadAdjust.RightBottom;
            }
            // 直線
            else if (top && bottom)
            {
                return RoadAdjust.TopBottom;
            }
            else if (left && right)
            {
                return RoadAdjust.LeftRight;
            }
            // 行き止まり
            else if (top)
            {
                return RoadAdjust.TopDeadEnd;
            }
            else if (left)
            {
                return RoadAdjust.LeftDeadEnd;
            }
            else if (right)
            {
                return RoadAdjust.RightDeadEnd;
            }
            else if (bottom)
            {
                return RoadAdjust.BottomDeadEnd;
            }
            else
            {
                return RoadAdjust.NoAdjust;
            }
        }

        /**
     * 道のアドレスを取得
     */
        private List<Dictionary<string, int>> GetRoadAddresses()
        {
            var roadAddresses = new List<Dictionary<string, int>>();
            for (var row = 0;
                 row < MazeRows;
                 row++)
            {
                for (var col = 0;
                     col < MazeColumns;
                     col++)
                {
                    if (Maze[row][col].GetTileType() == TileTypes.Road)
                    {
                        roadAddresses.Add(new Dictionary<string, int> { ["col"] = col, ["row"] = row });
                    }
                }
            }

            return roadAddresses;
        }

        public void CancelRoadEdit()
        {
            // プレビュー中のタイルを削除
            foreach (var address in _previewAddresses.Where(address =>
                         address != null && address.ContainsKey("row") && address.ContainsKey("col")))
            {
                Maze[address["row"]][address["col"]].ResetPreview();
            }

            _previewAddresses.Clear();

            // 道制作モードの終了処理
            IsEditingRoad = false;

            // 道制作モードの開始列と行をリセット
            _startEditCol = null;
            _startEditRow = null;
            EditingTargetTileType = null;
        }

        /**
         * シーン間のデータ共有用オブジェクトにデータを設定
         */
        public void SetS2SData()
        {
            // 迷路のタイル情報を設定
            for (var row = 0;
                 row < MazeRows;
                 row++)
            {
                for (var column = 0;
                     column < MazeColumns;
                     column++)
                {
                    var tile = Maze[row][column];

                    createToInvasionData.TileData[row][column] = new TileData(
                        row,
                        column,
                        tile.GetTileType(),
                        tile.RoadAdjust
                    );
                }
            }

            // トラップ情報を設定
            for (var i = 0;
                 i < TrapCount;
                 i++)
            {
                createToInvasionData.TrapData[i] = TrapData[i];
            }

            // 迷路の原点を設定
            createToInvasionData.MazeOrigin = _mazeOrigin;
        }

        protected override void Sync()
        {
            var maze = new ATile[Maze.Length][];
            for (var i = 0; i < Maze.Length; i++)
            {
                maze[i] = new ATile[Maze[i].Length];
                for (var j = 0; j < Maze[i].Length; j++)
                {
                    maze[i][j] = Maze[i][j];
                }
            }

            SyncMazeData(maze);
        }
    }
}