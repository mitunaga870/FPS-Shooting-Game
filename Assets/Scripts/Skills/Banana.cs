using System.Collections.Generic;
using AClass;
using DataClass;
using InvasionPhase;
using UnityEngine;

namespace Skills
{
    public class Banana : ASkill
    {
        private const int Distance = 2;
        private const int StunTime = 100;
        
        [SerializeField]
        private BananaActiveSklill_IgnitionAction anctionController;

        protected override void UseSkillMain(TilePosition targetPosition, InvasionController sceneController, InvasionMazeController mazeController,
            InvasionEnemyController enemyController)
        {
            // 対象を取得
            var targetTiles = GetSkillEffectArea(mazeController, targetPosition);
            
            // バナナ設置
            foreach (var targetTile in targetTiles)
            {
                var banana = Instantiate(anctionController);
                banana.transform.position = targetTile.ToVector3(mazeController.MazeOrigin);
                
                // 対象に対してスキルを使用
                mazeController.SetNockBackArea(
                    new List<TilePosition> { targetTile },
                    Distance,
                    StunTime,
                    () =>
                    {
                        banana.IgnitionAction();
                    }
                );
            }
        }

        protected override List<TilePosition> GetSkillRelativeEffectArea(InvasionMazeController mazeController)
        {
            return new List<TilePosition>{ new(0, 0) };
        }

        public override string GetSkillName()
        {
            return "Banana";
        }
    }
}