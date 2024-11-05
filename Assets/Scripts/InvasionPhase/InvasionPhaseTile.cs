using System;
using System.Collections;
using AClass;
using DataClass;
using Enums;
using lib;
using UnityEngine;

namespace InvasionPhase
{
    public class InvasionPhaseTile : ATile
    {
        
        private InvasionController _sceneController;
        
        private InvasionMazeController _mazeController;

        private int _prevTime;

        // ======== 燃焼床系の処理 =========
        [SerializeField]
        private GameObject igniteEffect;
        private GameObject igniteObject;
        public bool IsIgniteFloor { get; private set; }
        public int IgniteDamage { get; private set; }
        public int IgniteDuration { get; private set; }
        // =================================
        
        // ======== ワープホール系の処理 ===============================
        public bool IsWarpHole { get; private set; }
        public TilePosition WarpHoleDestination { get; private set; }
        private IEnumerator _warpHoleCoroutine;
        // ==========================================================
        
        // ============= 鈍化エリアの処理 ==============================
        public bool IsSlowArea { get; private set; }
        public float SlowAreaPower { get; private set; }
        private IEnumerator _slowAreaCoroutine;
        // ==========================================================
        
        // ============= ノックバックエリアの処理 ==============================
        public bool IsKnockBackArea { get; private set; }
        public int KnockBackDistance { get; private set; }
        public int KnockBackStunTime { get; private set; }
        public Action KnockBackCallback { get; private set; }
        // ==================================================================
        
        /** 初期化処理 */
        public void Initialize(
            int row, int column,
            TileTypes tileType,
            RoadAdjust roadAdjust,
            InvasionController sceneController,
            InvasionMazeController mazeController
        ) {
            _sceneController = sceneController;
            _mazeController = mazeController;
            Row = row;
            Column = column;

            // タイルのステータスによって処理を変える
            switch (tileType)
            {
                case TileTypes.Nothing:
                    SetNone();
                    break;
                case TileTypes.Road:
                case TileTypes.Start:
                case TileTypes.Goal:
                    SetRoad(roadAdjust);
                    break;
                default:
                    throw new Exception("未対応のタイルタイプです" + tileType);
            }
        }

        /**
         * 侵攻phaseようにセットアップしたトラップを設置する
         */
        public void SetInvasionTrap(
            string trapName,
            int trapAngle,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        )
        {
            SetTrap(mazeController, trapName, trapAngle);

            // 侵攻phase用に初期化
            Trap.InvasionInitialize(sceneController, mazeController, enemyController);
        }

        public void SetInvasionTurret(
            string turretTurret,
            int angle,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        )
        {
            SetTurret(turretTurret, angle);

            // 侵攻phase用に初期化
            Turret.InvasionInitialize(
                new TilePosition(Row, Column),
                sceneController,
                mazeController,
                enemyController
            );
        }

        public TilePosition GetPosition()
        {
            return new TilePosition(Row, Column);
        }

        // ======== 燃焼床系の処理 =========
        public void IgniteFloor(InvasionController sceneController, int igniteDamage, int igniteDuration)
        {
            IsIgniteFloor = true;
            IgniteDamage = igniteDamage;
            IgniteDuration = igniteDuration;

            // 燃焼床のエフェクトを生成
            igniteObject = Instantiate(igniteEffect, transform);
            igniteObject.transform.localPosition = new Vector3(0, 0, 0.005f);
            
            // 時間後に燃焼床を解除
            var igniteCoroutine = General.DelayCoroutineByGameTime(
                sceneController,
                igniteDuration,
                () =>
                {
                    IsIgniteFloor = false;
                    Destroy(igniteObject);
                }
            );
            StartCoroutine(igniteCoroutine);
        }

        private void FixedUpdate()
        {
            // 時間計算
            var currentTime = _sceneController.GameTime;
            var deltaTime = currentTime - _prevTime;
            _prevTime = currentTime;

            // 燃焼床の処理
            if (!IsIgniteFloor) return;

            IgniteDuration -= deltaTime;

            if (IgniteDuration <= 0)
            {
                IsIgniteFloor = false;
                ResetColor();
            }
        }
        
