using System.Collections.Generic;
using AClass;
using DataClass;
using JetBrains.Annotations;
using lib;
using UnityEngine;

namespace Turrets
{
    public class Stall : ATurret
    {
        private const int Height = 1;
        private const int Interval = 500;
        private const string TurretName = "Stall";
        private const int Duration = 200;
        
        private bool _isAwaken;
        // 効果を与えた敵のリスト
        private List<AEnemy> _enemies = new List<AEnemy>();

        public override float GetHeight()
        {
            return Height;
        }
        
        protected override void AwakeTurret(List<AEnemy> enemies)
        {
            _isAwaken = true;
            
            // 自身から最も近い道路の座標を取得
            var destination = MazeController.GetClosestRoadTilePosition(SetPosition);
            foreach (var enemy in enemies)
            {
                // 自分の位置を敵の目的地に設定
                enemy.SetDestination(destination);
                _enemies.Add(enemy);
            }
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
            if (!_isAwaken) return;
            _isAwaken = false;
            
            // 敵の目的地を元に戻す
            foreach (var enemy in _enemies) enemy.ReleaseDestination();
            
            _enemies.Clear();
        }

        protected override int GetDuration()
        {
            return Duration;
        }
        
        public override int GetDefaultDamage()
        {
            return 0;
        }
    }
}