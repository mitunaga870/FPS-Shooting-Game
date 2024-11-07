using System.Collections.Generic;
using AClass;
using DataClass;
using Enums;
using InvasionPhase;
using lib;
using UnityEngine;
using UnityEngine.UI;

namespace Skills
{
    public class SpiderWeb : ASkill
    {
        private int SkillRange => SkillDataObject.SpiderWebRange;
        private int Duration => SkillDataObject.SpiderWebDuration;
        private float SlowPower => SkillDataObject.SpiderWebSlowPower;
        
        protected override void UseSkillMain(
            TilePosition targetPosition,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        ) {
            // ターゲットを取得
            var targetTiles = GetSkillEffectArea(mazeController, targetPosition);
            
            // ターゲットがいない場合はスキルを使用しない
            if (targetTiles == null) return;

            foreach (var targetTile in targetTiles)
            {
                // 蜘蛛の巣を描写
                // ReSharper disable once LocalVariableHidesMember
                var spawnedObject = Instantiate(skillObject);
                
                // 位置指定
                spawnedObject.transform.position = targetTile.ToVector3(mazeController.MazeOrigin);
                
                // 時間が経ったら消す
                var destroyColine = General.DelayCoroutineByGameTime(
                    sceneController,
                    Duration,
                    () => Destroy(spawnedObject)
                );
                StartCoroutine(destroyColine);
            }
            
            mazeController.SetSlowArea(targetTiles, Duration, SlowPower);
        }

        protected override List<TilePosition> GetSkillRelativeEffectArea(InvasionMazeController mazeController)
        {
            return null;
        }
        
        public override List<TilePosition> GetSkillEffectArea(InvasionMazeController mazeController, TilePosition originPosition)
        {
            // 指定タイルから隣接の道をターゲットとする
            var effectArea = new List<TilePosition>(SkillRange);

            // 選択エリアから上下左右に指定範囲分の道を取得
            for (var i = -SkillRange; i <= SkillRange; i++)
            {
                for (var j = -SkillRange; j <= SkillRange; j++)
                {
                    var targetPosition = new TilePosition(originPosition.Row + i, originPosition.Col + j);
                    
                    // タイル取得
                    var tile = mazeController.GetTile(targetPosition.Row, targetPosition.Col);
                    
                    // 道以外はスキップ
                    if (tile == null || tile.TileType != TileTypes.Road) continue;
                    
                    // まだ効果範囲リストが小さい場合
                    if (effectArea.Count < SkillRange)
                    {
                        effectArea.Add(targetPosition);
                    } else// 既に効果範囲リストが超えている場合
                    {
                        // 一番遠いタイルを取得
                        var farthestPosition = targetPosition;
                        
                        foreach (var target in effectArea)
                        {
                            if (TilePosition.GetDistance(originPosition, target) 
                                > TilePosition.GetDistance(originPosition, farthestPosition))
                                farthestPosition = target;
                        }
                            
                        // 一番遠いタイルがターゲットと同じ場合はスキップ
                        if (farthestPosition.Equals(targetPosition)) continue;
                            
                        // 一番遠いタイルを削除して新しいタイルを追加
                        effectArea.Remove(farthestPosition);
                        effectArea.Add(targetPosition);
                    }
                }
            }
            
            return effectArea;
        }
        
        public override string GetSkillName()
        {
            return "SpiderWeb";
        }
    }
}