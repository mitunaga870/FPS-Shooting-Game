using System;
using System.Collections;
using Enums;
using UnityEngine;


public class Tile : MonoBehaviour
{
    /** 連続入力防止時間 */
    private const float ContinuousInputPreventionTime = 0.5f;
    
    /** 行き止まりのモデル */
    [SerializeField] private Mesh deadEndModel;

    private TileTypes _tileType;
    private TileTypes TileType
    {
        get => _tileType;
        set
        {
            _tileType = value;
            OnTileTypeChanged();
        }
    }
    private MazeController mazeController;
    private int row;
    private int column;
    
    /** クリックの連続入力を防ぐためのフラグ */
    private bool continuousClickFlag;
    /** マウスエンターの連続入力を防ぐためのフラグ */
    private bool continuousMouseEnterFlag;
    
    /** プレビュー中フラグ */
    public bool isPreview;

    // Start is called before the first frame update
    void Start()
    {
        TileType = TileTypes.None;
        continuousClickFlag = false;
        continuousMouseEnterFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        // タイルの種類によって色を変える
        switch (TileType)
        {
            case TileTypes.None:
                GetComponent<Renderer>().material.color = Color.white;
                break;
            case TileTypes.Road:
                GetComponent<MeshFilter>().mesh = deadEndModel;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        // プレビューの際は色を変える
        if (isPreview)
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    /**
     * クリック時にタイルの種類を変更開始・終了する
     */
    private void OnMouseOver()
    {
        // 連続入力を防ぐ
        if (continuousClickFlag) return;
        
        // タイルの種類を道と道路でトグル
        if (!Input.GetMouseButton(0)) return;
        
        // タイルの種類を道と道路でトグル
        if (!mazeController.IsEditingRoad)
        {
            mazeController.StartRoadEdit(column, row, TileType == TileTypes.None ? TileTypes.Road : TileTypes.None);
        }
        else
        {
            mazeController.EndRoadEdit();
        }
        
        // 連続入力フラグを立てる
        continuousClickFlag = true;
        // 0.5秒後に連続入力フラグを下ろす
        StartCoroutine(DelayCoroutine(ContinuousInputPreventionTime, () => continuousClickFlag = false));
    }

    /**
     * プレビュー用
     */
    private void OnMouseEnter()
    {
        // 連続入力を防ぐ
        if (continuousMouseEnterFlag) return;
        
        // 道編集中でない場合は処理しない
        if (!mazeController.IsEditingRoad) return;

        if (mazeController.IsOneStrokeMode)
        {
            mazeController.PreviewOneStrokeMode(column, row);
        }
        else
        {
            mazeController.PreviewRoadEdit(column, row);
        }

        // 連続入力フラグを立てる
        continuousMouseEnterFlag = true;
        // 0.5秒後に連続入力フラグを下ろす
        StartCoroutine(DelayCoroutine(ContinuousInputPreventionTime, () => continuousMouseEnterFlag = false));
    }
    
    
    /**
     * タイルのステータスの変更
     */
    private void OnTileTypeChanged()
    {
        // タイルのステータスが変わった時の処理
    }
    
    /**
     * 遅延処理用コルーチン
     */
    private IEnumerator DelayCoroutine(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
    
    /**
     * コントローラー・行列を設定する
     * 初回のみ設定可能
     */
    public void Initialize(MazeController mazeController, int row, int column)
    {
        if (this.mazeController != null) return;
        
        this.mazeController = mazeController;
        this.row = row;
        this.column = column;
    }

    /**
     * タイルの種類を設定する
     */
    public void SetTileType(TileTypes tileType)
    {
        TileType = tileType;
        isPreview = false;
    }
    
    /**
     * プレビュー中のフラグを立てる
     */
    public void SetPreview()
    {
        isPreview = true;
    }
    
    /**
     * プレビュー中のフラグを下ろす
     */
    public void ResetPreview()
    {
        isPreview = false;
    }
}
