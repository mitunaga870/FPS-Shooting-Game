using System.Collections.Generic;
using System.Linq;
using Enums;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using Traps;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;
using TrapData = DataClass.TrapData;


namespace CreatePhase
{
    /**
 * 迷路の生成を制御するクラス
 */
    public class MazeCreationController : MonoBehaviour
    {
        private const float TileSize = 1;

        /** 各迷路の行列数等の情報格納するスクリプタブルオブジェクト */
        [Header("迷路データ")] [SerializeField] public MazeData mazeData;

        /** MazeDataのゲッター */
        public MazeData MazeData => mazeData;

        /** タイルのプレハブ */
        [SerializeField] private Tile tile;

        /** リロールボタン */
        [SerializeField] private ReRollButton reRollButton;

        /** シーン間のデータ共有用オブジェクト */
        [SerializeField] private CreateToInvasionData createToInvasionData;

        /** 迷路の原点 */
        private Vector3 _mazeOrigin;

        /** 迷路のデータ */
        // ReSharper disable once MemberCanBePrivate.Global 確実に外部から呼ぶので一時的に黙らせる
        public Tile[][] Maze { get; private set; }

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
            reRollButton.Show(mazeData.ReRollWaitTime);

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
            _mazeOrigin = new Vector3(-(MazeData.MazeColumns - 1) / 2.0f, 0, -(mazeData.MazeRows - 1) / 2.0f);
            // すべてのタイルを生成し、初期化する
            // 行の初期化
            Maze = new Tile[mazeData.MazeRows][];
            createToInvasionData.Tiles = new Tile[mazeData.MazeRows][];
            for (var row = 0; row < mazeData.MazeRows; row++)
            {
                // 列の初期化
                Maze[row] = new Tile[MazeData.MazeColumns];
                createToInvasionData.Tiles[row] = new Tile[MazeData.MazeColumns];
                for (var column = 0; column < MazeData.MazeColumns; column++)
                {
                    // タイルの位置と回転を設定
                    var tilePosition = new Vector3(column, 0, row) * TileSize + _mazeOrigin;
                    var tileRotation = Quaternion.Euler(-90, 0, 0);
                    // タイルを生成し、初期化する
                    var newTile = Instantiate(tile, tilePosition, tileRotation);
                    newTile.Initialize(this, row, column);

                    // タイルを迷路に追加する
                    Maze[row][column] = newTile;

                    // シーン間のデータ共有用オブジェクトにも追加
                    createToInvasionData.Tiles[row][column] = newTile;
                }
            }

            // ============== トラップ設置 ================
            // トラップ配列を初期化
            TrapData = new TrapData[mazeData.TrapCount];
            createToInvasionData.TrapData = new TrapData[mazeData.TrapCount];
            // トラップの設置数分乱数をもとに場所を決定
            for (var i = 0; i < mazeData.TrapCount; i++)
            {
                ATrap trap = null;

                var loopCount = 0;

                // nullの場合は設置できてないので再度設置
                while (trap == null)
                {
                    // 乱数をもとに場所を決定
                    var row = Random.Range(0, mazeData.MazeRows);
                    var column = Random.Range(0, MazeData.MazeColumns);

                    // トラップを設置
                    trap = Maze[row][column].SetTrap();

                    // トラップ情報を格納
                    TrapData[i] = new TrapData(row, column, trap);

                    // シーン間のデータ共有用オブジェクトにも追加
                    createToInvasionData.TrapData[i] = TrapData[i];

                    // 設置できるものがない等で無限ループになる場合があるので、10回で終了
                    if (loopCount++ > 10) break;
                }
            }
        }

        /**
     * 迷路をリセットする
     */
        private void ResetMaze()
        {
            // すべてのタイルをリセットする
            for (var row = 0; row < mazeData.MazeRows; row++)
            {
                for (var column = 0; column < MazeData.MazeColumns; column++)
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

            // プレビュー中のタイルに追加
            Maze[startRow][startCol].SetPreview();
            _previewAddresses.Add(new Dictionary<string, int> { ["col"] = startCol, ["row"] = startRow });
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
            // 道を設置のとき
            if (this.EditingTargetTileType == TileTypes.Road)
            {
                // 既存の道にプレビュー中の道を追加
                foreach (var address in roadAddresses)
                {
                    newRoadAddresses.Add(address);
                }

                foreach (var address in _previewAddresses)
                {
                    newRoadAddresses.Add(address);
                }
            }
            // 道を削除のとき
            else if (this.EditingTargetTileType == TileTypes.Nothing)
            {
                // 既存の道からプレビュー中の道を削除
                foreach (var address in roadAddresses)
                {
                    if (_previewAddresses.Exists(previewAddress =>
                            previewAddress["col"] == address["col"] && previewAddress["row"] == address["row"]))
                    {
                        continue;
                    }

                    newRoadAddresses.Add(address);
                }
            }


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

            // 横からつないだか縦からつないだか判定
            var lastCol = _previewAddresses[^1]["col"];
            var lastRow = _previewAddresses[^1]["row"];
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
            var right = col + 1 < MazeData.MazeColumns &&
                        connectedTileAddress.Exists(address => address["col"] == col + 1 && address["row"] == row);
            var top = row + 1 < mazeData.MazeRows &&
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
                return RoadAdjust.None;
            }
        }

        /**
     * 道のアドレスを取得
     */
        private List<Dictionary<string, int>> GetRoadAddresses()
        {
            var roadAddresses = new List<Dictionary<string, int>>();
            for (var row = 0; row < mazeData.MazeRows; row++)
            {
                for (var col = 0; col < MazeData.MazeColumns; col++)
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
    }
}