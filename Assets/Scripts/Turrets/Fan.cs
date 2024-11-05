using System.Collections.Generic;
using AClass;
using DataClass;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turrets
{
    public class Fan : ATurret
    {
        private const float Height = 0.155f;
        private const float SlowPercentage = 0.8f;
        private const int EffectDuration = 100;
        private const int SlowDuration = 5;
        private const int Interval = 400;
        private const string TurretName = "Fan";

        private bool _isAwaken;
        
        [FormerlySerializedAs("_trapFanIgnitionAction")]
        [SerializeField]
        TrapFan_IgnitionAction trapFanIgnitionAction;

        public override float GetHeight()
        {
            return Height;
        }

        protected override void AwakeTurret(List<AEnemy> enemies)
        {
            _isAwaken = true;
            
            foreach (var enemy in enemies) enemy.Slow(SlowPercentage, SlowDuration);
            
            trapFanIgnitionAction.IgnitionAction();
        }

        public override List<TilePosition> GetEffectArea()
        {
            var baseList = new List<TilePosition>()
            {
                new(0, 1),
                new(0, 2),
                new(0, 3),
                new(0, 4)
            };

            var result = new List<TilePosition>();

            // 角度によってエフェクトエリアを変更
            foreach (var position in baseList) result.Add(position.Rotate(Angle));

            return result;
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
            Angle = angle;
        }

        protected override void AsleepTurret()
        {
            if (!_isAwaken) return;
            _isAwaken = false;
            
            trapFanIgnitionAction.StopAction();
        }

        protected override int GetDuration()
        {
            return EffectDuration;
        }

        public override int GetDefaultDamage()
        {
            return 0;
        }
    }
}