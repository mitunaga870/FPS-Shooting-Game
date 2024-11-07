using System.Collections.Generic;
using Shop.UI;
using UnityEngine;

namespace Shop
{
    public class ShopSkillIconGenerator : MonoBehaviour
    {
        [SerializeField]
        private List<ShopSkillIcon> skillIcons = new ();
        
        public ShopSkillIcon GetSkillIcon(string skillName)
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