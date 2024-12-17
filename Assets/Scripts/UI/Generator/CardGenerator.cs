using System.Collections.Generic;
using UI.Card;
using UnityEngine;

namespace UI.Generator
{
    public class CardGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<TrapCard> _trapCards;
        
        [SerializeField]
        private List<SkillCard> _skillCards;
        
        [SerializeField]
        private List<TurretCard> _turretCards;
        
        public TrapCard GetTrapCard(string trapName)
        {
            foreach (var trapCard in _trapCards)
            {
                if (trapCard.GetTrapName() == trapName)
                {
                    return trapCard;
                }
            }

            return null;
        }
        
        public SkillCard GetSkillCard(string skillName)
        {
            foreach (var skillCard in _skillCards)
            {
                if (skillCard.GetSkillName() == skillName)
                {
                    return skillCard;
                }
            }

            return null;
        }
        
        public TurretCard GetTurretCard(string turretName)
        {
            foreach (var turretCard in _turretCards)
            {
                if (turretCard.GetTurretName() == turretName)
                {
                    return turretCard;
                }
            }

            return null;
        }
    }
}