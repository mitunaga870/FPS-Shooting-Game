using System.Collections.Generic;
using AClass;
using DataClass;
using InvasionPhase;
using lib;
using UnityEngine;

namespace Skills
{
    public class Teleport : ASkill
    {
        private const int Duration = 500;

        protected override void UseSkillMain(
            TilePosition targetPosition,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        ) {
            // オブジェクトを召喚
            var sourceObject = Instantiate(skillObject);
            sourceObject.transform.position = 
                targetPosition.ToVector3(mazeController.MazeOrigin) + new Vector3(0, 0.5f, 0);
            var destinationObject = Instantiate(skillObject);
            destinationObject.transform.position = 
                mazeController.StartPosition.ToVector3(mazeController.MazeOrigin) + new Vector3(0, 0.5f, 0);
            
            // 時間後にワープホールを削除
            var sourceDestroyColutine = General.DelayCoroutineByGameTime(
                sceneController,
                Duration,
                () => Destroy(sourceObject)
            );
            var destinationDestroyColutine = General.DelayCoroutineByGameTime(
                sceneController,
                Duration,
                () => Destroy(destinationObject)
            );
            StartCoroutine(sourceDestroyColutine);
            StartCoroutine(destinationDestroyColutine);
            
            mazeController.SetWarpHole(targetPosition, mazeController.StartPosition, Duration);
        }

        protected override List<TilePosition> GetSkillRelativeEffectArea(InvasionMazeController mazeController)
        {
            return new List<TilePosition>()
            {
                new(0, 0)
            };
        }
        
        public override string GetSkillName()
        {
            return "Teleport";
        }
    }
}