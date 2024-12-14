using System.Collections;
using System.Collections.Generic;
using DataClass;
using Enums;
using lib;
using UnityEngine;

namespace AClass
{
    public abstract class ATile : MonoBehaviour
    {
        public TilePosition Position => new TilePosition(Row, Column);
        
        /** デフォルトのモデル */
        [SerializeField]
        protected GameObject defaultModel;

        /** どこにもつながっていないモデル */
        [SerializeField]
        protected GameObject noneModel;

        /** 行き止まりのモデル */
        [SerializeField]
        protected GameObject deadEndModel;

        /** 直線のモデル */
        [SerializeField]
        protected GameObject straightModel;

        /** コーナーのモデル */
        [SerializeField]
        protected GameObject cornerModel;

        /** T字路のモデル */
        [SerializeField]
        protected GameObject tJunctionModel;

        /** 十字路のモデル */
        [SerializeField]
        protected GameObject crossroadsModel;

        /** L字壁のモデル */
        [SerializeField]
        protected GameObject halfOnceModel;

        /** 太いやつの端っこ */
        [SerializeField]
        protected GameObject halfRoadModel;

        /** 太いやつの真ん中 */
        [SerializeField]
        protected GameObject noWallModel;

        /** 三角 */
        [SerializeField]
        protected GameObject tripleCurve;

        /** ２角　*/
        [SerializeField]
        protected GameObject doubleCurve;

        /** １角　*/
        [SerializeField]
        protected GameObject singleCurve;

        /** 角と壁 下に壁持ってきたときに右に角 */
        [SerializeField]
        protected GameObject rightCornerAndBottomWall;

        /** 角と壁 下に壁持ってきたときに左に角 */
        [SerializeField]
        protected GameObject leftCornerAndBottomWall;

        /** 斜め角 */
        [SerializeField]
        protected GameObject diagonalCorner;
        
        /** スタート用デカール */
        [SerializeField]
        protected GameObject startDecal;
        
        /** ゴール用デカール */
        [SerializeField]
        protected GameObject goalDecal;

        /** 現在のタイルタイプ */
        private TileTypes _tileType = TileTypes.Nothing;

        public bool SettableTurret => !HasTrap && TileType == TileTypes.Nothing && !hasTurret;

        /** トラップのプレビューの色 */
        private Color _prevColor;

        /** 設置されているturret */
        public ATurret Turret { get; private set; }

