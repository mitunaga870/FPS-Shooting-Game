using System.Collections.Generic;
using AClass;
using UI.Abstract;
using UI.Icon;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Generator
{
    public class IconGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<GeneralTrapIcon> trapIcons;
        
        [SerializeField]
        private List<GeneralSkillIcon> skillIcons;
        
        [SerializeField]
        private List<GeneralTurretIcon> turretIcons;
        
        public GeneralTrapIcon GenerateTrapIcon(ATrap trap)
        {
            foreach (var trapIcon in trapIcons)
            {
                if (trapIcon.GetTrapName() == trap.GetTrapName())
                {
                    trapIcon.IncrementAmount();
                    return trapIcon;
                }
            }

            return null;
        }
        
        public GeneralSkillIcon GenerateSkillIcon(ASkill skill)
        {
            foreach (var skillIcon in skillIcons)
            {
                if (skillIcon.GetSkillName() == skill.GetSkillName())
                {
                    skillIcon.IncrementAmount();
                    return skillIcon;
                }
            }

            return null;
        }
        
        public GeneralTurretIcon GenerateTurretIcon(ATurret turret)
        {
            foreach (var turretIcon in turretIcons)
            {
                if (turretIcon.GetTurretName() == turret.GetTurretName())
                {
                    turretIcon.IncrementAmount();
                    return turretIcon;
                }
            }

            return null;
        }
    }
}