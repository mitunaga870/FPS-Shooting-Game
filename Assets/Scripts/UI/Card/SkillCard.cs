using AClass;
using UI.Abstract;
using UnityEngine;

namespace UI.Card
{
    public class SkillCard : AGeneralCard
    {
        [SerializeField]
        private ASkill trapPrefab;
        
        public string GetSkillName()
        {
            return trapPrefab.GetSkillName();
        }
    }
}