using System.Collections.Generic;
using AClass;
using DataClass;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;

namespace Turrets
{
    public class TunaSword : ATurret
    {
        private const int Damage = 10;
        private const int Height = 1;
        private const int Interval = 1;


        public override float GetHeight()
        {
            return Height;
        }

        protected override void AwakeTurret(List<AEnemy> enemies)
        {
            foreach (var enemy in enemies) enemy.Damage(Damage);
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
            throw new System.NotImplementedException();
        }

        public override int GetInterval()
        {
            return Interval;
        }

        public override void SetAngle(int angle)
        {
        }
    }
}