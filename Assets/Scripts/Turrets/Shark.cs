using System;
using System.Collections.Generic;
using AClass;
using DataClass;
using Enums;
using JetBrains.Annotations;
using UnityEngine;

namespace Turrets
{
    public class Shark : ATurret
    {
        private const int InvasionDataAnalysisTimeSpan = 10;

        private int Height = 3;
        private int Interval = 1;
        private int Damage = 9999;
        private string TurretName = "Shark";

        // ======= 発火確率計算用のパラメータ =======
        private int maxSpawnTime;
        private const float SpawnPossibilityDistribution = 10;

        private bool isFired = false;

        // 開始時に最大スポーン時間を計算
        private void Start()
        {
            // サイズを設定
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            
            // 侵攻フェーズでだけやる
            if (Phase != Phase.Invade) return;

            var invasionData = EnemyController.CurrentInvasionData;

            // スパンごとにスポーン数をカウントし最大スポーン時間を取得
            // ReSharper disable once LocalVariableHidesMember
            var maxSpawnTime = 0;
            var maxSpawnCount = 0;
            var calculationTime = 0;
            while (true)
            {
                // このスパンでのスポーン数
                var spawnCount = 0;

                for (var time = calculationTime; time < calculationTime + InvasionDataAnalysisTimeSpan; time++)
                {
                    var spawnData = invasionData.GetSpawnData(time);
                    if (spawnData == null) continue;

                    spawnCount += spawnData.spawnCount;
                }

                // 最大スポーン数を更新
                if (spawnCount > maxSpawnCount)
                {
                    maxSpawnCount = spawnCount;
                    maxSpawnTime = calculationTime + InvasionDataAnalysisTimeSpan / 2;
                }

                calculationTime += InvasionDataAnalysisTimeSpan;

                // 最後のスポーン時間まで到達したら終了
                if (calculationTime > invasionData.GetLastSpawnTime()) break;
            }

            this.maxSpawnTime = maxSpawnTime;
        }

        protected override void AwakeTurret(List<AEnemy> enemies)
        {
            // 発火済みなら何もしない
            if (isFired) return;

            // 確率で敵にダメージを与える
            var possibility = GetPossibility(sceneController.GameTime);
            var random = UnityEngine.Random.value;
            if (possibility < random) return;

            foreach (var enemy in enemies) enemy.Damage(Damage);

            isFired = true;
            
            gameObject.SetActive(false);
        }

        private float GetPossibility(int time)
        {
            // 正規分布の確率密度関数
            return (float)(1 / (Math.Sqrt(2 * Math.PI) * SpawnPossibilityDistribution) *
                           Math.Exp(-Math.Pow(time - maxSpawnTime, 2) /
                                    (2 * Math.Pow(SpawnPossibilityDistribution, 2))));
        }

        public override float GetHeight()
        {
            return Height;
        }

        [CanBeNull]
        public override List<TilePosition> GetEffectArea()
        {
            return null;
        }

        public override string GetTurretName()
        {
            return TurretName;
        }

        public override int GetInterval()
        {
            return Interval;
        }

        public override void SetAngle(int angle)
        {
        }

        protected override void AsleepTurret()
        {
        }

        protected override int GetDuration()
        {
            return 0;
        }
    }
}