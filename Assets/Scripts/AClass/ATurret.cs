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

        private InvasionController _sceneController;

        protected InvasionEnemyController EnemyController;

        private bool _isInitialized;

        private bool _isEnable;

        protected Phase Phase;

        public int Angle { get; protected set; } = 0;

        private int chargeTime;

        protected TilePosition SetPosition { get; private set; }

        /**
         * 一定期間ごとにAwakeTurretを呼び出す
         */
        private void Update()
        {
            // 侵攻phaseのみ
            if (Phase != Phase.Invade) return;

            // 字関連処理
            var currentTime = _sceneController.GameTime;
            chargeTime = chargeTime % GetInterval();

            // 未チャージ状態なら戻す
            if (chargeTime != 0) return;

            // 範囲内に敵がいるか確認
            var effectArea = GetAbsoluteEffectArea(SetPosition);

            // 範囲の敵取得
            var enemies = EnemyController.GetEnemies(effectArea);

            // 敵がいない場合は戻す
            if (enemies.Count == 0) return;

            // タレットの処理
            AwakeTurret(enemies);
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

        /**
         * タレットの色を変更する
         */
        public void SetColor(Color color)
        {
            GetComponent<Renderer>().material.color = color;
        }

        public abstract void SetAngle(int angle);

        /**
         * 侵攻phase用の初期化
         */
        public void InvasionInitialize(
            TilePosition setPosition,
            InvasionController sceneController,
            InvasionEnemyController enemyController
        )
        {
            Phase = Phase.Invade;

            SetPosition = setPosition;
            _sceneController = sceneController;
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