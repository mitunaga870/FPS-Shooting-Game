#nullable enable
using System;
using System.Collections.Generic;
using CreatePhase;
using DataClass;
using Enums;
using InvasionPhase;
using JetBrains.Annotations;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.PlayerLoop;

#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。

namespace AClass
{
    public abstract class ATurret : MonoBehaviour
    {
        [SerializeField]
        protected TurretObject turretObject;

        protected InvasionController sceneController;

        protected InvasionEnemyController EnemyController;

        protected InvasionMazeController MazeController;

        private bool _isInitialized;

        private bool _isEnable;

        protected Phase Phase;

        public int Angle { get; protected set; } = 0;

        private int chargeTime;

        private int prevTime = 0;
        
        private int effectTime = 0;
        
        private bool _isFistAwake = true;

        protected TilePosition SetPosition { get; private set; }
        
        /**
         * 初期化処理
         */
        private void Start()
        {
            chargeTime = GetInterval();
        }

        /**
         * 一定期間ごとにAwakeTurretを呼び出す
         */
        private void FixedUpdate()
        {
            // 侵攻phaseのみ
            if (Phase != Phase.Invade) return;

            // 字関連処理
            var currentTime = sceneController.GameTime;
            var deltaTime = currentTime - prevTime;
            prevTime = currentTime;

            // インターバルの処理
            chargeTime -= deltaTime;
            
            // エフェクト時間の処理
            effectTime -= deltaTime;
            
            // エフェクト時間が終わったらタレットを休眠
            if (effectTime <= 0)
            {
                AsleepTurret();
                _isFistAwake = true;
            }

            // 未チャージ状態かつエフェクト時間が終わっている場合は戻す
            if (chargeTime > 0 && effectTime <= 0) return;

            // 範囲内に敵がいるか確認
            var effectArea = GetAbsoluteEffectArea(SetPosition);

            // 範囲の敵取得
            var enemies = EnemyController.GetEnemies(effectArea);

            chargeTime = GetInterval();

            // タレットの処理
            AwakeTurret(enemies);
            
            // エフェクト時間の設定
            if (_isFistAwake)
            {
                _isFistAwake = false;
                effectTime = GetDuration();
            }
        }

        /**
         * 90度回転させる
         */
        public void Rotate()
        {
            Angle += 90;
            transform.Rotate(0, 90, 0);
        }

        // ================= abstract =================
        public abstract float GetHeight();
        protected abstract void AwakeTurret(List<AEnemy> enemies);
        public abstract List<TilePosition>? GetEffectArea();
        public abstract string GetTurretName();
        public abstract int GetInterval();

        public abstract void SetAngle(int angle);
        protected abstract void AsleepTurret();
        protected abstract int GetDuration();

        /**
         * 侵攻phase用の初期化
         */
        public void InvasionInitialize(
            TilePosition setPosition,
            // ReSharper disable once ParameterHidesMember
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        )
        {
            Phase = Phase.Invade;

            SetPosition = setPosition;
            this.sceneController = sceneController;
            MazeController = mazeController;
            EnemyController = enemyController;
        }

        /**
         * タレットの絶対位置を取得
         */
        public TilePosition[]? GetAbsoluteEffectArea(TilePosition position)
        {
            if (Phase != Phase.Invade) throw new Exception("侵攻phase以外では使用できません");

            var relativeArea = GetEffectArea();

            if (relativeArea == null) return null;

            var result = new TilePosition[relativeArea.Count];

            for (var i = 0; i < relativeArea.Count; i++)
            {
                var relativePosition = relativeArea[i];
                result[i] = new TilePosition(position.Row + relativePosition.Row, position.Col + relativePosition.Col);
            }

            return result;
        }
    }
}