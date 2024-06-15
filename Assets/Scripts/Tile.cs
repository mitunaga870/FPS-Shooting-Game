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
     * タイルを道に設定する
     * @param roadAdjust 道の形状
     */
    public void SetRoad(RoadAdjust roadAdjust)
    {
        // 既に道・トラップが設定されている場合は処理しない
        if (TileType == TileTypes.Road) return;
        
        // タイルの種類を道に設定
        TileType = TileTypes.Road;
        
        // 道の形状によってモデルを変更
        // とりあえず色を変えるだけ
        Renderer renderer = GetComponent<Renderer>();
        switch (roadAdjust)
        {
            case RoadAdjust.LeftDeadEnd:
            case RoadAdjust.BottomDeadEnd:
            case RoadAdjust.RightDeadEnd:
            case RoadAdjust.TopDeadEnd:
                renderer.material.color = Color.red;
                // TODO: 行き止まりのモデルを設定
                break;
            case RoadAdjust.LeftRight:
            case RoadAdjust.TopBottom:
                renderer.material.color = Color.blue;
                // TODO: 直線のモデルを設定
                break;
            case RoadAdjust.LeftTop:
            case RoadAdjust.LeftBottom:
            case RoadAdjust.TopRight:
            case RoadAdjust.TopLeft:
            case RoadAdjust.RightBottom:
            case RoadAdjust.RightTop:
            case RoadAdjust.BottomRight:
            case RoadAdjust.BottomLeft:
                renderer.material.color = Color.green;
                // TODO: コーナーのモデルを設定
                break;
            case RoadAdjust.LeftTopBottom:
            case RoadAdjust.TopRightLeft:
            case RoadAdjust.RightBottomTop:
            case RoadAdjust.BottomLeftRight:
                renderer.material.color = Color.yellow;
                // TODO: T字路のモデルを設定
                break;
            case RoadAdjust.Cross:
                renderer.material.color = Color.magenta;
                // TODO: 交差点のモデルを設定
                break;
        }
    }
    
    /**
     * プレビュー中のフラグを立てる
     */
    public void SetPreview()
    {
        // 既にプレビュー中の場合は処理しない
        if (isPreview) return;
        
        isPreview = true;
        
        // プレビュー中のタイルの色を変更
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = Color.gray;
    }
    
    /**
     * プレビュー中のフラグを下ろす
     */
    public void ResetPreview()
    {
        // プレビュー中でない場合は処理しない
        if (!isPreview) return;
        
        isPreview = false;
        
        // プレビュー中のタイルの色を元に戻す
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = Color.white;
    }

    /**
     * タイルタイプ取得
     */
    public TileTypes GetTileType()
    {
        return TileType;
    }

    /**
     * タイルを空に設定する
     */
    public void SetNone()
    {
        // タイルの種類を空に設定
        TileType = TileTypes.None;
        
        // タイルを空のオブジェクトに変更
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = Color.white;
    }
}
