using System.Collections.Generic;
using AClass;
using DataClass;
using InvasionPhase;

namespace Skills
{
    public class RailwayCrossing : ASkill
    {
        private int Duration => SkillDataObject.RailwayCrossingDuration;
        
        protected override void UseSkillMain(
            TilePosition targetPosition,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        ) {
            // ターゲットを取得
            var target = GetSkillEffectArea(mazeController, targetPosition);
            
            // ターゲットに対してスキルを使用
            mazeController.SetBlockArea(target, Duration, true);
        }

        protected override List<TilePosition> GetSkillRelativeEffectArea(InvasionMazeController mazeController)
        {
            return new List<TilePosition>()
            {
                new(0, 0), new(0, 1), new(0, 2)
            };
        }
    }
}