using System.Collections.Generic;
using AClass;
using DataClass;
using InvasionPhase;

namespace Skills
{
    public class Teleport : ASkill
    {
        private const int Duration = 500;
        
        public override void UseSkill(
            TilePosition targetPosition,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        ) {
            mazeController.SetWarpHole(targetPosition, mazeController.StartPosition, Duration);
        }

        protected override List<TilePosition> GetSkillRelativeEffectArea(InvasionMazeController mazeController)
        {
            return new List<TilePosition>()
            {
                new(0, 0)
            };
        }
    }
}