        // =================================
        
        // スキル用の処理
        public void OnMouseOver()
        {
            if (_sceneController.GameState != GameState.Selecting) return;

            // スキルを取得
            var skill = _sceneController.Skill;
            
            // スキルがない場合は何もしない
            if (skill == null) return;
            
            
            _mazeController.PreviewSkillEffectArea(new TilePosition(Row, Column), skill, 10);

            if (Input.GetMouseButtonDown(0))
                _sceneController.UseSkill(new TilePosition(Row, Column));
        }


        public void SetWarpHoleSource(TilePosition destinationPosition , int duration)
        {
            IsWarpHole = true;
            WarpHoleDestination = destinationPosition;
            
            // 一定時間後にワープホールを解除
            _warpHoleCoroutine = General.DelayCoroutineByGameTime(
                _sceneController,
                duration,
                () =>
                {
                    IsWarpHole = false;
                }
            );
            StartCoroutine(_warpHoleCoroutine);
        }

        public void SetWarpHoleDestination(TilePosition sourcePosition, int duration)
        {
            // TODO: 受け側のワープホールの設置処理
        }

        public void AddDamage(int addDamage, int duration)
        {
            if (hasTurret) Turret.AddDamage(addDamage, duration);
            
            if (HasTrap) Trap.AddDamage(addDamage, duration);
        }

        /**
         * スキルの持続時間を変更
         */
        public void OverrideSkillTime(int duration)
        {
            // ワープホール処理
            if (IsWarpHole)
            {
                // ストップ用の子ルーチンを作り直す
                StopCoroutine(_warpHoleCoroutine);
                
                _warpHoleCoroutine = General.DelayCoroutineByGameTime(
                    _sceneController,
                    duration,
                    () =>
                    {
                        IsWarpHole = false;
                        ResetColor();
                    }
                );
                StartCoroutine(_warpHoleCoroutine);
            }
            
            // 鈍化エリア処理
            if (IsSlowArea)
            {
                // ストップ用の子ルーチンを作り直す
                StopCoroutine(_slowAreaCoroutine);
                
                _slowAreaCoroutine = General.DelayCoroutineByGameTime(
                    _sceneController,
                    duration,
                    () =>
                    {
                        IsSlowArea = false;
                        ResetColor();
                    }
                );
                StartCoroutine(_slowAreaCoroutine);
            }
            
            // 阻害エリア処理
            if (IsBlockArea)
            {
                // ストップ用の子ルーチンを作り直す
                StopCoroutine(BlockAreaCoroutine);
                
                BlockAreaCoroutine = General.DelayCoroutineByGameTime(
                    _sceneController,
                    duration,
                    () =>
                    {
                        IsBlockArea = false;
                        ResetColor();
                    }
                );
                StartCoroutine(BlockAreaCoroutine);
            }
        }

        public void SetSlowArea(int duration, float power)
        {
            IsSlowArea = true;
            SlowAreaPower = power;
            
            // 一定時間後に鈍化エリアを解除
            _slowAreaCoroutine = General.DelayCoroutineByGameTime(
                _sceneController,
                duration,
                () =>
                {
                    IsSlowArea = false;
                }
            );
            StartCoroutine(_slowAreaCoroutine);
        }

        public void SetBlockArea(int duration)
        {
            IsBlockArea = true;
            
            // 一定時間後に鈍化エリアを解除
            BlockAreaCoroutine = General.DelayCoroutineByGameTime(
                _sceneController,
                duration,
                () =>
                {
                    IsBlockArea = false;
                }
            );
            StartCoroutine(BlockAreaCoroutine);
        }

        public void SetNockBackArea(int distance, int stunTime, Action callback)
        {
            IsKnockBackArea = true;
            KnockBackDistance = distance;
            KnockBackStunTime = stunTime;
            KnockBackCallback = callback;
        }

        public void ReleaseKnockBack()
        {
            KnockBackCallback?.Invoke();
            IsKnockBackArea = false;
        }
    }
}