using System.Collections.Generic;
using AClass;
using DataClass;
using InvasionPhase;

namespace Skills
{
    public class Stamp: ASkill
    {
        private const int Damage = 5;
        private const int Duration = 100;
        
        public override void UseSkill(
            TilePosition targetPosition,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController) {
            // 範囲を取得
            var effectArea = GetSkillEffectArea( mazeController,targetPosition);
            
            // 範囲がnullならすべてのエリア
            if (effectArea == null)
            {
                effectArea = new List<TilePosition>();
                foreach (var tiles in mazeController.Maze)
                foreach (var tile in tiles)
                    effectArea.Add(tile.Position);
            }
            
            // 範囲内の敵にダメージを与える
            foreach (var position in effectArea)
            {
                enemyController.FlipEnemy(position, Damage, Duration);
            }
        }

        protected override List<TilePosition> GetSkillRelativeEffectArea(InvasionMazeController mazeController)
        {
            return new List<TilePosition>()
            {
                new(1, -1), new(1, 0), new(1, 1),
                new(0, -1), new(0, 0), new(0, 1),
                new(-1, -1), new(-1, 0), new(-1, 1)
            };
        }
    }
}