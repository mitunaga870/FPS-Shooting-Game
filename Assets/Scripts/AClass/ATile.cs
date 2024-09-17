using Enums;
using lib;
using UnityEngine;
using UnityEngine.Serialization;

namespace AClass
{
    public abstract class ATile : MonoBehaviour
    {
        /** デフォルトのモデル */
        [SerializeField] protected Mesh defaultModel;

        /** どこにもつながっていないモデル */
        [SerializeField] protected Mesh noneModel;

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

        /** L字壁のモデル */
        [SerializeField] protected Mesh halfOnceModel;

        /** 太いやつの端っこ */
        [SerializeField] protected Mesh halfRoadModel;

        /** 太いやつの真ん中 */
        [SerializeField] protected Mesh noWallModel;

        /** 三角 */
        [SerializeField] protected Mesh tripleCurve;

        /** ２角　*/
        [SerializeField] protected Mesh doubleCurve;

        /** １角　*/
        [SerializeField] protected Mesh singleCurve;

        /** 角と壁 下に壁持ってきたときに右に角 */
        [SerializeField] protected Mesh rightCornerAndBottomWall;

        /** 角と壁 下に壁持ってきたときに左に角 */
        [SerializeField] protected Mesh leftCornerAndBottomWall;

        /** 斜め角 */
        [SerializeField] protected Mesh diagonalCorner;

        /** 現在のタイルタイプ */
        private TileTypes _tileType = TileTypes.Nothing;

        public TileTypes TileType
        {
            get => _tileType;
            protected set
            {
                _tileType = value;
                OnTileTypeChanged();
            }
        }

        /** 道のつながり型 */
        public RoadAdjust RoadAdjust { get; private set; }

        protected int Row;
        protected int Column;

        /** 設置されているトラップ */
        private ATrap _trap = null;

        /** トラップの所持フラグ */
        private bool hasTrap = false;

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
                // ========== 太い道路 ==========
                // ========== 太い道路の真ん中 ==========
                case RoadAdjust.BottomLeftDot:
                    meshFilter.mesh = singleCurve;
                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.BottomRightDot:
                    meshFilter.mesh = singleCurve;
                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopLeftDot:
                    meshFilter.mesh = singleCurve;
                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.TopRightDot:
                    meshFilter.mesh = singleCurve;
                    break;
                // ２角
                case RoadAdjust.TopDoubleDot:
                    meshFilter.mesh = doubleCurve;
                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.BottomDoubleDot:
                    meshFilter.mesh = doubleCurve;
                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.LeftDoubleDot:
                    meshFilter.mesh = doubleCurve;
                    break;
                case RoadAdjust.RightDoubleDot:
                    meshFilter.mesh = doubleCurve;
                    rotation = Quaternion.Euler(-90, -180, 0);
                    break;
                case RoadAdjust.TopLeftAndBottomRightDot:
                    meshFilter.mesh = diagonalCorner;
                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopRightAndBottomLeftDot:
                    meshFilter.mesh = diagonalCorner;
                    break;
                // 三角
                case RoadAdjust.ExpectBottomLeftDot:
                    meshFilter.mesh = tripleCurve;
                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.ExpectBottomRightDot:
                    meshFilter.mesh = tripleCurve;
                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.ExpectTopLeftDot:
                    meshFilter.mesh = tripleCurve;
                    break;
                case RoadAdjust.ExpectTopRightDot:
                    meshFilter.mesh = tripleCurve;
                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.NoWall:
                    meshFilter.mesh = noWallModel;
                    break;
                // 壁と角　下に壁持ってきたときに右に角
                case RoadAdjust.BottomWallWithRightDot:
                    meshFilter.mesh = rightCornerAndBottomWall;
                    break;
                case RoadAdjust.LeftWallWithBottomDot:
                    meshFilter.mesh = rightCornerAndBottomWall;
                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopWallWithLeftDot:
                    meshFilter.mesh = rightCornerAndBottomWall;
                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.RightWallWithTopDot:
                    meshFilter.mesh = rightCornerAndBottomWall;
                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                // 壁と角　下に壁持ってきたときに左に角
                case RoadAdjust.BottomWallWithLeftDot:
                    meshFilter.mesh = leftCornerAndBottomWall;
                    break;
                case RoadAdjust.LeftWallWithTopDot:
                    meshFilter.mesh = leftCornerAndBottomWall;
                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopWallWithRightDot:
                    meshFilter.mesh = leftCornerAndBottomWall;
                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.RightWallWithBottomDot:
                    meshFilter.mesh = leftCornerAndBottomWall;
                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                // ========= 壁だけ ==========
                case RoadAdjust.LeftWall:
                    meshFilter.mesh = halfRoadModel;
                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.RightWall:
                    meshFilter.mesh = halfRoadModel;
                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.BottomWall:
                    meshFilter.mesh = halfRoadModel;
                    rotation = Quaternion.Euler(-90, 0, 0);
                    break;
                case RoadAdjust.TopWall:
                    meshFilter.mesh = halfRoadModel;
                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                // ========== L字壁 ==========
                case RoadAdjust.TopRightHalfOnce:
                    meshFilter.mesh = halfOnceModel;
                    break;
                case RoadAdjust.BottomRightHalfOnce:
                    meshFilter.mesh = halfOnceModel;
                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.BottomLeftHalfOnce:
                    meshFilter.mesh = halfOnceModel;
                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.TopLeftHalfOnce:
                    meshFilter.mesh = halfOnceModel;
                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                // つながっていない場合
                case RoadAdjust.NoAdjust:
                    meshFilter.mesh = noneModel;
                    break;
            }

            // つながった道の回転を設定
            transform.rotation = rotation;
        }


        public void ResetTile()
        {
            Destroy(gameObject);
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
            var outline = GetComponent<Outline>();
            if (outline) outline.enabled = false;


            // 回転を元に戻す
            transform.rotation = Quaternion.Euler(-90, 0, 0);
        }

        /**
     * デバッグの為にタイルの色を変更する
     */
        public void SetColor(Color color)
        {
            var meshRenderer = GetComponent<MeshRenderer>();
            var materials = meshRenderer.materials;

            foreach (var material in materials)
            {
                material.color = color;
            }
        }

        /**
     * スタート地点に設定する
     */
        public void SetStart()
        {
            // TODO: スタート地点の状態固定と示し方を決める
            SetColor(Color.blue);
        }

        /**
     * ゴール地点に設定する
     */
        public void SetGoal()
        {
            // TODO: ゴール地点の状態固定と示し方を決める
            SetColor(Color.red);
        }

        /**
         * 指定されたトラップを設置する
         * 設置出来たらtrueを返す
         */
        public bool SetTrap(string trapName)
        {
            var tilePosition = transform.position;

            // トラップを生成
            var trap = TrapGenerator.GenerateTrap(trapName);
            
            // トラップの位置を設定
            trap.transform.position = new Vector3(
                tilePosition.x,
                trap.GetHeight(),
                tilePosition.z
            );

            _trap = trap;
            hasTrap = true;

            return true;
        }

        /**
         * トラップを起動する
         */
        public void AwakeTrap()
        {
            if (hasTrap) _trap.AwakeTrap();
        }
    }
}