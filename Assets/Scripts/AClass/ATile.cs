using System;
using System.Collections.Generic;
using DataClass;
using Enums;
using lib;
using UnityEngine;
using UnityEngine.Serialization;

namespace AClass
{
    public abstract class ATile : MonoBehaviour
    {
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

        /** 現在のタイルタイプ */
        private TileTypes _tileType = TileTypes.Nothing;

        public bool SettableTurret => !hasTrap && TileType == TileTypes.Nothing && !hasTurret;

        /** トラップのプレビューの色 */
        private Color _prevColor;

        /** 設置されているturret */
        protected ATurret Turret;

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
        protected ATrap _trap = null;

        /** 設置されているturret */
        protected ATurret _turret = null;

        /** トラップの所持フラグ */
        protected bool hasTrap = false;

        /** turretの所持フラグ */
        protected bool hasTurret = false;

        /** 色を変えたときの元の色を持たせる */
        private Dictionary<string, Color> prevColor;

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

            // turretが設置されている場合は処理しない
            if (hasTurret) return;

            // タイルの種類を道に設定
            TileType = TileTypes.Road;

            // 道のつながり型を設定
            RoadAdjust = roadAdjust;

            // 道の形状によってモデルを変更
            var meshFilter = GetComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>();

            // つながった道の回転を設定
            var rotation = transform.rotation;

            GameObject _model = null;

