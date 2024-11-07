using System.Collections.Generic;
using AClass;
using DataClass;
using InvasionPhase;
using lib;

namespace Skills
{
    public class Flag : ASkill
    {
        private int Duration => SkillDataObject.FlagDuration;
        private int AddDamage => SkillDataObject.FlagAddDamage;
        
        protected override void UseSkillMain(
            TilePosition targetPosition,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        ) {
            // 範囲取得
            var effectArea = GetSkillEffectArea(mazeController, targetPosition);

            // 旗をスポーン
            var flag = Instantiate(skillObject);
            var position = targetPosition.ToVector3(mazeController.MazeOrigin);
            // 高さを調整
            position += new UnityEngine.Vector3(0, 0.7f, 0);
            flag.transform.position = position;

            mazeController.AddDamage(effectArea, AddDamage, Duration);
            mazeController.OverrideSkillTime(effectArea, Duration);
            
            // 時間経過で消滅
            var destroyDelay = General.DelayCoroutineByGameTime(
                sceneController,
                Duration,
                () => { Destroy(flag); }
            );
            StartCoroutine(destroyDelay);
        }

        protected override List<TilePosition> GetSkillRelativeEffectArea(InvasionMazeController mazeController)
        {
            return new List<TilePosition>
            {
                new(2, -2), new(2, -1), new(2, 0), new(2, 1), new(2, 2),
                new(1, -2), new(1, -1), new(1, 0), new(1, 1), new(1, 2),
                new(0, -2), new(0, -1), new(0, 0), new(0, 1), new(0, 2),
                new(-1, -2), new(-1, -1), new(-1, 0), new(-1, 1), new(-1, 2),
                new(-2, -2), new(-2, -1), new(-2, 0), new(-2, 1), new(-2, 2)
            };
        }
        
        public override string GetSkillName()
        {
            return "Flag";
        }
    }
}