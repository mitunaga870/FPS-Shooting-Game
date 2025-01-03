using AClass;
using DataClass;

namespace Traps
{
    public class PoisonSwamp : ATrap
    {
        private const string TrapName = "PoisonSwamp";
        
        private int Damage => trapObject.PoisonSwampDamage;
        private float Height => trapObject.PoisonSwampHeight;
        private int Duration => trapObject.PoisonSwampDuration;
        private int SetRange => trapObject.PoisonSwampSetRange;

        private int _level = 1;

        public override void AwakeTrap(TilePosition position)
        {
            if (EnemyController == null) return;
            
            if (ChargeTime < 0) return;
            ChargeTime = 1;

            var damage = GetDamage();

            EnemyController.InfusePoison(position, damage, Duration, _level);
            EnemyController.InfusePoison(position.GetUp(), damage, Duration, _level);
            EnemyController.InfusePoison(position.GetDown(), damage, Duration, _level);
            EnemyController.InfusePoison(position.GetLeft(), damage, Duration, _level);
            EnemyController.InfusePoison(position.GetRight(), damage, Duration, _level);
            EnemyController.InfusePoison(position.GetRightUp(), damage, Duration, _level);
            EnemyController.InfusePoison(position.GetRightDown(), damage, Duration, _level);
            EnemyController.InfusePoison(position.GetLeftUp(), damage, Duration, _level);
            EnemyController.InfusePoison(position.GetLeftDown(), damage, Duration, _level);
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

        public override int GetDefaultDamage()
        {
            return Damage;
        }
    }
}