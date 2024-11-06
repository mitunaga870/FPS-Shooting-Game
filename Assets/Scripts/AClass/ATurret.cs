#nullable enable
using System.Collections.Generic;
using DataClass;
using Enums;
using InvasionPhase;
using lib;
using ScriptableObjects;
using UnityEngine;

#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。

namespace AClass
{
    public abstract class ATurret : MonoBehaviour
    {
        [SerializeField]
        // ReSharper disable once NotAccessedField.Global
        protected TurretObject turretObject;

        protected InvasionController SceneController;

        protected InvasionEnemyController EnemyController;

        protected InvasionMazeController MazeController;

        private bool _isInitialized;

        private bool _isEnable;

        protected Phase Phase;

        public int Angle { get; protected set; }

        private int _chargeTime;

        private int _prevTime;
        
        private int _effectTime;
        
        private bool _isFistAwake = true;

        protected TilePosition SetPosition { get; private set; }
        
        /** バフダメージ */
        protected int AmpDamage { get; private set; }
        
        /**
         * 初期化処理
         */
        private void Start()
        {
            _chargeTime = GetInterval();
        }

        /**
         * 一定期間ごとにAwakeTurretを呼び出す
         */
        private void FixedUpdate()
        {
            // 侵攻phaseのみ
            if (Phase != Phase.Invade) return;

            // 字関連処理
            var currentTime = SceneController.GameTime;
            var deltaTime = currentTime - _prevTime;
            _prevTime = currentTime;

            // インターバルの処理
            _chargeTime -= deltaTime;
            
            // エフェクト時間の処理
            _effectTime -= deltaTime;
            
            // エフェクト時間が終わったらタレットを休眠
            if (_effectTime <= 0)
            {
                AsleepTurret();
                _isFistAwake = true;
            }

            // 未チャージ状態かつエフェクト時間が終わっている場合は戻す
            if (_chargeTime > 0 && _effectTime <= 0) return;

            // 範囲内に敵がいるか確認
            var effectArea = GetAbsoluteEffectArea(SetPosition);

            // 範囲の敵取得
            var enemies = EnemyController.GetEnemies(effectArea);

            _chargeTime = GetInterval();

            // タレットの処理
            AwakeTurret(enemies);
            
            // エフェクト時間の設定
            if (_isFistAwake)
            {
                _isFistAwake = false;
                _effectTime = GetDuration();
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
        /** バフとか入れる前のデフォダメージ取得 */
        public abstract int GetDefaultDamage();

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
            this.SceneController = sceneController;
            MazeController = mazeController;
            EnemyController = enemyController;
        }

        /**
         * タレットの絶対位置を取得
         */
        public TilePosition[]? GetAbsoluteEffectArea(TilePosition position)
        {
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

        /**
         * 実際のダメージを取得
         */
        public int GetDamage()
        {
            if (Phase == Phase.Invade)
                return (int)(GetDefaultDamage() * SceneController.StageData.StageCustomData.PlayerAttackScale) + AmpDamage;
                
            return GetDefaultDamage() + AmpDamage;
        }

        /**
         * ダメージを追加
         */
        public void AddDamage(int addDamage, int duration)
        {
            AmpDamage = addDamage;

            var delay = General.DelayCoroutineByGameTime(
                SceneController,
                duration,
                () => AmpDamage = 0
            );
            StartCoroutine(delay);
        }
    }
}