using System;
using DataClass;
using InvasionPhase;
using JetBrains.Annotations;
using lib;
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
        // ReSharper disable once NotAccessedField.Global
        protected TrapObject trapObject;

        /**
         * シーンコントローラー
         * 時刻取得
         */
        [CanBeNull]
        protected InvasionController SceneController;
        
        /**
         * 迷路コントローラー
         */
        [CanBeNull]
        protected InvasionMazeController MazeController;

        /**
         * 敵をコントロールしてるクラス
         * これにアクセスして敵に影響を与える
         */
        [CanBeNull]
        protected InvasionEnemyController EnemyController;

        /**
         * 侵攻準備ができているか
         */
        private bool _isInvasionReady;

        /**
         * チャージ時間
         * 0の場合は即発火
         */
        protected int ChargeTime;

        /** 前読み込んだ時のゲーム内時間 */
        private int _prevTime;
        
        /** ダメージの増加量 */
        private int _ampDamage = 0;

        private void Update()
        {
            // 侵攻phaseじゃないと処理しない
            if (SceneController == null || !_isInvasionReady) return;
            
            // 時間処理
            var currentTime = SceneController.GameTime;
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
        public abstract int GetDefaultDamage();

        /** カスタムのコンストラクタ */
        public void InvasionInitialize(
            InvasionController sceneController,
            InvasionMazeController mazeController, 
            InvasionEnemyController enemyController
        ) {
            _isInvasionReady = true;
            this.MazeController = mazeController;
            this.SceneController = sceneController;
            this.EnemyController = enemyController;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
        
        public int GetDamage()
        {
            return GetDefaultDamage() + _ampDamage;
        }
        
        /**
         * ダメージを追加する
         */
        public void AddDamage(int addDamage, int duration)
        {
            _ampDamage += addDamage;
            
            // 一定時間後にダメージを元に戻す
            var delay = General.DelayCoroutineByGameTime(
                SceneController,
                duration,
                () => _ampDamage -= addDamage
            );
            StartCoroutine(delay);
        }
    }
}