using AClass;
using DataClass;

namespace Traps
{
    public class PoisonSwamp : ATrap
    {
        private const int Damage = 10;
        private const int Duration = 100;

        private int level = 1;

        public override void AwakeTrap(TilePosition position)
        {
            if (enemyController == null) return;

            enemyController.InfusePoison(position, Damage, Duration, level);
            enemyController.InfusePoison(position.GetUp(), Damage, Duration, level);
            enemyController.InfusePoison(position.GetDown(), Damage, Duration, level);
            enemyController.InfusePoison(position.GetLeft(), Damage, Duration, level);
            enemyController.InfusePoison(position.GetRight(), Damage, Duration, level);
            enemyController.InfusePoison(position.GetRightUp(), Damage, Duration, level);
            enemyController.InfusePoison(position.GetRightDown(), Damage, Duration, level);
            enemyController.InfusePoison(position.GetLeftUp(), Damage, Duration, level);
            enemyController.InfusePoison(position.GetLeftDown(), Damage, Duration, level);
        }

        public override float GetHeight()
        {
            throw new System.NotImplementedException();
        }

        public override string GetTrapName()
        {
            throw new System.NotImplementedException();
        }
    }
}