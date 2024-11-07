using System.Collections.Generic;
using DataClass;
using InvasionPhase;
using JetBrains.Annotations;
using ScriptableObjects;
using UnityEngine;

namespace AClass
{
    public abstract class ASkill : MonoBehaviour
    {
        [SerializeField]
        protected SkillObject SkillDataObject;
        
        [SerializeField]
        protected GameObject skillObject;
        
        public abstract void UseSkill(
            TilePosition targetPosition,
            InvasionController sceneController,
            InvasionMazeController mazeController,
            InvasionEnemyController enemyController
        );
        
        /**
         * スキルの相対効果範囲を取得する
         */
        [CanBeNull]
        protected abstract List<TilePosition> GetSkillRelativeEffectArea(InvasionMazeController mazeController);
        
        /**
         * スキルの効果範囲を取得する
         */
        [CanBeNull]
        public virtual List<TilePosition> GetSkillEffectArea(InvasionMazeController mazeController, TilePosition originPosition)
        {
            var relativeEffectArea = GetSkillRelativeEffectArea(mazeController);
            if (relativeEffectArea == null)
            {
                return null;
            }

            var effectArea = new List<TilePosition>();
            foreach (var relativePosition in relativeEffectArea)
            {
                effectArea.Add(
                    new TilePosition(relativePosition.Row + originPosition.Row, relativePosition.Col + originPosition.Col)
                );
            }

            return effectArea;
        }
    }
}