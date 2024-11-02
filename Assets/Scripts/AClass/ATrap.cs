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
            if (sceneController == null || IsInvasionReady) return;

            // CD中の場合は時間を進める
            if (ChargeTime <= 0) return;

            // 時間処理
            var currentTime = sceneController.GameTime;
            var timeDiff = currentTime - _prevTime;

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

        public void InvasionInitialize(InvasionController sceneController, InvasionEnemyController enemyController)
        {
            IsInvasionReady = true;
            this.sceneController = sceneController;
            this.enemyController = enemyController;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}