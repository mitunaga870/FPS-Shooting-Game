using AClass;
using DataClass;
using UnityEngine;

namespace Traps
{
    public class PoisonSwamp : ATrap
    {
        private const int Damage = 1;
        private const int Duration = 100;
        private const int SetRange = 3;
        private const float Height = 0.5f;
        private const string TrapName = "PoisonSwamp";

        private int _level = 1;

        public override void AwakeTrap(TilePosition position)
        {
            if (enemyController == null) return;
            
            if (ChargeTime < 0) return;
            ChargeTime = 1;

            enemyController.InfusePoison(position, Damage, Duration, _level);
            enemyController.InfusePoison(position.GetUp(), Damage, Duration, _level);
            enemyController.InfusePoison(position.GetDown(), Damage, Duration, _level);
            enemyController.InfusePoison(position.GetLeft(), Damage, Duration, _level);
            enemyController.InfusePoison(position.GetRight(), Damage, Duration, _level);
            enemyController.InfusePoison(position.GetRightUp(), Damage, Duration, _level);
            enemyController.InfusePoison(position.GetRightDown(), Damage, Duration, _level);
            enemyController.InfusePoison(position.GetLeftUp(), Damage, Duration, _level);
            enemyController.InfusePoison(position.GetLeftDown(), Damage, Duration, _level);
        }

        public override float GetHeight()
        {
            return Height;
        }

        public override int GetSetRange()
        {
            return SetRange;
        }

        public override string GetTrapName()
        {
            return TrapName;
        }

        public override int GetTrapAngle()
        {
            return 0;
        }

        public override void SetAngle(int trapAngle)
        {
        }
    }
}