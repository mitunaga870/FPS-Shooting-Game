using Enums;
using lib;
using Traps;
using UnityEngine;


public class Tile : MonoBehaviour
{
    /** 連続入力防止時間 */
    private const float ContinuousInputPreventionTime = 1f;

    /** デフォルトのモデル */
    [SerializeField] private Mesh defaultModel;

    /** どこにもつながっていないモデル */
#pragma warning disable CS0414 // フィールドは割り当てられていますがその値は使用されていません
    [SerializeField] private Mesh noneModel;
#pragma warning restore CS0414 // フィールドは割り当てられていますがその値は使用されていません

    /** 行き止まりのモデル */
    [SerializeField] private Mesh deadEndModel;

    /** 直線のモデル */
    [SerializeField] private Mesh straightModel;

    /** コーナーのモデル */
    [SerializeField] private Mesh cornerModel;

    /** T字路のモデル */
    [SerializeField] private Mesh tJunctionModel;

    /** 十字路のモデル */
    [SerializeField] private Mesh crossroadsModel;

    /** 現在のタイルタイプ */
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

    private MazeController _mazeController;
    private int _row;
    private int _column;

    /** クリックの連続入力を防ぐためのフラグ */
    private bool _continuousClickFlag;

    /** マウスエンターの連続入力を防ぐためのフラグ */
    private bool _continuousMouseEnterFlag;

    /** プレビュー中フラグ */
    public bool isPreview;

    // Start is called before the first frame update
    void Start()
    {
        // 初期状態指定
        TileType = TileTypes.Nothing;
        _continuousClickFlag = false;
        _continuousMouseEnterFlag = false;
        GetComponent<Outline>().enabled = false;
        // ========= トラップのプレハブを取得 =========
        // ゲームオブジェクトとしてトラップを取得
    }

