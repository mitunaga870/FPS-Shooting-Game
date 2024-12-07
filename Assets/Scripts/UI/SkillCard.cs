using AClass;
using UnityEngine;

namespace Shop.UI
{
    public class SkillCard : MonoBehaviour
    {
        [SerializeField]
        private ASkill trapPrefab;
        
        public string GetSkillName()
        {
            return trapPrefab.GetSkillName();
        }
    }
}