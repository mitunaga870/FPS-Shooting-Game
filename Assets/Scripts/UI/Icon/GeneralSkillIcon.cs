using AClass;
using UI.Abstract;
using UnityEngine;

namespace UI.Icon
{
    public class GeneralSkillIcon : AGeneralIcon
    {
        [SerializeField]
        private ASkill skill;
        
        public string GetSkillName()
        {
            return skill.GetSkillName();
        }
    }
}