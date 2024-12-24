using System.Collections.Generic;
using AClass;
using DataClass;
using DG.Tweening;
using InvasionPhase;
using UnityEngine;

namespace Skills
{
    public class Stamp: ASkill
    {
        [SerializeField]
        private AudioSource audioSource;
        
        [SerializeField]
        private AudioClip awakeSound;
        
        private int Damage => SkillDataObject.StampDamage;
        private int Duration => SkillDataObject.StampDuration;
        
        protected override void UseSkillMain(
            TilePosition targetPosition,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController) {
            
            // ちょい上にオブジェクトを生成
            var spawnedObject = Instantiate(skillObject);
            spawnedObject.transform.position =
                targetPosition.ToVector3(mazeController.MazeOrigin) + new UnityEngine.Vector3(0, 10, 0);
            
            // 下に落とす
            spawnedObject.transform.DOMove(targetPosition.ToVector3(mazeController.MazeOrigin), 0.5f)
                .SetEase(Ease.InCubic)
                .onComplete += () =>
            {
                // 効果音再生
                audioSource.PlayOneShot(awakeSound);
                
                Destroy(spawnedObject, 1);
            
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
            };
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
        
        public override string GetSkillName()
        {
            return "Stamp";
        }
    }
}