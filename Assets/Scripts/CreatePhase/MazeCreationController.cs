using System;
using System.Collections.Generic;
using System.Linq;
using AClass;
using CreatePhase.UI;
using DataClass;
using Enums;
using JetBrains.Annotations;
using lib;
using Turrets;
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
        [FormerlySerializedAs("tile")]
        [SerializeField]
        private CreatePhaseTile createPhaseTile;

        /** リロールボタン */
        [SerializeField]
        private ReRollButton reRollButton;

        /** デッキシステムつなぎこみ */
        [SerializeField]
        private DeckController deck;
        
        /** turretのUIシステムつなぎこみ */
        [SerializeField]
        private TurretController turretController;

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
        public TrapData[] TrapData { get; private set; }

        /** トラップ設置中フラグ */
        public bool IsSettingTurret { get; private set; }

        /** 設置中トラップのアクセサ */
        private ATurret _settingTurret;

        /** プレビューのトラップ */
        private ATurret _previewTurret;

        /** プレビュー中のトラップのアドレス */
        private TilePosition _previewTurretAddress;

        // Start is called before the first frame update
        private void Start()
        {
            // 初期値設定
            IsEditingRoad = false;
            _mazeOrigin = new Vector3(0, 0, 0);
            _previewAddresses = new List<Dictionary<string, int>>();
            _lastEditVertical = false;
            
            // デッキの初期化
            deck.DiscardHandTrap();

            // セーブデータ読み込み
            var tileData = SaveController.LoadTileData();
            var trapData = SaveController.LoadTrapData();
            var turretData = SaveController.LoadTurretData();

            // 迷路の生成
            CreateMaze(tileData, trapData, turretData);

            // リロールボタンの表示
            reRollButton.Show(ReRollWaitTime);

            reRollButton.AddClickEvent(() =>
            {
                // 迷路を消す
                ResetMaze();
                // 迷路の再生成
                CreateMaze(tileData);
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
        private void CreateMaze(
            [CanBeNull]
            TileData[][] tileData,
            [CanBeNull]
            TrapData[] trapData = null,
            [CanBeNull]
            TurretData[] turretData = null
        )
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

                    CreatePhaseTile newTile;

                    // セーブデータがある場合はセーブデータを使用
                    if (tileData == null)
                    {
                        // タイルを生成し、初期化する
                        newTile = Instantiate(createPhaseTile, tilePosition, tileRotation);
                        newTile.Initialize(this, row, column);
                    }
                    else
                    {
                        var data = tileData[row][column];

                        // タイルを生成し、初期化する
                        newTile = Instantiate(createPhaseTile, tilePosition, tileRotation);
                        newTile.Initialize(this, row, column, data.TileType, data.RoadAdjust);
                    }

                    Maze[row][column] = newTile;
                }
            }

            Sync();

            // スタートとゴールを設置
            Maze[StartPosition.Row][StartPosition.Col].SetStart();
            Maze[GoalPosition.Row][GoalPosition.Col].SetGoal();

            // ============== トラップ設置 ================
            if (trapData != null)
            {
                // トラップ情報を設定
                TrapData = trapData;
                createToInvasionData.TrapData = trapData;

                // トラップ数を設定
                TrapCount = trapData.Length;

                // トラップを設置
                for (var i = 0; i < TrapData.Length; i++)
                {
                    var trap = TrapData[i];
                    Maze[trap.Row][trap.Column].SetTrap(this, trapData[i].Trap, trapData[i].Angle);
                }
            }
            else
            {
                SetRandomTrap();
            }

            // ============== タレット設置 ================
            if (turretData != null)
            {
                // タレット情報を設定
                TurretData = turretData.ToList();

                // タレットを設置
                foreach (var turret in TurretData)
                    Maze[turret.Row][turret.Column].SetTurret(turret.Turret, turret.angle);
            }

            // スタート・ゴールを迷路にする
            var roadData = GetRoadAddresses();
            var startAdjust = GetRoadAdjust(StartPosition.Col, StartPosition.Row, roadData);
            var goalAdjust = GetRoadAdjust(GoalPosition.Col, GoalPosition.Row, roadData);
            Maze[StartPosition.Row][StartPosition.Col].SetRoad(startAdjust);
            Maze[GoalPosition.Row][GoalPosition.Col].SetRoad(goalAdjust);
        }

        /**
         * ランダムにトラップを設置
         */
        private void SetRandomTrap()
        {
            // トラップを取得
            var traps = deck.DrowTraps(TrapCount);

            // 一時トラップリストを作成
            var tempTrapData = new List<TrapData>();

            // トラップの設置数分乱数をもとに場所を決定
            foreach (var trap in traps)
            {
                // 乱数をもとに場所を決定
                var row = Random.Range(0, MazeRows);
                var column = Random.Range(0, MazeColumns);

                var loopCount = 0;
                bool setTrapResult;

                // nullの場合は設置できてないので再度設置
                while (true)
                {
                    // トラップを設置
                    setTrapResult = Maze[row][column].SetTrap(this, trap.GetTrapName());

                    // 設置できてたらbreak
                    if (setTrapResult) break;

                    // 設置できるものがない等で無限ループになる場合があるので、10回で終了
                    if (loopCount++ > 10) break;
                }

                // 設置できなかった場合はリストに追加せずに次のトラップへ
                if (!setTrapResult) continue;
                
                // 設置したトラップを取得
                var setTrap = Maze[row][column].Trap;

                // トラップ情報を格納
                tempTrapData.Add(new TrapData(row, column, setTrap));
            }

            // トラップ情報を設定
            TrapData = tempTrapData.ToArray();
            createToInvasionData.TrapData = TrapData;

            // 実際に設置できたトラップ数を設定
            TrapCount = tempTrapData.Count;
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
            for (var column = 0;
                 column < MazeColumns;
                 column++)
                Maze[row][column].ResetTile();
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

            // プレビューを下げる
            foreach (var address in _previewAddresses) Maze[address["row"]][address["col"]].ResetRoadPreview();

            var newRoadAddresses = new List<Dictionary<string, int>>();

            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (EditingTargetTileType)
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
                        if (
                            _previewAddresses.Exists(previewAddress =>
                                previewAddress["col"] == address["col"] && previewAddress["row"] == address["row"]) &&
                            Maze[address["row"]][address["col"]].TileType == TileTypes.Road // 道のみ削除(スタート・ゴールは削除しない)
                        )
                            continue;

                        newRoadAddresses.Add(address);
                    }

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // 道を削除
            foreach (var address in roadAddresses) Maze[address["row"]][address["col"]].SetNone();

            // 道のアドレス群からタレット設置済みのアドレスを削除
            newRoadAddresses = newRoadAddresses.FindAll(address =>
                Maze[address["row"]][address["col"]].HasTurret == false);
            
            // 道の容量を超えていたら削除
            var capacityRoad = StageData.StageCustomData.CapacityRoad;
            if (newRoadAddresses.Count > capacityRoad && capacityRoad != -1)
            {
                // 道の容量を超えていたら削除
                // TODO: ここで削除するのはランダムでいいのか？
                newRoadAddresses.RemoveRange(capacityRoad, newRoadAddresses.Count - capacityRoad);
            }

            // 道を設置
            foreach (var address in newRoadAddresses)
            {
                if (!address.ContainsKey("row") || !address.ContainsKey("col")) continue;

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
                _lastEditVertical = false;
            else if (lastRow == endRow) _lastEditVertical = true;

            // プレビュー中のタイルを削除
            foreach (var address in _previewAddresses)
            {
                if (address == null || !address.ContainsKey("row") || !address.ContainsKey("col")) continue;

                Maze[address["row"]][address["col"]].ResetRoadPreview();
            }

            _previewAddresses.Clear();

            // 行を左右どちらにずらすか
            int diffCol;
            if (startCol < endCol)
                diffCol = 1;
            else
                diffCol = -1;

            // 列を上下どちらにずらすか
            int diffRow;
            if (startRow < endRow)
                diffRow = 1;
            else
                diffRow = -1;

            // カレントの列と行を設定
            var currentCol = startCol;
            var currentRow = startRow;

            if (_lastEditVertical)
            {
                // 行を合わせる
                for (var i = 0; i < Mathf.Abs(endRow - startRow); i++)
                {
                    // タイルの種類を変更
                    Maze[currentRow][currentCol].SetRoadPreview();
                    // プレビュー用のタイルを追加
                    _previewAddresses.Add(new Dictionary<string, int> { ["col"] = currentCol, ["row"] = currentRow });
                    // インクリメント
                    currentRow += diffRow;
                }

                // 列を先に合わせる
                for (var i = 0; i < Mathf.Abs(endCol - startCol) + 1; i++)
                {
                    // タイルの種類を変更
                    Maze[currentRow][currentCol].SetRoadPreview();
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
                    Maze[currentRow][currentCol].SetRoadPreview();
                    // プレビュー用のタイルを追加
                    _previewAddresses.Add(new Dictionary<string, int> { ["col"] = currentCol, ["row"] = currentRow });
                    // インクリメント
                    currentCol += diffCol;
                }

                // 行を合わせる
                for (var i = 0; i < Mathf.Abs(endRow - startRow) + 1; i++)
                {
                    // タイルの種類を変更
                    Maze[currentRow][currentCol].SetRoadPreview();
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
                Maze[lastRow][col].SetRoadPreview();
                _previewAddresses.Add(new Dictionary<string, int> { ["col"] = col, ["row"] = lastRow });
            }

            // プレビュー中のタイルに追加
            Maze[row][col].SetRoadPreview();
            _previewAddresses.Add(new Dictionary<string, int> { ["col"] = col, ["row"] = row });
        }

        // ==================== プライベート関数 ====================

        /**
         * 道のアドレスを取得
         */
        private List<Dictionary<string, int>> GetRoadAddresses()
        {
            var roadAddresses = new List<Dictionary<string, int>>();
            for (var row = 0;
                 row < MazeRows;
                 row++)
            for (var col = 0;
                 col < MazeColumns;
                 col++)
                if (Maze[row][col].TileType == TileTypes.Road ||
                    Maze[row][col].TileType == TileTypes.Start ||
                    Maze[row][col].TileType == TileTypes.Goal)
                    roadAddresses.Add(new Dictionary<string, int> { ["col"] = col, ["row"] = row });

            return roadAddresses;
        }

        public void CancelRoadEdit()
        {
            // プレビュー中のタイルを削除
            foreach (var address in _previewAddresses.Where(address =>
                         address != null && address.ContainsKey("row") && address.ContainsKey("col")))
                Maze[address["row"]][address["col"]].ResetRoadPreview();

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
            for (var column = 0;
                 column < MazeColumns;
                 column++)
            {
                var tile = Maze[row][column];

                createToInvasionData.TileData[row][column] = new TileData(
                    row,
                    column,
                    tile.TileType,
                    tile.RoadAdjust
                );
            }

            // トラップ情報を設定
            for (var i = 0; i < TrapCount; i++)
            {
                createToInvasionData.TrapData[i] = TrapData[i];
            }

            // タレット情報を設定
            createToInvasionData.TurretData = TurretData.ToArray();
        }

        protected override void Sync()
        {
            var maze = new ATile[Maze.Length][];
            for (var i = 0; i < Maze.Length; i++)
            {
                maze[i] = new ATile[Maze[i].Length];
                for (var j = 0; j < Maze[i].Length; j++) maze[i][j] = Maze[i][j];
            }

            SyncMazeData(maze);
        }

        public override ATile GetTile(int row, int column)
        {
            if (row < 0 || row >= MazeRows || column < 0 || column >= MazeColumns)
                return null;

            return Maze[row][column];
        }

        /**
         * タイルデータを取得
         */
        public TileData[][] GetTileData()
        {
            var result = new TileData[Maze.Length][];

            for (var i = 0; i < Maze.Length; i++)
            {
                result[i] = new TileData[Maze[i].Length];
                for (var j = 0; j < Maze[i].Length; j++)
                    result[i][j] = new TileData(i, j, Maze[i][j].TileType, Maze[i][j].RoadAdjust);
            }

            return result;
        }

        /**
         * トラップ設置モードに変換
         */
        public void StartSettingTurret(ATurret turretPrefab)
        {
            IsSettingTurret = true;
            _settingTurret = turretPrefab;
        }

        public void PreviewTurret(int column, int row)
        {
            if (!_settingTurret && _settingTurret == null)
                return;

            // 半透明のトラップを設置
            if (_previewTurret == null)
                _previewTurret = Instantiate(_settingTurret,
                    new Vector3(column, 0, row) * Environment.TileSize + _mazeOrigin,
                    Quaternion.identity);
            
            // 設置できるか確認
            if (Maze[row][column].SettableTurret)
            {
                // 効果エリアをプレビュー中
                SetPreviewTurretEffectArea(_settingTurret, new TilePosition(row, column), 5000);
            }
            else
            {
                // 置けないので下に赤くする
                SetPreviewProhibitTurretArea(new TilePosition(row, column), 5000);
            }

            // トラップの位置を設定
            _previewTurret.transform.position = new Vector3(column, 0, row) * Environment.TileSize + _mazeOrigin;
            _previewTurretAddress = new TilePosition(row, column);
        }

        private void SetPreviewProhibitTurretArea(TilePosition tilePosition, int i)
        {
            // 既存のプレビューを削除
            foreach (var address in _previewAddresses)
            {
                var row = address["row"];
                var col = address["col"];
                Maze[row][col].ResetAreaPreview();
            }
            
            // 該当エリアを赤くする
            Maze[tilePosition.Row][tilePosition.Col].SetProhibitedArea();
            
            // プレビューの持続時間を設定
            var delay = General.DelayCoroutine(
                i / 1000f,
                () => 
                    Maze[tilePosition.Row][tilePosition.Col].ResetProhibitedArea()
                    );
            StartCoroutine(delay);
        }

        /**
         * トラップ設置モードを終了
         */
        public void EndSettingTurret()
        {
            if (!_settingTurret && _settingTurret == null)
                return;
            // トラップ設置モードを終了
            IsSettingTurret = false;

            // トラップを固定
            if (Maze[_previewTurretAddress.Row][_previewTurretAddress.Col].SettableTurret)
            {
                Maze[_previewTurretAddress.Row][_previewTurretAddress.Col].SetTurret(_settingTurret);
                // タレット情報を設定
                TurretData.Add(
                    new TurretData(_previewTurretAddress.Row, _previewTurretAddress.Col, _settingTurret)
                );
            }


            // トラップを設置したのでプレビューを削除
            if (_previewTurret != null)
            {
                Destroy(_previewTurret.gameObject);
            }

            // プレビュー中のエリアを削除
            HideEffectRange();

            _previewAddresses.Clear();

            // トラップ設置モードを終了
            _settingTurret = null;
        }

        /**
         * トラップの効果範囲をプレビュー
         * @param turret プレビューするトラップ
         * @param origin プレビューの原点
         * @param duration プレビューの持続時間(ms)　デフォルトは100
         */
        public void SetPreviewTurretEffectArea(ATurret turret, TilePosition origin, float duration)
        {
            // トラップの効果範囲を取得
            var effectArea = turret.GetAbsoluteEffectArea(origin);

            // 既存のプレビューを削除
            foreach (var address in _previewAddresses)
            {
                var row = address["row"];
                var col = address["col"];
                Maze[row][col].ResetAreaPreview();
            }
            
            // プレビューを設定
            ShowEffectRange(effectArea, duration);
        }

        /**
         * turretデータを更新
         * 同じ場所のやつが合ったら書き換え、そうじゃなきゃついか
         */
        public void UpdateTurretData(TurretData turretData)
        {
            foreach (var data in TurretData.Where(
                         data => data.Row == turretData.Row && data.Column == turretData.Column))
            {
                TurretData.Remove(data);
                TurretData.Add(turretData);
                return;
            }
        }

        /**
         * トラップを削除
         */
        public void RemoveTurret(int column, int row)
        {
            // 対象タイルを取得
            var targetTile = Maze[row][column];
            
            // turretがない場合は何もしない
            if (!targetTile.HasTurret) return;
            
            // 削除するturretを取得
            
            // 対象タイルのturretを削除
            var removedTurret = targetTile.RemoveTurret();
            
            // 対象タイルのturret情報を削除
            TurretData.RemoveAll(data => data.Row == row && data.Column == column);

            // デッキに戻す
            turretController.AddTurret(new []{removedTurret});
        }
    }
}