using System;
using AClass;
using Enums;
using lib;
using Traps;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CreatePhase
{
    public class CreatePhaseTile : ATile
    {
        /** 連続入力防止時間 */
        private const float ContinuousInputPreventionTime = 1f;

        private MazeCreationController _mazeCreationController;

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
            // UIでブロックされている場合は処理しない
            if (General.IsPointerOverUIObject()) return;

            // 道編集中でない場合は処理しない
            if (!_mazeCreationController.IsEditingRoad) return;

            if (_mazeCreationController.IsOneStrokeMode)
            {
                _mazeCreationController.PreviewOneStrokeMode(Column, Row);
            }
            else
            {
                _mazeCreationController.PreviewRoadEdit(Column, Row);
            }

            // 連続入力を防ぐ
            if (_continuousMouseEnterFlag) return;

            // 連続入力フラグを立てる
            _continuousMouseEnterFlag = true;
            // 0.5秒後に連続入力フラグを下ろす
            StartCoroutine(General.DelayCoroutine(ContinuousInputPreventionTime,
                () => _continuousMouseEnterFlag = false));
        }


        /**
     * コントローラー・行列を設定する
     * 初回のみ設定可能
     */
        public void Initialize(MazeCreationController mazeCreationController, int row, int column)
        {
            if (this._mazeCreationController != null) throw new Exception("Already initialized.");

            this._mazeCreationController = mazeCreationController;
            this.Row = row;
            this.Column = column;
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

            // タイルの種類を設定
            switch (tileType)
            {
                case TileTypes.Road:
                    SetRoad(roadAdjust);
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
         * ランダムなトラップを設定する
         */
        public ATrap SetRandTrap()
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
    }
}