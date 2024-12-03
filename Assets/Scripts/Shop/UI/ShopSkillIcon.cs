using System;
using AClass;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.UI
{
    public class ShopSkillIcon : MonoBehaviour
    {
        [SerializeField]
        private ASkill trapPrefab;
        
        public string GetSkillName()
        {
            return trapPrefab.GetSkillName();
        }

        public void SetButtonAction(Action action)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() => action());
        }
    }
}