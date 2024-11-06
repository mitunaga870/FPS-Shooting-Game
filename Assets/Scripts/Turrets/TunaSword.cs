using System.Collections.Generic;
using AClass;
using DataClass;
using Enums;
using UnityEngine;

namespace Turrets
{
    public class TunaSword : ATurret
    {
        private const int Damage = 1;
        private const float Height = -0.5f;
        private const int Interval = 1;
        private const string TurretName = "TunaSword";
        
        [SerializeField]
        TrapKihada_IgnitionAction trapKihadaIgnitionAction;

        private void Start()
        {
            if (Phase == Phase.Invade)
            {
                // 横に向ける
                trapKihadaIgnitionAction.IgnitionAction();
            }
        }

        public override float GetHeight()
        {
            return Height;
        }

        protected override void AwakeTurret(List<AEnemy> enemies)
        {
            foreach (var enemy in enemies) enemy.Damage(GetDamage());
        }

        public override List<TilePosition> GetEffectArea()
        {
            return new List<TilePosition>()
            {
                new(0, 1),
                new(1, 1),
                new(1, 0),
                new(1, -1),
                new(0, -1),
                new(-1, -1),
                new(-1, 0),
                new(-1, 1)
            };
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

        public override int GetDefaultDamage()
        {
            return Damage;
        }
    }
}