    /**
     * クリック時にタイルの種類を変更開始・終了する
     */
    private void OnMouseOver()
    {
        // UIでブロックされている場合は処理しない
        if (General.IsPointerOverUIObject()) return;
        // 連続入力を防ぐ
        if (_continuousClickFlag) return;

        // タイルの種類を道と道路でトグル
        if (!_mazeController.IsEditingRoad && Input.GetMouseButton(0))
        {
            _mazeController.StartRoadEdit(_column, _row, TileTypes.Road);
            // 連続入力フラグを立てる
            _continuousClickFlag = true;
            // 0.5秒後に連続入力フラグを下ろす
            StartCoroutine(General.DelayCoroutine(ContinuousInputPreventionTime, () => _continuousClickFlag = false));
        }
        else if (!_mazeController.IsEditingRoad && Input.GetMouseButton(1))
        {
            _mazeController.StartRoadEdit(_column, _row, TileTypes.Nothing);
            // 連続入力フラグを立てる
            _continuousClickFlag = true;
            // 0.5秒後に連続入力フラグを下ろす
            StartCoroutine(General.DelayCoroutine(ContinuousInputPreventionTime, () => _continuousClickFlag = false));
        }
        // 道編集中の同ボタンは終了
        else if (_mazeController.IsEditingRoad && _mazeController.EditingTargetTileType == TileTypes.Road &&
                 Input.GetMouseButtonUp(0))
        {
            _mazeController.EndRoadEdit();
        }
        else if (_mazeController.IsEditingRoad && _mazeController.EditingTargetTileType == TileTypes.Nothing &&
                 Input.GetMouseButtonUp(1))
        {
            _mazeController.EndRoadEdit();
        }
        // プレビュー中の場合は終了
        else if (_mazeController.IsEditingRoad && (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0)))
        {
            _mazeController.CancelRoadEdit();
        }
    }

    /**
     * プレビュー用
     */
    private void OnMouseEnter()
    {
        // UIでブロックされている場合は処理しない
        if (General.IsPointerOverUIObject()) return;
        // 連続入力を防ぐ
        if (_continuousMouseEnterFlag) return;
        // 道編集中でない場合は処理しない
        if (!_mazeController.IsEditingRoad) return;

        if (_mazeController.IsOneStrokeMode)
        {
            _mazeController.PreviewOneStrokeMode(_column, _row);
        }
        else
        {
            _mazeController.PreviewRoadEdit(_column, _row);
        }

        // 連続入力フラグを立てる
        _continuousMouseEnterFlag = true;
        // 0.5秒後に連続入力フラグを下ろす
        StartCoroutine(General.DelayCoroutine(ContinuousInputPreventionTime, () => _continuousMouseEnterFlag = false));
    }


    /**
     * タイルのステータスの変更
     */
    private void OnTileTypeChanged()
    {
        // タイルのステータスが変わった時の処理
    }

    /**
     * コントローラー・行列を設定する
     * 初回のみ設定可能
     */
    public void Initialize(MazeController mazeController, int row, int column)
    {
        if (this._mazeController != null) return;

        this._mazeController = mazeController;
        this._row = row;
        this._column = column;
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
        var meshFilter = GetComponent<MeshFilter>();
        // つながった道の回転を設定
        var rotation = transform.rotation;
        switch (roadAdjust)
        {
            // ========== 行き止まり ==========
            case RoadAdjust.LeftDeadEnd:
                meshFilter.mesh = deadEndModel;
                rotation = Quaternion.Euler(-90, 180, 0);
                break;
            case RoadAdjust.BottomDeadEnd:
                meshFilter.mesh = deadEndModel;
                rotation = Quaternion.Euler(-90, 90, 0);
                break;
            case RoadAdjust.RightDeadEnd:
                meshFilter.mesh = deadEndModel;
                break;
            case RoadAdjust.TopDeadEnd:
                meshFilter.mesh = deadEndModel;
                rotation = Quaternion.Euler(-90, -90, 0);
                break;
            // ========== 直線 ==========
            case RoadAdjust.TopBottom:
                rotation = Quaternion.Euler(-90, 90, 0);
                meshFilter.mesh = straightModel;
                break;
            case RoadAdjust.LeftRight:
                meshFilter.mesh = straightModel;
                break;
            // ========== コーナー ==========
            case RoadAdjust.TopRight:
            case RoadAdjust.RightTop:
                meshFilter.mesh = cornerModel;
                break;
            case RoadAdjust.LeftTop:
            case RoadAdjust.TopLeft:
                meshFilter.mesh = cornerModel;
                rotation = Quaternion.Euler(-90, -90, 0);
                break;
            case RoadAdjust.LeftBottom:
            case RoadAdjust.BottomLeft:
                meshFilter.mesh = cornerModel;
                rotation = Quaternion.Euler(-90, 180, 0);
                break;
            case RoadAdjust.RightBottom:
            case RoadAdjust.BottomRight:
                meshFilter.mesh = cornerModel;
                rotation = Quaternion.Euler(-90, 90, 0);
                break;
            // ========== T字路 ==========
            case RoadAdjust.TopRightLeft:
                meshFilter.mesh = tJunctionModel;
                break;
            case RoadAdjust.RightBottomTop:
                meshFilter.mesh = tJunctionModel;
                rotation = Quaternion.Euler(-90, 90, 0);
                break;
            case RoadAdjust.LeftTopBottom:
                meshFilter.mesh = tJunctionModel;
                rotation = Quaternion.Euler(-90, -90, 0);
                break;
            case RoadAdjust.BottomLeftRight:
                meshFilter.mesh = tJunctionModel;
                rotation = Quaternion.Euler(-90, 180, 0);
                break;
            case RoadAdjust.Cross:
                meshFilter.mesh = crossroadsModel;
                break;
        }

        // つながった道の回転を設定
        transform.rotation = rotation;
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
        GetComponent<Outline>().enabled = true;
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
        GetComponent<Outline>().enabled = false;
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
        TileType = TileTypes.Nothing;

        // モデルを変更
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = defaultModel;

        // アウトラインを消す
        GetComponent<Outline>().enabled = false;


        // 回転を元に戻す
        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    public ATrap SetTrap()
    {
        // 既に道・トラップが設定されている場合は処理しない
        if (TileType == TileTypes.Trap) return null;

        // タイルの種類をトラップに設定
        TileType = TileTypes.Trap;

        var traps = Resources.LoadAll<ATrap>("Prefabs/Traps");
        ATrap trap = null;

        // ランダムなトラップを設定
        do
        {
            // トラップがある場合は削除
            if (trap != null) Destroy(trap);

            // ランダムなトラップ用インデックスを取得
            var randomIndex = Random.Range(0, traps.Length);

            // トラップを生成
            trap = Instantiate(traps[randomIndex], transform.position, Quaternion.identity);

            // トラップの高さを設定
            var position = trap.transform.position;
            position = new Vector3(position.x, trap.GetHeight(), position.z);
            trap.transform.position = position;
        } while (ATrap.IsProhibitedArea(_row, _column));

        return trap;
    }

    public void ResetTile()
    {
        Destroy(gameObject);
    }
}