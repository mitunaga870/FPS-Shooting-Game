using System.Collections.Generic;
using AClass;
using DataClass;
using InvasionPhase;
using lib;

namespace Skills
{
    public class TriangularCone : ASkill
    {
        private int Duration => SkillDataObject.TriangularConeDuration;
        
        public override void UseSkill(
            TilePosition targetPosition,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController) {
            // スキルオブジェクトを生成
            var skillObjectInstance = Instantiate(skillObject);
            skillObjectInstance.transform.position = targetPosition.ToVector3(mazeController.MazeOrigin);
            
            // ターゲットを取得
            var target = GetSkillEffectArea(mazeController, targetPosition);
            
            // ターゲットに対してスキルを使用
            mazeController.SetBlockArea(target, Duration, false);
            
            // 一定時間後にスキルを削除
            var destroyCoroutine = General.DelayCoroutineByGameTime(
                sceneController,
                Duration,
                () => Destroy(skillObjectInstance)
            );
            sceneController.StartCoroutine(destroyCoroutine);
        }

        protected override List<TilePosition> GetSkillRelativeEffectArea(InvasionMazeController mazeController)
        {
            return new List<TilePosition>{ new(0, 0) };
        }
    }
}