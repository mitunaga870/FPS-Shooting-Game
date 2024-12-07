using System.Collections.Generic;
using Shop.UI;
using UnityEngine;

namespace UI.Generator
{
    public class SkillCardGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<SkillCard> skillIcons = new ();
        
        public SkillCard GetSkillIcon(string skillName)
        {
            foreach (var skillIcon in skillIcons)
            {
                if (skillIcon.GetSkillName() == skillName)
                {
                    return skillIcon;
                }
            }

            return null;
        }
    }
}