            switch (roadAdjust)
            {
                // ========== 行き止まり ==========
                case RoadAdjust.LeftDeadEnd:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(deadEndModel);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.BottomDeadEnd:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(deadEndModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.RightDeadEnd:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(deadEndModel);

                    rotation = Quaternion.Euler(-90, 0, 0);
                    break;
                case RoadAdjust.TopDeadEnd:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(deadEndModel);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                // ========== 直線 ==========
                case RoadAdjust.TopBottom:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(straightModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.LeftRight:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(straightModel);
                    break;
                // ========== コーナー ==========
                case RoadAdjust.TopRight:
                case RoadAdjust.RightTop:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(cornerModel);

                    break;
                case RoadAdjust.LeftTop:
                case RoadAdjust.TopLeft:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(cornerModel);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.LeftBottom:
                case RoadAdjust.BottomLeft:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(cornerModel);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.RightBottom:
                case RoadAdjust.BottomRight:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(cornerModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                // ========== T字路 ==========
                case RoadAdjust.TopRightLeft:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(tJunctionModel);

                    break;
                case RoadAdjust.RightBottomTop:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(tJunctionModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.LeftTopBottom:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(tJunctionModel);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.BottomLeftRight:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(tJunctionModel);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.Cross:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(crossroadsModel);
                    break;
                // ========== 太い道路 ==========
                // ========== 太い道路の真ん中 ==========
                case RoadAdjust.BottomLeftDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(singleCurve);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.BottomRightDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(singleCurve);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopLeftDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(singleCurve);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.TopRightDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(singleCurve);

                    break;
                // ２角
                case RoadAdjust.TopDoubleDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(doubleCurve);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.BottomDoubleDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(doubleCurve);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.LeftDoubleDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(doubleCurve);

                    break;
                case RoadAdjust.RightDoubleDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(doubleCurve);

                    rotation = Quaternion.Euler(-90, -180, 0);
                    break;
                case RoadAdjust.TopLeftAndBottomRightDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(diagonalCorner);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopRightAndBottomLeftDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(diagonalCorner);

                    break;
                // 三角
                case RoadAdjust.ExpectBottomLeftDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(tripleCurve);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.ExpectBottomRightDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(tripleCurve);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.ExpectTopLeftDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(tripleCurve);
                    break;
                case RoadAdjust.ExpectTopRightDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(tripleCurve);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.NoWall:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(noWallModel);
                    break;
                // 壁と角　下に壁持ってきたときに右に角
                case RoadAdjust.BottomWallWithRightDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(rightCornerAndBottomWall);
                    break;
                case RoadAdjust.LeftWallWithBottomDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(rightCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopWallWithLeftDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(rightCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.RightWallWithTopDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(rightCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                // 壁と角　下に壁持ってきたときに左に角
                case RoadAdjust.BottomWallWithLeftDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(leftCornerAndBottomWall);

                    break;
                case RoadAdjust.LeftWallWithTopDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(leftCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.TopWallWithRightDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(leftCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.RightWallWithBottomDot:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(leftCornerAndBottomWall);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                // ========= 壁だけ ==========
                case RoadAdjust.LeftWall:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(halfRoadModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.RightWall:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(halfRoadModel);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                case RoadAdjust.BottomWall:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(halfRoadModel);

                    rotation = Quaternion.Euler(-90, 0, 0);
                    break;
                case RoadAdjust.TopWall:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(halfRoadModel);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                // ========== L字壁 ==========
                case RoadAdjust.TopRightHalfOnce:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(halfOnceModel);

                    break;
                case RoadAdjust.BottomRightHalfOnce:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(halfOnceModel);

                    rotation = Quaternion.Euler(-90, 90, 0);
                    break;
                case RoadAdjust.BottomLeftHalfOnce:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(halfOnceModel);

                    rotation = Quaternion.Euler(-90, 180, 0);
                    break;
                case RoadAdjust.TopLeftHalfOnce:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(halfOnceModel);

                    rotation = Quaternion.Euler(-90, -90, 0);
                    break;
                // つながっていない場合
                case RoadAdjust.NoAdjust:
                    // 読み取り用にインスタンス化
                    _model = Instantiate(noneModel);
                    break;
            }

            // モデルを変更
            if (_model != null)
            {
                GetComponent<MeshFilter>().mesh = _model.GetComponent<MeshFilter>().sharedMesh;
                GetComponent<MeshRenderer>().materials = _model.GetComponent<MeshRenderer>().materials;

                Destroy(_model);
            }

            // ゴール地点の場合は色を変更
            switch (TileType)
            {
                case TileTypes.Goal:
                    SetGoal();
                    break;
                case TileTypes.Start:
                    SetStart();
                    break;
            }


            // つながった道の回転を設定
            transform.rotation = rotation;
        }


        public void ResetTile()
        {
            if (hasTrap)
            {
                Destroy(_trap.gameObject);
                hasTrap = false;
            }

            Destroy(gameObject);
        }

        /**
     * タイルを空に設定する
     */
        public void SetNone()
        {
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
            prevColor ??= new Dictionary<string, Color>();

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
        private void ResetColor()
        {
            if (prevColor == null) return;

            var meshRenderer = GetComponent<MeshRenderer>();
            var materials = meshRenderer.materials;

            for (var i = 0; i < materials.Length; i++)
            {
                var material = materials[i];

                // 色を取得
                Color? materialColor =
                    material.HasProperty("_Color") ? material.color : null;

                // 既存の色を保存
                if (materialColor != null)
                    material.color = prevColor[material.name];
            }

            prevColor = null;
        }

        /**
     * スタート地点に設定する
     */
        public void SetStart()
        {
            // TODO: スタート地点の状態固定と示し方を決める
            SetColor(Color.blue);

            TileType = TileTypes.Start;
        }

        /**
     * ゴール地点に設定する
     */
        public void SetGoal()
        {
            // TODO: ゴール地点の状態固定と示し方を決める
            SetColor(Color.red);

            TileType = TileTypes.Goal;
        }

        /**
         * 指定されたトラップを設置する
         * 設置出来たらtrueを返す
         */
        public bool SetTrap(string trapName)
        {
            var tilePosition = transform.position;

            // トラップを生成
            var trap = InstanceGenerator.GenerateTrap(trapName);

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
            if (hasTrap) _trap.AwakeTrap(new TilePosition(Row, Column));
        }

        /**
         * turretを設置する
         */
        public void SetTurret(ATurret settingTurret, int angle = 0)
        {
            // 既に道・トラップが設定されている場合は処理しない
            if (hasTrap) return;

            hasTurret = true;

            // トラップを生成
            Turret = Instantiate(settingTurret, transform.position, Quaternion.identity);

            Turret.SetAngle(angle);

            // トラップの高さを設定
            var position = Turret.transform.position;
            position = new Vector3(position.x, Turret.GetHeight(), position.z);
            Turret.transform.position = position;
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
            _prevColor = GetComponent<Renderer>().material.color;
            // とりあえず色を変える
            SetColor(Color.green);
        }

        /**
         * 効果エリアを元に戻す
         */
        public void ResetAreaPreview()
        {
            ResetColor();
        }
    }
}