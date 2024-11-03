using System;
using DataClass;
using InvasionPhase;
using JetBrains.Annotations;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace AClass
{
    public abstract class ATrap : MonoBehaviour
    {
        /** トラップ用のデータ用スクリプタブルオブジェクト */
        [FormerlySerializedAs("trapData")]
        [SerializeField]
        protected TrapObject trapObject;

        /**
         * シーンコントローラー
         * 時刻取得
         */
        [CanBeNull]
        protected InvasionController sceneController = null;
        
        /**
         * 迷路コントローラー
         */
        [CanBeNull]
        protected InvasionMazeController mazeController = null;

        /**
         * 敵をコントロールしてるクラス
         * これにアクセスして敵に影響を与える
         */
        [CanBeNull]
        protected InvasionEnemyController enemyController = null;

        /**
         * 侵攻準備ができているか
         */
        protected bool IsInvasionReady = false;

        /**
         * チャージ時間
         * 0の場合は即発火
         */
        protected int ChargeTime = 0;

        /** 前読み込んだ時のゲーム内時間 */
        private int _prevTime;

        private void Update()
        {
            // 侵攻phaseじゃないと処理しない
            if (sceneController == null || !IsInvasionReady) return;
            
            // 時間処理
            var currentTime = sceneController.GameTime;
            _prevTime = currentTime;
            var timeDiff = currentTime - _prevTime;

            // CD中の場合は時間を進める
            if (ChargeTime <= 0) return;

            // CD時間を減らす
            ChargeTime -= timeDiff;

            // 0以下になったら0にする
            if (ChargeTime < 0) ChargeTime = 0;

        }

        /**
         * トラップの発火
         */
        public abstract void AwakeTrap(TilePosition position);

        /**
         * トラップの高さ取得
         */
        public abstract float GetHeight();

        /**
         * トラップの設置範囲取得
         * 1なら1＊1、2なら2＊2って感じ
         */
        public abstract int GetSetRange();

        /**
         * トラップごとの禁止エリア処理
         * - 禁止エリアのときはtrueを返す
         */
        public static bool IsProhibitedArea(int row, int col)
        {
            return false;
        }

        /**
         * トラップデータ用文字列の取得
         */
        public abstract string GetTrapName();

        public abstract int GetTrapAngle();
        public abstract void SetAngle(int trapAngle);

        /** カスタムのコンストラクタ */
        public void InvasionInitialize(
            InvasionController sceneController,
            InvasionMazeController mazeController, 
            InvasionEnemyController enemyController
        ) {
            IsInvasionReady = true;
            this.mazeController = mazeController;
            this.sceneController = sceneController;
            this.enemyController = enemyController;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}