using Enums;
using Traps;
using UnityEngine;

public class ATile : MonoBehaviour
{
    /** デフォルトのモデル */
    [SerializeField] protected Mesh defaultModel;

    /** どこにもつながっていないモデル */
#pragma warning disable CS0414 // フィールドは割り当てられていますがその値は使用されていません
    [SerializeField] protected Mesh noneModel;
#pragma warning restore CS0414 // フィールドは割り当てられていますがその値は使用されていません

    /** 行き止まりのモデル */
    [SerializeField] protected Mesh deadEndModel;

    /** 直線のモデル */
    [SerializeField] protected Mesh straightModel;

    /** コーナーのモデル */
    [SerializeField] protected Mesh cornerModel;

    /** T字路のモデル */
    [SerializeField] protected Mesh tJunctionModel;

    /** 十字路のモデル */
    [SerializeField] protected Mesh crossroadsModel;

    /** 現在のタイルタイプ */
    private TileTypes _tileType;

    protected TileTypes TileType
    {
        get => _tileType;
        set
        {
            _tileType = value;
            OnTileTypeChanged();
        }
    }

    /** 道のつながり型 */
    public RoadAdjust RoadAdjust { get; private set; }

    protected int Row;
    protected int Column;

    /**
     * タイルのステータスの変更
     */
    private void OnTileTypeChanged()
    {
        // タイルのステータスが変わった時の処理
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

        // 道のつながり型を設定
        RoadAdjust = roadAdjust;

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

    public ATrap SetTrap()
    {
        // 既に道・トラップが設定されている場合は処理しない
        if (TileType == TileTypes.Trap) return null;

        // タイルの種類をトラップに設定
        TileType = TileTypes.Trap;

        var traps = Resources.LoadAll<ATrap>("Prefabs/Traps");
        ATrap trap = null;

        // 無限ループ禁止用
        var loopCount = 0;

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

            // 設置できるものがない等で無限ループになる場合があるので、10回で終了
            if (loopCount++ > 10) break;

            // トラップが禁止エリアかどうか
        } while (ATrap.IsProhibitedArea(Row, Column));

        return trap;
    }

    public void ResetTile()
    {
        Destroy(gameObject);
    }
}