        public TileTypes TileType
        {
            get => _tileType;
            private set
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
        public ATrap Trap { get; private set; }

        /** トラップの所持フラグ */
        protected bool HasTrap;

        /** turretの所持フラグ */
        // ReSharper disable once InconsistentNaming
        protected bool hasTurret;

        /** 色を変えたときの元の色を持たせる */
        // ReSharper disable once InconsistentNaming
        private Dictionary<string, Color> prevColor;
        
        /** 禁止エリア・効果エリアの前の色 */
        private Dictionary<string, Color> _prevPreviewColor;
        
        /**
         * タイルのステータスの変更
         */
        private void OnTileTypeChanged()
        {
            // タイルのステータスが変わった時の処理
        }
        
        // ============= 阻害エリアの処理 ==============================
        public bool IsBlockArea { get; protected set; }
        protected IEnumerator BlockAreaCoroutine;
        // ==========================================================


        /**
         * タイルを道に設定する
         * @param roadAdjust 道の形状
         */
        public void SetRoad(RoadAdjust roadAdjust)
        {
            // 既に道・トラップが設定されている場合は処理しない
            if (TileType == TileTypes.Road) return;

            // turretが設置されている場合は処理しない
            if (hasTurret) return;

            // タイルの種類を道に設定
            if (TileType != TileTypes.Goal && TileType != TileTypes.Start)
                TileType = TileTypes.Road;

            // 道のつながり型を設定
            RoadAdjust = roadAdjust;

            // つながった道の回転を設定
            var rotation = transform.rotation;

            GameObject model = null;

            switch (roadAdjust)
            {
                // ========== 行き止まり ==========
                case RoadAdjust.LeftDeadEnd:
                    // 読み取り用にインスタンス化
                    model = Instantiate(deadEndModel);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.BottomDeadEnd:
                    // 読み取り用にインスタンス化
                    model = Instantiate(deadEndModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.RightDeadEnd:
                    // 読み取り用にインスタンス化
                    model = Instantiate(deadEndModel);

                    rotation = Quaternion.Euler(-90, 0, 0);
                    break;
                case RoadAdjust.TopDeadEnd:
                    // 読み取り用にインスタンス化
                    model = Instantiate(deadEndModel);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                // ========== 直線 ==========
                case RoadAdjust.TopBottom:
                    // 読み取り用にインスタンス化
                    model = Instantiate(straightModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.LeftRight:
                    // 読み取り用にインスタンス化
                    model = Instantiate(straightModel);
                    break;
                // ========== コーナー ==========
                case RoadAdjust.TopRight:
                case RoadAdjust.RightTop:
                    // 読み取り用にインスタンス化
                    model = Instantiate(cornerModel);

                    break;
                case RoadAdjust.LeftTop:
                case RoadAdjust.TopLeft:
                    // 読み取り用にインスタンス化
                    model = Instantiate(cornerModel);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.LeftBottom:
                case RoadAdjust.BottomLeft:
                    // 読み取り用にインスタンス化
                    model = Instantiate(cornerModel);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.RightBottom:
                case RoadAdjust.BottomRight:
                    // 読み取り用にインスタンス化
                    model = Instantiate(cornerModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                // ========== T字路 ==========
                case RoadAdjust.TopRightLeft:
                    // 読み取り用にインスタンス化
                    model = Instantiate(tJunctionModel);

                    break;
                case RoadAdjust.RightBottomTop:
                    // 読み取り用にインスタンス化
                    model = Instantiate(tJunctionModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.LeftTopBottom:
                    // 読み取り用にインスタンス化
                    model = Instantiate(tJunctionModel);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.BottomLeftRight:
                    // 読み取り用にインスタンス化
                    model = Instantiate(tJunctionModel);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.Cross:
                    // 読み取り用にインスタンス化
                    model = Instantiate(crossroadsModel);
                    break;
                // ========== 太い道路 ==========
                // ========== 太い道路の真ん中 ==========
                case RoadAdjust.BottomLeftDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(singleCurve);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.BottomRightDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(singleCurve);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopLeftDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(singleCurve);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.TopRightDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(singleCurve);

                    break;
                // ２角
                case RoadAdjust.TopDoubleDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(doubleCurve);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.BottomDoubleDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(doubleCurve);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.LeftDoubleDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(doubleCurve);

                    break;
                case RoadAdjust.RightDoubleDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(doubleCurve);

                    rotation = Quaternion.Euler(-90, -180, 0);
                    break;
                case RoadAdjust.TopLeftAndBottomRightDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(diagonalCorner);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopRightAndBottomLeftDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(diagonalCorner);

                    break;
                // 三角
                case RoadAdjust.ExpectBottomLeftDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(tripleCurve);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.ExpectBottomRightDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(tripleCurve);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.ExpectTopLeftDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(tripleCurve);
                    break;
                case RoadAdjust.ExpectTopRightDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(tripleCurve);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.NoWall:
                    // 読み取り用にインスタンス化
                    model = Instantiate(noWallModel);
                    break;
                // 壁と角　下に壁持ってきたときに右に角
                case RoadAdjust.BottomWallWithRightDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(rightCornerAndBottomWall);
                    break;
                case RoadAdjust.LeftWallWithBottomDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(rightCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopWallWithLeftDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(rightCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.RightWallWithTopDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(rightCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                // 壁と角　下に壁持ってきたときに左に角
                case RoadAdjust.BottomWallWithLeftDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(leftCornerAndBottomWall);

                    break;
                case RoadAdjust.LeftWallWithTopDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(leftCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopWallWithRightDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(leftCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.RightWallWithBottomDot:
                    // 読み取り用にインスタンス化
                    model = Instantiate(leftCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                // ========= 壁だけ ==========
                case RoadAdjust.LeftWall:
                    // 読み取り用にインスタンス化
                    model = Instantiate(halfRoadModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.RightWall:
                    // 読み取り用にインスタンス化
                    model = Instantiate(halfRoadModel);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.BottomWall:
                    // 読み取り用にインスタンス化
                    model = Instantiate(halfRoadModel);

                    rotation = Quaternion.Euler(-90, 0, 0);
                    break;
                case RoadAdjust.TopWall:
                    // 読み取り用にインスタンス化
                    model = Instantiate(halfRoadModel);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                // ========== L字壁 ==========
                case RoadAdjust.TopRightHalfOnce:
                    // 読み取り用にインスタンス化
                    model = Instantiate(halfOnceModel);

                    break;
                case RoadAdjust.BottomRightHalfOnce:
                    // 読み取り用にインスタンス化
                    model = Instantiate(halfOnceModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.BottomLeftHalfOnce:
                    // 読み取り用にインスタンス化
                    model = Instantiate(halfOnceModel);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.TopLeftHalfOnce:
                    // 読み取り用にインスタンス化
                    model = Instantiate(halfOnceModel);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                // つながっていない場合
                case RoadAdjust.NoAdjust:
                    // 読み取り用にインスタンス化
                    model = Instantiate(noneModel);
                    break;
            }

            // モデルを変更
            if (model != null)
            {
                GetComponent<MeshFilter>().mesh = model.GetComponent<MeshFilter>().sharedMesh;
                GetComponent<MeshRenderer>().materials = model.GetComponent<MeshRenderer>().materials;

                Destroy(model);
            }

            // つながった道の回転を設定
            transform.rotation = rotation;
        }


        public void ResetTile()
        {
            if (HasTrap)
            {
                Destroy(Trap.gameObject);
                HasTrap = false;
            }

            Destroy(gameObject);
        }

        /**
     * タイルを空に設定する
     */
        public void SetNone()
        {
            if (TileType is TileTypes.Goal or TileTypes.Start) return;
            // タイルの種類を空に設定
            TileType = TileTypes.Nothing;


            var model = Instantiate(defaultModel);
            GetComponent<MeshFilter>().mesh = model.GetComponent<MeshFilter>().sharedMesh;
            GetComponent<MeshRenderer>().materials = model.GetComponent<MeshRenderer>().materials;
            Destroy(model);

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
            // 既存の色リストを初期化
            prevColor = new Dictionary<string, Color>();

            var meshRenderer = GetComponent<MeshRenderer>();
            var materials = meshRenderer.materials;

            foreach (var material in materials)
            {
                // 色を取得
                Color? materialColor =
                    material.HasProperty("_Color") ? material.color : null;

                // 既存の色を保存
                if (materialColor != null)
                    prevColor.Add(material.name, materialColor.Value);

                material.color = color;
            }
        }

        /**
         * 既存の色に戻す
         */
        // ReSharper disable once ParameterHidesMember
        // ReSharper disable once InconsistentNaming
        public void ResetColor(Dictionary<string, Color> _prevColor = null)
        {
            
            if (prevColor == null ) return;

            if (_prevColor == null)
            {
                _prevColor = prevColor;
                prevColor = null;
            }
            

            var meshRenderer = GetComponent<MeshRenderer>();
            var materials = meshRenderer.materials;

            for (var i = 0; i < materials.Length; i++)
            {
                var material = materials[i];

                // 色を取得
                Color? materialColor =
                    material.HasProperty("_Color") ? material.color : null;

                // 既存の色を当てる
                if (materialColor != null)
                    material.color = _prevColor[material.name];
            }
        }

        /**
     * スタート地点に設定する
     */
        public void SetStart()
        {
            TileType = TileTypes.Start;
            
            // デカール設置
            var decal = Instantiate(startDecal, transform.position, Quaternion.identity);
            decal.transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        /**
     * ゴール地点に設定する
     */
        public void SetGoal()
        {
            TileType = TileTypes.Goal;
            
            // デカール設置
            var decal = Instantiate(goalDecal, transform.position, Quaternion.identity);
            decal.transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        /**
         * 指定されたトラップを設置する
         * 設置出来たらtrueを返す
         */
        public bool SetTrap(AMazeController mazeController, string trapName, int trapAngle = -1)
        {
            // 既に道・トラップが設定されている場合は処理しない
            if (HasTrap) return false;
            // スタート・ゴールは設置しない
            if (TileType is TileTypes.Start or TileTypes.Goal) return false;

            var tilePosition = transform.position;

            // トラップを取得
            var trap = InstanceGenerator.GenerateTrap(trapName);

            // 周囲のタイルを取得
            var tiles = new List<ATile>();
            for (var i = 0; i < trap.GetSetRange(); i++)
            for (var j = 0; j < trap.GetSetRange(); j++)
            {
                if (i == 0 && j == 0) continue;

                var tile = mazeController.GetTile(Row + i, Column + j);

                if (tile == null) return false;

                if (tile.HasTrap) return false;
                
                if (tile._tileType is TileTypes.Goal or TileTypes.Start) return false;

                tiles.Add(tile);
            }

            // トラップの位置を設定
            tilePosition = new Vector3(
                tilePosition.x + Environment.TileSize / 2f * (trap.GetSetRange() - 1),
                trap.GetHeight(),
                tilePosition.z + Environment.TileSize / 2f * (trap.GetSetRange() - 1)
            );


            // トラップを生成
            trap = Instantiate(trap, tilePosition, Quaternion.identity);

            // トラップの角度を指定
            if (trapAngle != -1)
                trap.SetAngle(trapAngle);

            trap.transform.rotation = Quaternion.Euler(0, trap.GetTrapAngle(), 0);

            // 周囲タイルにトラップを設置したことにする
            foreach (var tile in tiles)
            {
                tile.HasTrap = true;
                tile.Trap = trap;
            }

            Trap = trap;
            HasTrap = true;

            return true;
        }

        /**
         * トラップを起動する
         */
        public void AwakeTrap()
        {
            if (HasTrap) Trap.AwakeTrap(new TilePosition(Row, Column));
        }

        /**
         * turretを設置する
         */
        public void SetTurret(ATurret settingTurret, int angle = 0)
        {
            // 既に道・トラップが設定されている場合は処理しない
            if (HasTrap) return;

            hasTurret = true;

            // トラップを生成
            Turret = Instantiate(settingTurret, transform.position, Quaternion.identity);

            Turret.SetAngle(angle);

            // トラップの高さを設定
            var position = Turret.transform.position;
            position = new Vector3(position.x, Turret.GetHeight(), position.z);
            Turret.transform.position = position;
            
            // turretの向きを設定
            Turret.transform.rotation = Quaternion.Euler(0, Turret.Angle, 0);
        }

        /**
         * turretを設置する
         */
        public void SetTurret(string turretName, int angle = 0)
        {
            var turret = InstanceGenerator.GenerateTurret(turretName);
            SetTurret(turret, angle);
        }

        /**
         * 効果エリアを見せる
         * @return 元の色
         */
        public void SetAreaPreview()
        {
            if (_prevPreviewColor == null)
            {
                _prevPreviewColor = new Dictionary<string, Color>();
                
                // マテリアル取得
                var meshRenderer = GetComponent<MeshRenderer>();
                var materials = meshRenderer.materials;

                // 既存の色を保存
                foreach (var material in materials)
                {
                    // 色を取得
                    Color? materialColor =
                        material.HasProperty("_Color") ? material.color : null;

                    // 既存の色を保存
                    if (materialColor != null)
                        _prevPreviewColor.Add(material.name, materialColor.Value);
                }
            }

            // とりあえず色を変える
            SetColor(Color.green);
        }

        /**
         * 効果エリアを元に戻す
         */
        public void ResetAreaPreview()
        {
            ResetColor(_prevPreviewColor);
            
            _prevPreviewColor = null;
        }
        
        /**
         * 禁止エリア処理
         */
        public void SetProhibitedArea()
        {
            if (_prevPreviewColor == null)
            {
                _prevPreviewColor = new Dictionary<string, Color>();
                
                // マテリアル取得
                var meshRenderer = GetComponent<MeshRenderer>();
                var materials = meshRenderer.materials;

                // 既存の色を保存
                foreach (var material in materials)
                {
                    // 色を取得
                    Color? materialColor =
                        material.HasProperty("_Color") ? material.color : null;

                    // 既存の色を保存
                    if (materialColor != null)
                        _prevPreviewColor.Add(material.name, materialColor.Value);
                }
            }
            
            SetColor(Color.red);
        }

        /**
         * 禁止エリア処理
         */
        public void ResetProhibitedArea()
        {
            ResetColor( _prevPreviewColor);
            
            _prevPreviewColor = null;
        }

        /**
         * turretを削除する
         */
        public ATurret RemoveTurret()
        {
            // turretが設置されていない場合は処理しない
            if (!hasTurret) return null;
            
            // クラスを作成して返す
            var returnTurret = InstanceGenerator.GenerateTurret(Turret.GetTurretName());

            hasTurret = false;
            Destroy(Turret.gameObject);
            
            return returnTurret;
        }
    }
}