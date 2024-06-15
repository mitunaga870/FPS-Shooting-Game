using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MazeController : MonoBehaviour
{
    
    [SerializeField] private MazeData mazeData;
    [SerializeField] private Tile tile;
    
    /** 迷路の原点 */
    private Vector3 mazeOrigin;
    
    /** 迷路のデータ */
    private Tile[][] maze;
    
    /** 道路編集中フラグ */
    private bool isEditingRoad;
    /** 道路編集中フラグのゲッター */
    public bool IsEditingRoad
    {
        get => isEditingRoad;
    }
    /** 道路の編集開始列 */
    private int? startEditCol;
    /** 道路の編集開始行 */
    private int? startEditRow;
    /** 道路編集のターゲット状態 */
    private TileTypes? editingTargetTileType;
    /** プレビュー中のタイル */
    private List<Dictionary<string,int>> previewAddresses;
    /** 最後に縦からつないだ時のフラグ */
    private bool lastEditVertical;
    
    /** 道制作モードの一筆書きモードのフラグ */
    public bool isOneStrokeMode;
    /** 道制作モードの一筆書きモードのフラグのゲッター */
    public bool IsOneStrokeMode
    {
        get => isOneStrokeMode;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        isEditingRoad = false;
        mazeOrigin = new Vector3(0, 0, 0);
        previewAddresses = new List<Dictionary<string, int>>();
        lastEditVertical = false;
        CreateMaze();

        // ユーザーに設定される可能性あり
        isOneStrokeMode = false;
    }
    
    // Update is called once per frame
    void Update()
    {
    }

    /**
     * 迷路の生成
     * すべてのタイルを生成し、初期化する
     */
    private void CreateMaze()
    {
        // 原点を設定
        mazeOrigin = new Vector3(mazeData.MAZE_ROWS / 2, 0, mazeData.MAZE_COLUMNS / 2);
        // すべてのタイルを生成し、初期化する
        // 行の初期化
        maze = new Tile[mazeData.MAZE_ROWS][];
        for (int row = 0; row < mazeData.MAZE_ROWS; row++)
        {
            // 列の初期化
            maze[row] = new Tile[mazeData.MAZE_COLUMNS];
            for (int column = 0; column < mazeData.MAZE_COLUMNS; column++)
            {
                // タイルを生成し、初期化する
                Tile newTile = Instantiate(tile, new Vector3(column, 0, row) - mazeOrigin , Quaternion.identity);
                newTile.Initialize(this, row, column);
                
                // タイルを迷路に追加する
                maze[row][column] = newTile;
            }
        }
        
        // トラップ設置
    }
    
    // パブリック関数
    /**
     * 道制作モードの開始処理
     */
    public void StartRoadEdit( int startCol, int startRow, TileTypes targetTypes)
    {
        // 道制作モードの開始処理
        startEditCol = startCol;
        startEditRow = startRow;
        isEditingRoad = true;
        editingTargetTileType = targetTypes;
        
        // プレビュー中のタイルに追加
        maze[startRow][startCol].SetPreview();
        previewAddresses.Add(new Dictionary<string, int>{["col"] = startCol, ["row"] = startRow});
    }

    /**
     * 道制作モードの終了処理
     */
    public void EndRoadEdit()
    {
        // 変数確認
        if (startEditRow == null || startEditCol == null || editingTargetTileType == null) return;
        
        TileTypes targetType = (TileTypes)editingTargetTileType;
        
        foreach (var address in previewAddresses)
        {
            if (address == null || !address.ContainsKey("row") || !address.ContainsKey("col")) continue;
            
            maze[address["row"]][address["col"]].SetTileType(targetType);
        }
        previewAddresses.Clear();
        
        // 道制作モードの終了処理
        isEditingRoad = false;
        
        // 道制作モードの開始列と行をリセット
        startEditCol = null;
        startEditRow = null;
        editingTargetTileType = null;
    }

    
    /**
     * 道制作モードのプレビュー
     */
    public void PreviewRoadEdit(int endCol, int endRow)
    {
        // 変数確認
        if (startEditRow == null || startEditCol == null || editingTargetTileType == null) return;
        
        // 最初の列と行を取得
        int startCol = (int) startEditCol;
        int startRow = (int) startEditRow;
        
        // 横からつないだか縦からつないだか判定
        int lastCol = previewAddresses[previewAddresses.Count - 1]["col"];
        int lastRow = previewAddresses[previewAddresses.Count - 1]["row"];
        if (lastCol == endCol)
        {
            lastEditVertical = false;
        }
        else if (lastRow == endRow)
        {
            lastEditVertical = true;
        }
        
        // プレビュー中のタイルを削除
        foreach (var address in previewAddresses)
        {
            if (address == null || !address.ContainsKey("row") || !address.ContainsKey("col")) continue;
            
            maze[address["row"]][address["col"]].ResetPreview();
        }
        previewAddresses.Clear();
        
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
        int currentCol = startCol;
        int currentRow = startRow;

        if (lastEditVertical)
        {
            // 行を合わせる
            for (int i = 0; i < Mathf.Abs(endRow - startRow); i++)
            {
                // タイルの種類を変更
                maze[currentRow][currentCol].SetPreview();
                // プレビュー用のタイルを追加
                previewAddresses.Add(new Dictionary<string, int> { ["col"] = currentCol, ["row"] = currentRow });
                // インクリメント
                currentRow += diffRow;
            }

            // 列を先に合わせる
            for (int i = 0; i < Mathf.Abs(endCol - startCol) + 1; i++)
            {
                // タイルの種類を変更
                maze[currentRow][currentCol].SetPreview();
                // プレビュー用のタイルを追加
                previewAddresses.Add(new Dictionary<string, int> { ["col"] = currentCol, ["row"] = currentRow });
                // インクリメント
                currentCol += diffCol;
            }
        }
        else
        {
            // 列を先に合わせる
            for (int i = 0; i < Mathf.Abs(endCol - startCol); i++)
            {
                // タイルの種類を変更
                maze[currentRow][currentCol].SetPreview();
                // プレビュー用のタイルを追加
                previewAddresses.Add(new Dictionary<string, int> { ["col"] = currentCol, ["row"] = currentRow });
                // インクリメント
                currentCol += diffCol;
            }

            // 行を合わせる
            for (int i = 0; i < Mathf.Abs(endRow - startRow) + 1; i++)
            {
                // タイルの種類を変更
                maze[currentRow][currentCol].SetPreview();
                // プレビュー用のタイルを追加
                previewAddresses.Add(new Dictionary<string, int> { ["col"] = currentCol, ["row"] = currentRow });
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
        if (startEditRow == null || startEditCol == null || editingTargetTileType == null) return;
        
        // 斜めにつなごうとしたときに中継地点を設定
        int lastCol = previewAddresses[previewAddresses.Count - 1]["col"];
        int lastRow = previewAddresses[previewAddresses.Count - 1]["row"];
        if(lastCol != col && lastRow != row)
        {
            // 中継地点を設定
            maze[lastRow][col].SetPreview();
            previewAddresses.Add(new Dictionary<string, int>{["col"] = col, ["row"] = lastRow});
        }
        
        // プレビュー中のタイルに追加
        maze[row][col].SetPreview();
        previewAddresses.Add(new Dictionary<string, int>{["col"] = col, ["row"] = row});
    }
}
