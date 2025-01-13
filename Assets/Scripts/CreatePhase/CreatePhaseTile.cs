using System;
using AClass;
using CreatePhase.UI;
using DataClass;
using Enums;
using lib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace CreatePhase
{
    public class CreatePhaseTile : ATile
    {
        /** 連続入力防止時間 */
        private const float ContinuousInputPreventionTime = 0.1f;

        private MazeCreationController _mazeCreationController;

        /** クリックの連続入力を防ぐためのフラグ */
        private bool _continuousClickFlag;

        /** マウスエンターの連続入力を防ぐためのフラグ */
        private bool _continuousMouseEnterFlag;

        /** プレビュー中フラグ */
        public bool isPreview;

        private IMouseEvent _mouseEventImplementation;
        public bool HasTurret => hasTurret;


        // Start is called before the first frame update
        private void Start()
        {
            // 初期状態指定
            _continuousClickFlag = false;
            _continuousMouseEnterFlag = false;
            GetComponent<Outline>().enabled = false;
            // ========= トラップのプレハブを取得 =========
            // ゲームオブジェクトとしてトラップを取得
        }

        private void OnMouseOver()
        {
            // UIでブロックされている場合は処理しない
            if (General.IsPointerOverUIObject()) return;

            // ======== トラップ設置用処理 =========

            // トラップ設置中の場合はトラップのプレビューを表示
            if (_mazeCreationController.IsSettingTurret)
            {
                _mazeCreationController.PreviewTurret(Column, Row);

                return;
            }

            if (hasTurret && Input.GetMouseButtonDown(0))
            {
                // タレットを回転
                Turret.Rotate();

                // タレット情報を上書き
                _mazeCreationController.UpdateTurretData(
                    new TurretData(Row, Column, Turret)
                );

                // プレビュー
                _mazeCreationController.SetPreviewTurretEffectArea(
                    Turret,
                    new TilePosition(Row, Column),
                    1000
                );

                return;
            }
            else if (hasTurret && Input.GetMouseButtonDown(1))
            {
                // タレットを削除
                _mazeCreationController.RemoveTurret(Column, Row);
                
                return;
            }

            // ======== トラップタレット詳細表示用処理 =========
            if (hasTurret)
                _mazeCreationController.ShowTurretDetail(Turret);
            else if (HasTrap)
                _mazeCreationController.ShowTrapDetail(Trap);

            // ======== 道路設置用処理 =========

            // 連続入力を防ぐ
            if (_continuousClickFlag) return;

            // タレットが設置されている場合は処理しない
            if (hasTurret) return;

            // タイルの種類を道と道路でトグル
            if (!_mazeCreationController.IsEditingRoad && Input.GetMouseButton(0))
            {
                _mazeCreationController.StartRoadEdit(Column, Row, TileTypes.Road);
                // 連続入力フラグを立てる
                _continuousClickFlag = true;
                // 0.5秒後に連続入力フラグを下ろす
                StartCoroutine(
                    General.DelayCoroutine(ContinuousInputPreventionTime, () => _continuousClickFlag = false)
                );
            }
            else if (!_mazeCreationController.IsEditingRoad && Input.GetMouseButton(1))
            {
                _mazeCreationController.StartRoadEdit(Column, Row, TileTypes.Nothing);
                // 連続入力フラグを立てる
                _continuousClickFlag = true;
                // 0.5秒後に連続入力フラグを下ろす
                StartCoroutine(
                    General.DelayCoroutine(ContinuousInputPreventionTime, () => _continuousClickFlag = false)
                );
            }
            // 道編集中の同ボタンは終了
            else if (_mazeCreationController.IsEditingRoad &&
                     _mazeCreationController.EditingTargetTileType == TileTypes.Road &&
                     Input.GetMouseButtonUp(0))
            {
                _mazeCreationController.EndRoadEdit();
            }
            else if (_mazeCreationController.IsEditingRoad &&
                     _mazeCreationController.EditingTargetTileType == TileTypes.Nothing &&
                     Input.GetMouseButtonUp(1))
            {
                _mazeCreationController.EndRoadEdit();
            }
            // プレビュー中の場合は終了
            else if (_mazeCreationController.IsEditingRoad && (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0)))
            {
                _mazeCreationController.CancelRoadEdit();
            }
        }


        /**
         * プレビュー用
         */
        private void OnMouseEnter()
        {
            // トラップ設置中の場合は処理しない
            if (_mazeCreationController.IsSettingTurret) return;

            // タレットがある時は処理しない
            if (hasTurret) return;

            // UIでブロックされている場合は処理しない
            if (General.IsPointerOverUIObject()) return;

            // 道編集中でない場合は処理しない
            if (!_mazeCreationController.IsEditingRoad) return;

            if (_mazeCreationController.IsOneStrokeMode)
                _mazeCreationController.PreviewOneStrokeMode(Column, Row);
            else
                _mazeCreationController.PreviewRoadEdit(Column, Row);

            // 連続入力を防ぐ
            if (_continuousMouseEnterFlag) return;

            // 連続入力フラグを立てる
            _continuousMouseEnterFlag = true;
            // 0.5秒後に連続入力フラグを下ろす
            StartCoroutine(General.DelayCoroutine(ContinuousInputPreventionTime,
                () => _continuousMouseEnterFlag = false));
        }

        /**
         * 離れた時にタレットトラップ詳細を非表示にする
         */
        private void OnMouseExit()
        {
            _mazeCreationController.CloseDetail();
        }


        /**
     * コントローラー・行列を設定する
     * 初回のみ設定可能
     */
        public void Initialize(MazeCreationController mazeCreationController, int row, int column)
        {
            if (_mazeCreationController != null) throw new Exception("Already initialized.");

            _mazeCreationController = mazeCreationController;
            Row = row;
            Column = column;
        }

        public void Initialize(
            MazeCreationController mazeCreationController,
            int row,
            int column,
            TileTypes tileType,
            RoadAdjust roadAdjust
        )
        {
            Initialize(mazeCreationController, row, column);

            // タイルの種類によって処理を変える
            switch (tileType)
            {
                case TileTypes.Road:
                case TileTypes.Goal:
                case TileTypes.Start:
                    SetRoad(roadAdjust);
                    break;
            }
        }


        /**
     * プレビュー中のフラグを立てる
     */
        public void SetRoadPreview()
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
        public void ResetRoadPreview()
        {
            // プレビュー中でない場合は処理しない
            if (!isPreview) return;

            isPreview = false;

            // プレビュー中のタイルの色を元に戻す
            GetComponent<Outline>().enabled = false;
        }

        /**
         * ランダムなトラップを設定する
         */
        public ATrap SetRandTrap()
        {
            // 既に道・トラップが設定されている場合は処理しない
            if (HasTrap) return null;
            HasTrap = true;

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
    }
}